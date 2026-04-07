import { useRouter } from '@/i18n/routing';
import type { HistorySessionDto } from '@/features/reading/application/actions/history';
import { cn } from '@/lib/utils';
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
 const router = useRouter();

 return (
  <div className={cn('grid grid-cols-1 md:grid-cols-2 gap-4 animate-in fade-in slide-in-from-bottom-8 duration-700 delay-200')}>
   {items.map((session) => (
    <HistorySessionCard
     key={session.id}
     locale={locale}
     session={session}
     spreadName={getSpreadName(session.spreadType)}
     completedLabel={completedLabel}
     interruptedLabel={interruptedLabel}
     onOpenSession={(id) => router.push(`/reading/history/${id}`)}
    />
   ))}
  </div>
 );
}
