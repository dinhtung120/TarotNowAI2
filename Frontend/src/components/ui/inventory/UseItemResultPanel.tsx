'use client';

import { cn, formatCardStat } from '@/lib/utils';
import type { UseInventoryItemEffectSummary, UseInventoryCardStatSnapshot } from '@/shared/infrastructure/inventory/inventoryTypes';


interface UseItemResultPanelLabels {
  effectType: string;
  rolledValue: string;
  beforeAfter: string;
  done: string;
}

interface UseItemResultPanelProps {
  effectSummaries: UseInventoryItemEffectSummary[];
  labels: UseItemResultPanelLabels & { useAgain: string };
  onDone: () => void;
  onUseAgain: () => void;
}

function formatValue(effectSummary: UseInventoryItemEffectSummary): string {
  const value = formatCardStat(effectSummary.rolledValue ?? 0);
  if (effectSummary.effectType === 'power' || effectSummary.effectType === 'defense') {
    return `+${value}%`;
  }

  if (effectSummary.effectType === 'exp') {
    return `+${value} EXP`;
  }

  return `+${value}`;
}

function resolveEffectTypeLabel(effectType: string): string {
  switch (effectType.toLowerCase()) {
    case 'power': return 'ATK %';
    case 'defense': return 'DEF %';
    case 'exp': return 'EXP';
    case 'free_draw': return 'Lượt xem bài';
    default: return effectType.toUpperCase();
  }
}



function buildRows(summary: UseInventoryItemEffectSummary) {
  const { before, after, effectType, beforeValue, afterValue } = summary;
  const rows: Array<{ label: string; before: string; after: string }> = [];

  // Trường hợp cho Card (stat snapshots)
  if (before && after) {
    // Luôn hiện Level/EXP nếu có level up hoặc exp tăng
    if (after.level > before.level || after.currentExp > before.currentExp || effectType === 'exp') {
      rows.push({ label: 'Level', before: `${before.level}`, after: `${after.level}` });
      rows.push({
        label: 'EXP',
        before: `${formatCardStat(before.currentExp)}`,
        after: `${formatCardStat(after.currentExp)}`,
      });
    }

    // Lọc chỉ số ATK/DEF dựa trên item type
    if (effectType === 'power') {
      rows.push({ label: 'ATK', before: formatCardStat(before.totalAtk), after: formatCardStat(after.totalAtk) });
    }
    if (effectType === 'defense') {
      rows.push({ label: 'DEF', before: formatCardStat(before.totalDef), after: formatCardStat(after.totalDef) });
    }
  }

  // Trường hợp cho special items ( Reading Booster...)
  if (beforeValue !== undefined && afterValue !== undefined && beforeValue !== null && afterValue !== null) {
      rows.push({ 
        label: 'Số lượng', 
        before: formatCardStat(beforeValue), 
        after: formatCardStat(afterValue) 
      });
  }

  return rows;
}

export default function UseItemResultPanel({ effectSummaries, labels, onDone }: UseItemResultPanelProps) {
  return (
    <div className={cn('flex flex-col gap-5')}>
      <div className={cn('max-h-[320px] overflow-y-auto pr-1 tn-scrollbar-thin space-y-4')}>
        {effectSummaries.map((summary, index) => {
          const rows = buildRows(summary);
          return (
            <section 
              key={index} 
              className={cn(
                'relative overflow-hidden space-y-3 rounded-2xl border tn-border-soft bg-white/[0.03] p-4',
                'animate-in fade-in slide-in-from-bottom-2 duration-500'
              )}
              style={{ animationDelay: `${index * 100}ms` }}
            >
              <div className="absolute -left-1 -top-1 h-2 w-2 rounded-full bg-emerald-500 blur-[1px] opacity-40" />
              
              <div className={cn('flex items-center justify-between')}>
                <div className="flex items-center gap-2">
                  <div className="flex h-5 w-5 items-center justify-center rounded-full bg-emerald-500/10 text-[10px] font-black text-emerald-400">
                    {index + 1}
                  </div>
                  <span className={cn('text-[10px] font-black uppercase tracking-[0.2em] tn-text-muted opacity-40')}>
                    {resolveEffectTypeLabel(summary.effectType)}
                  </span>
                </div>
                <div className="flex items-center gap-1.5">
                  <span className="text-[10px] font-black uppercase tracking-widest tn-text-muted opacity-40">Rolled:</span>
                  <span className="text-sm font-black text-emerald-400">{formatValue(summary)}</span>
                </div>
              </div>

              {rows.length > 0 ? (
                <div className={cn('space-y-2 rounded-xl border border-white/5 bg-black/20 p-3')}>
                  <div className="grid grid-cols-3 gap-2 border-b border-white/5 pb-1 mb-1">
                     <span className="text-[8px] font-black uppercase tracking-widest tn-text-muted opacity-30">Stat</span>
                     <span className="text-right text-[8px] font-black uppercase tracking-widest tn-text-muted opacity-30">Before</span>
                     <span className="text-right text-[8px] font-black uppercase tracking-widest tn-text-muted opacity-30">After</span>
                  </div>
                  {rows.map((row) => (
                    <div key={row.label} className={cn('grid grid-cols-3 gap-2 text-xs')}>
                      <span className={cn('font-bold tn-text-secondary')}>{row.label}</span>
                      <span className={cn('text-right text-[11px] font-medium tn-text-muted line-through opacity-40')}>{row.before}</span>
                      <span className={cn('text-right font-black tn-text-primary text-emerald-300')}>
                        {row.after}
                      </span>
                    </div>
                  ))}
                </div>
              ) : null}
            </section>
          );
        })}
      </div>

      <div className="flex items-center gap-3">
        <button
          type="button"
          onClick={onDone}
          className={cn(
            'flex-1 flex items-center justify-center rounded-2xl bg-white/[0.03] border tn-border-soft py-4 transition-all hover:bg-white/[0.08] active:scale-95'
          )}
        >
          <span className={cn('text-sm font-black uppercase tracking-[0.2em] tn-text-muted')}>
            {labels.done}
          </span>
        </button>

        <button
          type="button"
          onClick={onUseAgain}
          className={cn(
            'flex-[1.5] group relative flex items-center justify-center gap-2 overflow-hidden rounded-2xl bg-emerald-500/10 py-4 transition-all hover:bg-emerald-500/20 active:scale-95'
          )}
        >
          <div className="absolute inset-0 translate-x-[-100%] bg-gradient-to-r from-transparent via-emerald-500/10 to-transparent transition-transform duration-1000 group-hover:translate-x-[100%]" />
          <span className={cn('text-sm font-black uppercase tracking-[0.2em] text-emerald-400')}>
            {labels.useAgain}
          </span>
        </button>
      </div>
    </div>
  );
}
