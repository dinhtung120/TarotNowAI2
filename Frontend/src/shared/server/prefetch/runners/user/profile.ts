import type { QueryClient } from '@tanstack/react-query';
import { getProfileAction } from '@/features/profile/application/actions/get-profile';
import { getMyReaderRequest } from '@/features/reader/application/actions';
import { getMfaStatus } from '@/features/profile/mfa/application/actions/status';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

async function profileMeQueryFn() {
 const result = await getProfileAction();
 return result.success ? { profile: result.data ?? null, error: '' } : { profile: null, error: result.error };
}

export async function prefetchProfilePage(qc: QueryClient): Promise<void> {
 await Promise.all([
  qc.prefetchQuery({
   queryKey: userStateQueryKeys.profile.me(),
   queryFn: profileMeQueryFn,
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
