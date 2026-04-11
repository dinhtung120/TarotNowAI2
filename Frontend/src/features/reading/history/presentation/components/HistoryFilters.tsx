import { Sparkles } from 'lucide-react';
import { cn } from '@/lib/utils';

interface HistoryFiltersProps {
 filterType: string;
 filterDate: string;
 labels: {
  all: string;
  daily: string;
  spread3: string;
  spread5: string;
  spread10: string;
 };
 onFilterTypeChange: (value: string) => void;
 onFilterDateChange: (value: string) => void;
}

export function HistoryFilters({
 filterType,
 filterDate,
 labels,
 onFilterTypeChange,
 onFilterDateChange,
}: HistoryFiltersProps) {
 return (
  <div className={cn('flex flex-wrap items-center gap-3')}>
   <div className={cn('relative')}>
    <select
     value={filterType}
     onChange={(event) => onFilterTypeChange(event.target.value)}
     className={cn('appearance-none tn-field tn-field-accent rounded-xl px-4 py-2.5 pr-10 tn-text-11 font-black uppercase tracking-widest tn-text-secondary cursor-pointer min-h-11')}
    >
     <option value="all" className={cn('tn-surface')}>{labels.all}</option>
     <option value="daily_1" className={cn('tn-surface')}>{labels.daily}</option>
     <option value="spread_3" className={cn('tn-surface')}>{labels.spread3}</option>
     <option value="spread_5" className={cn('tn-surface')}>{labels.spread5}</option>
     <option value="spread_10" className={cn('tn-surface')}>{labels.spread10}</option>
    </select>
    <div className={cn('absolute right-4 top-1/2 -translate-y-1/2 pointer-events-none opacity-40 transition-opacity')}>
     <Sparkles className={cn('w-3.5 h-3.5')} />
    </div>
   </div>
   <input
    type="date"
    value={filterDate}
    onChange={(event) => onFilterDateChange(event.target.value)}
    className={cn('tn-field tn-field-accent rounded-xl px-4 py-2.5 tn-text-11 font-black uppercase tracking-widest tn-text-secondary cursor-pointer min-h-11')}
   />
  </div>
 );
}
