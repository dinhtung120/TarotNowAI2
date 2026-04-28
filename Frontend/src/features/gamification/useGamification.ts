import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import {
  claimGamificationQuestRewardServer,
  fetchGamificationAchievements,
  fetchGamificationLeaderboard,
  fetchGamificationQuests,
  fetchGamificationTitles,
  setGamificationActiveTitleServer,
} from '@/features/gamification/application/gamificationServerActions';
import { gamificationKeys } from '@/features/gamification/gamificationQueryKeys';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

export { gamificationKeys };

export function useQuests(type?: string) {
  return useQuery({
    queryKey: gamificationKeys.quests(type ?? 'none'),
    queryFn: () => fetchGamificationQuests(type!),
    enabled: Boolean(type),
  });
}

export function useClaimQuestReward() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ questCode, periodKey }: { questCode: string; periodKey: string }) =>
      claimGamificationQuestRewardServer(questCode, periodKey),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [...gamificationKeys.all, 'quests'] });
      queryClient.invalidateQueries({ queryKey: userStateQueryKeys.wallet.all });
    },
  });
}

export function useAchievements() {
  return useQuery({
    queryKey: gamificationKeys.achievements(),
    queryFn: () => fetchGamificationAchievements(),
  });
}

export function useTitles() {
  return useQuery({
    queryKey: gamificationKeys.titles(),
    queryFn: () => fetchGamificationTitles(),
  });
}

export function useSetActiveTitle() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (titleCode: string) => setGamificationActiveTitleServer(titleCode),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: gamificationKeys.titles() });
      queryClient.invalidateQueries({ queryKey: userStateQueryKeys.profile.detail() });
    },
  });
}

export function useLeaderboard(track?: string, periodKey?: string) {
  return useQuery({
    queryKey: gamificationKeys.leaderboard(track ?? 'none', periodKey),
    queryFn: () => fetchGamificationLeaderboard(track!, periodKey),
    enabled: Boolean(track),
  });
}
