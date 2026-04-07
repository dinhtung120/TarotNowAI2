'use client';

import { Scale } from 'lucide-react';
import { useTranslations } from 'next-intl';
import type { AdminDisputeItemDto } from '@/features/chat/application/actions';
import { useAdminDisputes } from '@/features/admin/disputes/application/useAdminDisputes';
import { AdminDisputeCard } from '@/features/admin/disputes/presentation/components/AdminDisputeCard';
import { AdminDisputesEmptyState } from '@/features/admin/disputes/presentation/components/AdminDisputesEmptyState';
import { AdminDisputesLoadingState } from '@/features/admin/disputes/presentation/components/AdminDisputesLoadingState';
import { SectionHeader } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

export default function AdminDisputesPage() {
 const t = useTranslations('Admin');
 const {
  disputes,
  loading,
  processingId,
  noteById,
  setNoteById,
  splitPercentById,
  setSplitPercentById,
  resolveDispute,
 } = useAdminDisputes(t);

 const renderDisputeCard = (item: AdminDisputeItemDto) => {
  const note = noteById[item.id] ?? '';
  const splitPercent = splitPercentById[item.id] ?? 50;
  return (
   <AdminDisputeCard
    key={item.id}
    amountLabel={t('disputes.card.amount')}
    isProcessing={processingId === item.id}
    item={item}
    itemIdLabel={t('disputes.card.item_id')}
    note={note}
    notePlaceholder={t('disputes.form.note_placeholder')}
    onChangeNote={(value) => setNoteById((prev) => ({ ...prev, [item.id]: value }))}
    onChangeSplitPercent={(value) => setSplitPercentById((prev) => ({ ...prev, [item.id]: value }))}
    onRelease={() => resolveDispute(item.id, 'release')}
    onRefund={() => resolveDispute(item.id, 'refund')}
    onSplit={() => resolveDispute(item.id, 'split')}
    payerLabel={t('disputes.card.payer')}
    readerLabel={t('disputes.card.reader')}
    refundLabel={t('disputes.form.refund_button')}
    releaseLabel={t('disputes.form.release_button')}
    splitLabel="Split"
    splitPercent={splitPercent}
    splitPercentLabel="Split % Reader"
   />
  );
 };

 return (
  <div className={cn('max-w-6xl mx-auto px-4 sm:px-6 py-10 space-y-6')}>
   <SectionHeader
    tag={t('disputes.header.tag')}
    tagIcon={<Scale className={cn('w-3 h-3 text-[var(--danger)]')} />}
    title={t('disputes.header.title')}
    subtitle={t('disputes.header.subtitle')}
    className={cn('mb-0')}
   />
   {loading ? <AdminDisputesLoadingState /> : null}
   {!loading && disputes.length === 0 ? <AdminDisputesEmptyState label={t('disputes.empty')} /> : null}
   {!loading && disputes.length > 0 ? <div className={cn('space-y-4')}>{disputes.map(renderDisputeCard)}</div> : null}
  </div>
 );
}
