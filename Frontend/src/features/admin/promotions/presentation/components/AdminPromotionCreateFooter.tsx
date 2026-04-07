'use client';

import { CheckCircle2, Loader2 } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { Button } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface AdminPromotionCreateFooterProps {
 submitting: boolean;
}

export function AdminPromotionCreateFooter({ submitting }: AdminPromotionCreateFooterProps) {
 const t = useTranslations('Admin');

 return (
  <div className={cn('flex flex-col sm:flex-row justify-end items-center gap-4 pt-2 border-t tn-border-soft')}>
   <p className={cn('text-[9px] font-bold text-[var(--text-tertiary)] uppercase tracking-tighter text-center sm:text-right leading-relaxed flex-1')}>
    {t('promotions.create.note')}
   </p>
   <Button
    type="submit"
    disabled={submitting}
    className={cn('w-full sm:w-auto px-8 py-4 bg-[var(--warning)] tn-text-ink hover:bg-[var(--warning)] hover:brightness-110 shadow-[0_0_20px_var(--c-245-158-11-20)]')}
   >
    {submitting ? <Loader2 className={cn('w-4 h-4 animate-spin mr-2')} /> : <CheckCircle2 className={cn('w-4 h-4 mr-2')} />}
    {submitting ? t('promotions.create.submitting') : t('promotions.create.submit')}
   </Button>
  </div>
 );
}
