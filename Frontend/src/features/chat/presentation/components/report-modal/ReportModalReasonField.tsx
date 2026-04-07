import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

interface ReportModalReasonFieldProps {
  reason: string;
  setReason: (value: string) => void;
}

export default function ReportModalReasonField({ reason, setReason }: ReportModalReasonFieldProps) {
  const t = useTranslations('Chat');

  return (
    <div className={cn('space-y-2')}>
      <label className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-muted)]')}>{t('report.reason_label')}</label>
      <textarea
        value={reason}
        onChange={(event) => setReason(event.target.value)}
        placeholder={t('report.reason_placeholder')}
        rows={4}
        className={cn('w-full resize-none rounded-xl border border-[var(--danger)]/20 bg-white/5 p-4 text-sm text-[var(--text-primary)] placeholder:text-[var(--text-muted)] transition-all')}
      />
      <span className={cn('text-[10px]', reason.length >= 10 ? 'text-[var(--text-muted)]' : 'text-[var(--danger)]')}>{reason.length}/500</span>
    </div>
  );
}
