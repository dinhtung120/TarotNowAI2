import { memo, useMemo } from 'react';
import type { MyDepositOrderHistoryItemResponse } from '@/features/wallet/application/actions/deposit';
import { cn } from '@/lib/utils';

interface DepositHistoryRowProps {
 locale: string;
 item: MyDepositOrderHistoryItemResponse;
 formatVnd: (value: number) => string;
 labels: {
  amount: string;
  diamond: string;
  bonusGold: string;
  transaction: string;
  failureReason: string;
  reconcile: string;
 };
 statusText: {
  pending: string;
  success: string;
  failed: string;
 };
 onReconcile: (orderId: string) => void;
}

function DepositHistoryRowComponent({
 locale,
 item,
 formatVnd,
 labels,
 statusText,
 onReconcile,
}: DepositHistoryRowProps) {
 const createdAtText = useMemo(() => {
  return new Intl.DateTimeFormat(locale, {
   hour12: false,
   year: 'numeric',
   month: '2-digit',
   day: '2-digit',
   hour: '2-digit',
   minute: '2-digit',
  }).format(new Date(item.createdAt));
 }, [item.createdAt, locale]);

 const statusBadge = useMemo(() => {
  if (item.status === 'success') {
   return { text: statusText.success, className: 'border-emerald-400/50 bg-emerald-500/15 text-emerald-100' };
  }
  if (item.status === 'failed') {
   return { text: statusText.failed, className: 'border-rose-400/50 bg-rose-500/15 text-rose-100' };
  }
  return { text: statusText.pending, className: 'border-amber-400/50 bg-amber-500/15 text-amber-100' };
 }, [item.status, statusText.failed, statusText.pending, statusText.success]);

 return (
  <article className={cn('rounded-2xl border tn-border-soft tn-surface p-4')}>
   <div className={cn('flex flex-wrap items-center justify-between gap-2')}>
    <p className={cn('text-xs font-black uppercase tracking-[0.16em] tn-text-secondary')}>
     #{item.orderId.slice(0, 8)} · {createdAtText}
    </p>
    <span className={cn('rounded-full border px-3 py-1 text-[11px] font-black uppercase tracking-wider', statusBadge.className)}>
     {statusBadge.text}
    </span>
   </div>
   <div className={cn('mt-3 grid grid-cols-1 gap-2 text-sm tn-text-primary sm:grid-cols-3')}>
    <p><span className={cn('tn-text-secondary')}>{labels.amount}:</span> {formatVnd(item.amountVnd)}</p>
    <p><span className={cn('tn-text-secondary')}>{labels.diamond}:</span> +{item.totalDiamondAmount}</p>
    <p><span className={cn('tn-text-secondary')}>{labels.bonusGold}:</span> +{item.bonusGoldAmount}</p>
   </div>
   <div className={cn('mt-3 flex flex-wrap items-center gap-3 text-xs tn-text-secondary')}>
    <span>{labels.transaction}: {item.transactionId ?? '--'}</span>
    {item.failureReason ? <span>{labels.failureReason}: {item.failureReason}</span> : null}
    {item.status === 'pending' ? (
     <button
      type="button"
      onClick={() => onReconcile(item.orderId)}
      className={cn('min-h-11 rounded-xl border border-amber-400/50 bg-amber-500/10 px-3 py-1 text-[11px] font-black uppercase tracking-wider text-amber-100 hover:bg-amber-500/20')}
     >
      {labels.reconcile}
     </button>
    ) : null}
   </div>
  </article>
 );
}

export const DepositHistoryRow = memo(DepositHistoryRowComponent);
DepositHistoryRow.displayName = 'DepositHistoryRow';
