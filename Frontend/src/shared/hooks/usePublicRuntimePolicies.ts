'use client';

import { useQuery, type UseQueryResult } from '@tanstack/react-query';
import type { PublicRuntimePoliciesDto } from '@/shared/actions/runtime-policies';
import { fetchJsonOrThrow } from '@/shared/gateways/clientFetch';
import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';

const PUBLIC_RUNTIME_POLICIES_TIMEOUT_MS = 8_000;
const PUBLIC_RUNTIME_POLICIES_STALE_TIME_MS = 5 * 60_000;

export function usePublicRuntimePolicies(): UseQueryResult<PublicRuntimePoliciesDto, Error> {
  const query = useQuery({
    queryKey: userStateQueryKeys.system.publicRuntimePolicies(),
    queryFn: ({ signal }) =>
      fetchJsonOrThrow<PublicRuntimePoliciesDto>(
        '/api/legal/runtime-policies',
        {
          method: 'GET',
          credentials: 'include',
          cache: 'force-cache',
          signal,
        },
        'Failed to load public runtime policies',
        PUBLIC_RUNTIME_POLICIES_TIMEOUT_MS,
    ),
    staleTime: PUBLIC_RUNTIME_POLICIES_STALE_TIME_MS,
  });

  return query as UseQueryResult<PublicRuntimePoliciesDto, Error>;
}
