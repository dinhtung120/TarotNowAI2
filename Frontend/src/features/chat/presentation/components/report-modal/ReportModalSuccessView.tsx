import { CheckCircle2 } from 'lucide-react';
import { useTranslations } from 'next-intl';
import ReportModalShell from '@/features/chat/presentation/components/report-modal/ReportModalShell';
import { cn } from '@/lib/utils';

interface ReportModalSuccessViewProps {
  onClose: () => void;
}

export default function ReportModalSuccessView({ onClose }: ReportModalSuccessViewProps) {
  const t = useTranslations('Chat');
  const tCommon = useTranslations('Common');

 return (
  <ReportModalShell onClose={onClose} panelClassName={cn('space-y-4 p-10 text-center')}>
   <CheckCircle2 className={cn('mx-auto h-12 w-12 tn-text-success')} />
   <h3 className={cn('text-lg font-black uppercase italic tn-text-primary')}>{t('report.success_title')}</h3>
   <p className={cn('text-xs tn-text-muted')}>{t('report.success_desc')}</p>
   <button type="button" onClick={onClose} className={cn('rounded-xl tn-bg-accent px-6 py-2 text-xs font-bold uppercase tracking-widest text-white transition-all tn-hover-brightness-110')}>
    {tCommon('close')}
   </button>
  </ReportModalShell>
  );
}
