import type { FormEvent } from 'react';
import type { DepositPromotion } from '@/features/admin/application/actions/promotion';

export interface AdminPromotionsCommonProps {
 promotions: DepositPromotion[];
 loading: boolean;
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
 onSubmit: (event: FormEvent<HTMLFormElement>) => void;
}
