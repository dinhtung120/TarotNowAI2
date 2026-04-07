import { FileText, Loader2 } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface ReaderRequestsStatesProps {
 loading: boolean;
 isEmpty: boolean;
 emptyLabel: string;
}

export function ReaderRequestsStates({
 loading,
 isEmpty,
 emptyLabel,
}: ReaderRequestsStatesProps) {
 if (loading) {
  return (
   <div className={cn('py-20 flex flex-col items-center justify-center space-y-4')}>
    <Loader2 className={cn('w-8 h-8 text-[var(--purple-accent)] animate-spin')} />
   </div>
  );
 }

 if (isEmpty) {
  return (
   <GlassCard className={cn('py-20 flex flex-col items-center justify-center space-y-4 text-center')}>
    <div className={cn('w-16 h-16 rounded-full tn-surface flex items-center justify-center mb-2 shadow-inner')}>
     <FileText className={cn('w-8 h-8 text-[var(--text-tertiary)]')} />
    </div>
    <p className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]')}>{emptyLabel}</p>
   </GlassCard>
  );
 }

 return null;
}
