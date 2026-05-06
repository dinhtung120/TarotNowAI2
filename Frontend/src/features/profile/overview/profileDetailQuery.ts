import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';
import { getProfileAction, type ProfileDto } from '@/features/profile/overview/actions/actions';

export interface ProfileDetailQueryData {
 profile: ProfileDto | null;
 error: string;
}

export const profileDetailQueryKey = userStateQueryKeys.profile.detail();

export async function fetchProfileDetail(): Promise<ProfileDetailQueryData> {
 const result = await getProfileAction();
 return result.success ? { profile: result.data ?? null, error: '' } : { profile: null, error: result.error };
}
