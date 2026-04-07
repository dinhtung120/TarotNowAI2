import { Clock, Search } from 'lucide-react';
import { cn } from '@/lib/utils';

interface WalletLedgerToolbarProps {
 title: string;
 placeholder: string;
}

export function WalletLedgerToolbar({ title, placeholder }: WalletLedgerToolbarProps) {
 return (
  <div className={cn('mb-6 tn-flex-col-row-sm tn-items-center-sm justify-between gap-4')}>
   <h2 className={cn('tn-text-xl-2xl-md font-black tn-text-primary uppercase italic tracking-tighter flex items-center gap-3')}>
    <Clock className={cn('w-5 h-5 tn-text-accent')} />
    {title}
   </h2>
   <div className={cn('flex items-center gap-2 px-4 py-2 border tn-border tn-focus-within-border-accent-50 transition-all duration-300 rounded-xl tn-overlay min-h-11')}>
    <Search className={cn('w-4 h-4 tn-text-muted')} />
    <input
     type="text"
     placeholder={placeholder}
     className={cn('bg-transparent border-none tn-text-overline tn-text-primary w-full tn-w-full-48-sm tn-placeholder min-h-11 py-2')}
    />
   </div>
  </div>
 );
}
