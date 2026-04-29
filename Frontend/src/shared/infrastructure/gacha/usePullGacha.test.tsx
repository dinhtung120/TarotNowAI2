'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { usePullGacha } from '@/shared/infrastructure/gacha/usePullGacha';
import { fetchJsonOrThrow } from '@/shared/infrastructure/http/clientFetch';
import { useMutation, useQueryClient } from '@tanstack/react-query';

vi.mock('@tanstack/react-query', () => ({
 useMutation: vi.fn(),
 useQueryClient: vi.fn(),
}));

vi.mock('@/shared/infrastructure/http/clientFetch', () => ({
 fetchJsonOrThrow: vi.fn(),
}));

vi.mock('@/shared/infrastructure/gacha/gachaRealtimeDedup', () => ({
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
