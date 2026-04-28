import type { MyReaderRequest } from '@/features/reader/application/actions';

export function resolveReaderApplyHydrationIdentity(
 existingRequest: MyReaderRequest | null | undefined,
): string {
 if (!existingRequest?.hasRequest) {
  return 'new-reader-request';
 }

 return `${existingRequest.status}:${existingRequest.createdAt ?? 'unknown'}`;
}
