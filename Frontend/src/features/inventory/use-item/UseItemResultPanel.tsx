'use client';

import { cn } from '@/lib/utils';
import type { UseInventoryItemEffectSummary } from '@/features/inventory/shared/inventoryTypes';
import {
  buildResultRows,
  formatRolledValue,
  resolveAnimationDelayClass,
  resolveEffectTypeLabel,
} from '@/features/inventory/use-item/UseItemResultPanel.helpers';

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

function EmptyResultState() {
  return (
    <div className={cn('flex flex-col items-center justify-center space-y-3 py-10 text-center')}>
      <div className={cn('flex h-12 w-12 items-center justify-center rounded-full bg-emerald-500/10')}>
        <div className={cn('h-2 w-2 animate-ping rounded-full bg-emerald-500')} />
      </div>
      <div className={cn('space-y-1')}>
        <p className={cn('text-sm font-black uppercase tracking-wider tn-text-primary')}>Xử lý hoàn tất</p>
        <p className={cn('max-w-[200px] text-[10px] font-medium tn-text-muted opacity-60')}>
          Vật phẩm đã được sử dụng thành công. Không có thay đổi chỉ số hiển thị thêm.
        </p>
      </div>
    </div>
  );
}

export default function UseItemResultPanel({ effectSummaries, labels, onDone, onUseAgain }: UseItemResultPanelProps) {
  return (
    <div className={cn('flex flex-col gap-5')}>
      <div className={cn('tn-scrollbar-thin max-h-[320px] space-y-4 overflow-y-auto pr-1')}>
        {effectSummaries.length > 0 ? effectSummaries.map((summary, index) => {
          const rows = buildResultRows(summary);

          return (
            <section key={index} className={cn('relative overflow-hidden space-y-3 rounded-2xl border bg-white/[0.03] p-4 tn-border-soft animate-in fade-in slide-in-from-bottom-2 duration-500', resolveAnimationDelayClass(index))}>
              <div className={cn('absolute -left-1 -top-1 h-2 w-2 rounded-full bg-emerald-500 opacity-40 blur-[1px]')} />

              <div className={cn('flex items-center justify-between')}>
                <div className={cn('flex items-center gap-2')}>
                  <div className={cn('flex h-5 w-5 items-center justify-center rounded-full bg-emerald-500/10 text-[10px] font-black text-emerald-400')}>{index + 1}</div>
                  <span className={cn('tn-text-muted text-[10px] font-black uppercase tracking-[0.2em] opacity-40')}>{resolveEffectTypeLabel(summary.effectType)}</span>
                </div>
                <div className={cn('flex items-center gap-1.5')}>
                  <span className={cn('tn-text-muted text-[10px] font-black uppercase tracking-widest opacity-40')}>Rolled:</span>
                  <span className={cn('text-sm font-black text-emerald-400')}>{formatRolledValue(summary)}</span>
                </div>
              </div>

              {rows.length > 0 ? (
                <div className={cn('space-y-2 rounded-xl border border-white/5 bg-black/20 p-3')}>
                  <div className={cn('mb-1 grid grid-cols-3 gap-2 border-b border-white/5 pb-1')}>
                    <span className={cn('tn-text-muted text-[8px] font-black uppercase tracking-widest opacity-30')}>Stat</span>
                    <span className={cn('tn-text-muted text-right text-[8px] font-black uppercase tracking-widest opacity-30')}>Before</span>
                    <span className={cn('tn-text-muted text-right text-[8px] font-black uppercase tracking-widest opacity-30')}>After</span>
                  </div>
                  {rows.map((row) => (
                    <div key={row.label} className={cn('grid grid-cols-3 gap-2 text-xs')}>
                      <span className={cn('font-bold tn-text-secondary')}>{row.label}</span>
                      <span className={cn('tn-text-muted text-right text-[11px] font-medium line-through opacity-40')}>{row.before}</span>
                      <span className={cn('text-right font-black text-emerald-300 tn-text-primary')}>{row.after}</span>
                    </div>
                  ))}
                </div>
              ) : null}
            </section>
          );
        }) : <EmptyResultState />}
      </div>

      <div className={cn('flex items-center gap-3')}>
        <button type="button" onClick={onDone} className={cn('flex flex-1 items-center justify-center rounded-2xl border py-4 transition-all hover:bg-white/[0.08] active:scale-95 tn-border-soft bg-white/[0.03]')}>
          <span className={cn('tn-text-muted text-sm font-black uppercase tracking-[0.2em]')}>{labels.done}</span>
        </button>

        <button type="button" onClick={onUseAgain} className={cn('group relative flex flex-[1.5] items-center justify-center gap-2 overflow-hidden rounded-2xl bg-emerald-500/10 py-4 transition-all hover:bg-emerald-500/20 active:scale-95')}>
          <div className={cn('absolute inset-0 translate-x-[-100%] bg-gradient-to-r from-transparent via-emerald-500/10 to-transparent transition-transform duration-1000 group-hover:translate-x-[100%]')} />
          <span className={cn('text-sm font-black uppercase tracking-[0.2em] text-emerald-400')}>{labels.useAgain}</span>
        </button>
      </div>
    </div>
  );
}
