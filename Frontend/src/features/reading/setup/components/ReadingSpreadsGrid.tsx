import type { ReadingSpreadOption } from '@/features/reading/setup/useReadingSetupPage';
import { cn } from '@/lib/utils';
import { ReadingSpreadCard } from './ReadingSpreadCard';

interface ReadingSpreadsGridProps {
 spreads: ReadingSpreadOption[];
 selectedSpread: ReadingSpreadOption['id'];
 selectedCurrency: 'gold' | 'diamond';
 expBonusLabel: (amount: number) => string;
 onSelectSpread: (spreadId: ReadingSpreadOption['id']) => void;
}

export function ReadingSpreadsGrid({
 spreads,
 selectedSpread,
 selectedCurrency,
 expBonusLabel,
 onSelectSpread,
}: ReadingSpreadsGridProps) {
 return (
  <div className={cn('tn-grid-cols-1-2-md gap-4 animate-in fade-in slide-in-from-bottom-8 duration-700 delay-200')}>
   {spreads.map((spread) => (
    <ReadingSpreadCard
     key={spread.id}
     spread={spread}
     selectedSpreadId={selectedSpread}
     selectedCurrency={selectedCurrency}
     expBonusLabel={expBonusLabel}
     onSelectSpread={onSelectSpread}
    />
   ))}
  </div>
 );
}
