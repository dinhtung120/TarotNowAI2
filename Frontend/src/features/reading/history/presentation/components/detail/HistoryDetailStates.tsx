import { AlertCircle } from 'lucide-react';
import { useRouter } from '@/i18n/routing';
import { Button } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface HistoryDetailStatesProps {
 isLoading: boolean;
 error: string | null;
 backLabel: string;
}

export function HistoryDetailStates({
 isLoading,
 error,
 backLabel,
}: HistoryDetailStatesProps) {
 const router = useRouter();

 if (isLoading) {
  return (
   <div className={cn('grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6 animate-pulse')}>
    {[1, 2, 3, 4, 5].map((index) => (
     <div key={`history-detail-skeleton-${index}`} className={cn('aspect-[14/22] tn-surface rounded-[2.5rem] border tn-border-soft')} />
    ))}
   </div>
  );
 }

 if (error) {
  return (
   <div className={cn('bg-[var(--danger-bg)] border border-[var(--danger)] p-12 rounded-[3rem] text-center max-w-2xl mx-auto')}>
    <AlertCircle className={cn('w-12 h-12 text-[var(--danger)] mx-auto mb-4 opacity-80')} />
    <p className={cn('text-[var(--danger)] font-bold mb-8 italic')}>{error}</p>
    <Button variant="primary" onClick={() => router.push('/reading/history')}>
     {backLabel}
    </Button>
   </div>
  );
 }

 return null;
}
