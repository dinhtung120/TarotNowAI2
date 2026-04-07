import { Loader2, Wallet } from 'lucide-react';
import type { WalletTransaction } from '@/features/wallet/domain/types';
import { TableStates } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import { WalletLedgerRow } from './WalletLedgerRow';

interface WalletLedgerTableBodyProps {
 locale: string;
 isLoading: boolean;
 items: WalletTransaction[];
 formatType: (type: string) => string;
 labels: {
  loading: string;
  empty: string;
  balanceAfter: (amount: string) => string;
 };
}

export function WalletLedgerTableBody({
 locale,
 isLoading,
 items,
 formatType,
 labels,
}: WalletLedgerTableBodyProps) {
 return (
  <tbody className={cn('divide-y divide-white/5')}>
   <TableStates
    colSpan={4}
    isLoading={isLoading}
    isEmpty={!isLoading && items.length === 0}
    loadingLabel={labels.loading}
    emptyLabel={labels.empty}
    loadingIcon={<Loader2 className={cn('w-8 h-8 animate-spin text-[var(--purple-accent)]')} />}
    emptyIcon={<Wallet className={cn('w-12 h-12 tn-text-muted')} />}
   />
   {!isLoading
    ? items.map((tx) => (
       <WalletLedgerRow
        key={tx.id}
        tx={tx}
        locale={locale}
        formattedType={formatType(tx.type)}
        actionLabel={labels.balanceAfter(tx.balanceAfter.toLocaleString(locale))}
       />
      ))
    : null}
  </tbody>
 );
}
