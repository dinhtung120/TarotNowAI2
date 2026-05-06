import { Clock, ScrollText, Star } from 'lucide-react';
import { useTranslations } from 'next-intl';
import type { LucideIcon } from 'lucide-react';
import { cn } from '@/lib/utils';

type StepColor = 'purple' | 'amber' | 'emerald';

interface StepItem {
  color: StepColor;
  description: string;
  icon: LucideIcon;
  title: string;
}

const ICON_CLASS_BY_COLOR: Record<StepColor, string> = {
  purple: 'tn-text-accent',
  amber: 'tn-text-warning',
  emerald: 'tn-text-success',
};

export default function ReaderApplyStepsGrid() {
  const t = useTranslations('ReaderApply');
  const steps: StepItem[] = [
    { icon: ScrollText, title: t('steps.intro.title'), description: t('steps.intro.desc'), color: 'purple' },
    { icon: Clock, title: t('steps.review.title'), description: t('steps.review.desc'), color: 'amber' },
    { icon: Star, title: t('steps.start.title'), description: t('steps.start.desc'), color: 'emerald' },
  ];

  return (
    <div className={cn('tn-grid-1-3-md gap-4')}>
      {steps.map((step) => (
        <div key={step.title} className={cn('space-y-2 rounded-2xl bg-white/5 p-5 text-center')}>
          <step.icon className={cn('mx-auto h-6 w-6', ICON_CLASS_BY_COLOR[step.color])} />
          <div className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary')}>{step.title}</div>
          <p className={cn('tn-text-10 tn-text-muted')}>{step.description}</p>
        </div>
      ))}
    </div>
  );
}
