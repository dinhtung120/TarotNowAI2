'use client';

import { Loader2, Ticket } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

interface AdminPromotionsStateBlockProps {
 loading: boolean;
}

export function AdminPromotionsStateBlock({ loading }: AdminPromotionsStateBlockProps) {
 const t = useTranslations('Admin');

 if (loading) {
  return (
   <div className={cn('py-16 text-center')}>
    <div className={cn('flex flex-col items-center justify-center space-y-4')}>
     <Loader2 className={cn('w-8 h-8 animate-spin tn-text-warning')} />
     <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary')}>
      {t('promotions.states.loading')}
     </span>
    </div>
   </div>
  );
 }

 return (
  <div className={cn('py-16 text-center')}>
   <div className={cn('flex flex-col items-center justify-center space-y-4')}>
    <div className={cn('w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center')}>
     <Ticket className={cn('w-8 h-8 tn-text-tertiary opacity-50')} />
    </div>
    <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-tertiary')}>
     {t('promotions.states.empty')}
    </span>
   </div>
  </div>
 );
}
