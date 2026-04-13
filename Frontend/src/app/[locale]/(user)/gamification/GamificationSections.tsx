import dynamic from 'next/dynamic';

export const GamificationStatsBar = dynamic(() =>
  import('@/features/gamification/components/GamificationStatsBar').then((m) => m.default),
);

export const QuestsPanel = dynamic(() =>
  import('@/features/gamification/components/QuestsPanel').then((m) => m.default),
);

export const TitleSelector = dynamic(() =>
  import('@/features/gamification/components/TitleSelector').then((m) => m.default),
);

export const AchievementsGrid = dynamic(() =>
  import('@/features/gamification/components/AchievementsGrid').then((m) => m.default),
);
