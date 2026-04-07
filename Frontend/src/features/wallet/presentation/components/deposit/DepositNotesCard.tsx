import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface DepositNotesCardProps {
 title: string;
 items: [string, string, string];
}

export function DepositNotesCard({ title, items }: DepositNotesCardProps) {
 return (
  <GlassCard className={cn('space-y-3')}>
   <div className={cn('text-[9px] font-black uppercase tracking-widest tn-text-muted')}>{title}</div>
   <ul className={cn('space-y-1 text-[11px] tn-text-secondary font-medium leading-relaxed list-disc list-inside')}>
    {items.map((item) => (
     <li key={item}>{item}</li>
    ))}
   </ul>
  </GlassCard>
 );
}
