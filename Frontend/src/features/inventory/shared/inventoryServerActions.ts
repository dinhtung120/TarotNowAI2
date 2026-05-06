'use server';

import { AUTH_ERROR } from '@/shared/models/authErrors';
import { getServerAccessToken } from '@/shared/auth/serverAuth';
import { serverHttpRequest } from '@/shared/http/serverHttpClient';
import type { InventoryResponse } from '@/features/inventory/shared/inventoryTypes';

export async function fetchInventoryServer(): Promise<InventoryResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  throw new Error(AUTH_ERROR.UNAUTHORIZED);
 }

 const result = await serverHttpRequest<InventoryResponse>('/inventory', {
  method: 'GET',
  token,
  fallbackErrorMessage: 'Failed to load inventory.',
 });

 if (!result.ok) {
  throw new Error(result.error);
 }

 return result.data;
}
