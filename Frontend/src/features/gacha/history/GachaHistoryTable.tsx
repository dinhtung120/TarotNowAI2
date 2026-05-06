'use client';

import { memo } from 'react';
import { cn } from '@/lib/utils';
import type { GachaHistoryEntry, GachaHistoryReward } from '@/features/gacha/shared/gachaTypes';
import Badge from '@/shared/ui/Badge';
import GlassCard from '@/shared/ui/GlassCard';

interface GachaHistoryTableLabels {
  empty: string;
  pityReset: string;
  pityStayed: string;
  pullCount: string;
}

interface GachaHistoryTableProps {
  entries: GachaHistoryEntry[];
  locale: string;
  labels: GachaHistoryTableLabels;
}

function localizeRewardName(reward: GachaHistoryReward, locale: string): string {
  if (locale === 'en') return reward.nameEn;
  if (locale === 'zh') return reward.nameZh;
  return reward.nameVi;
}

function RewardChip({ entry, reward, index, locale }: { entry: GachaHistoryEntry; reward: GachaHistoryReward; index: number; locale: string }) {
  const isRare = reward.rarity.toString().includes('5');
  const isEpic = reward.rarity.toString().includes('4');

  return (
    <div key={`${entry.pullOperationId}_${index}`} className={cn('inline-flex items-center gap-1.5 rounded-lg border tn-border-soft bg-white/[0.02] px-2 py-1', isRare ? 'border-amber-500/30' : isEpic ? 'border-purple-500/30' : '')}>
      <span className={cn('text-[10px] font-black', isRare ? 'text-amber-500' : isEpic ? 'text-purple-400' : 'tn-text-muted')}>
        {reward.rarity}★
      </span>
      <span className={cn('max-w-[120px] truncate text-xs font-bold', isRare ? 'tn-text-warning' : isEpic ? 'text-purple-300' : 'tn-text-primary')}>
        {localizeRewardName(reward, locale)}
      </span>
      <span className={cn('tn-text-muted text-[10px]')}>
        {reward.kind === 'currency' ? `${reward.amount ?? 0}${reward.currency?.toUpperCase() ?? ''}` : `x${reward.quantityGranted}`}
      </span>
    </div>
  );
}

function GachaHistoryTableComponent({ entries, locale, labels }: GachaHistoryTableProps) {
  if (!entries.length) {
    return (
      <div className={cn('flex flex-col items-center justify-center rounded-3xl border border-dashed tn-border-soft p-12 text-center')}>
        <p className={cn('tn-text-muted text-sm font-medium italic')}>{labels.empty}</p>
      </div>
    );
  }

  return (
    <div className={cn('flex flex-col gap-3')}>
      {entries.map((entry) => (
        <article key={entry.pullOperationId} className={cn('group transition-all duration-300')}>
          <GlassCard variant="default" padding="none" className={cn('overflow-hidden border-white/5 bg-white/[0.01] transition-colors hover:bg-white/[0.03]')}>
            <div className={cn('flex flex-col justify-between gap-3 px-6 py-3 sm:flex-row sm:items-center')}>
              <div className={cn('flex shrink-0 items-center gap-4')}>
                <time className={cn('min-w-[110px] tn-text-secondary text-[10px] font-black uppercase tracking-widest opacity-50')}>
                  {new Date(entry.createdAtUtc).toLocaleString(locale, { month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit' })}
                </time>
                <Badge variant="purple" size="sm" className={cn('origin-left scale-90 font-black tracking-tighter')}>
                  {entry.poolCode.toUpperCase()}
                </Badge>
              </div>

              <div className={cn('flex min-w-0 flex-1 flex-wrap items-center gap-2 sm:justify-end')}>
                {entry.rewards.map((reward, index) => (
                  <RewardChip key={`${entry.pullOperationId}_${index}`} entry={entry} reward={reward} index={index} locale={locale} />
                ))}
              </div>
            </div>
          </GlassCard>
        </article>
      ))}
    </div>
  );
}

const GachaHistoryTable = memo(GachaHistoryTableComponent);
GachaHistoryTable.displayName = 'GachaHistoryTable';

export default GachaHistoryTable;
