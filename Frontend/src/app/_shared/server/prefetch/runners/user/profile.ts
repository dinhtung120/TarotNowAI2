import type { QueryClient } from '@tanstack/react-query';
import { fetchProfileDetail, profileDetailQueryKey } from '@/features/profile/overview/profileDetailQuery';
import { getMfaStatus } from '@/features/profile/mfa/actions/actions/status';
import { userStateQueryKeys } from '@/shared/query/userStateQueryKeys';

export async function prefetchProfilePage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: profileDetailQueryKey,
  queryFn: fetchProfileDetail,
 });
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
