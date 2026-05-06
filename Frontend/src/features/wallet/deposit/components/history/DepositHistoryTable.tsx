import { memo } from 'react';
import type { MyDepositOrderHistoryItemResponse } from '@/features/wallet/deposit/actions';
import { cn } from '@/lib/utils';
import { DepositHistoryRow } from './DepositHistoryRow';

interface DepositHistoryTableProps {
 locale: string;
 items: MyDepositOrderHistoryItemResponse[];
 isLoading: boolean;
 error: string | null;
 emptyLabel: string;
 loadingLabel: string;
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

function DepositHistoryTableComponent({
 locale,
 items,
 isLoading,
 error,
 emptyLabel,
 loadingLabel,
 formatVnd,
 labels,
 statusText,
 onReconcile,
}: DepositHistoryTableProps) {
 if (isLoading) {
  return (
   <div className={cn('rounded-2xl border tn-border-soft tn-surface p-6 text-sm tn-text-secondary')}>
    {loadingLabel}
   </div>
  );
 }

 if (error) {
  return (
   <div className={cn('rounded-2xl border border-red-400/50 bg-red-500/10 p-6 text-sm text-red-100')}>
    {error}
   </div>
  );
 }

 if (items.length === 0) {
  return (
   <div className={cn('rounded-2xl border tn-border-soft tn-surface p-6 text-sm tn-text-secondary')}>
    {emptyLabel}
   </div>
  );
 }

 return (
  <div className={cn('space-y-3')}>
   {items.map((item) => (
    <DepositHistoryRow
     key={item.orderId}
     locale={locale}
     item={item}
     formatVnd={formatVnd}
     labels={labels}
     statusText={statusText}
     onReconcile={onReconcile}
    />
   ))}
  </div>
 );
}

export const DepositHistoryTable = memo(DepositHistoryTableComponent);
DepositHistoryTable.displayName = 'DepositHistoryTable';
