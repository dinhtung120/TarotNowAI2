'use client';

import { useQuery } from '@tanstack/react-query';
import type { RuntimePoliciesDto } from '@/shared/application/actions/runtime-policies';
import { fetchJsonOrThrow } from '@/shared/infrastructure/http/clientFetch';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

const RUNTIME_POLICIES_TIMEOUT_MS = 8_000;
const RUNTIME_POLICIES_STALE_TIME_MS = 15_000;

export function useRuntimePolicies() {
  return useQuery({
    queryKey: userStateQueryKeys.system.runtimePolicies(),
    queryFn: ({ signal }) =>
      fetchJsonOrThrow<RuntimePoliciesDto>(
        '/api/me/runtime-policies',
        {
          method: 'GET',
          credentials: 'include',
          cache: 'no-store',
          signal,
        },
        'Failed to load runtime policies',
        RUNTIME_POLICIES_TIMEOUT_MS,
      ),
    staleTime: RUNTIME_POLICIES_STALE_TIME_MS,
  });
}
