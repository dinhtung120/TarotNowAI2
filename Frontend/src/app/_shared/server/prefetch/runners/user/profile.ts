import type { QueryClient } from '@tanstack/react-query';
import { getMyReaderRequest } from '@/features/reader/shared';
import { fetchProfileDetail, profileDetailQueryKey } from '@/features/profile/overview/profileDetailQuery';
import { getMfaStatus } from '@/features/profile/mfa/actions/actions/status';
import { userStateQueryKeys } from '@/shared/query/userStateQueryKeys';

export async function prefetchProfilePage(qc: QueryClient): Promise<void> {
 await Promise.all([
  qc.prefetchQuery({
   queryKey: profileDetailQueryKey,
   queryFn: fetchProfileDetail,
  }),
  qc.prefetchQuery({
   queryKey: userStateQueryKeys.reader.myRequest(),
   queryFn: async () => {
    const result = await getMyReaderRequest();
    return result.success ? result.data ?? null : null;
   },
  }),
 ]);
}

export async function prefetchProfileMfaPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.profile.mfaStatus(),
  queryFn: async () => {
   const result = await getMfaStatus();
   return result.success ? result.data ?? false : false;
  },
 });
}
