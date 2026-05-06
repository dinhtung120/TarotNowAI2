import type { QueryClient } from '@tanstack/react-query';
import {
 invalidateUserStateQueries,
 type UserStateInvalidationDomain,
} from '@/app/_shared/gateways/invalidateUserStateQueries';

const INVALIDATION_BATCH_DELAY_MS = 320;
const MIN_RETRY_INVALIDATION_DELAY_MS = 80;

const DOMAIN_INVALIDATION_COOLDOWN_MS: Record<UserStateInvalidationDomain, number> = {
 wallet: 1_000,
 inventory: 1_800,
 collection: 1_800,
 readingSetup: 1_200,
 readingHistory: 1_000,
 profile: 1_000,
 readerRequest: 1_000,
 notifications: 800,
 chat: 1_000,
 gacha: 1_000,
 gamification: 1_000,
};

export interface PresenceInvalidationScheduler {
 queueInvalidation: (domains: readonly UserStateInvalidationDomain[]) => void;
 dispose: () => void;
}

export function createPresenceInvalidationScheduler(
 queryClient: QueryClient,
): PresenceInvalidationScheduler {
 let invalidationBatchTimeout: NodeJS.Timeout | null = null;
 const pendingInvalidationDomains = new Set<UserStateInvalidationDomain>();
 const domainLastInvalidatedAt = new Map<UserStateInvalidationDomain, number>();

 const scheduleInvalidationFlush = (delayMs = INVALIDATION_BATCH_DELAY_MS) => {
  if (invalidationBatchTimeout) {
   return;
  }

  invalidationBatchTimeout = setTimeout(flushInvalidationBatch, delayMs);
 };

 const flushInvalidationBatch = () => {
  invalidationBatchTimeout = null;
  if (pendingInvalidationDomains.size === 0) {
   return;
  }

  const now = Date.now();
  const runnableDomains: UserStateInvalidationDomain[] = [];
  let earliestRetryDelayMs: number | null = null;

  for (const domain of [...pendingInvalidationDomains]) {
   const lastInvalidatedAt = domainLastInvalidatedAt.get(domain) ?? 0;
   const cooldownMs = DOMAIN_INVALIDATION_COOLDOWN_MS[domain];
   const elapsedMs = now - lastInvalidatedAt;
   if (elapsedMs >= cooldownMs) {
    pendingInvalidationDomains.delete(domain);
    domainLastInvalidatedAt.set(domain, now);
    runnableDomains.push(domain);
    continue;
   }

   const retryDelayMs = Math.max(MIN_RETRY_INVALIDATION_DELAY_MS, cooldownMs - elapsedMs);
   earliestRetryDelayMs = earliestRetryDelayMs === null
    ? retryDelayMs
    : Math.min(earliestRetryDelayMs, retryDelayMs);
  }

  if (runnableDomains.length > 0) {
   void invalidateUserStateQueries(queryClient, runnableDomains);
  }

  if (pendingInvalidationDomains.size > 0) {
   scheduleInvalidationFlush(earliestRetryDelayMs ?? INVALIDATION_BATCH_DELAY_MS);
  }
 };

 return {
  queueInvalidation: (domains: readonly UserStateInvalidationDomain[]) => {
   domains.forEach((domain) => pendingInvalidationDomains.add(domain));
   scheduleInvalidationFlush();
  },
  dispose: () => {
   if (invalidationBatchTimeout) {
    clearTimeout(invalidationBatchTimeout);
   }
  },
 };
}
