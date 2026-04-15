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

export async function GET(): Promise<NextResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  return NextResponse.json({ error: AUTH_ERROR.UNAUTHORIZED }, { status: 401 });
 }

 const result = await serverHttpRequest<InventoryResponse>('/inventory', {
  method: 'GET',
  token,
  fallbackErrorMessage: 'Failed to load inventory.',
 });

 if (!result.ok) {
  return NextResponse.json({ error: result.error }, { status: result.status });
 }

 return NextResponse.json(result.data, { status: 200 });
}

export async function POST(request: NextRequest): Promise<NextResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  return NextResponse.json({ error: AUTH_ERROR.UNAUTHORIZED }, { status: 401 });
 }

 let payload: UseInventoryItemPayload;
 try {
  payload = (await request.json()) as UseInventoryItemPayload;
 } catch {
  return NextResponse.json({ error: 'Invalid request payload.' }, { status: 400 });
 }

 const idempotencyKey = request.headers.get(INVENTORY_IDEMPOTENCY_HEADER) || payload.idempotencyKey || '';
 if (!idempotencyKey) {
  return NextResponse.json({ error: 'Missing idempotency key.' }, { status: 400 });
 }

 const result = await serverHttpRequest<UseInventoryItemResponse>('/inventory/use', {
  method: 'POST',
  token,
  json: {
   itemCode: payload.itemCode,
   targetCardId: payload.targetCardId,
   idempotencyKey,
  },
  headers: {
   [INVENTORY_IDEMPOTENCY_HEADER]: idempotencyKey,
  },
  fallbackErrorMessage: 'Failed to use inventory item.',
 });

 if (!result.ok) {
  return NextResponse.json({ error: result.error }, { status: result.status });
 }

 return NextResponse.json(result.data, { status: 200 });
}
