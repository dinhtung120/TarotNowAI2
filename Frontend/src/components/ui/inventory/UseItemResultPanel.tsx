'use client';

import { cn } from '@/lib/utils';
import type { UseInventoryItemEffectSummary, UseInventoryCardStatSnapshot } from '@/shared/infrastructure/inventory/inventoryTypes';

interface UseItemResultPanelLabels {
  effectType: string;
  rolledValue: string;
  beforeAfter: string;
  done: string;
}

interface UseItemResultPanelProps {
  effectSummary: UseInventoryItemEffectSummary;
  labels: UseItemResultPanelLabels;
  onDone: () => void;
}

function formatValue(effectSummary: UseInventoryItemEffectSummary): string {
  const value = Number(effectSummary.rolledValue ?? 0).toFixed(2);
  if (effectSummary.effectType === 'power' || effectSummary.effectType === 'defense') {
    return `+${value}%`;
  }

  if (effectSummary.effectType === 'exp') {
    return `+${value} EXP`;
  }

  return `+${value}`;
}

function resolveEffectTypeLabel(effectType: string): string {
  if (effectType === 'power') {
    return 'ATK %';
  }

  if (effectType === 'defense') {
    return 'DEF %';
  }

  if (effectType === 'exp') {
    return 'EXP';
  }

  if (effectType === 'free_draw') {
    return 'Free Draw';
  }

  return effectType.toUpperCase();
}

function formatStat(value: number): string {
  return Number.isFinite(value) ? value.toFixed(2) : '0.00';
}

function buildRows(before?: UseInventoryCardStatSnapshot | null, after?: UseInventoryCardStatSnapshot | null) {
  if (!before || !after) {
    return [] as Array<{ label: string; before: string; after: string }>;
  }

  return [
    { label: 'Level', before: `${before.level}`, after: `${after.level}` },
    {
      label: 'EXP',
      before: `${formatStat(before.currentExp)} / ${formatStat(before.expToNextLevel)}`,
      after: `${formatStat(after.currentExp)} / ${formatStat(after.expToNextLevel)}`,
    },
    { label: 'ATK', before: formatStat(before.totalAtk), after: formatStat(after.totalAtk) },
    { label: 'DEF', before: formatStat(before.totalDef), after: formatStat(after.totalDef) },
  ];
}

export default function UseItemResultPanel({ effectSummary, labels, onDone }: UseItemResultPanelProps) {
  const rows = buildRows(effectSummary.before, effectSummary.after);

  return (
    <section className={cn('space-y-3 rounded-2xl border border-emerald-500/20 bg-emerald-500/5 p-4')}>
      <div className={cn('grid grid-cols-2 gap-2 text-sm')}>
        <p className={cn('tn-text-muted')}>{labels.effectType}</p>
        <p className={cn('text-right font-bold tn-text-primary uppercase')}>
          {resolveEffectTypeLabel(effectSummary.effectType)}
        </p>
        <p className={cn('tn-text-muted')}>{labels.rolledValue}</p>
        <p className={cn('text-right font-bold text-emerald-300')}>{formatValue(effectSummary)}</p>
      </div>
      {rows.length > 0 ? (
        <div className={cn('space-y-2 rounded-xl border tn-border-soft bg-black/20 p-3')}>
          <p className={cn('tn-text-muted text-[10px] font-black tracking-widest uppercase')}>{labels.beforeAfter}</p>
          {rows.map((row) => (
            <div key={row.label} className={cn('grid grid-cols-3 gap-2 text-xs')}>
              <span className={cn('tn-text-muted')}>{row.label}</span>
              <span className={cn('text-right tn-text-secondary')}>{row.before}</span>
              <span className={cn('text-right font-bold tn-text-primary')}>{row.after}</span>
            </div>
          ))}
        </div>
      ) : null}
      <button
        type="button"
        onClick={onDone}
        className={cn('w-full rounded-xl bg-emerald-500/15 py-2 text-sm font-black text-emerald-200 transition hover:bg-emerald-500/25')}
      >
        {labels.done}
      </button>
    </section>
  );
}
