import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { gamificationApi } from './gamification.api';

export const gamificationKeys = {
  all: ['gamification'] as const,
  quests: (type: string) => [...gamificationKeys.all, 'quests', type] as const,
  achievements: () => [...gamificationKeys.all, 'achievements'] as const,
  titles: () => [...gamificationKeys.all, 'titles'] as const,
  leaderboard: (track: string, periodKey?: string) => [...gamificationKeys.all, 'leaderboard', track, periodKey] as const,
};

export function useQuests(type: string = 'daily') {
  return useQuery({
    queryKey: gamificationKeys.quests(type),
    queryFn: () => gamificationApi.getQuests(type),
  });
}

export function useClaimQuestReward() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ questCode, periodKey }: { questCode: string; periodKey: string }) =>
      gamificationApi.claimQuestReward(questCode, periodKey),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: gamificationKeys.quests('daily') });
      queryClient.invalidateQueries({ queryKey: ['wallet'] });
    },
  });
}

export function useAchievements() {
  return useQuery({
    queryKey: gamificationKeys.achievements(),
    queryFn: () => gamificationApi.getAchievements(),
  });
}

export function useTitles() {
  return useQuery({
    queryKey: gamificationKeys.titles(),
    queryFn: () => gamificationApi.getTitles(),
  });
}

export function useSetActiveTitle() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (titleCode: string) => gamificationApi.setActiveTitle(titleCode),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: gamificationKeys.titles() });
      queryClient.invalidateQueries({ queryKey: ['profile'] }); 
    },
  });
}

export function useGrantSandboxTitles() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: () => gamificationApi.grantSandboxTitles(),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: gamificationKeys.titles() });
    },
  });
}

export function useLeaderboard(track: string = 'daily_rank_score', periodKey?: string) {
  return useQuery({
    queryKey: gamificationKeys.leaderboard(track, periodKey),
    queryFn: () => gamificationApi.getLeaderboard(track, periodKey),
  });
}
