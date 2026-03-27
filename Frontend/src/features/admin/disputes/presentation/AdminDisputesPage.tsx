'use client';

import { Loader2, Scale, ShieldAlert } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { SectionHeader, GlassCard, Button } from '@/shared/components/ui';
import { useAdminDisputes } from '@/features/admin/disputes/application/useAdminDisputes';

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

 return (
  <div className="max-w-6xl mx-auto px-4 sm:px-6 py-10 space-y-6">
   <SectionHeader
    tag={t('disputes.header.tag')}
    tagIcon={<Scale className="w-3 h-3 text-[var(--danger)]" />}
    title={t('disputes.header.title')}
    subtitle={t('disputes.header.subtitle')}
    className="mb-0"
   />

   {loading ? (
    <GlassCard className="h-52 flex items-center justify-center">
     <Loader2 className="w-6 h-6 animate-spin text-[var(--text-secondary)]" />
    </GlassCard>
   ) : null}

   {!loading && disputes.length === 0 ? (
    <GlassCard className="h-52 flex flex-col items-center justify-center gap-3">
     <ShieldAlert className="w-8 h-8 text-[var(--text-tertiary)]" />
     <p className="text-sm text-[var(--text-secondary)]">{t('disputes.empty')}</p>
    </GlassCard>
   ) : null}

   {!loading && disputes.length > 0 ? (
    <div className="space-y-4">
     {disputes.map((item) => {
      const isProcessing = processingId === item.id;
      const note = noteById[item.id] ?? '';
      const splitPercent = splitPercentById[item.id] ?? 50;

      return (
       <GlassCard key={item.id} className="space-y-4 !p-5">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-3 text-xs">
         <div>
          <div className="text-[var(--text-tertiary)] uppercase tracking-widest text-[10px]">{t('disputes.card.item_id')}</div>
          <div className="font-mono text-[var(--text-secondary)] break-all">{item.id}</div>
         </div>
         <div>
          <div className="text-[var(--text-tertiary)] uppercase tracking-widest text-[10px]">{t('disputes.card.amount')}</div>
          <div className="font-bold text-[var(--warning)]">{item.amountDiamond} 💎</div>
         </div>
         <div>
          <div className="text-[var(--text-tertiary)] uppercase tracking-widest text-[10px]">{t('disputes.card.payer')}</div>
          <div className="font-mono text-[var(--text-secondary)] break-all">{item.payerId}</div>
         </div>
         <div>
          <div className="text-[var(--text-tertiary)] uppercase tracking-widest text-[10px]">{t('disputes.card.reader')}</div>
          <div className="font-mono text-[var(--text-secondary)] break-all">{item.receiverId}</div>
         </div>
        </div>

        <textarea
         value={note}
         onChange={(event) =>
          setNoteById((prev) => ({
           ...prev,
           [item.id]: event.target.value,
          }))
         }
         rows={2}
         placeholder={t('disputes.form.note_placeholder')}
         className="w-full rounded-xl bg-white/5 border border-white/10 px-3 py-2 text-sm text-[var(--text-primary)]"
        />

        <div className="flex items-center gap-2">
         <span className="text-xs text-[var(--text-secondary)]">Split % Reader</span>
         <input
          type="number"
          min={1}
          max={99}
          value={splitPercent}
          onChange={(event) => {
           const value = Number(event.target.value);
           if (Number.isFinite(value) == false) return;
           setSplitPercentById((prev) => ({
            ...prev,
            [item.id]: Math.max(1, Math.min(99, value)),
           }));
          }}
          className="w-24 rounded-lg bg-white/5 border border-white/10 px-2 py-1 text-xs text-[var(--text-primary)]"
         />
        </div>

        <div className="flex flex-col sm:flex-row gap-3">
         <Button
          variant="primary"
          disabled={isProcessing}
          onClick={() => resolveDispute(item.id, 'release')}
          className="flex-1"
         >
          {isProcessing ? <Loader2 className="w-4 h-4 animate-spin" /> : null}
          {t('disputes.form.release_button')}
         </Button>
         <Button
          variant="secondary"
          disabled={isProcessing}
          onClick={() => resolveDispute(item.id, 'refund')}
          className="flex-1"
         >
          {isProcessing ? <Loader2 className="w-4 h-4 animate-spin" /> : null}
          {t('disputes.form.refund_button')}
         </Button>
         <Button
          variant="secondary"
          disabled={isProcessing}
          onClick={() => resolveDispute(item.id, 'split')}
          className="flex-1"
         >
          {isProcessing ? <Loader2 className="w-4 h-4 animate-spin" /> : null}
          Split
         </Button>
        </div>
       </GlassCard>
      );
     })}
    </div>
   ) : null}
  </div>
 );
}
