import { XCircle } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

interface ReaderApplyRejectedNoticeProps {
  adminNote?: string;
}

export default function ReaderApplyRejectedNotice({ adminNote }: ReaderApplyRejectedNoticeProps) {
  const t = useTranslations('ReaderApply');

  return (
    <div className={cn('space-y-2 rounded-2xl border border-[var(--danger)]/20 bg-[var(--danger)]/5 p-6')}>
      <div className={cn('flex items-center gap-2')}>
        <XCircle className={cn('h-4 w-4 text-[var(--danger)]')} />
        <span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--danger)]')}>{t('rejected.title')}</span>
      </div>
      {adminNote ? <p className={cn('text-xs leading-relaxed text-[var(--text-secondary)]')}>{t('rejected.reason', { note: adminNote })}</p> : null}
      <p className={cn('text-xs text-[var(--text-muted)]')}>{t('rejected.can_resubmit')}</p>
    </div>
  );
}
