import { useTranslations } from 'next-intl';
import type { ReportTargetType } from '@/features/chat/presentation/components/report-modal/types';
import { cn } from '@/lib/utils';

interface ReportModalTargetTypeButtonsProps {
  targetType: ReportTargetType;
  setTargetType: (value: ReportTargetType) => void;
}

export default function ReportModalTargetTypeButtons({ targetType, setTargetType }: ReportModalTargetTypeButtonsProps) {
  const t = useTranslations('Chat');
  const options: { label: string; value: ReportTargetType }[] = [
    { value: 'conversation', label: t('report.target_conversation') },
    { value: 'user', label: t('report.target_user') },
    { value: 'message', label: t('report.target_message') },
  ];

 return (
  <div className={cn('space-y-2')}>
   <label className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-muted')}>{t('report.target_type_label')}</label>
   <div className={cn('flex gap-2')}>
    {options.map((option) => (
     <button
      key={option.value}
      type="button"
      onClick={() => setTargetType(option.value)}
      className={cn('flex-1 rounded-xl border px-3 py-2 tn-text-10 font-bold uppercase tracking-wider transition-all', targetType === option.value ? 'tn-border-danger tn-bg-danger-soft tn-text-danger' : 'border-white/10 bg-white/5 tn-text-muted')}
     >
      {option.label}
     </button>
        ))}
      </div>
    </div>
  );
}
