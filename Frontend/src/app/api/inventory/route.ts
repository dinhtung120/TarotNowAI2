import { NextRequest, NextResponse } from 'next/server';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { INVENTORY_IDEMPOTENCY_HEADER } from '@/shared/infrastructure/inventory/inventoryConstants';
import type {
 InventoryResponse,
 UseInventoryItemPayload,
 UseInventoryItemResponse,
} from '@/shared/infrastructure/inventory/inventoryTypes';

interface ProblemDetailsPayload {
 type: string;
 title: string;
 status: number;
 detail: string;
 errorCode?: string;
}

function buildProblemResponse(status: number, detail: string, errorCode?: string): NextResponse {
 const payload: ProblemDetailsPayload = {
  type: 'about:blank',
  title: status >= 500 ? 'Server Error' : status === 401 ? 'Unauthorized' : 'Bad Request',
  status,
  detail,
  ...(errorCode ? { errorCode } : {}),
 };

 return NextResponse.json(payload, { status });
}

export async function GET(): Promise<NextResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 }

 const result = await serverHttpRequest<InventoryResponse>('/inventory', {
  method: 'GET',
  token,
  fallbackErrorMessage: 'Failed to load inventory.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data, { status: 200 });
}

export async function POST(request: NextRequest): Promise<NextResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 }

 let payload: UseInventoryItemPayload;
 try {
  payload = (await request.json()) as UseInventoryItemPayload;
 } catch {
  return buildProblemResponse(400, 'Invalid request payload.');
 }

 if (!payload.itemCode || payload.itemCode.trim().length === 0) {
  return buildProblemResponse(400, 'Item code is required.');
 }

 const quantity = Number.isFinite(payload.quantity)
  ? Math.max(1, Math.min(10, Math.trunc(payload.quantity)))
  : 1;

 const idempotencyKey = request.headers.get(INVENTORY_IDEMPOTENCY_HEADER) || payload.idempotencyKey || '';
 if (!idempotencyKey) {
  return buildProblemResponse(400, 'Missing idempotency key.');
 }

 const result = await serverHttpRequest<UseInventoryItemResponse>('/inventory/use', {
  method: 'POST',
  token,
  json: {
   itemCode: payload.itemCode,
   quantity,
   targetCardId: payload.targetCardId,
   idempotencyKey,
  },
  headers: {
   [INVENTORY_IDEMPOTENCY_HEADER]: idempotencyKey,
  },
  fallbackErrorMessage: 'Failed to use inventory item.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data, { status: 200 });
}
