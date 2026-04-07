'use client';

import { Coins } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

interface AdminPromotionCreateFieldsProps {
 minAmount: number;
 bonusGold: number;
 onMinAmountChange: (value: number) => void;
 onBonusGoldChange: (value: number) => void;
}

export function AdminPromotionCreateFields({
 minAmount,
 bonusGold,
 onMinAmountChange,
 onBonusGoldChange,
}: AdminPromotionCreateFieldsProps) {
 const t = useTranslations('Admin');

 return (
  <div className={cn('tn-grid-cols-1-2-md gap-8')}>
   <div className={cn('space-y-3 text-left')}>
    <label className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary')}>
     {t('promotions.create.min_amount_label')}
    </label>
    <div className={cn('relative')}>
     <input
      type="number"
      min="0"
      required
      value={minAmount}
      onChange={(event) => onMinAmountChange(Number(event.target.value))}
      placeholder={t('promotions.create.min_amount_placeholder')}
      className={cn('w-full tn-field rounded-2xl pl-12 pr-4 py-4 text-xs font-black tn-text-primary tn-field-warning tn-placeholder shadow-inner')}
     />
     <div className={cn('absolute inset-y-0 left-0 pl-5 flex items-center pointer-events-none')}>
      <span className={cn('text-sm font-black tn-text-secondary')}>₫</span>
     </div>
    </div>
   </div>

   <div className={cn('space-y-3 text-left')}>
    <label className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary')}>
     {t('promotions.create.bonus_label')}
    </label>
    <div className={cn('relative')}>
     <input
      type="number"
      min="0"
      required
      value={bonusGold}
      onChange={(event) => onBonusGoldChange(Number(event.target.value))}
      placeholder={t('promotions.create.bonus_placeholder')}
      className={cn('w-full tn-field rounded-2xl pl-12 pr-4 py-4 text-xs font-black tn-text-primary tn-field-warning tn-placeholder shadow-inner')}
     />
     <div className={cn('absolute inset-y-0 left-0 pl-5 flex items-center pointer-events-none')}>
      <Coins className={cn('w-4 h-4 tn-text-warning')} />
     </div>
    </div>
   </div>
  </div>
 );
}
