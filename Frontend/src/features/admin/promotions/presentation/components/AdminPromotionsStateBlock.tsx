'use client';

import { AlertTriangle, Loader2, RefreshCcw, Ticket } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import { Button } from '@/shared/components/ui';

interface AdminPromotionsStateBlockProps {
 loading: boolean;
 error: string;
 onRetry: () => void | Promise<void>;
}

export function AdminPromotionsStateBlock({ loading, error, onRetry }: AdminPromotionsStateBlockProps) {
 const t = useTranslations('Admin');
 const hasError = !loading && error.trim().length > 0;

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

 if (hasError) {
  return (
   <div className={cn('py-16 text-center')}>
    <div className={cn('mx-auto flex max-w-md flex-col items-center justify-center gap-4 rounded-2xl border border-red-400/40 bg-red-500/10 px-6 py-6')}>
     <AlertTriangle className={cn('h-8 w-8 text-red-300')} />
     <p className={cn('text-sm font-semibold text-red-100')}>{error}</p>
     <Button variant="secondary" onClick={() => {
      void onRetry();
     }}>
      <span className={cn('inline-flex items-center gap-2')}>
       <RefreshCcw className={cn('h-4 w-4')} />
       {t('promotions.states.retry')}
      </span>
     </Button>
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
