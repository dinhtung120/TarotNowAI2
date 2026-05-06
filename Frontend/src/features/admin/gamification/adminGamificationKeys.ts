export const adminGamificationKeys = {
 all: ['admin', 'gamification'] as const,
 quests: () => [...adminGamificationKeys.all, 'quests'] as const,
 achievements: () => [...adminGamificationKeys.all, 'achievements'] as const,
 titles: () => [...adminGamificationKeys.all, 'titles'] as const,
};
