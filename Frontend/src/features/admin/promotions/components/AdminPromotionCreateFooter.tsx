'use client';

import { CheckCircle2, Loader2 } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { Button } from '@/shared/ui';
import { cn } from '@/lib/utils';

interface AdminPromotionCreateFooterProps {
 submitting: boolean;
}

export function AdminPromotionCreateFooter({ submitting }: AdminPromotionCreateFooterProps) {
 const t = useTranslations('Admin');

 return (
  <div className={cn('tn-flex-col-row-sm justify-end items-center gap-4 pt-2 border-t tn-border-soft')}>
   <p className={cn('tn-text-9 font-bold tn-text-tertiary uppercase tracking-tighter tn-text-center-right-sm leading-relaxed flex-1')}>
    {t('promotions.create.note')}
   </p>
   <Button
    type="submit"
    disabled={submitting}
    className={cn('tn-w-full-auto-sm px-8 py-4 tn-btn-warning-solid')}
   >
    {submitting ? <Loader2 className={cn('w-4 h-4 animate-spin mr-2')} /> : <CheckCircle2 className={cn('w-4 h-4 mr-2')} />}
    {submitting ? t('promotions.create.submitting') : t('promotions.create.submit')}
   </Button>
  </div>
 );
}
