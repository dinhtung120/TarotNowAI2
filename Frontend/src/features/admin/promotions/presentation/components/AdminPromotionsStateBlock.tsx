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
     <Loader2 className={cn('w-8 h-8 animate-spin text-[var(--warning)]')} />
     <span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]')}>
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
     <Ticket className={cn('w-8 h-8 text-[var(--text-tertiary)] opacity-50')} />
    </div>
    <span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]')}>
     {t('promotions.states.empty')}
    </span>
   </div>
  </div>
 );
}
