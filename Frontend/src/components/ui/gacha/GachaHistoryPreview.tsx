import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { cn } from '@/lib/utils';
import GlassCard from '@/shared/components/ui/GlassCard';
import SectionHeader from '@/shared/components/ui/SectionHeader';
import type { GachaHistoryEntry } from '@/shared/infrastructure/gacha/gachaTypes';
import { localizeGachaName } from '@/components/ui/gacha/gachaLocalization';

interface GachaHistoryPreviewProps {
 historyItems: GachaHistoryEntry[];
 locale: string;
 title: string;
 viewAllLabel: string;
 emptyLabel: string;
}

function resolveRewardLabel(entry: GachaHistoryEntry, locale: string, emptyLabel: string): string {
 const firstReward = entry.rewards[0];
 if (!firstReward) {
  return emptyLabel;
 }

 return localizeGachaName(locale, firstReward);
}

function resolveRewardTone(entry: GachaHistoryEntry): 'rare' | 'epic' | 'normal' {
 const rarity = entry.rewards[0]?.rarity?.toString() ?? '';
 if (rarity.includes('5')) {
  return 'rare';
 }
 if (rarity.includes('4')) {
  return 'epic';
 }
 return 'normal';
}

export default function GachaHistoryPreview({
 historyItems,
 locale,
 title,
 viewAllLabel,
 emptyLabel,
}: GachaHistoryPreviewProps) {
 return (
  <section className={cn('space-y-6')}>
   <div className={cn('flex items-center justify-between gap-4')}>
    <SectionHeader title={title} className={cn('mb-0')} />
    <Link
     href="/gacha/history"
     className={cn('text-xs font-black uppercase tracking-[0.15em] tn-text-accent transition-all hover:brightness-110')}
    >
     {viewAllLabel} →
    </Link>
   </div>

   <div className={cn('grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3')}>
    {historyItems.map((item) => {
     const tone = resolveRewardTone(item);
     return (
      <GlassCard
       key={item.pullOperationId}
       variant="elevated"
       padding="sm"
       className={cn(
        'group relative overflow-hidden transition-all duration-500 hover:-translate-y-1',
        tone === 'rare'
         ? 'border-amber-500/20 shadow-[0_0_20px_rgba(245,158,11,0.05)]'
         : tone === 'epic'
          ? 'border-purple-500/20'
          : 'tn-border-soft',
       )}
      >
       <div className={cn('flex flex-col')}>
        <p
         className={cn(
          'mb-1 truncate text-sm font-black tracking-tight',
          tone === 'rare' ? 'text-amber-500' : tone === 'epic' ? 'text-purple-400' : 'tn-text-primary',
         )}
        >
         {resolveRewardLabel(item, locale, emptyLabel)}
        </p>
        <div className={cn('flex items-center gap-2 text-[10px] font-bold uppercase tracking-wider tn-text-muted')}>
         <span className={cn('opacity-70')}>{item.poolCode}</span>
         <span className={cn('h-1 w-1 rounded-full bg-current opacity-30')} />
         <span>{item.pullCount}x</span>
        </div>
       </div>
      </GlassCard>
     );
    })}
   </div>
  </section>
 );
}
