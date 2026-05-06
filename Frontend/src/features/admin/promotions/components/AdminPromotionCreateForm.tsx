'use client';

import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { Sparkles } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { useForm, useWatch } from 'react-hook-form';
import { z } from 'zod';
import { GlassCard } from '@/shared/ui';
import { cn } from '@/lib/utils';
import type { AdminPromotionCreateFormProps } from './types';
import { AdminPromotionCreateFields } from './AdminPromotionCreateFields';
import { AdminPromotionCreateFooter } from './AdminPromotionCreateFooter';

const adminPromotionCreateFormSchema = z.object({
 minAmount: z.number().min(0),
 bonusGold: z.number().min(0),
});

type AdminPromotionCreateFormValues = z.infer<typeof adminPromotionCreateFormSchema>;

export function AdminPromotionCreateForm({
 minAmount,
 bonusGold,
 submitting,
 onMinAmountChange,
 onBonusGoldChange,
 onSubmit,
}: AdminPromotionCreateFormProps) {
 const t = useTranslations('Admin');
 const { handleSubmit, control, setValue } = useForm<AdminPromotionCreateFormValues>({
  resolver: zodResolver(adminPromotionCreateFormSchema),
  defaultValues: {
   minAmount,
   bonusGold,
  },
 });

 const watchedMinAmount = useWatch({ control, name: 'minAmount' }) ?? 0;
 const watchedBonusGold = useWatch({ control, name: 'bonusGold' }) ?? 0;

 useEffect(() => {
  setValue('minAmount', minAmount, { shouldDirty: false, shouldValidate: false });
 }, [minAmount, setValue]);

 useEffect(() => {
  setValue('bonusGold', bonusGold, { shouldDirty: false, shouldValidate: false });
 }, [bonusGold, setValue]);

 useEffect(() => {
  onMinAmountChange(watchedMinAmount);
 }, [onMinAmountChange, watchedMinAmount]);

 useEffect(() => {
  onBonusGoldChange(watchedBonusGold);
 }, [onBonusGoldChange, watchedBonusGold]);

 const submitWithValidation = handleSubmit(async (values) => {
  await onSubmit({
   minAmount: values.minAmount,
   bonusGold: values.bonusGold,
  });
 });

 return (
  <div className={cn('animate-in fade-in slide-in-from-top-4 duration-500')}>
   <GlassCard className={cn('relative overflow-hidden group tn-hover-border-warning-30 transition-all !p-8')}>
    <div className={cn('absolute top-0 right-0 p-16 tn-warning-glow-orb rounded-full')} />
    <form onSubmit={submitWithValidation} className={cn('relative z-10 space-y-8')}>
     <div className={cn('flex items-center gap-3 border-b tn-border-soft pb-4')}>
      <Sparkles className={cn('w-5 h-5 tn-text-warning')} />
      <h2 className={cn('text-sm font-black tn-text-primary uppercase tracking-widest drop-shadow-sm')}>
       {t('promotions.create.title')}
      </h2>
     </div>
     <AdminPromotionCreateFields
      minAmount={watchedMinAmount}
      bonusGold={watchedBonusGold}
      onMinAmountChange={(value) =>
       setValue('minAmount', value, { shouldDirty: true, shouldValidate: true })
      }
      onBonusGoldChange={(value) =>
       setValue('bonusGold', value, { shouldDirty: true, shouldValidate: true })
      }
     />
     <AdminPromotionCreateFooter submitting={submitting} />
    </form>
   </GlassCard>
  </div>
 );
}
