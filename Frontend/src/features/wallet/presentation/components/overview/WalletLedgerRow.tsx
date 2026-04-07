import { ArrowDownLeft, ArrowUpRight, Coins, Gem } from 'lucide-react';
import type { WalletTransaction } from '@/features/wallet/domain/types';
import { formatDate, formatTime } from '@/shared/utils/format/formatDateTime';
import { cn } from '@/lib/utils';

interface WalletLedgerRowProps {
 tx: WalletTransaction;
 locale: string;
 actionLabel: string;
 formattedType: string;
}

export function WalletLedgerRow({ tx, locale, actionLabel, formattedType }: WalletLedgerRowProps) {
 const isPositive = tx.amount > 0;

 return (
  <tr className={cn('transition-colors group')}>
   <td className={cn('px-6 py-5')}>
    <div className={cn('tn-text-11 font-bold tn-text-primary')}>{formatDate(tx.createdAt, locale)}</div>
    <div className={cn('tn-text-overline font-medium tn-text-secondary mt-0.5')}>
     {formatTime(tx.createdAt, locale, { hour: '2-digit', minute: '2-digit' })}
    </div>
   </td>
   <td className={cn('px-6 py-5')}>
    <div className={cn('flex items-center gap-2')}>
     {tx.currency.toLowerCase() === 'diamond' ? <Gem className={cn('w-4 h-4 tn-text-accent')} /> : <Coins className={cn('w-4 h-4 tn-text-warning')} />}
     <span className={cn('tn-text-overline tn-text-primary')}>{tx.currency}</span>
    </div>
   </td>
   <td className={cn('px-6 py-5 max-w-52')}>
    <div className={cn('tn-text-11 font-black tn-text-primary uppercase tracking-tighter truncate')}>{formattedType}</div>
    {tx.description ? <div className={cn('tn-text-overline tn-text-tertiary font-medium truncate mt-0.5')} title={tx.description}>{tx.description}</div> : null}
   </td>
   <td className={cn('px-6 py-5 text-right')}>
    <div className={cn('flex items-center justify-end gap-1 font-black text-sm italic', isPositive ? 'tn-text-success' : 'tn-text-secondary')}>
     {isPositive ? '+' : ''}
     {tx.amount.toLocaleString(locale)}
     {isPositive ? <ArrowUpRight className={cn('w-4 h-4')} /> : <ArrowDownLeft className={cn('w-4 h-4 opacity-50 tn-text-muted')} />}
    </div>
    <div className={cn('tn-text-9 font-black uppercase tracking-widest tn-text-secondary mt-1 transition-opacity')}>
     {actionLabel}
    </div>
   </td>
  </tr>
 );
}
