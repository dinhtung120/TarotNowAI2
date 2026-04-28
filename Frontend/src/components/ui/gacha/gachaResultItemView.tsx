'use client';

import { Coins, Gem, Package } from 'lucide-react';
import { cn } from '@/lib/utils';
import type { PullGachaReward } from '@/shared/infrastructure/gacha/gachaTypes';

export const gachaResultRarityConfig: Record<string, { shadow: string; border: string; bg: string; text: string }> = {
 mythic: { shadow: 'shadow-[0_0_20px_rgba(236,72,153,0.3)]', border: 'border-pink-500/50', bg: 'bg-pink-500/10', text: 'text-pink-400' },
 legendary: { shadow: 'shadow-[0_0_20px_rgba(245,158,11,0.3)]', border: 'border-amber-500/50', bg: 'bg-amber-500/10', text: 'text-amber-400' },
 '5': { shadow: 'shadow-[0_0_20px_rgba(245,158,11,0.3)]', border: 'border-amber-500/50', bg: 'bg-amber-500/10', text: 'text-amber-400' },
 epic: { shadow: 'shadow-[0_0_15px_rgba(168,85,247,0.2)]', border: 'border-purple-500/50', bg: 'bg-purple-500/10', text: 'text-purple-400' },
 '4': { shadow: 'shadow-[0_0_15px_rgba(168,85,247,0.2)]', border: 'border-purple-500/50', bg: 'bg-purple-500/10', text: 'text-purple-400' },
 rare: { shadow: 'shadow-[0_0_10px_rgba(14,165,233,0.1)]', border: 'border-sky-500/30', bg: 'bg-sky-500/5', text: 'text-sky-400' },
 '3': { shadow: 'shadow-[0_0_10px_rgba(14,165,233,0.1)]', border: 'border-sky-500/30', bg: 'bg-sky-500/5', text: 'text-sky-400' },
};

export function resolveGachaResultName(reward: PullGachaReward, locale: string) {
 return locale === 'en' ? reward.nameEn : locale === 'zh' ? reward.nameZh : reward.nameVi;
}

export function resolveGachaResultSummary(reward: PullGachaReward) {
 return reward.currency
  ? `${reward.amount ?? 0} ${reward.currency.toUpperCase()}`
  : `x${reward.quantityGranted}`;
}

export function RewardFallbackIcon({ reward }: { reward: PullGachaReward }) {
 const currency = reward.currency?.toLowerCase();
 return (
  <div className={cn('opacity-40')}>
   {currency === 'gold' ? <Coins size={28} /> : currency === 'diamond' ? <Gem size={28} /> : <Package size={28} />}
  </div>
 );
}
