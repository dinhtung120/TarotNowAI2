import { Gem } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import type { WalletBalance } from '@/features/wallet/domain/types';
import { cn } from '@/lib/utils';

interface DepositBalanceCardProps {
 balance: WalletBalance | null;
 locale: string;
 exchangeRate: number;
 balanceLabel: string;
}

export function DepositBalanceCard({
 balance,
 locale,
 exchangeRate,
 balanceLabel,
}: DepositBalanceCardProps) {
 return (
  <GlassCard className={cn('flex flex-col sm:flex-row sm:items-center justify-between gap-4 sm:gap-6')}>
   <div className={cn('flex items-center gap-4')}>
    <div className={cn('w-10 h-10 rounded-xl bg-[var(--purple-accent)]/10 flex items-center justify-center border border-[var(--purple-accent)]/20')}>
     <Gem className={cn('w-5 h-5 text-[var(--purple-accent)]')} />
    </div>
    <div className={cn('space-y-1')}>
     <div className={cn('text-[9px] font-black uppercase tracking-widest tn-text-muted')}>{balanceLabel}</div>
     <div className={cn('text-xl font-black tn-text-primary italic')}>
      {(balance?.diamondBalance ?? 0).toLocaleString(locale)} 💎
     </div>
    </div>
   </div>
   <div className={cn('text-[10px] font-black uppercase tracking-widest tn-text-muted self-start sm:self-auto')}>
    {exchangeRate.toLocaleString(locale)} VND / 💎
   </div>
  </GlassCard>
 );
}
