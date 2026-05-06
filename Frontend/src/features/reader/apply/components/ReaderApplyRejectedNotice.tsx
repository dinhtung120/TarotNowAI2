import { XCircle } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

interface ReaderApplyRejectedNoticeProps {
  adminNote?: string;
}

export default function ReaderApplyRejectedNotice({ adminNote }: ReaderApplyRejectedNoticeProps) {
  const t = useTranslations('ReaderApply');

  return (
    <div className={cn('space-y-2 rounded-2xl border tn-border-danger tn-bg-danger-soft p-6')}>
      <div className={cn('flex items-center gap-2')}>
        <XCircle className={cn('h-4 w-4 tn-text-danger')} />
        <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-danger')}>{t('rejected.title')}</span>
      </div>
      {adminNote ? <p className={cn('text-xs leading-relaxed tn-text-secondary')}>{t('rejected.reason', { note: adminNote })}</p> : null}
      <p className={cn('text-xs tn-text-muted')}>{t('rejected.can_resubmit')}</p>
    </div>
  );
}
