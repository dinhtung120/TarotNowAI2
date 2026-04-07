import { AlertCircle, ExternalLink, Sparkles } from 'lucide-react';
import type { CreateDepositOrderResponse } from '@/features/wallet/application/actions/deposit';
import { cn } from '@/lib/utils';

interface DepositOrderStateProps {
 order: CreateDepositOrderResponse | null;
 error: string | null;
 labels: {
  orderReady: string;
  payNow: string;
 };
}

export function DepositOrderState({ order, error, labels }: DepositOrderStateProps) {
 if (error) {
  return (
   <div className={cn('p-4 rounded-2xl tn-bg-danger-soft border tn-border-danger tn-text-danger tn-text-10 font-bold uppercase tracking-widest flex items-center gap-2')}>
    <AlertCircle className={cn('w-4 h-4')} />
    {error}
   </div>
  );
 }

 if (!order) {
  return null;
 }

 return (
  <div className={cn('p-4 rounded-2xl tn-bg-success-10 border tn-border-success-20 space-y-3')}>
   <div className={cn('flex items-center gap-2 tn-text-10 font-black uppercase tracking-widest tn-text-success')}>
    <Sparkles className={cn('w-4 h-4')} />
    {labels.orderReady}
   </div>
   {order.paymentUrl ? (
    <a
     href={order.paymentUrl}
     target="_blank"
     rel="noopener noreferrer"
     className={cn('w-full inline-flex items-center justify-center gap-2 px-5 py-3 rounded-2xl tn-bg-success tn-text-ink tn-text-11 font-black uppercase tracking-widest')}
    >
     {labels.payNow}
     <ExternalLink className={cn('w-4 h-4')} />
    </a>
   ) : null}
  </div>
 );
}
