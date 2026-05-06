import type { HistorySessionDto } from '@/features/reading/history/actions';
import { cn } from '@/lib/utils';
import { useOptimizedNavigation } from '@/shared/navigation/useOptimizedNavigation';
import { HistorySessionCard } from './HistorySessionCard';

interface HistorySessionsGridProps {
 locale: string;
 items: HistorySessionDto[];
 getSpreadName: (spreadType: string) => string;
 completedLabel: string;
 interruptedLabel: string;
}

export function HistorySessionsGrid({
 locale,
 items,
 getSpreadName,
 completedLabel,
 interruptedLabel,
}: HistorySessionsGridProps) {
 const navigation = useOptimizedNavigation();

 return (
  <div className={cn('tn-grid-cols-1-2-md gap-4 animate-in fade-in slide-in-from-bottom-8 duration-700 delay-200')}>
   {items.map((session) => (
    <HistorySessionCard
     key={session.id}
     locale={locale}
     session={session}
     spreadName={getSpreadName(session.spreadType)}
     completedLabel={completedLabel}
     interruptedLabel={interruptedLabel}
     onOpenSession={(id) => navigation.push(`/reading/history/${id}`)}
    />
   ))}
  </div>
 );
}
