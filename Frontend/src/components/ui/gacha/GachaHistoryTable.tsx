'use client';

import { memo } from 'react';
import { cn } from '@/lib/utils';
import type { GachaHistoryEntry, GachaHistoryReward } from '@/shared/infrastructure/gacha/gachaTypes';

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

function GachaHistoryTableComponent({ entries, locale, labels }: GachaHistoryTableProps) {
  if (!entries.length) {
    return (
      <div className={cn('rounded-xl border border-dashed border-slate-300 p-4 text-sm text-slate-600 dark:border-slate-700 dark:text-slate-300')}>
        {labels.empty}
      </div>
    );
  }

  return (
    <div className={cn('space-y-3')}>
      {entries.map((entry) => (
        <article key={entry.pullOperationId} className={cn('rounded-xl border border-slate-200 bg-white p-4 dark:border-slate-700 dark:bg-slate-900')}>
          <div className={cn('flex flex-wrap items-center justify-between gap-2')}>
            <p className={cn('text-sm font-semibold text-slate-900 dark:text-slate-100')}>{entry.poolCode.toUpperCase()}</p>
            <p className={cn('text-xs text-slate-500 dark:text-slate-300')}>{new Date(entry.createdAtUtc).toLocaleString(locale)}</p>
          </div>
          <p className={cn('mt-1 text-xs text-slate-600 dark:text-slate-300')}>
            {labels.pullCount}: {entry.pullCount} • {entry.pityBefore} → {entry.pityAfter}
          </p>
          <p className={cn('mt-1 text-xs font-medium text-amber-700 dark:text-amber-300')}>
            {entry.wasPityReset ? labels.pityReset : labels.pityStayed}
          </p>
          <div className={cn('mt-3 grid grid-cols-1 gap-2 sm:grid-cols-2')}>
            {entry.rewards.map((reward, index) => (
              <div
                key={`${entry.pullOperationId}_${index}`}
                className={cn('rounded-lg border border-slate-200 px-3 py-2 text-sm dark:border-slate-700')}
              >
                <p className={cn('font-medium text-slate-900 dark:text-slate-100')}>{localizeRewardName(reward, locale)}</p>
                <p className={cn('text-xs text-slate-500 dark:text-slate-300')}>
                  {reward.rarity} • {reward.kind === 'currency' ? `${reward.amount ?? 0} ${reward.currency?.toUpperCase() ?? ''}` : `x${reward.quantityGranted}`}
                </p>
              </div>
            ))}
          </div>
        </article>
      ))}
    </div>
  );
}

const GachaHistoryTable = memo(GachaHistoryTableComponent);
GachaHistoryTable.displayName = 'GachaHistoryTable';

export default GachaHistoryTable;

