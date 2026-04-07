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
  <div className={cn('p-4 sm:p-6 space-y-4 hover:tn-surface transition-colors')}>
   <div className={cn('flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3')}>
    <WithdrawHistoryAmountRow
     locale={locale}
     amountDiamond={item.amountDiamond}
     netAmountVnd={item.netAmountVnd}
    />
    <span className={cn('w-fit px-2.5 py-1 rounded-full text-[8px] font-black uppercase tracking-widest border', badge.className)}>
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
    <div className={cn('text-[10px] text-[var(--warning)] italic bg-[var(--warning)]/5 p-3 rounded-xl border border-[var(--warning)]/10 mt-3 flex gap-2 w-full')}>
     <span className={cn('font-bold opacity-70')}>{adminNotePrefix}</span>
     {item.adminNote}
    </div>
   ) : null}
  </div>
 );
}
