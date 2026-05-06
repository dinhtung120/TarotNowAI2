'use client';

import { memo } from 'react';
import { Package2 } from 'lucide-react';
import Image from 'next/image';
import { cn } from '@/lib/utils';
import type { InventoryItem } from '@/features/inventory/shared/inventoryTypes';
import GlassCard from '@/shared/ui/GlassCard';
import Badge from '@/shared/ui/Badge';
import {
 getLocalizedInventoryText,
 inventoryRarityConfig,
 inventoryRarityGlowClass,
 resolveInventoryRarity,
} from '@/features/inventory/browse/inventoryItemCardView';

interface InventoryItemCardProps {
  item: InventoryItem;
  locale: string;
  onSelect: (item: InventoryItem) => void;
}

function InventoryItemCardComponent({ item, locale, onSelect }: InventoryItemCardProps) {
  const text = getLocalizedInventoryText(item, locale);
  const rarity = resolveInventoryRarity(item.rarity);
  const config = inventoryRarityConfig[rarity];

  return (
    <div className={cn('group relative')}>
      <div className={cn('tn-inventory-glow-shell absolute -inset-0.5 opacity-0 blur-xl transition-opacity duration-700 group-hover:opacity-40', inventoryRarityGlowClass[rarity])} />

      <GlassCard
        variant="interactive"
        padding="none"
        onClick={() => onSelect(item)}
        className={cn('tn-inventory-card-surface relative flex flex-col overflow-hidden transition-all duration-500', config.glowClass)}
      >
        <div className={cn('absolute inset-0 translate-x-[-100%] bg-gradient-to-r from-transparent via-white/[0.03] to-transparent transition-transform duration-1000 group-hover:translate-x-[100%]')} />

        <div className={cn('p-6')}>
          <div className={cn('mb-6 flex items-start justify-between gap-4')}>
            <div className={cn('min-w-0 flex-1 space-y-2')}>
              <h3 className={cn('truncate text-lg font-black tracking-tight tn-text-primary transition-colors duration-500 group-hover:lunar-metallic-text')}>
                {text.name}
              </h3>
              <p className={cn('line-clamp-2 text-xs font-medium leading-relaxed tn-text-secondary opacity-60')}>{text.description}</p>
            </div>

            <div className={cn('flex h-14 w-14 shrink-0 items-center justify-center overflow-hidden rounded-2xl border bg-gradient-to-br from-white/[0.05] to-transparent shadow-xl transition-all duration-500 group-hover:scale-110 group-hover:shadow-violet-500/10', rarity === 'legendary' ? 'border-amber-500/30' : rarity === 'epic' ? 'border-purple-500/30' : 'tn-border-soft')}>
              <div className={cn('relative flex h-full w-full items-center justify-center')}>
                {item.iconUrl ? (
                  <Image src={item.iconUrl} alt={text.name} width={36} height={36} className={cn('h-9 w-9 object-contain transition-transform duration-500 group-hover:scale-110')} />
                ) : (
                  <Package2 className={cn('h-6 w-6 opacity-40 transition-opacity group-hover:opacity-80')} />
                )}
              </div>
            </div>
          </div>

          <div className={cn('flex items-center justify-between border-t pt-4 tn-border-soft')}>
            <Badge variant={config.badgeVariant} size="sm" className={cn('border-none px-3 py-1 text-[9px] font-black uppercase tracking-[0.1em] shadow-sm')}>
              {config.label}
            </Badge>

            <div className={cn('flex items-center gap-2')}>
              <span className={cn('tn-text-muted text-[10px] font-black uppercase tracking-[0.15em] opacity-40')}>Owned:</span>
              <div className={cn('flex h-7 min-w-[32px] items-center justify-center rounded-lg border px-2 tn-border-soft bg-white/[0.03]')}>
                <span className={cn('text-sm font-black tn-text-primary')}>x{item.quantity}</span>
              </div>
            </div>
          </div>
        </div>
      </GlassCard>
    </div>
  );
}

const InventoryItemCard = memo(InventoryItemCardComponent);
InventoryItemCard.displayName = 'InventoryItemCard';

export default InventoryItemCard;
