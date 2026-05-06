export interface DepositPromotion {
 id: string;
 minAmountVnd: number;
 bonusGold: number;
 bonusDiamond?: number;
 isActive: boolean;
 createdAt: string;
 updatedAt?: string;
}
