'use client';

import { Sparkles } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import type { AdminPromotionCreateFormProps } from './types';
import { AdminPromotionCreateFields } from './AdminPromotionCreateFields';
import { AdminPromotionCreateFooter } from './AdminPromotionCreateFooter';

export function AdminPromotionCreateForm({
 minAmount,
 bonusGold,
 submitting,
 onMinAmountChange,
 onBonusGoldChange,
 onSubmit,
}: AdminPromotionCreateFormProps) {
 const t = useTranslations('Admin');

 return (
  <div className={cn('animate-in fade-in slide-in-from-top-4 duration-500')}>
   <GlassCard className={cn('relative overflow-hidden group hover:border-[var(--warning)]/30 transition-all !p-8')}>
    <div className={cn('absolute top-0 right-0 p-16 bg-[var(--warning)]/10 blur-[100px] rounded-full')} />
    <form onSubmit={onSubmit} className={cn('relative z-10 space-y-8')}>
     <div className={cn('flex items-center gap-3 border-b tn-border-soft pb-4')}>
      <Sparkles className={cn('w-5 h-5 text-[var(--warning)]')} />
      <h2 className={cn('text-sm font-black tn-text-primary uppercase tracking-widest drop-shadow-sm')}>
       {t('promotions.create.title')}
      </h2>
     </div>
     <AdminPromotionCreateFields
      minAmount={minAmount}
      bonusGold={bonusGold}
      onMinAmountChange={onMinAmountChange}
      onBonusGoldChange={onBonusGoldChange}
     />
     <AdminPromotionCreateFooter submitting={submitting} />
    </form>
   </GlassCard>
  </div>
 );
}
