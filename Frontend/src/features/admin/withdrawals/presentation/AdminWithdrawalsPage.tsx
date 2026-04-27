'use client';

import { useCallback } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { useAdminWithdrawals } from '@/features/admin/withdrawals/application/useAdminWithdrawals';
import { AdminWithdrawalCard } from '@/features/admin/withdrawals/presentation/components/AdminWithdrawalCard';
import { AdminWithdrawalDetailModal } from '@/features/admin/withdrawals/presentation/components/AdminWithdrawalDetailModal';
import { AdminWithdrawalsEmptyState } from '@/features/admin/withdrawals/presentation/components/AdminWithdrawalsEmptyState';
import { AdminWithdrawalsHeader } from '@/features/admin/withdrawals/presentation/components/AdminWithdrawalsHeader';
import { AdminWithdrawalsLoadingState } from '@/features/admin/withdrawals/presentation/components/AdminWithdrawalsLoadingState';
import { cn } from '@/lib/utils';

export default function AdminWithdrawalsPage() {
 const t = useTranslations('Admin');
 const locale = useLocale();
 const vm = useAdminWithdrawals(t, locale);

 const renderCard = useCallback((item: (typeof vm.queue)[number]) => (
  <AdminWithdrawalCard
   key={item.id}
   item={item}
   locale={locale}
   formatVnd={vm.formatVnd}
   grossLabel={t('withdrawals.row.gross')}
   feeLabel={t('withdrawals.row.fee')}
   netLabel={t('withdrawals.row.net')}
   idLabel={t('withdrawals.row.id_prefix', { id: item.userId.substring(0, 8) })}
   notePlaceholder={t('withdrawals.input.admin_note_placeholder')}
   detailLabel={t('withdrawals.actions.view_detail')}
   value={vm.notes[item.id] || ''}
   onChangeNote={(value) => vm.setNotes((prev) => ({ ...prev, [item.id]: value }))}
   onViewDetail={() => vm.openDetail(item.id)}
   onApprove={() => void vm.handleProcess(item.id, 'approve')}
   onReject={() => void vm.handleProcess(item.id, 'reject')}
   approveLabel={t('withdrawals.actions.approve')}
   rejectLabel={t('withdrawals.actions.reject')}
   processing={vm.processing === item.id}
  />
 ), [locale, t, vm]);

 return (
  <div className={cn('space-y-8 pb-20 animate-in fade-in duration-700 max-w-5xl mx-auto')}>
   <AdminWithdrawalsHeader tag={t('withdrawals.header.tag')} title={t('withdrawals.header.title')} subtitle={t('withdrawals.header.subtitle')} />
   {vm.loading ? <AdminWithdrawalsLoadingState label={t('withdrawals.states.loading')} /> : null}
   {!vm.loading && vm.queueError ? <AdminWithdrawalsEmptyState label={vm.queueError} /> : null}
   {!vm.loading && !vm.queueError && vm.queue.length === 0 ? <AdminWithdrawalsEmptyState label={t('withdrawals.states.empty')} /> : null}
   {!vm.loading && !vm.queueError && vm.queue.length > 0 ? <div className={cn('space-y-6')}>{vm.queue.map(renderCard)}</div> : null}
   <AdminWithdrawalDetailModal
    open={Boolean(vm.selectedWithdrawalId)}
    onClose={vm.closeDetail}
    detail={vm.detail}
    loading={vm.loadingDetail}
    detailError={vm.detailError}
    formatVnd={vm.formatVnd}
    title={t('withdrawals.detail.title')}
    qrTitle={t('withdrawals.detail.qr_title')}
    transferContentLabel={t('withdrawals.detail.transfer_content')}
    amountLabel={t('withdrawals.detail.amount')}
    bankNameLabel={t('withdrawals.detail.bank_name')}
    accountNumberLabel={t('withdrawals.detail.account_number')}
    accountHolderLabel={t('withdrawals.detail.account_holder')}
    closeLabel={t('withdrawals.detail.close')}
   />
  </div>
 );
}
