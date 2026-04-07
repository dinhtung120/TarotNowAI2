import type { LucideIcon } from 'lucide-react';
import { cn } from '@/lib/utils';

interface WithdrawBankFieldProps {
 label: string;
 placeholder: string;
 value: string;
 onChange: (value: string) => void;
 icon?: LucideIcon;
 uppercase?: boolean;
}

export function WithdrawBankField({
 label,
 placeholder,
 value,
 onChange,
 icon: Icon,
 uppercase = false,
}: WithdrawBankFieldProps) {
 return (
  <div className={cn('space-y-3')}>
   <label className={cn('tn-text-10 font-black uppercase tn-tracking-02 tn-text-secondary block')}>{label}</label>
   <div className={cn('relative')}>
    {Icon ? <Icon className={cn('absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 tn-text-muted')} /> : null}
    <input
     type="text"
     value={value}
     onChange={(event) => onChange(event.target.value)}
     placeholder={placeholder}
     className={cn('w-full py-3 tn-field rounded-xl text-sm tn-text-primary placeholder:tn-text-muted tn-field-accent transition-all', Icon ? 'pl-12 pr-4' : 'px-4', uppercase ? 'uppercase' : '')}
    />
   </div>
  </div>
 );
}
