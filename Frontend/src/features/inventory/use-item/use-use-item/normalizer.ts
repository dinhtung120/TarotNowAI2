import { INVENTORY_API_ROUTE, INVENTORY_IDEMPOTENCY_HEADER } from '@/features/inventory/shared/inventoryConstants';
import type {
 UseInventoryCardStatSnapshot,
 UseInventoryItemEffectSummary,
 UseInventoryItemPayload,
 UseInventoryItemResponse,
} from '@/features/inventory/shared/inventoryTypes';
import { fetchJsonOrThrow } from '@/shared/infrastructure/http/clientFetch';

interface InventoryEffectSummaryPayload {
 EffectType?: unknown;
 effectType?: unknown;
 RolledValue?: unknown;
 rolledValue?: unknown;
 CardId?: unknown;
 cardId?: unknown;
 BeforeValue?: unknown;
 beforeValue?: unknown;
 AfterValue?: unknown;
 afterValue?: unknown;
 Before?: InventorySnapshotPayload | null;
 before?: InventorySnapshotPayload | null;
 After?: InventorySnapshotPayload | null;
 after?: InventorySnapshotPayload | null;
}

interface InventorySnapshotPayload {
 Level?: unknown;
 level?: unknown;
 CurrentExp?: unknown;
 currentExp?: unknown;
 ExpToNextLevel?: unknown;
 expToNextLevel?: unknown;
 BaseAtk?: unknown;
 baseAtk?: unknown;
 BaseDef?: unknown;
 baseDef?: unknown;
 BonusAtkPercent?: unknown;
 bonusAtkPercent?: unknown;
 BonusDefPercent?: unknown;
 bonusDefPercent?: unknown;
 TotalAtk?: unknown;
 totalAtk?: unknown;
 TotalDef?: unknown;
 totalDef?: unknown;
}

interface UseItemResponsePayload {
 ItemCode?: unknown;
 itemCode?: unknown;
 TargetCardId?: unknown;
 targetCardId?: unknown;
 IsIdempotentReplay?: unknown;
 isIdempotentReplay?: unknown;
 Message?: unknown;
 message?: unknown;
 EffectSummaries?: unknown;
 effectSummaries?: unknown;
}

function toNumber(value: unknown): number {
 if (typeof value === 'number' && Number.isFinite(value)) {
  return value;
 }

 if (typeof value === 'string') {
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : 0;
 }

 return 0;
}

function toStringValue(value: unknown): string {
 return typeof value === 'string' ? value : '';
}

function toBoolean(value: unknown): boolean {
 return value === true;
}

function hasValue(value: unknown): boolean {
 if (value === null || value === undefined) {
  return false;
 }

 if (typeof value === 'string') {
  return value.trim().length > 0;
 }

 return true;
}

function normalizeSnapshot(snapshot: InventorySnapshotPayload | null | undefined): UseInventoryCardStatSnapshot | null {
 if (!snapshot) {
  return null;
 }

 return {
  level: toNumber(snapshot.level ?? snapshot.Level),
  currentExp: toNumber(snapshot.currentExp ?? snapshot.CurrentExp),
  expToNextLevel: toNumber(snapshot.expToNextLevel ?? snapshot.ExpToNextLevel),
  baseAtk: toNumber(snapshot.baseAtk ?? snapshot.BaseAtk),
  baseDef: toNumber(snapshot.baseDef ?? snapshot.BaseDef),
  bonusAtkPercent: toNumber(snapshot.bonusAtkPercent ?? snapshot.BonusAtkPercent),
  bonusDefPercent: toNumber(snapshot.bonusDefPercent ?? snapshot.BonusDefPercent),
  totalAtk: toNumber(snapshot.totalAtk ?? snapshot.TotalAtk),
  totalDef: toNumber(snapshot.totalDef ?? snapshot.TotalDef),
 };
}

function normalizeSummary(rawSummary: InventoryEffectSummaryPayload): UseInventoryItemEffectSummary {
 const cardId = rawSummary.cardId ?? rawSummary.CardId;
 const beforeValue = rawSummary.beforeValue ?? rawSummary.BeforeValue;
 const afterValue = rawSummary.afterValue ?? rawSummary.AfterValue;

 return {
  effectType: toStringValue(rawSummary.effectType ?? rawSummary.EffectType),
  rolledValue: toNumber(rawSummary.rolledValue ?? rawSummary.RolledValue),
  cardId: hasValue(cardId) ? toNumber(cardId) : null,
  beforeValue: hasValue(beforeValue) ? toNumber(beforeValue) : null,
  afterValue: hasValue(afterValue) ? toNumber(afterValue) : null,
  before: normalizeSnapshot(rawSummary.before ?? rawSummary.Before),
  after: normalizeSnapshot(rawSummary.after ?? rawSummary.After),
 };
}

function normalizeUseItemResponse(rawData: unknown): UseInventoryItemResponse {
 const data = (rawData as UseItemResponsePayload | null) ?? {};
 const effectSummariesRaw = data.effectSummaries ?? data.EffectSummaries;
 const targetCardId = data.targetCardId ?? data.TargetCardId;
 const effectSummaries = Array.isArray(effectSummariesRaw)
  ? effectSummariesRaw.map((summary) => normalizeSummary((summary as InventoryEffectSummaryPayload | null) ?? {}))
  : [];

 return {
  itemCode: toStringValue(data.itemCode ?? data.ItemCode),
  targetCardId: hasValue(targetCardId) ? toNumber(targetCardId) : undefined,
  isIdempotentReplay: toBoolean(data.isIdempotentReplay ?? data.IsIdempotentReplay),
  message: toStringValue(data.message ?? data.Message),
  effectSummaries,
 };
}

export async function sendUseItemRequest(
 payload: Readonly<UseInventoryItemPayload>,
 idempotencyKey: string,
): Promise<UseInventoryItemResponse> {
 const rawData = await fetchJsonOrThrow<unknown>(
  INVENTORY_API_ROUTE,
  {
   method: 'POST',
   credentials: 'include',
   headers: {
    'Content-Type': 'application/json',
    [INVENTORY_IDEMPOTENCY_HEADER]: idempotencyKey,
   },
   body: JSON.stringify({
    itemCode: payload.itemCode,
    quantity: payload.quantity,
    targetCardId: payload.targetCardId,
    idempotencyKey,
   }),
  },
  'Failed to use item.',
  10_000,
 );

 return normalizeUseItemResponse(rawData);
}
