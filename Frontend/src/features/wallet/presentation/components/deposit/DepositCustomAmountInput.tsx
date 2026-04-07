import { cn } from '@/lib/utils';

interface DepositCustomAmountInputProps {
 label: string;
 placeholder: string;
 minAmount: number;
 value: string;
 onFocus: () => void;
 onChange: (value: string) => void;
}

export function DepositCustomAmountInput({
 label,
 placeholder,
 minAmount,
 value,
 onFocus,
 onChange,
}: DepositCustomAmountInputProps) {
 return (
  <div className={cn('pt-2 space-y-2')}>
   <label className={cn('text-[10px] font-black uppercase tracking-widest tn-text-muted')}>{label}</label>
   <input
    type="number"
    value={value}
    onFocus={onFocus}
    onChange={(event) => onChange(event.target.value)}
    placeholder={placeholder}
    min={minAmount}
    className={cn('w-full px-4 py-3 tn-field rounded-2xl tn-text-primary placeholder:tn-text-muted')}
   />
  </div>
 );
}
