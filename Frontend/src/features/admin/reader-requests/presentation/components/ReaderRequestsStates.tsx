import { FileText, Loader2 } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface ReaderRequestsStatesProps {
 loading: boolean;
 hasError: boolean;
 errorLabel: string;
 isEmpty: boolean;
 emptyLabel: string;
}

export function ReaderRequestsStates({
 loading,
 hasError,
 errorLabel,
 isEmpty,
 emptyLabel,
}: ReaderRequestsStatesProps) {
 if (loading) {
  return (
   <div className={cn('py-20 flex flex-col items-center justify-center space-y-4')}>
    <Loader2 className={cn('w-8 h-8 tn-text-accent animate-spin')} />
   </div>
  );
 }

 if (hasError) {
  return (
   <GlassCard className={cn('py-20 flex flex-col items-center justify-center space-y-4 text-center')}>
    <p className={cn('tn-text-10 font-black uppercase tracking-widest text-red-300')}>{errorLabel}</p>
   </GlassCard>
  );
 }

 if (isEmpty) {
  return (
   <GlassCard className={cn('py-20 flex flex-col items-center justify-center space-y-4 text-center')}>
    <div className={cn('w-16 h-16 rounded-full tn-surface flex items-center justify-center mb-2 shadow-inner')}>
     <FileText className={cn('w-8 h-8 tn-text-tertiary')} />
    </div>
   <p className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-tertiary')}>{emptyLabel}</p>
   </GlassCard>
  );
 }

 return null;
}
