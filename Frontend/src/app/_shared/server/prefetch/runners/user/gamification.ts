import type { QueryClient } from '@tanstack/react-query';
import {
 fetchGamificationLeaderboard,
 fetchGamificationQuests,
} from '@/features/gamification/shared/gamificationServerActions';
import { gamificationKeys } from '@/features/gamification/shared/gamificationQueryKeys';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';
import { swallowPrefetch } from '@/app/_shared/server/prefetch/runners/user/shared';

export async function prefetchGamificationHubPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  const defaultQuestType = RUNTIME_POLICY_FALLBACKS.gamification.defaultQuestType;
  await qc.prefetchQuery({
   queryKey: gamificationKeys.quests(defaultQuestType),
   queryFn: () => fetchGamificationQuests(defaultQuestType),
  });
 });
}

export async function prefetchLeaderboardPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  const defaultTrack = RUNTIME_POLICY_FALLBACKS.gamification.defaultLeaderboardTrack;
  await qc.prefetchQuery({
   queryKey: gamificationKeys.leaderboard(defaultTrack),
   queryFn: () => fetchGamificationLeaderboard(defaultTrack),
  });
 });
}
