'use client';

import { memo } from 'react';
import type { WithdrawalResult } from '@/features/wallet/public';
import { GlassCard } from '@/shared/ui';
import { cn } from '@/lib/utils';
import { AdminWithdrawalActions } from './AdminWithdrawalActions';
import { AdminWithdrawalAmountInfo } from './AdminWithdrawalAmountInfo';
import { AdminWithdrawalBankInfo } from './AdminWithdrawalBankInfo';
import { AdminWithdrawalMetaInfo } from './AdminWithdrawalMetaInfo';

interface AdminWithdrawalCardProps {
 approveLabel: string;
 feeLabel: string;
 formatVnd: (value: number | null | undefined) => string;
 grossLabel: string;
 idLabel: string;
 item: WithdrawalResult;
 locale: string;
 netLabel: string;
 notePlaceholder: string;
 onApprove: () => void;
 onViewDetail: () => void;
 onChangeNote: (value: string) => void;
 onReject: () => void;
 processing: boolean;
 rejectLabel: string;
 detailLabel: string;
 value: string;
}

function AdminWithdrawalCardComponent(props: AdminWithdrawalCardProps) {
 return (
  <GlassCard className={cn('space-y-6 group tn-hover-border transition-all')}>
   <div className={cn('flex flex-col tn-md-flex-row tn-md-items-center justify-between gap-4')}>
    <AdminWithdrawalAmountInfo amountDiamond={props.item.amountDiamond} grossLabel={props.grossLabel} grossVnd={props.formatVnd(props.item.amountVnd)} feeLabel={props.feeLabel} feeVnd={props.formatVnd(props.item.feeVnd)} netLabel={props.netLabel} netVnd={props.formatVnd(props.item.netAmountVnd)} />
    <AdminWithdrawalMetaInfo createdAt={props.item.createdAt} idLabel={props.idLabel} locale={props.locale} />
   </div>
   <AdminWithdrawalBankInfo bankName={props.item.bankName} bankAccountName={props.item.bankAccountName} bankAccountNumber={props.item.bankAccountNumber} />
   <div className={cn('flex justify-end')}>
    <button type="button" onClick={props.onViewDetail} className={cn('rounded-xl px-3 py-2 text-sm font-semibold tn-surface tn-text-primary')}>
     {props.detailLabel}
    </button>
   </div>
   <AdminWithdrawalActions
    approveLabel={props.approveLabel}
    disabled={props.processing}
    notePlaceholder={props.notePlaceholder}
    onApprove={props.onApprove}
    onChangeNote={props.onChangeNote}
    onReject={props.onReject}
    rejectLabel={props.rejectLabel}
    value={props.value}
   />
  </GlassCard>
 );
}

export const AdminWithdrawalCard = memo(AdminWithdrawalCardComponent);
