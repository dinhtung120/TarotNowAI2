import type { UserCollectionDto } from '@/features/collection/cards/actions/actions';
import CollectionZoomStatCard from '@/features/collection/cards/components/zoom-modal/CollectionZoomStatCard';
import { cn, formatCardStat } from '@/lib/utils';

interface CollectionZoomStatsGridProps {
  labels: { copiesLabel: string; levelLabel: string };
  userCard: UserCollectionDto;
}


export default function CollectionZoomStatsGrid({
  labels,
  userCard,
}: CollectionZoomStatsGridProps) {
  const expValue = userCard.expToNextLevel <= 0
    ? 'MAX'
    : `${formatCardStat(userCard.currentExp)} / ${formatCardStat(userCard.expToNextLevel)}`;

  const stats = [
    {
      label: labels.levelLabel,
      value: userCard.level,
      valueClassName: 'text-[var(--warning)]',
      wrapperClassName: 'tn-panel tn-border-soft',
    },
    {
      label: 'EXP',
      value: expValue,
      valueClassName: 'text-purple-300',
      wrapperClassName: 'tn-panel border-purple-500/20 bg-purple-500/5',
    },
    {
      label: 'ATK',
      value: `${formatCardStat(userCard.totalAtk)} (${formatCardStat(userCard.baseAtk)} + ${formatCardStat(userCard.bonusAtkPercent)}%)`,
      valueClassName:
        'text-red-400 drop-shadow-[0_0_8px_rgba(248,113,113,0.5)]',
      wrapperClassName: 'tn-panel border-red-500/20 bg-red-500/5',
    },
    {
      label: 'DEF',
      value: `${formatCardStat(userCard.totalDef)} (${formatCardStat(userCard.baseDef)} + ${formatCardStat(userCard.bonusDefPercent)}%)`,
      valueClassName:
        'text-blue-400 drop-shadow-[0_0_8px_rgba(96,165,250,0.5)]',
      wrapperClassName: 'tn-panel border-blue-500/20 bg-blue-500/5',
    },
  ];

  return (
    <div
      className={cn(
        'mx-auto mt-6 grid grid-cols-1 gap-3 md:mx-0 md:mt-8 md:grid-cols-2 lg:gap-4',
      )}
    >
      {stats.map((stat) => (
        <CollectionZoomStatCard key={stat.label} {...stat} />
      ))}
    </div>
  );
}
