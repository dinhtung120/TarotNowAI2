'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { usePullGacha } from '@/features/gacha/shared/usePullGacha';
import { fetchJsonOrThrow } from '@/shared/http/clientFetch';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { inventoryQueryKeys } from '@/features/inventory/shared/inventoryConstants';
import type { InventoryResponse } from '@/features/inventory/shared/inventoryTypes';

vi.mock('@tanstack/react-query', () => ({
 useMutation: vi.fn(),
 useQueryClient: vi.fn(),
}));

vi.mock('@/shared/http/clientFetch', () => ({
 fetchJsonOrThrow: vi.fn(),
}));

vi.mock('@/features/gacha/shared/gachaRealtimeDedup', () => ({
 markLocalGachaCacheSynced: vi.fn(),
}));

const mockedUseMutation = vi.mocked(useMutation);
const mockedUseQueryClient = vi.mocked(useQueryClient);
const mockedFetchJsonOrThrow = vi.mocked(fetchJsonOrThrow);

function Harness({ onChange }: { onChange: (value: ReturnType<typeof usePullGacha>) => void }) {
 const value = usePullGacha();
 onChange(value);
 return null;
}

describe('usePullGacha', () => {
 let container: HTMLDivElement;
 let root: Root;

 beforeEach(() => {
  container = document.createElement('div');
  document.body.appendChild(container);
  root = createRoot(container);
  mockedUseQueryClient.mockReturnValue({
   setQueryData: vi.fn(),
   setQueriesData: vi.fn(),
   invalidateQueries: vi.fn().mockResolvedValue(undefined),
  } as never);
 });

 afterEach(() => {
  act(() => root.unmount());
  container.remove();
  vi.clearAllMocks();
 });

 it('adds newly rewarded inventory items to the optimistic inventory cache', () => {
  let capturedOptions: Record<string, unknown> | null = null;
  const inventory: InventoryResponse = { items: [] };
  const setQueryData = vi.fn((queryKey, updater) => {
   if (JSON.stringify(queryKey) !== JSON.stringify(inventoryQueryKeys.mine())) return undefined;
   const nextInventory = typeof updater === 'function' ? updater(inventory) : updater;
   inventory.items = nextInventory.items;
   return nextInventory;
  });
  mockedUseQueryClient.mockReturnValue({
   setQueryData,
   setQueriesData: vi.fn(),
   invalidateQueries: vi.fn().mockResolvedValue(undefined),
  } as never);
  mockedUseMutation.mockImplementation((options: unknown) => {
   capturedOptions = options as Record<string, unknown>;
   return {
    mutateAsync: vi.fn(),
    isPending: false,
   } as never;
  });

  act(() => {
   root.render(<Harness onChange={() => {}} />);
  });

  (capturedOptions?.onSuccess as (
   result: unknown,
   variables: unknown,
   context: unknown,
  ) => void)(
   {
    success: true,
    isIdempotentReplay: false,
    poolCode: 'daily',
    currentPityCount: 1,
    hardPityThreshold: 50,
    wasPityTriggered: false,
    rewards: [{
     kind: 'item',
     rarity: 'rare',
     itemDefinitionId: 'new-item-id',
     itemCode: 'new-item-code',
     quantityGranted: 2,
     iconUrl: null,
     nameVi: 'Vật phẩm mới',
     nameEn: 'New item',
     nameZh: '新物品',
    }],
   },
   { poolCode: 'daily', count: 1 },
   { idempotencyKey: 'local-key' },
  );

  expect(inventory.items).toEqual(
   expect.arrayContaining([
    expect.objectContaining({
     itemDefinitionId: 'new-item-id',
     itemCode: 'new-item-code',
     quantity: 2,
     canUse: true,
    }),
   ]),
  );
 });

 it('does not mutate input payload when generating idempotency key', async () => {
  let capturedOptions: Record<string, unknown> | null = null;
  mockedUseMutation.mockImplementation((options: unknown) => {
   capturedOptions = options as Record<string, unknown>;
   return {
    mutateAsync: (variables: unknown) => (capturedOptions?.mutationFn as (input: unknown) => Promise<unknown>)(variables),
    isPending: false,
   } as never;
  });

  mockedFetchJsonOrThrow.mockResolvedValue({
   poolCode: 'daily',
   currentPityCount: 1,
   wasPityTriggered: false,
   rewards: [],
  } as never);

  let latest: ReturnType<typeof usePullGacha> | null = null;
  act(() => {
   root.render(<Harness onChange={(value) => { latest = value; }} />);
  });

  const payload = { poolCode: 'daily', count: 1 };
  await act(async () => {
   await latest?.mutateAsync(payload as never);
  });

  expect(payload).toEqual({ poolCode: 'daily', count: 1 });
  const [, requestInit] = mockedFetchJsonOrThrow.mock.calls[0] ?? [];
  expect(requestInit).toEqual(expect.objectContaining({
   headers: expect.objectContaining({
    'x-idempotency-key': expect.any(String),
   }),
  }));
 });
});
