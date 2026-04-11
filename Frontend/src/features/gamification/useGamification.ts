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

export { gamificationKeys };

export function useQuests(type: string = 'daily') {
  return useQuery({
    queryKey: gamificationKeys.quests(type),
    queryFn: () => fetchGamificationQuests(type),
  });
}

export function useClaimQuestReward() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ questCode, periodKey }: { questCode: string; periodKey: string }) =>
      claimGamificationQuestRewardServer(questCode, periodKey),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: gamificationKeys.quests('daily') });
      queryClient.invalidateQueries({ queryKey: gamificationKeys.quests('weekly') });
      queryClient.invalidateQueries({ queryKey: ['wallet'] });
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
      queryClient.invalidateQueries({ queryKey: ['profile'] }); 
    },
  });
}

export function useLeaderboard(track: string = 'daily_rank_score', periodKey?: string) {
  return useQuery({
    queryKey: gamificationKeys.leaderboard(track, periodKey),
    queryFn: () => fetchGamificationLeaderboard(track, periodKey),
  });
}
