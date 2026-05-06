import type { DepositPromotion } from '@/features/admin/promotions/actions';

export interface AdminPromotionsCommonProps {
 promotions: DepositPromotion[];
 loading: boolean;
 listError: string;
 onRetry: () => void | Promise<void>;
 locale: string;
 formatMoney: (value: number) => string;
 togglingId: string | null;
 onToggle: (promotion: DepositPromotion) => void;
 onDelete: (promotionId: string) => void;
}

export interface AdminPromotionCreateFormProps {
 minAmount: number;
 bonusGold: number;
 submitting: boolean;
 onMinAmountChange: (value: number) => void;
 onBonusGoldChange: (value: number) => void;
 onSubmit: (data: { minAmount: number; bonusGold: number }) => void | Promise<void>;
}
