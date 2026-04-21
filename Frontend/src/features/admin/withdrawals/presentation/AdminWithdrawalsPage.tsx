'use client';
import { useLocale, useTranslations } from 'next-intl';
import { useAdminWithdrawals } from '@/features/admin/withdrawals/application/useAdminWithdrawals';
import { AdminWithdrawalCard } from '@/features/admin/withdrawals/presentation/components/AdminWithdrawalCard';
import { AdminWithdrawalsEmptyState } from '@/features/admin/withdrawals/presentation/components/AdminWithdrawalsEmptyState';
import { AdminWithdrawalsHeader } from '@/features/admin/withdrawals/presentation/components/AdminWithdrawalsHeader';
import { AdminWithdrawalsLoadingState } from '@/features/admin/withdrawals/presentation/components/AdminWithdrawalsLoadingState';
import { cn } from '@/lib/utils';

export default function AdminWithdrawalsPage() {
 const t = useTranslations('Admin');
 const locale = useLocale();
 const {
  queue,
  loading,
  processing,
  notes,
  setNotes,
  handleProcess,
  formatVnd,
 } = useAdminWithdrawals(t, locale);

 const renderCard = (item: (typeof queue)[number]) => (
  <AdminWithdrawalCard
   key={item.id}
   item={item}
   locale={locale}
   formatVnd={formatVnd}
   grossLabel={t('withdrawals.row.gross')}
   feeLabel={t('withdrawals.row.fee')}
   netLabel={t('withdrawals.row.net')}
   idLabel={t('withdrawals.row.id_prefix', { id: item.userId.substring(0, 8) })}
   notePlaceholder={t('withdrawals.input.admin_note_placeholder')}
   value={notes[item.id] || ''}
   onChangeNote={(value) => setNotes((prev) => ({ ...prev, [item.id]: value }))}
   onApprove={() => void handleProcess(item.id, 'approve')}
   onReject={() => void handleProcess(item.id, 'reject')}
   approveLabel={t('withdrawals.actions.approve')}
   rejectLabel={t('withdrawals.actions.reject')}
   processing={processing === item.id}
  />
 );

 return (
  <div className={cn('space-y-8 pb-20 animate-in fade-in duration-700 max-w-5xl mx-auto')}>
   <AdminWithdrawalsHeader tag={t('withdrawals.header.tag')} title={t('withdrawals.header.title')} subtitle={t('withdrawals.header.subtitle')} />
   {loading ? <AdminWithdrawalsLoadingState label={t('withdrawals.states.loading')} /> : null}
   {!loading && queue.length === 0 ? <AdminWithdrawalsEmptyState label={t('withdrawals.states.empty')} /> : null}
   {!loading && queue.length > 0 ? <div className={cn('space-y-6')}>{queue.map(renderCard)}</div> : null}
  </div>
 );
}
