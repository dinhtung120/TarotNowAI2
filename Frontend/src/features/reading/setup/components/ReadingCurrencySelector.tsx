import { Star, Zap } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ReadingCurrencySelectorProps {
 selectedCurrency: 'gold' | 'diamond';
 labels: {
  title: string;
  gold: string;
  diamond: string;
 };
 onSelectCurrency: (currency: 'gold' | 'diamond') => void;
}

export function ReadingCurrencySelector({
 selectedCurrency,
 labels,
 onSelectCurrency,
}: ReadingCurrencySelectorProps) {
 return (
  <div className={cn('flex flex-col items-center mb-10 space-y-4 animate-in fade-in slide-in-from-top-4 duration-700')}>
   <label className={cn('tn-text-overline tn-text-tertiary italic')}>
    {labels.title}
   </label>
   <div className={cn('inline-flex p-1.5 tn-bg-glass border tn-border rounded-2xl shadow-lg backdrop-blur-xl')}>
    <button
     type="button"
     onClick={() => onSelectCurrency('gold')}
     className={cn('flex items-center gap-2 px-6 py-2.5 rounded-xl text-xs font-black uppercase tracking-wider transition-all duration-500', selectedCurrency === 'gold' ? 'tn-bg-accent tn-text-ink tn-shadow-glow-accent' : 'tn-text-secondary tn-reading-currency-gold-hover')}
    >
     <Zap className={cn('w-3.5 h-3.5')} />
     {labels.gold}
    </button>
    <button
     type="button"
     onClick={() => onSelectCurrency('diamond')}
     className={cn('flex items-center gap-2 px-6 py-2.5 rounded-xl text-xs font-black uppercase tracking-wider transition-all duration-500', selectedCurrency === 'diamond' ? 'tn-bg-warning tn-text-ink tn-shadow-glow-warning' : 'tn-text-secondary tn-reading-currency-diamond-hover')}
    >
     <Star className={cn('w-3.5 h-3.5')} />
     {labels.diamond}
    </button>
   </div>
  </div>
 );
}
