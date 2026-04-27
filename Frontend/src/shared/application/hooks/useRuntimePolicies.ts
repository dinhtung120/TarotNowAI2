'use client';

import { useEffect } from 'react';
import { useQuery, useQueryClient, type UseQueryResult } from '@tanstack/react-query';
import type { RuntimePoliciesDto } from '@/shared/application/actions/runtime-policies';
import { fetchJsonOrThrow } from '@/shared/application/gateways/clientFetch';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';
import { updateRuntimePolicyStore } from '@/shared/config/runtimePolicyStore';

const RUNTIME_POLICIES_TIMEOUT_BOOTSTRAP_MS = RUNTIME_POLICY_FALLBACKS.runtimePoliciesClient.timeoutMs;
const RUNTIME_POLICIES_STALE_BOOTSTRAP_MS = RUNTIME_POLICY_FALLBACKS.runtimePoliciesClient.staleMs;

export function useRuntimePolicies(): UseQueryResult<RuntimePoliciesDto, Error> {
  const queryClient = useQueryClient();
  const cached = queryClient.getQueryData<RuntimePoliciesDto>(userStateQueryKeys.system.runtimePolicies());
  const timeoutMs = cached?.runtimePoliciesClient.timeoutMs ?? RUNTIME_POLICIES_TIMEOUT_BOOTSTRAP_MS;
  const staleMs = cached?.runtimePoliciesClient.staleMs ?? RUNTIME_POLICIES_STALE_BOOTSTRAP_MS;

  const query = useQuery({
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
        timeoutMs,
      ),
    staleTime: staleMs,
  });

  useEffect(() => {
    if (query.data) {
      updateRuntimePolicyStore(query.data as RuntimePoliciesDto);
    }
  }, [query.data]);

  return query as UseQueryResult<RuntimePoliciesDto, Error>;
}
