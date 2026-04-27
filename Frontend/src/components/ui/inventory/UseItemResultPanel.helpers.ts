import { formatCardStat } from '@/lib/utils';
import type { UseInventoryItemEffectSummary } from '@/shared/infrastructure/inventory/inventoryTypes';

export function formatRolledValue(effectSummary: UseInventoryItemEffectSummary): string {
  const value = formatCardStat(effectSummary.rolledValue ?? 0);
  if (effectSummary.effectType === 'power' || effectSummary.effectType === 'defense') return `+${value}%`;
  if (effectSummary.effectType === 'exp') return `+${value} EXP`;
  return `+${value}`;
}

export function resolveEffectTypeLabel(effectType: string): string {
  switch (effectType.toLowerCase()) {
    case 'power': return 'ATK %';
    case 'defense': return 'DEF %';
    case 'exp': return 'EXP';
    case 'free_draw': return 'Lượt xem bài';
    default: return effectType.toUpperCase();
  }
}

export function resolveAnimationDelayClass(index: number): string {
  const delay = Math.min(index * 100, 900);
  return `tn-anim-delay-${delay}`;
}

export function buildResultRows(summary: UseInventoryItemEffectSummary): Array<{ label: string; before: string; after: string }> {
  const { before, after, effectType, beforeValue, afterValue } = summary;
  const rows: Array<{ label: string; before: string; after: string }> = [];

  if (before && after) {
    if (after.level > before.level || after.currentExp > before.currentExp || effectType === 'exp') {
      rows.push({ label: 'Level', before: `${before.level}`, after: `${after.level}` });
      rows.push({ label: 'EXP', before: `${formatCardStat(before.currentExp)}`, after: `${formatCardStat(after.currentExp)}` });
    }

    if (effectType === 'power') {
      rows.push({ label: 'ATK', before: formatCardStat(before.totalAtk), after: formatCardStat(after.totalAtk) });
    }
    if (effectType === 'defense') {
      rows.push({ label: 'DEF', before: formatCardStat(before.totalDef), after: formatCardStat(after.totalDef) });
    }
  }

  if (beforeValue !== undefined && afterValue !== undefined && beforeValue !== null && afterValue !== null) {
    rows.push({ label: 'Số lượng', before: formatCardStat(beforeValue), after: formatCardStat(afterValue) });
  }

  return rows;
}
