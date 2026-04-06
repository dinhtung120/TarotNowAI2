// types
// DTOs matching backend

export interface QuestRewardItem {
  type: string; // 'Gold', 'Diamond', 'Title'
  amount: number;
  titleCode?: string;
}

export interface QuestDefinition {
  code: string;
  titleVi: string;
  titleEn: string;
  descriptionVi: string;
  descriptionEn: string;
  questType: string;
  triggerEvent: string;
  target: number;
  rewards: QuestRewardItem[];
  isActive: boolean;
}

export interface QuestProgress {
  questCode: string;
  periodKey: string;
  currentProgress: number;
  target: number;
  isCompleted: boolean;
  isClaimed: boolean;
}

export interface QuestWithProgress {
  definition: QuestDefinition;
  progress: QuestProgress | null;
}

export interface TitleDefinition {
  code: string;
  nameVi: string;
  nameEn: string;
  descriptionVi: string;
  descriptionEn: string;
  rarity: string;
}

export interface UserTitle {
  titleCode: string;
  grantedAt: string;
}

export interface UserTitlesData {
  definitions: TitleDefinition[];
  unlockedList: UserTitle[];
  activeTitleCode: string | null;
}

export interface AchievementDefinition {
  code: string;
  titleVi: string;
  titleEn: string;
  descriptionVi: string;
  descriptionEn: string;
  icon: string;
  grantsTitleCode: string | null;
  isHidden: boolean;
}

export interface UserAchievement {
  achievementCode: string;
  unlockedAt: string;
}

export interface UserAchievementsData {
  definitions: AchievementDefinition[];
  unlockedList: UserAchievement[];
}

export interface LeaderboardEntry {
  userId: string;
  displayName: string;
  avatar: string | null;
  activeTitle: string | null;
  score: number;
  rank: number;
}

export interface LeaderboardResult {
  entries: LeaderboardEntry[];
  userRank: LeaderboardEntry | null;
}

export interface ClaimQuestRewardResult {
  success: boolean;
  rewardType: string;
  rewardAmount: number;
  alreadyClaimed: boolean;
}
