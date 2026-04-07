import { Clock, Search } from 'lucide-react';
import { cn } from '@/lib/utils';

interface WalletLedgerToolbarProps {
 title: string;
 placeholder: string;
}

export function WalletLedgerToolbar({ title, placeholder }: WalletLedgerToolbarProps) {
 return (
  <div className={cn('mb-6 flex flex-col sm:flex-row sm:items-center justify-between gap-4')}>
   <h2 className={cn('text-xl md:text-2xl font-black tn-text-primary uppercase italic tracking-tighter flex items-center gap-3')}>
    <Clock className={cn('w-5 h-5 text-[var(--purple-accent)]')} />
    {title}
   </h2>
   <div className={cn('flex items-center gap-2 px-4 py-2 border tn-border focus-within:border-[var(--purple-accent)]/50 transition-all duration-300 rounded-xl tn-overlay min-h-11')}>
    <Search className={cn('w-4 h-4 tn-text-muted')} />
    <input
     type="text"
     placeholder={placeholder}
     className={cn('bg-transparent border-none text-[10px] font-black uppercase tracking-widest tn-text-primary w-full sm:w-48 placeholder:tn-text-muted min-h-11 py-2')}
    />
   </div>
  </div>
 );
}
