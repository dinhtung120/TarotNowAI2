'use client';

import { useEffect } from 'react';
import { useQuery, type UseQueryResult } from '@tanstack/react-query';
import type { RuntimePoliciesDto } from '@/shared/actions/runtime-policies';
import { fetchJsonOrThrow } from '@/shared/gateways/clientFetch';
import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';
import { readRuntimePolicySessionCache, updateRuntimePolicyStore, writeRuntimePolicySessionCache } from '@/shared/config/runtimePolicyStore';

const { timeoutMs: BOOTSTRAP_TIMEOUT_MS, staleMs: BOOTSTRAP_STALE_MS } = RUNTIME_POLICY_FALLBACKS.runtimePoliciesClient;

export function useRuntimePolicies(enabled = true): UseQueryResult<RuntimePoliciesDto, Error> {
 const sessionData = readRuntimePolicySessionCache(BOOTSTRAP_STALE_MS);
 const query = useQuery({
  queryKey: userStateQueryKeys.system.runtimePolicies(),
  enabled,
  initialData: sessionData,
  queryFn: ({ signal }) => fetchJsonOrThrow<RuntimePoliciesDto>(
   '/api/me/runtime-policies',
   { method: 'GET', credentials: 'include', cache: 'no-store', signal },
   'Failed to load runtime policies',
   sessionData?.runtimePoliciesClient.timeoutMs ?? BOOTSTRAP_TIMEOUT_MS,
  ),
  staleTime: sessionData?.runtimePoliciesClient.staleMs ?? BOOTSTRAP_STALE_MS,
  refetchOnMount: false,
  refetchOnWindowFocus: false,
 });
 useEffect(() => {
  if (query.data) {
   updateRuntimePolicyStore(query.data);
   writeRuntimePolicySessionCache(query.data);
  }
 }, [query.data]);
 return query as UseQueryResult<RuntimePoliciesDto, Error>;
}
