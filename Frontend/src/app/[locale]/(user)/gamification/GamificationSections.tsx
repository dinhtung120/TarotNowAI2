import dynamic from 'next/dynamic';

export const GamificationStatsBar = dynamic(() =>
  import('@/features/gamification/hub/GamificationStatsBar').then((m) => m.default),
);

export const QuestsPanel = dynamic(() =>
  import('@/features/gamification/quests/QuestsPanel').then((m) => m.default),
);

export const TitleSelector = dynamic(() =>
  import('@/features/gamification/titles/TitleSelector').then((m) => m.default),
);

export const AchievementsGrid = dynamic(() =>
  import('@/features/gamification/achievements/AchievementsGrid').then((m) => m.default),
);
