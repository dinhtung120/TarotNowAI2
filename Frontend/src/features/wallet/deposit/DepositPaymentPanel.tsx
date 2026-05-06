'use client';

import { memo } from 'react';
import { useQRCode } from 'next-qrcode';
import type { MyDepositOrderResponse } from '@/features/wallet/deposit/actions';
import { cn } from '@/lib/utils';

interface DepositPaymentPanelProps {
 order: MyDepositOrderResponse | null;
 polling: boolean;
 formatVnd: (value: number) => string;
 statusLabel: Record<'pending' | 'success' | 'failed', string>;
 labels: {
  paymentTitle: string;
  openCheckout: string;
  qrTitle: string;
  orderTitle: string;
  orderId: string;
  gatewayCode: string;
  amountLabel: string;
  diamondLabel: string;
  bonusGoldLabel: string;
  waitingWebhook: string;
  failedReasonPrefix: string;
 };
}

export const DepositPaymentPanel = memo(function DepositPaymentPanel({
 order,
 polling,
 formatVnd,
 statusLabel,
 labels,
}: DepositPaymentPanelProps) {
 const { Canvas } = useQRCode();
 if (!order) return null;
 const statusClass = order.status === 'success'
  ? 'bg-emerald-500/15 text-emerald-300'
  : order.status === 'failed'
   ? 'bg-red-500/15 text-red-300'
   : 'bg-sky-500/15 text-sky-300';

 return (
  <section className={cn('rounded-3xl border tn-border-soft tn-panel-soft p-5 space-y-4')}>
   <div className={cn('flex items-center justify-between gap-3')}>
    <h2 className={cn('text-lg font-black tn-text-primary')}>{labels.paymentTitle}</h2>
    <span className={cn('rounded-full px-3 py-1 text-xs font-black uppercase tracking-widest', statusClass)}>
     {statusLabel[order.status]}
    </span>
   </div>
   <a href={order.checkoutUrl} target="_blank" rel="noopener noreferrer" className={cn('tn-btn-primary inline-flex rounded-xl px-4 py-2 text-sm font-black')}>
    {labels.openCheckout}
   </a>
   <div className={cn('grid grid-cols-1 gap-4 md:grid-cols-[220px_1fr]')}>
   <div>
     <p className={cn('tn-text-overline tn-text-tertiary mb-2')}>{labels.qrTitle}</p>
     <div className={cn('h-[220px] w-[220px] rounded-2xl border tn-border-soft bg-white p-2')}>
      <Canvas
       text={order.qrCode}
       options={{
        errorCorrectionLevel: 'M',
        margin: 2,
        width: 204,
        color: {
         dark: '#111827',
         light: '#ffffff',
        },
       }}
      />
     </div>
   </div>
    <div className={cn('space-y-2 text-sm')}>
     <p><strong>{labels.orderId}</strong>: {order.orderId}</p>
     <p><strong>{labels.gatewayCode}</strong>: {order.payOsOrderCode}</p>
     <p><strong>{labels.amountLabel}</strong>: {formatVnd(order.amountVnd)}</p>
     <p><strong>{labels.diamondLabel}</strong>: +{order.totalDiamondAmount.toLocaleString()}</p>
     <p><strong>{labels.bonusGoldLabel}</strong>: +{order.bonusGoldAmount.toLocaleString()}</p>
     {order.status === 'pending' ? <p className={cn('text-xs tn-text-secondary')}>{labels.waitingWebhook}{polling ? '...' : ''}</p> : null}
     {order.status === 'failed' && order.failureReason ? (
      <p className={cn('text-xs text-red-300')}>{labels.failedReasonPrefix}: {order.failureReason}</p>
     ) : null}
    </div>
   </div>
  </section>
 );
});
