import { cn } from '@/lib/utils';

interface ReadingFreeDrawQuotaSummaryProps {
 title: string;
 spread3Label: string;
 spread5Label: string;
 spread10Label: string;
 spread3Value: number;
 spread5Value: number;
 spread10Value: number;
}

function QuotaBadge({ label, value }: { label: string; value: number }) {
 return (
  <div className={cn('rounded-xl border tn-border-soft tn-surface px-3 py-2')}>
   <p className={cn('text-xs font-semibold tn-text-tertiary')}>{label}</p>
   <p className={cn('text-lg font-black tn-text-primary')}>{value}</p>
  </div>
 );
}

export function ReadingFreeDrawQuotaSummary({
 title,
 spread3Label,
 spread5Label,
 spread10Label,
 spread3Value,
 spread5Value,
 spread10Value,
}: ReadingFreeDrawQuotaSummaryProps) {
 return (
  <section className={cn('mb-6 rounded-2xl border tn-border-soft tn-surface px-4 py-3')} aria-label={title}>
   <p className={cn('mb-3 text-sm font-bold tn-text-secondary')}>{title}</p>
   <div className={cn('grid grid-cols-1 gap-2 sm:grid-cols-3')}>
    <QuotaBadge label={spread3Label} value={spread3Value} />
    <QuotaBadge label={spread5Label} value={spread5Value} />
    <QuotaBadge label={spread10Label} value={spread10Value} />
   </div>
  </section>
 );
}
