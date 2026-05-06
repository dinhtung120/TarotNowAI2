import type { QueryClient } from '@tanstack/react-query';
import { listReaders, getMyReaderRequest, getReaderProfile } from '@/features/reader/shared';
import { getProfileAction } from '@/features/profile/overview/actions/actions/get-profile';
import { userStateQueryKeys } from '@/shared/query/userStateQueryKeys';
import {
 READERS_DIRECTORY_PAGE_SIZE,
 readersDirectoryQueryKey,
 swallowPrefetch,
} from '@/app/_shared/server/prefetch/runners/user/shared';

async function readersDirectoryQueryFn() {
 const result = await listReaders(1, READERS_DIRECTORY_PAGE_SIZE, '', '', '');
 if (result.success && result.data) {
  return result.data;
 }
 return { readers: [], totalCount: 0 };
}

export async function prefetchReadersDirectoryPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: readersDirectoryQueryKey,
  queryFn: readersDirectoryQueryFn,
 });
}

export async function prefetchReaderApplyPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.reader.myRequest(),
  queryFn: async () => {
   const result = await getMyReaderRequest();
   return result.success ? result.data ?? null : null;
  },
 });
}

export async function prefetchReaderPublicProfilePage(qc: QueryClient, readerId: string): Promise<void> {
 if (!readerId) return;
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.reader.profile(readerId),
  queryFn: async () => {
   const result = await getReaderProfile(readerId);
   return result.success ? result.data ?? null : null;
  },
 });
}

export async function prefetchProfileReaderSettingsPage(qc: QueryClient): Promise<void> {
 const profileResult = await getProfileAction();
 if (!profileResult.success || !profileResult.data) {
  return;
 }
 if (profileResult.data.role !== 'tarot_reader') {
  return;
 }
 const userId = profileResult.data.id;
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: ['reader-profile-settings', userId],
   queryFn: async () => {
    const result = await getReaderProfile(userId);
    return result.success ? result.data ?? null : null;
   },
  });
 });
}
