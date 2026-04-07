import type { ReactNode } from 'react';
import { cn } from '@/lib/utils';

export interface FilterTabOption {
 value: string;
 label: string;
 icon?: ReactNode;
 activeClassName?: string;
}

interface FilterTabsProps {
 value: string;
 options: FilterTabOption[];
 onChange: (value: string) => void;
 containerClassName?: string;
 buttonClassName?: string;
 inactiveClassName?: string;
}

export default function FilterTabs({
 value,
 options,
 onChange,
 containerClassName = 'flex flex-wrap gap-2',
 buttonClassName = 'flex items-center gap-2 px-5 py-3 rounded-2xl text-[10px] font-black uppercase tracking-widest transition-all shadow-inner min-h-11',
 inactiveClassName = 'tn-panel-soft text-[var(--text-secondary)] hover:tn-text-primary hover:tn-surface',
}: FilterTabsProps) {
 return (
  <div className={cn(containerClassName)}>
   {options.map((option) => (
   <button
     key={option.value}
     type="button"
     onClick={() => onChange(option.value)}
     className={cn(buttonClassName, value === option.value ? option.activeClassName ?? '' : inactiveClassName)}
   >
     {option.icon}
     {option.label}
    </button>
   ))}
  </div>
 );
}
