'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import toast from 'react-hot-toast';
import { useAdminGamification } from '@/features/admin/gamification/hooks/useAdminGamification';
import {
 useMutation,
 useQuery,
 useQueryClient,
} from '@tanstack/react-query';
import { adminGamificationKeys } from '@/features/admin/gamification/adminGamificationKeys';

vi.mock('@tanstack/react-query', () => ({
 useMutation: vi.fn(),
 useQuery: vi.fn(),
 useQueryClient: vi.fn(),
}));

vi.mock('react-hot-toast', () => ({
 default: {
  success: vi.fn(),
  error: vi.fn(),
 },
}));

const mockedUseQuery = vi.mocked(useQuery);
const mockedUseMutation = vi.mocked(useMutation);
const mockedUseQueryClient = vi.mocked(useQueryClient);
const mockedToast = vi.mocked(toast);

function Harness({ onChange }: { onChange: (value: ReturnType<typeof useAdminGamification>) => void }) {
 const value = useAdminGamification();
 onChange(value);
 return null;
}

describe('useAdminGamification', () => {
 let container: HTMLDivElement;
 let root: Root;

 beforeEach(() => {
  container = document.createElement('div');
  document.body.appendChild(container);
  root = createRoot(container);
  mockedUseQuery
   .mockReturnValueOnce({ data: [], isLoading: false } as never)
   .mockReturnValueOnce({ data: [], isLoading: false } as never)
   .mockReturnValueOnce({ data: [], isLoading: false } as never);
 });

 afterEach(() => {
  act(() => root.unmount());
  container.remove();
  vi.clearAllMocks();
 });

 it('invalidates quest data and shows a success toast after quest upsert', async () => {
  const invalidateQueries = vi.fn().mockResolvedValue(undefined);
  mockedUseQueryClient.mockReturnValue({ invalidateQueries } as never);
  mockedUseMutation
   .mockReturnValueOnce({ mutateAsync: vi.fn(), isPending: false } as never)
   .mockReturnValueOnce({ mutateAsync: vi.fn(), isPending: false } as never)
   .mockReturnValueOnce({ mutateAsync: vi.fn(), isPending: false } as never)
   .mockReturnValueOnce({ mutateAsync: vi.fn(), isPending: false } as never)
   .mockReturnValueOnce({ mutateAsync: vi.fn(), isPending: false } as never)
   .mockReturnValueOnce({ mutateAsync: vi.fn(), isPending: false } as never);

  act(() => {
   root.render(<Harness onChange={() => undefined} />);
  });

  const upsertQuestMutation = mockedUseMutation.mock.calls[0]?.[0];
  const deleteTitleMutation = mockedUseMutation.mock.calls[5]?.[0];

  await act(async () => {
   await upsertQuestMutation?.onSuccess?.();
   deleteTitleMutation?.onError?.(new Error('delete failed'));
  });

 expect(mockedToast.success).toHaveBeenCalledWith('Luu nhiem vu thanh cong.');
  expect(invalidateQueries).toHaveBeenCalledWith({ queryKey: adminGamificationKeys.quests() });
  expect(mockedToast.error).toHaveBeenCalledWith('delete failed');
 });

 it('wires success and error handlers for every admin mutation to the matching cache owner', async () => {
  const invalidateQueries = vi.fn().mockResolvedValue(undefined);
  mockedUseQueryClient.mockReturnValue({ invalidateQueries } as never);
  mockedUseMutation
   .mockReturnValueOnce({ mutateAsync: vi.fn(), isPending: false } as never)
   .mockReturnValueOnce({ mutateAsync: vi.fn(), isPending: false } as never)
   .mockReturnValueOnce({ mutateAsync: vi.fn(), isPending: false } as never)
   .mockReturnValueOnce({ mutateAsync: vi.fn(), isPending: false } as never)
   .mockReturnValueOnce({ mutateAsync: vi.fn(), isPending: false } as never)
   .mockReturnValueOnce({ mutateAsync: vi.fn(), isPending: false } as never);

  act(() => {
   root.render(<Harness onChange={() => undefined} />);
  });

  const mutationConfigs = mockedUseMutation.mock.calls.map(([config]) => config);
  const [
   upsertQuestMutation,
   deleteQuestMutation,
   upsertAchievementMutation,
   deleteAchievementMutation,
   upsertTitleMutation,
   deleteTitleMutation,
  ] = mutationConfigs;

  await act(async () => {
   await upsertQuestMutation?.onSuccess?.();
   deleteQuestMutation?.onError?.(new Error('delete quest failed'));
   await deleteQuestMutation?.onSuccess?.();
   upsertAchievementMutation?.onError?.(new Error('achievement failed'));
   await upsertAchievementMutation?.onSuccess?.();
   await deleteAchievementMutation?.onSuccess?.();
   deleteAchievementMutation?.onError?.(new Error('delete achievement failed'));
   await upsertTitleMutation?.onSuccess?.();
   upsertTitleMutation?.onError?.(new Error('title failed'));
   await deleteTitleMutation?.onSuccess?.();
   deleteTitleMutation?.onError?.(new Error('delete title failed'));
  });

  expect(mockedToast.success).toHaveBeenCalledWith('Xoa nhiem vu thanh cong.');
  expect(mockedToast.success).toHaveBeenCalledWith('Luu thanh tuu thanh cong.');
  expect(mockedToast.success).toHaveBeenCalledWith('Xoa thanh tuu thanh cong.');
  expect(mockedToast.success).toHaveBeenCalledWith('Luu danh hieu thanh cong.');
  expect(mockedToast.success).toHaveBeenCalledWith('Xoa danh hieu thanh cong.');
  expect(invalidateQueries).toHaveBeenCalledWith({ queryKey: adminGamificationKeys.quests() });
  expect(invalidateQueries).toHaveBeenCalledWith({ queryKey: adminGamificationKeys.achievements() });
  expect(invalidateQueries).toHaveBeenCalledWith({ queryKey: adminGamificationKeys.titles() });
  expect(mockedToast.error).toHaveBeenCalledWith('delete quest failed');
  expect(mockedToast.error).toHaveBeenCalledWith('achievement failed');
  expect(mockedToast.error).toHaveBeenCalledWith('delete achievement failed');
  expect(mockedToast.error).toHaveBeenCalledWith('title failed');
  expect(mockedToast.error).toHaveBeenCalledWith('delete title failed');
 });
});
