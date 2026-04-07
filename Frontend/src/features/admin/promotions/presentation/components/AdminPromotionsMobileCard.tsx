'use client';

import { Coins, Trash2 } from 'lucide-react';
import type { DepositPromotion } from '@/features/admin/application/actions/promotion';
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
    <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]')}>{conditionLabel}</div>
    <div className={cn('text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm text-right')}>
     {formatMoney(promotion.minAmountVnd)}
    </div>
   </div>
   <div className={cn('flex items-center justify-between gap-3')}>
    <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]')}>{rewardLabel}</div>
    <div className={cn('flex items-center gap-2 text-sm font-black text-[var(--warning)] italic drop-shadow-sm')}>
     <Coins className={cn('w-4 h-4')} />+{promotion.bonusDiamond.toLocaleString(locale)}
    </div>
   </div>
   <div className={cn('flex items-center gap-2')}>
    <AdminPromotionStatusButton promotion={promotion} togglingId={togglingId} onToggle={onToggle} fullWidth />
    <button
     type="button"
     onClick={() => onDelete(promotion.id)}
     className={cn('p-3 min-h-11 min-w-11 rounded-xl text-[var(--text-secondary)] tn-panel-soft hover:tn-text-primary hover:bg-[var(--danger)] hover:border-transparent transition-all shadow-sm group')}
     aria-label={deleteLabel}
    >
     <Trash2 className={cn('w-4 h-4 group-hover:scale-110 transition-transform')} />
    </button>
   </div>
  </div>
 );
}
