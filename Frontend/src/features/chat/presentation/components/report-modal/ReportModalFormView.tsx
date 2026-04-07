import { Flag, Loader2, X } from 'lucide-react';
import { useTranslations } from 'next-intl';
import ReportModalReasonField from '@/features/chat/presentation/components/report-modal/ReportModalReasonField';
import ReportModalShell from '@/features/chat/presentation/components/report-modal/ReportModalShell';
import ReportModalTargetTypeButtons from '@/features/chat/presentation/components/report-modal/ReportModalTargetTypeButtons';
import type { ReportTargetType } from '@/features/chat/presentation/components/report-modal/types';
import { cn } from '@/lib/utils';

interface ReportModalFormViewProps {
  error: string;
  reason: string;
  submitting: boolean;
  targetType: ReportTargetType;
  onClose: () => void;
  onSubmit: () => void;
  setReason: (value: string) => void;
  setTargetType: (value: ReportTargetType) => void;
}

export default function ReportModalFormView({ error, reason, submitting, targetType, onClose, onSubmit, setReason, setTargetType }: ReportModalFormViewProps) {
  const t = useTranslations('Chat');

  return (
    <ReportModalShell onClose={onClose} panelClassName={cn('space-y-6')}>
      <div className={cn('flex items-center justify-between')}>
        <div className={cn('flex items-center gap-2')}>
          <Flag className={cn('h-4 w-4 tn-text-danger')} />
          <h3 className={cn('text-sm font-black uppercase tracking-widest tn-text-primary')}>{t('report.title')}</h3>
        </div>
        <button type="button" onClick={onClose} className={cn('rounded-lg p-1 transition-colors tn-hover-surface-hover')}>
          <X className={cn('h-4 w-4 tn-text-muted')} />
        </button>
      </div>
      <ReportModalTargetTypeButtons targetType={targetType} setTargetType={setTargetType} />
      <ReportModalReasonField reason={reason} setReason={setReason} />
      {error ? <div className={cn('rounded-lg border tn-border-danger tn-bg-danger-soft p-3 text-xs tn-text-danger')}>{error}</div> : null}
      <button type="button" onClick={onSubmit} disabled={submitting || reason.length < 10} className={cn('tn-report-submit-btn flex w-full items-center justify-center gap-2 rounded-xl p-3 tn-text-10 font-black uppercase tracking-widest transition-all')}>
        {submitting ? <Loader2 className={cn('h-3 w-3 animate-spin')} /> : <Flag className={cn('h-3 w-3')} />}
        {t('report.submit')}
      </button>
    </ReportModalShell>
  );
}
