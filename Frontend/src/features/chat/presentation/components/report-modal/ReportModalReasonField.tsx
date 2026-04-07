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
   <label className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-muted')}>{t('report.reason_label')}</label>
   <textarea
    value={reason}
    onChange={(event) => setReason(event.target.value)}
    placeholder={t('report.reason_placeholder')}
    rows={4}
    className={cn('w-full resize-none rounded-xl border tn-border-danger bg-white/5 p-4 text-sm tn-text-primary tn-placeholder transition-all')}
   />
   <span className={cn('tn-text-10', reason.length >= 10 ? 'tn-text-muted' : 'tn-text-danger')}>{reason.length}/500</span>
  </div>
 );
}
