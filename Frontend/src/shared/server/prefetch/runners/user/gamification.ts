import type { QueryClient } from '@tanstack/react-query';
import { getRuntimePoliciesAction } from '@/shared/application/actions/runtime-policies';
import {
 fetchGamificationAchievements,
 fetchGamificationLeaderboard,
 fetchGamificationQuests,
 fetchGamificationTitles,
} from '@/features/gamification/application/gamificationServerActions';
import { gamificationKeys } from '@/features/gamification/application/gamificationQueryKeys';
import { swallowPrefetch } from '@/shared/server/prefetch/runners/user/shared';

export async function prefetchGamificationHubPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  const runtimePolicies = await getRuntimePoliciesAction();
  const defaultQuestType = runtimePolicies.success && runtimePolicies.data
   ? runtimePolicies.data.gamification.defaultQuestType
   : null;

  await Promise.all([
   ...(defaultQuestType
    ? [
      qc.prefetchQuery({
       queryKey: gamificationKeys.quests(defaultQuestType),
       queryFn: () => fetchGamificationQuests(defaultQuestType),
      }),
     ]
    : []),
   qc.prefetchQuery({
    queryKey: gamificationKeys.achievements(),
    queryFn: () => fetchGamificationAchievements(),
   }),
   qc.prefetchQuery({
    queryKey: gamificationKeys.titles(),
    queryFn: () => fetchGamificationTitles(),
   }),
  ]);
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
