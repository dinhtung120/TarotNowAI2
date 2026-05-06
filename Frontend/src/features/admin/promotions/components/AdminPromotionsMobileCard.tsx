'use client';

import { Coins, Trash2 } from 'lucide-react';
import type { DepositPromotion } from '@/features/admin/promotions/actions';
import { cn } from '@/lib/utils';
import { AdminPromotionStatusButton } from './AdminPromotionStatusButton';

interface AdminPromotionsMobileCardProps {
 promotion: DepositPromotion;
 locale: string;
 formatMoney: (value: number) => string;
 togglingId: string | null;
 onToggle: (promotion: DepositPromotion) => void;
 onDelete: (promotionId: string) => void;
 conditionLabel: string;
 rewardLabel: string;
 deleteLabel: string;
}

export function AdminPromotionsMobileCard({
 promotion,
 locale,
 formatMoney,
 togglingId,
 onToggle,
 onDelete,
 conditionLabel,
 rewardLabel,
 deleteLabel,
}: AdminPromotionsMobileCardProps) {
 return (
  <div className={cn('rounded-2xl tn-panel-soft border tn-border-soft p-4 space-y-4')}>
   <div className={cn('flex items-center justify-between gap-3')}>
    <div className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-tertiary')}>{conditionLabel}</div>
    <div className={cn('tn-text-11 font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm text-right')}>
     {formatMoney(promotion.minAmountVnd)}
    </div>
   </div>
   <div className={cn('flex items-center justify-between gap-3')}>
    <div className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-tertiary')}>{rewardLabel}</div>
    <div className={cn('flex items-center gap-2 text-sm font-black tn-text-warning italic drop-shadow-sm')}>
     <Coins className={cn('w-4 h-4')} />+{promotion.bonusGold.toLocaleString(locale)}
    </div>
   </div>
   <div className={cn('flex items-center gap-2')}>
    <AdminPromotionStatusButton promotion={promotion} togglingId={togglingId} onToggle={onToggle} fullWidth />
    <button
     type="button"
     onClick={() => onDelete(promotion.id)}
     className={cn('tn-admin-promo-delete-btn p-3 min-h-11 min-w-11 rounded-xl tn-text-secondary tn-panel-soft transition-all shadow-sm group')}
     aria-label={deleteLabel}
    >
     <Trash2 className={cn('tn-group-scale-110 w-4 h-4 transition-transform')} />
    </button>
   </div>
  </div>
 );
}
