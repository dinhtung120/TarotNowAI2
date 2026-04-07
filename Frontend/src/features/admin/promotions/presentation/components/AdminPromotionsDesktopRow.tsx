'use client';

import { Coins, Trash2 } from 'lucide-react';
import { useTranslations } from 'next-intl';
import type { DepositPromotion } from '@/features/admin/application/actions/promotion';
import { cn } from '@/lib/utils';
import { AdminPromotionStatusButton } from './AdminPromotionStatusButton';

interface AdminPromotionsDesktopRowProps {
 promotion: DepositPromotion;
 locale: string;
 formatMoney: (value: number) => string;
 togglingId: string | null;
 onToggle: (promotion: DepositPromotion) => void;
 onDelete: (promotionId: string) => void;
}

export function AdminPromotionsDesktopRow({
 promotion,
 locale,
 formatMoney,
 togglingId,
 onToggle,
 onDelete,
}: AdminPromotionsDesktopRowProps) {
 const t = useTranslations('Admin');

 return (
  <tr className={cn('group/row hover:tn-surface transition-colors')}>
   <td className={cn('px-8 py-6')}>
    <div className={cn('flex items-center gap-4')}>
     <div className={cn('w-10 h-10 rounded-xl tn-panel-overlay flex items-center justify-center shadow-inner group-hover/row:scale-110 transition-transform')}>
      <span className={cn('text-[12px] font-black text-[var(--text-secondary)]')}>₫</span>
     </div>
     <div className={cn('text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm')}>
      {t('promotions.row.condition_from', { amount: formatMoney(promotion.minAmountVnd) })}
     </div>
    </div>
   </td>
   <td className={cn('px-8 py-6')}>
    <div className={cn('flex items-center gap-2 text-sm font-black text-[var(--warning)] italic drop-shadow-sm')}>
     <Coins className={cn('w-4 h-4')} />+{promotion.bonusDiamond.toLocaleString(locale)}
    </div>
   </td>
   <td className={cn('px-8 py-6 text-center')}>
    <AdminPromotionStatusButton promotion={promotion} togglingId={togglingId} onToggle={onToggle} />
   </td>
   <td className={cn('px-8 py-6 text-right')}>
    <button
     type="button"
     onClick={() => onDelete(promotion.id)}
     className={cn('p-3 min-h-11 min-w-11 rounded-xl text-[var(--text-secondary)] tn-panel-soft hover:tn-text-primary hover:bg-[var(--danger)] hover:border-transparent transition-all shadow-sm group')}
    >
     <Trash2 className={cn('w-4 h-4 group-hover:scale-110 transition-transform')} />
    </button>
   </td>
  </tr>
 );
}
