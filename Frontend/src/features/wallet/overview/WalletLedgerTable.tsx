import type { WalletPaginatedList, WalletTransaction } from '@/features/wallet/shared/types';
import { GlassCard, Pagination } from '@/shared/ui';
import { cn } from '@/lib/utils';
import { WalletLedgerTableBody } from './WalletLedgerTableBody';

interface WalletLedgerTableProps {
 locale: string;
 ledger: WalletPaginatedList<WalletTransaction> | null | undefined;
 isLoading: boolean;
 formatType: (type: string) => string;
 onPageChange: (page: number) => void;
 labels: {
  tableTime: string;
  tableAsset: string;
  tableAction: string;
  tableAmount: string;
  ledgerLoading: string;
  ledgerEmpty: string;
  balanceAfter: (amount: string) => string;
 };
}

export function WalletLedgerTable({
 locale,
 ledger,
 isLoading,
 formatType,
 onPageChange,
 labels,
}: WalletLedgerTableProps) {
 return (
  <GlassCard className={cn('!p-0 overflow-hidden')}>
   <div className={cn('overflow-x-auto custom-scrollbar')}>
    <table className={cn('w-full text-left tn-minw-600')}>
     <thead>
      <tr className={cn('border-b tn-border-soft tn-overlay-soft')}>
       <th className={cn('px-6 py-5 tn-text-overline tn-text-tertiary')}>{labels.tableTime}</th>
       <th className={cn('px-6 py-5 tn-text-overline tn-text-tertiary')}>{labels.tableAsset}</th>
       <th className={cn('px-6 py-5 tn-text-overline tn-text-tertiary')}>{labels.tableAction}</th>
       <th className={cn('px-6 py-5 tn-text-overline tn-text-tertiary text-right')}>{labels.tableAmount}</th>
      </tr>
     </thead>
     <WalletLedgerTableBody
      locale={locale}
      isLoading={isLoading}
      items={ledger?.items ?? []}
      formatType={formatType}
      labels={{
       loading: labels.ledgerLoading,
       empty: labels.ledgerEmpty,
       balanceAfter: labels.balanceAfter,
      }}
     />
    </table>
   </div>
   {ledger ? (
    <Pagination
     currentPage={ledger.pageIndex}
     totalPages={ledger.totalPages}
     onPageChange={onPageChange}
     isLoading={isLoading}
     className={cn('tn-overlay-soft')}
    />
   ) : null}
  </GlassCard>
 );
}
