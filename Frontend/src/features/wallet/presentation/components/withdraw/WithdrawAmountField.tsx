import { Diamond } from 'lucide-react';
import { cn } from '@/lib/utils';

interface WithdrawAmountFieldProps {
 label: string;
 placeholder: string;
 value: string;
 onChange: (value: string) => void;
}

export function WithdrawAmountField({
 label,
 placeholder,
 value,
 onChange,
}: WithdrawAmountFieldProps) {
 return (
  <div className={cn('space-y-4')}>
   <label className={cn('text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)] block')}>
    {label}
   </label>
   <div className={cn('relative')}>
    <Diamond className={cn('absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-[var(--warning)]')} />
    <input
     type="number"
     value={value}
     onChange={(event) => onChange(event.target.value)}
     placeholder={placeholder}
     min={50}
     className={cn('w-full pl-12 pr-4 py-4 tn-field rounded-2xl tn-text-primary text-xl font-black italic placeholder:tn-text-muted placeholder:not-italic tn-field-success transition-all font-sans')}
    />
   </div>
  </div>
 );
}
