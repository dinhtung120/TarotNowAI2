import type { UserCollectionDto } from '@/features/collection/cards/actions/actions';
import { cn, formatCardStat } from '@/lib/utils';

interface CollectionDeckCardHeaderProps {
  userCard: UserCollectionDto | null;
}

export default function CollectionDeckCardHeader({
  userCard,
}: CollectionDeckCardHeaderProps) {
  if (!userCard) {
    return <div className={cn('h-4 w-full')} />;
  }

  const expToNext = Math.max(userCard.expToNextLevel || 0, 0);
  const currentExp = Math.max(userCard.currentExp || 0, 0);
  const progress = expToNext <= 0 ? 100 : Math.min(100, (currentExp / expToNext) * 100);

  return (
    <div className={cn('flex w-full flex-col gap-1.5')}>
      <div
        className={cn(
          'tn-text-primary flex items-center justify-between px-1 tn-text-10 font-black tracking-tighter uppercase',
        )}
      >
        <span>Lv. {userCard.level}</span>
        <span className={cn('tn-text-warning')}>
          {expToNext <= 0 ? 'MAX' : `${formatCardStat(currentExp)} / ${formatCardStat(expToNext)}`}
        </span>
      </div>
      <progress
        className={cn('tn-progress tn-progress-xs tn-progress-warning')}
        max={100}
        value={progress}
      />
    </div>
  );
}
