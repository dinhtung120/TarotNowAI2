'use client';

import { useQuery, type UseQueryResult } from '@tanstack/react-query';
import type { PublicRuntimePoliciesDto } from '@/shared/application/actions/runtime-policies';
import { fetchJsonOrThrow } from '@/shared/infrastructure/http/clientFetch';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

const PUBLIC_RUNTIME_POLICIES_TIMEOUT_MS = 8_000;
const PUBLIC_RUNTIME_POLICIES_STALE_TIME_MS = 60_000;

export function usePublicRuntimePolicies(): UseQueryResult<PublicRuntimePoliciesDto, Error> {
  const query = useQuery({
    queryKey: userStateQueryKeys.system.publicRuntimePolicies(),
    queryFn: ({ signal }) =>
      fetchJsonOrThrow<PublicRuntimePoliciesDto>(
        '/api/legal/runtime-policies',
        {
          method: 'GET',
          credentials: 'include',
          cache: 'no-store',
          signal,
        },
        'Failed to load public runtime policies',
        PUBLIC_RUNTIME_POLICIES_TIMEOUT_MS,
    ),
    staleTime: PUBLIC_RUNTIME_POLICIES_STALE_TIME_MS,
  });

  return query as UseQueryResult<PublicRuntimePoliciesDto, Error>;
}
