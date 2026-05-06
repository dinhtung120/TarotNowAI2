export interface GachaPool {
  code: string;
  poolType: string;
  nameVi: string;
  nameEn: string;
  nameZh: string;
  descriptionVi: string;
  descriptionEn: string;
  descriptionZh: string;
  costCurrency: string;
  costAmount: number;
  oddsVersion: string;
  userCurrentPity: number;
  pityEnabled: boolean;
  hardPityCount: number;
}

export interface GachaPoolRewardRate {
  kind: string;
  rarity: string;
  currency?: string | null;
  amount?: number | null;
  itemDefinitionId?: string | null;
  itemCode?: string | null;
  quantityGranted: number;
  probabilityBasisPoints: number;
  probabilityPercent: number;
  iconUrl?: string | null;
  nameVi: string;
  nameEn: string;
  nameZh: string;
}

export interface GachaPoolOdds {
  poolCode: string;
  oddsVersion: string;
  rewards: GachaPoolRewardRate[];
}

export interface GachaHistoryReward {
  kind: string;
  rarity: string;
  currency?: string | null;
  amount?: number | null;
  itemCode?: string | null;
  quantityGranted: number;
  iconUrl?: string | null;
  nameVi: string;
  nameEn: string;
  nameZh: string;
  isHardPityReward: boolean;
}

export interface GachaHistoryEntry {
  pullOperationId: string;
  poolCode: string;
  pullCount: number;
  pityBefore: number;
  pityAfter: number;
  wasPityReset: boolean;
  createdAtUtc: string;
  rewards: GachaHistoryReward[];
}

export interface GachaHistoryPage {
  page: number;
  pageSize: number;
  totalCount: number;
  items: GachaHistoryEntry[];
}

export interface PullGachaReward {
  kind: string;
  rarity: string;
  currency?: string | null;
  amount?: number | null;
  itemDefinitionId?: string | null;
  itemCode?: string | null;
  quantityGranted: number;
  iconUrl?: string | null;
  nameVi: string;
  nameEn: string;
  nameZh: string;
}

export interface PullGachaResult {
  success: boolean;
  isIdempotentReplay: boolean;
  poolCode: string;
  currentPityCount: number;
  hardPityThreshold: number;
  wasPityTriggered: boolean;
  rewards: PullGachaReward[];
}

export interface PullGachaPayload {
  poolCode: string;
  count: number;
  idempotencyKey?: string;
}
