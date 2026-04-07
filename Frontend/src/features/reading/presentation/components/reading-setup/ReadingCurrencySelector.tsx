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
   <label className={cn('text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] italic')}>
    {labels.title}
   </label>
   <div className={cn('inline-flex p-1.5 bg-[var(--bg-glass)] border border-[var(--border-default)] rounded-2xl shadow-lg backdrop-blur-xl')}>
    <button
     type="button"
     onClick={() => onSelectCurrency('gold')}
     className={cn('flex items-center gap-2 px-6 py-2.5 rounded-xl text-xs font-black uppercase tracking-wider transition-all duration-500', selectedCurrency === 'gold' ? 'bg-[var(--purple-accent)] tn-text-ink shadow-[0_0_20px_var(--purple-accent)]' : 'tn-text-secondary hover:tn-text-primary hover:bg-[var(--purple-50)]')}
    >
     <Zap className={cn('w-3.5 h-3.5')} />
     {labels.gold}
    </button>
    <button
     type="button"
     onClick={() => onSelectCurrency('diamond')}
     className={cn('flex items-center gap-2 px-6 py-2.5 rounded-xl text-xs font-black uppercase tracking-wider transition-all duration-500', selectedCurrency === 'diamond' ? 'bg-[var(--amber-accent)] tn-text-ink shadow-[0_0_20px_var(--amber-accent)]' : 'tn-text-secondary hover:tn-text-primary hover:bg-[var(--amber-50)]')}
    >
     <Star className={cn('w-3.5 h-3.5')} />
     {labels.diamond}
    </button>
   </div>
  </div>
 );
}
