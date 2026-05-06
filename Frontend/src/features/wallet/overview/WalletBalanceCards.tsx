import { Coins, Gem } from 'lucide-react';
import type { WalletBalance } from '@/features/wallet/shared/types';
import { cn } from '@/lib/utils';
import { WalletAssetCard } from './WalletAssetCard';

interface WalletBalanceCardsProps {
 balance: WalletBalance | null;
 locale: string;
 labels: {
  diamondLabel: string;
  diamondDesc: string;
  frozenLabel: (amount: string) => string;
  goldLabel: string;
  goldDesc: string;
  goldNote: string;
 };
}

export function WalletBalanceCards({ balance, locale, labels }: WalletBalanceCardsProps) {
 return (
  <div className={cn('tn-grid-cols-1-2-md gap-8 mb-16 animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-300')}>
   <WalletAssetCard
    icon={Gem}
    iconColorClass="text-[var(--purple-accent)]"
    iconGlowClass="bg-[var(--purple-accent)]/10 border-[var(--purple-accent)]/20"
    panelGlowClass="from-[var(--purple-accent)]/10 to-transparent"
    watermarkIconColorClass="text-[var(--purple-accent)]"
    label={labels.diamondLabel}
    description={labels.diamondDesc}
    value={balance?.diamondBalance.toLocaleString(locale) ?? '...'}
    footer={labels.frozenLabel((balance?.frozenDiamondBalance ?? 0).toLocaleString(locale))}
    isDiamond
   />
   <WalletAssetCard
    icon={Coins}
    iconColorClass="text-[var(--warning)]"
    iconGlowClass="bg-[var(--warning)]/10 border-[var(--warning)]/20"
    panelGlowClass="from-[var(--warning)]/10 to-transparent"
    watermarkIconColorClass="text-[var(--warning)]"
    label={labels.goldLabel}
    description={labels.goldDesc}
    value={balance?.goldBalance.toLocaleString(locale) ?? '...'}
    footer={labels.goldNote}
   />
  </div>
 );
}
