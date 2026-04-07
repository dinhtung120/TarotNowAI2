

export interface GachaBannerItemDto {
  rarity: string;
  rewardType: string;
  rewardValue: string;
  weightBasisPoints: number;
  displayNameVi: string;
  displayNameEn: string;
  displayIcon: string | null;
  probabilityPercent: number;
}

export interface GachaBannerDto {
  code: string;
  nameVi: string;
  nameEn: string;
  descriptionVi: string | null;
  descriptionEn: string | null;
  costDiamond: number;
  oddsVersion: string;
  userCurrentPity: number;
}

export interface GachaBannerOddsDto {
  bannerCode: string;
  oddsVersion: string;
  items: GachaBannerItemDto[];
}

export interface GachaHistoryItemDto {
  bannerCode: string;
  rarity: string;
  rewardType: string;
  rewardValue: string;
  spentDiamond: number;
  wasPityTriggered: boolean;
  createdAt: string;
}

export interface SpinGachaItemResult {
  rarity: string;
  rewardType: string;
  rewardValue: string;
  displayNameVi: string;
  displayNameEn: string;
  displayIcon: string | null;
}

export interface SpinGachaResult {
  success: boolean;
  isIdempotentReplay: boolean;
  currentPityCount: number;
  hardPityThreshold: number;
  wasPityTriggered: boolean;
  items: SpinGachaItemResult[];
}

export interface SpinGachaRequestDto {
  bannerCode: string;
  count: number;
}
