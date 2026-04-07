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
      <label className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-muted)]')}>{t('report.target_type_label')}</label>
      <div className={cn('flex gap-2')}>
        {options.map((option) => (
          <button
            key={option.value}
            type="button"
            onClick={() => setTargetType(option.value)}
            className={cn('flex-1 rounded-xl border px-3 py-2 text-[10px] font-bold uppercase tracking-wider transition-all', targetType === option.value ? 'border-[var(--danger)]/30 bg-[var(--danger)]/10 text-[var(--danger)]' : 'border-white/10 bg-white/5 text-[var(--text-muted)]')}
          >
            {option.label}
          </button>
        ))}
      </div>
    </div>
  );
}
