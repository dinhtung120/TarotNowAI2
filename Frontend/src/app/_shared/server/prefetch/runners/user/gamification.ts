import type { QueryClient } from '@tanstack/react-query';
import { getRuntimePoliciesAction } from '@/shared/actions/runtime-policies';
import {
 fetchGamificationLeaderboard,
 fetchGamificationQuests,
} from '@/features/gamification/shared/gamificationServerActions';
import { gamificationKeys } from '@/features/gamification/shared/gamificationQueryKeys';
import { swallowPrefetch } from '@/app/_shared/server/prefetch/runners/user/shared';

export async function prefetchGamificationHubPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  const runtimePolicies = await getRuntimePoliciesAction();
  const defaultQuestType = runtimePolicies.success && runtimePolicies.data
   ? runtimePolicies.data.gamification.defaultQuestType
   : null;

  if (!defaultQuestType) {
   return;
  }

  await qc.prefetchQuery({
   queryKey: gamificationKeys.quests(defaultQuestType),
   queryFn: () => fetchGamificationQuests(defaultQuestType),
  });
 });
}

export async function prefetchLeaderboardPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  const runtimePolicies = await getRuntimePoliciesAction();
  if (!runtimePolicies.success || !runtimePolicies.data) {
   return;
  }

  const defaultTrack = runtimePolicies.data.gamification.defaultLeaderboardTrack;
  await qc.prefetchQuery({
   queryKey: gamificationKeys.leaderboard(defaultTrack),
   queryFn: () => fetchGamificationLeaderboard(defaultTrack),
  });
 });
}
