export interface AdminQuestRewardItem {
 amount: number;
 titleCode?: string | null;
 type: string;
}

export interface AdminQuestDefinition {
 code: string;
 descriptionEn: string;
 descriptionVi: string;
 descriptionZh: string;
 isActive: boolean;
 questType: string;
 rewards: AdminQuestRewardItem[];
 target: number;
 titleEn: string;
 titleVi: string;
 titleZh: string;
 triggerEvent: string;
}

export interface AdminAchievementDefinition {
 code: string;
 descriptionEn: string;
 descriptionVi: string;
 descriptionZh: string;
 grantsTitleCode: string | null;
 icon: string | null;
 isActive: boolean;
 titleEn: string;
 titleVi: string;
 titleZh: string;
}

export interface AdminTitleDefinition {
 code: string;
 descriptionEn: string;
 descriptionVi: string;
 descriptionZh: string;
 isActive: boolean;
 nameEn: string;
 nameVi: string;
 nameZh: string;
 rarity: string;
}

export type AdminGamificationTab = 'quests' | 'achievements' | 'titles';
export type AdminGamificationEntityType = 'quest' | 'achievement' | 'title';
