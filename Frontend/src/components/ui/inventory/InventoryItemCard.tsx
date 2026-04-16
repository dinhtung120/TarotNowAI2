'use client';

/* 
 * Import các thành phần và hooks cần thiết.
 * - memo: Tối ưu hóa render danh sách vật phẩm lớn.
 * - Package2: Icon mặc định cho vật phẩm.
 * - GlassCard: Thành phần chủ đạo tạo hiệu ứng kính.
 * - Badge: Hiển thị độ hiếm và số lượng.
 */
import { memo } from 'react';
import { Package2 } from 'lucide-react';
import Image from 'next/image';
import { cn } from '@/lib/utils';
import type { InventoryItem, InventoryRarity } from '@/shared/infrastructure/inventory/inventoryTypes';
import GlassCard from '@/shared/components/ui/GlassCard';
import Badge from '@/shared/components/ui/Badge';

/**
 * Thuộc tính của thành phần InventoryItemCard.
 */
interface InventoryItemCardProps {
  item: InventoryItem;
  locale: string;
  onSelect: (item: InventoryItem) => void;
}

/**
 * Bản đồ ánh xạ độ hiếm sang các màu sắc đặc trưng của hệ thống.
 * Giúp người dùng nhận diện giá trị vật phẩm ngay từ cái nhìn đầu tiên.
 */
const rarityConfig: Record<InventoryRarity, { 
    badgeVariant: 'amber' | 'purple' | 'info' | 'success' | 'default',
    glowClass: string,
    label: string
}> = {
  common: { 
    badgeVariant: 'default', 
    glowClass: 'tn-border-soft', 
    label: 'Phổ thông' 
  },
  uncommon: { 
    badgeVariant: 'success', 
    glowClass: 'border-emerald-500/20 shadow-[0_0_15px_rgba(16,185,129,0.05)]', 
    label: 'Bất thường' 
  },
  rare: { 
    badgeVariant: 'info', 
    glowClass: 'border-sky-500/20 shadow-[0_0_15px_rgba(14,165,233,0.05)]', 
    label: 'Hiếm' 
  },
  epic: { 
    badgeVariant: 'purple', 
    glowClass: 'border-purple-500/20 shadow-[0_0_15px_rgba(168,85,247,0.05)]', 
    label: 'Sử thi' 
  },
  legendary: { 
    badgeVariant: 'amber', 
    glowClass: 'border-amber-500/20 shadow-[0_0_15px_rgba(245,158,11,0.05)]', 
    label: 'Huyền thoại' 
  },
};

/**
 * Kiểm tra xem độ hiếm có nằm trong danh sách đã cấu hình hay không.
 */
function isKnownRarity(rarity: string): rarity is InventoryRarity {
  return rarity in rarityConfig;
}

/**
 * Lấy văn bản đã bản địa hóa cho vật phẩm.
 */
function getLocalizedText(item: InventoryItem, locale: string) {
  if (locale === 'en') return { name: item.nameEn, description: item.descriptionEn };
  if (locale === 'zh') return { name: item.nameZh, description: item.descriptionZh };
  return { name: item.nameVi, description: item.descriptionVi };
}

/**
 * InventoryItemCard - Thành phần hiển thị một vật phẩm trong kho đồ.
 * Thiết kế theo dạng thẻ kính tương tác cao cấp.
 */
function InventoryItemCardComponent({ item, locale, onSelect }: InventoryItemCardProps) {
  const text = getLocalizedText(item, locale);
  const config = isKnownRarity(item.rarity) ? rarityConfig[item.rarity] : rarityConfig.common;

  return (
    <div className="group relative">
      {/* Glow Effect Layer */}
      <div className={cn(
        'absolute -inset-0.5 rounded-[2rem] opacity-0 blur-xl transition-opacity duration-700 group-hover:opacity-40',
        item.rarity === 'legendary' ? 'bg-amber-500' :
        item.rarity === 'epic' ? 'bg-purple-500' :
        item.rarity === 'rare' ? 'bg-sky-500' :
        item.rarity === 'uncommon' ? 'bg-emerald-500' : 'bg-slate-500'
      )} />

      <GlassCard
        variant="interactive"
        padding="none"
        onClick={() => onSelect(item)}
        className={cn(
          'relative flex flex-col overflow-hidden rounded-[1.8rem] border-t border-white/10 transition-all duration-500',
          config.glowClass,
          'bg-[#0a0a0c]/80 hover:bg-[#121216]/90'
        )}
      >
        {/* Shimmer effect on hover */}
        <div className="absolute inset-0 translate-x-[-100%] bg-gradient-to-r from-transparent via-white/[0.03] to-transparent transition-transform duration-1000 group-hover:translate-x-[100%]" />

        <div className="p-6">
          <div className="flex items-start justify-between gap-4 mb-6">
            <div className="min-w-0 flex-1 space-y-2">
              <h3 className={cn(
                'truncate text-lg font-black tracking-tight tn-text-primary transition-colors duration-500',
                'group-hover:lunar-metallic-text'
              )}>
                {text.name}
              </h3>
              <p className={cn('line-clamp-2 text-xs font-medium tn-text-secondary leading-relaxed opacity-60')}>
                {text.description}
              </p>
            </div>
            
            {/* Professional Icon Box */}
            <div className={cn(
              'flex h-14 w-14 shrink-0 items-center justify-center rounded-2xl border tn-border-soft overflow-hidden',
              'bg-gradient-to-br from-white/[0.05] to-transparent shadow-xl transition-all duration-500 group-hover:scale-110 group-hover:shadow-violet-500/10',
              item.rarity === 'legendary' ? 'border-amber-500/30' : 
              item.rarity === 'epic' ? 'border-purple-500/30' : ''
            )}>
              <div className="relative h-full w-full flex items-center justify-center">
                {item.iconUrl ? (
                  <Image
                    src={item.iconUrl} 
                    alt={text.name}
                    width={36}
                    height={36}
                    unoptimized
                    className={cn('h-9 w-9 object-contain transition-transform duration-500 group-hover:scale-110')}
                  />
                ) : (
                  <Package2 className={cn('h-6 w-6 opacity-40 transition-opacity group-hover:opacity-80')} />
                )}
                {item.rarity === 'legendary' && (
                  <div className="absolute -right-1 -top-1 flex h-4 w-4 animate-bounce items-center justify-center">
                    <div className="h-1.5 w-1.5 rounded-full bg-amber-400 blur-[1px]" />
                  </div>
                )}
              </div>
            </div>
          </div>

          <div className="flex items-center justify-between border-t tn-border-soft pt-4">
            <Badge 
              variant={config.badgeVariant} 
              size="sm" 
              className="px-3 py-1 font-black uppercase tracking-[0.1em] text-[9px] border-none shadow-sm"
            >
              {config.label}
            </Badge>
            
            <div className="flex items-center gap-2">
              <span className={cn('tn-text-muted text-[10px] font-black uppercase tracking-[0.15em] opacity-40')}>
                Owned:
              </span>
              <div className="flex h-7 min-w-[32px] items-center justify-center rounded-lg bg-white/[0.03] px-2 border tn-border-soft">
                <span className={cn('text-sm font-black tn-text-primary')}>
                  x{item.quantity}
                </span>
              </div>
            </div>
          </div>
        </div>
        
        {/* Bottom light beam */}
        <div className={cn(
          'absolute inset-x-0 bottom-0 h-[1px] bg-gradient-to-r from-transparent via-current to-transparent opacity-0 transition-opacity duration-500 group-hover:opacity-30',
          item.rarity === 'legendary' ? 'text-amber-500' : 'text-violet-500'
        )} />
      </GlassCard>
    </div>
  );
}

const InventoryItemCard = memo(InventoryItemCardComponent);
InventoryItemCard.displayName = 'InventoryItemCard';

export default InventoryItemCard;
