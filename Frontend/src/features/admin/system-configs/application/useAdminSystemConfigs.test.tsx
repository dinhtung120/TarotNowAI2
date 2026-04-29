'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useAdminSystemConfigs } from '@/features/admin/system-configs/application/useAdminSystemConfigs';
import { listSystemConfigs } from '@/features/admin/application/actions';
import type { AdminSystemConfigItem } from '@/features/admin/system-configs/system-config.types';
import toast from 'react-hot-toast';

vi.mock('@tanstack/react-query', () => ({
 useQuery: vi.fn(),
 useMutation: vi.fn(),
 useQueryClient: vi.fn(),
}));

vi.mock('next-intl', () => ({
 useTranslations: vi.fn(() => (key: string) => key),
}));

vi.mock('react-hot-toast', () => ({
 default: {
  error: vi.fn(),
  success: vi.fn(),
 },
}));

vi.mock('@/features/admin/application/actions', () => ({
 listSystemConfigs: vi.fn(),
 updateSystemConfig: vi.fn(),
 restartServer: vi.fn(),
}));

const mockedUseQuery = vi.mocked(useQuery);
const mockedUseMutation = vi.mocked(useMutation);
const mockedUseQueryClient = vi.mocked(useQueryClient);
const mockedListSystemConfigs = vi.mocked(listSystemConfigs);
const mockedToast = vi.mocked(toast);

function Harness({
 initialConfigs,
 onChange,
}: {
 initialConfigs: AdminSystemConfigItem[];
 onChange: (value: ReturnType<typeof useAdminSystemConfigs>) => void;
}) {
 const value = useAdminSystemConfigs(initialConfigs);
 onChange(value);
 return null;
}

describe('useAdminSystemConfigs', () => {
 let container: HTMLDivElement;
 let root: Root;

 beforeEach(() => {
  container = document.createElement('div');
  document.body.appendChild(container);
  root = createRoot(container);
  mockedUseQueryClient.mockReturnValue({
   setQueryData: vi.fn(),
   invalidateQueries: vi.fn(),
  } as never);
  mockedUseMutation.mockReturnValue({
   isPending: false,
   mutateAsync: vi.fn(),
  } as never);
 });

 afterEach(() => {
  act(() => {
   root.unmount();
  });
  container.remove();
  vi.clearAllMocks();
 });

 it('throws typed query errors and exposes explicit load error with retry action', async () => {
  const refetch = vi.fn().mockResolvedValue(undefined);
  let capturedQueryFn: (() => Promise<AdminSystemConfigItem[]>) | null = null;
  mockedUseQuery.mockImplementation((options) => {
   capturedQueryFn = options.queryFn as () => Promise<AdminSystemConfigItem[]>;
   return {
    data: [],
    isLoading: false,
    isFetching: false,
    isError: true,
    isSuccess: false,
    error: new Error('System config backend unavailable'),
    refetch,
   } as never;
  });
  mockedListSystemConfigs.mockResolvedValue({
   success: false,
   error: 'System config backend unavailable',
  } as never);

  let latest: ReturnType<typeof useAdminSystemConfigs> | null = null;
  act(() => {
   root.render(<Harness initialConfigs={[]} onChange={(value) => {
    latest = value;
   }} />);
  });

  await expect(capturedQueryFn?.() ?? Promise.resolve([])).rejects.toThrow('System config backend unavailable');
  expect(latest?.hasLoadError).toBe(true);
  expect(latest?.loadError).toBe('System config backend unavailable');

 await act(async () => {
  await latest?.retryLoadConfigs();
 });
 expect(refetch).toHaveBeenCalledTimes(1);
 });

 it('updates selected config and syncs query cache on successful save', async () => {
  const queryClient = {
   setQueryData: vi.fn(),
   invalidateQueries: vi.fn().mockResolvedValue(undefined),
  };
  mockedUseQueryClient.mockReturnValue(queryClient as never);
  const updateMutateAsync = vi.fn().mockResolvedValue({
   success: true,
   data: {
    key: 'wallet.timeout',
    value: '{"seconds":20}',
    description: 'updated',
    valueKind: 'json',
    updatedAt: '2026-04-29T00:00:00.000Z',
   },
  });
  const restartMutateAsync = vi.fn().mockResolvedValue({ success: true });
  let mutationInvocation = 0;
  mockedUseMutation.mockImplementation(() => {
   mutationInvocation += 1;
   if (mutationInvocation % 2 === 1) {
    return {
     isPending: false,
     mutateAsync: updateMutateAsync,
    } as never;
   }

   return {
    isPending: false,
    mutateAsync: restartMutateAsync,
   } as never;
  });
  mockedUseQuery.mockReturnValue({
   data: [{
    key: 'wallet.timeout',
    value: '{"seconds":10}',
    description: 'timeout',
    valueKind: 'json',
   }],
   isLoading: false,
   isFetching: false,
   isError: false,
   isSuccess: true,
   error: null,
   refetch: vi.fn().mockResolvedValue(undefined),
  } as never);

  let latest: ReturnType<typeof useAdminSystemConfigs> | null = null;
  act(() => {
   root.render(<Harness initialConfigs={[{
    key: 'wallet.timeout',
    value: '{"seconds":10}',
    description: 'timeout',
    valueKind: 'json',
   }]} onChange={(value) => {
    latest = value;
   }} />);
  });

  act(() => {
   latest?.setDraftDescription('updated');
   latest?.setDraftValue('{"seconds":20}');
   latest?.setDraftValueKind('json');
  });

  await act(async () => {
   await latest?.saveSelectedConfig();
  });

  expect(queryClient.setQueryData).toHaveBeenCalled();
  expect(queryClient.invalidateQueries).toHaveBeenCalledWith({ queryKey: ['admin', 'system-configs'] });
  expect(mockedToast.success).toHaveBeenCalledWith('system_configs.toast.save_success');
 });

 it('groups and filters configs by namespace and search keyword', () => {
  mockedUseQuery.mockReturnValue({
   data: [
    {
     key: 'wallet.timeout',
     value: '10',
     description: 'wallet timeout',
     valueKind: 'scalar',
    },
    {
     key: 'chat.max_messages',
     value: '50',
     description: 'chat cap',
     valueKind: 'scalar',
    },
   ],
   isLoading: false,
   isFetching: false,
   isError: false,
   isSuccess: true,
   error: null,
   refetch: vi.fn().mockResolvedValue(undefined),
  } as never);

  let latest: ReturnType<typeof useAdminSystemConfigs> | null = null;
  act(() => {
   root.render(<Harness initialConfigs={[]} onChange={(value) => {
    latest = value;
   }} />);
  });

  expect(latest?.groupedItems.length).toBe(2);

  act(() => {
   latest?.setSearchText('wallet');
  });

  expect(latest?.filteredItems.length).toBe(1);
  expect(latest?.filteredItems[0]?.key).toBe('wallet.timeout');
 });
});
