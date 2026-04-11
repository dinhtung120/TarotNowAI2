export const gamificationKeys = {
 all: ['gamification'] as const,
 quests: (type: string) => [...gamificationKeys.all, 'quests', type] as const,
 achievements: () => [...gamificationKeys.all, 'achievements'] as const,
 titles: () => [...gamificationKeys.all, 'titles'] as const,
 leaderboard: (track: string, periodKey?: string) =>
  [...gamificationKeys.all, 'leaderboard', track, periodKey ?? null] as const,
};
