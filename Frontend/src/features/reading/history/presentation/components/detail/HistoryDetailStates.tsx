import { AlertCircle } from 'lucide-react';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';
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
 const navigation = useOptimizedNavigation();

 if (isLoading) {
  return (
   <div className={cn('tn-history-detail-skeleton-grid gap-6 animate-pulse')}>
    {[1, 2, 3, 4, 5].map((index) => (
     <div key={`history-detail-skeleton-${index}`} className={cn('tn-aspect-14-22 tn-surface tn-rounded-2_5xl border tn-border-soft')} />
    ))}
   </div>
  );
 }

 if (error) {
  return (
   <div className={cn('tn-bg-danger-soft border tn-border-danger p-12 tn-rounded-3xl text-center max-w-2xl mx-auto')}>
    <AlertCircle className={cn('w-12 h-12 tn-text-danger mx-auto mb-4 opacity-80')} />
    <p className={cn('tn-text-danger font-bold mb-8 italic')}>{error}</p>
    <Button variant="primary" onClick={() => navigation.push('/reading/history')}>
     {backLabel}
    </Button>
   </div>
  );
 }

 return null;
}
