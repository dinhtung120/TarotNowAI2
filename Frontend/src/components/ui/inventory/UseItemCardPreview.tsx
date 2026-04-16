'use client';

import Image from 'next/image';
import { cn, formatCardStat } from '@/lib/utils';
import type { CardOption } from '@/components/ui/inventory/UseItemCardSelector';

interface UseItemCardPreviewProps {
  card: CardOption | null;
  label: string;
}

export default function UseItemCardPreview({ card, label }: UseItemCardPreviewProps) {
  if (!card) {
    return null;
  }

  return (
    <section className={cn('rounded-2xl border tn-border-soft bg-white/[0.02] p-3')}>
      <p className={cn('tn-text-muted mb-2 text-[10px] font-black tracking-widest uppercase')}>
        {label}
      </p>
      <div className={cn('flex items-center gap-3')}>
        <div className={cn('relative h-16 w-12 overflow-hidden rounded-lg border tn-border-soft')}>
          {card.imageUrl ? (
            <Image src={card.imageUrl} alt={card.name} fill className={cn('object-cover')} sizes="48px" />
          ) : (
            <div className={cn('flex h-full w-full items-center justify-center text-xs tn-text-muted')}>?</div>
          )}
        </div>
        <div className={cn('min-w-0 flex-1')}>
          <p className={cn('truncate text-sm font-bold tn-text-primary')}>{card.name}</p>
          {card.stats ? (
            <p className={cn('text-xs tn-text-muted')}>
              Lv {card.stats.level} · ATK {formatCardStat(card.stats.totalAtk)} · DEF {formatCardStat(card.stats.totalDef)} · EXP {formatCardStat(card.stats.currentExp)} / {formatCardStat(card.stats.expToNextLevel)}
            </p>
          ) : null}
        </div>
      </div>
    </section>
  );
}
