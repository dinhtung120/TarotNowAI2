import type { WithdrawalResult } from '@/features/wallet/application/actions/withdrawal';
import { cn } from '@/lib/utils';
import { WithdrawHistoryAmountRow } from './WithdrawHistoryAmountRow';
import { WithdrawHistoryMetaRow } from './WithdrawHistoryMetaRow';

interface WithdrawHistoryItemProps {
 locale: string;
 item: WithdrawalResult;
 adminNotePrefix: string;
 getStatusBadge: (status: string) => { text: string; className: string };
}

export function WithdrawHistoryItem({
 locale,
 item,
 adminNotePrefix,
 getStatusBadge,
}: WithdrawHistoryItemProps) {
 const badge = getStatusBadge(item.status);

 return (
  <div className={cn('p-4 space-y-4 tn-hover-surface-strong transition-colors')}>
   <div className={cn('tn-withdraw-history-row')}>
    <WithdrawHistoryAmountRow
     locale={locale}
     amountDiamond={item.amountDiamond}
     netAmountVnd={item.netAmountVnd}
    />
    <span className={cn('w-fit px-2.5 py-1 rounded-full tn-text-8 font-black uppercase tracking-widest border', badge.className)}>
     {badge.text}
    </span>
   </div>

   <WithdrawHistoryMetaRow
    locale={locale}
    bankName={item.bankName}
    bankAccountNumber={item.bankAccountNumber}
    createdAt={item.createdAt}
   />

   {item.adminNote ? (
    <div className={cn('tn-text-10 tn-text-warning italic tn-withdraw-note p-3 rounded-xl border mt-3 flex gap-2 w-full')}>
     <span className={cn('font-bold opacity-70')}>{adminNotePrefix}</span>
     {item.adminNote}
    </div>
   ) : null}
  </div>
 );
}
