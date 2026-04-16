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
    <GlassCard
      variant="interactive"
      padding="none"
      onClick={() => onSelect(item)}
      className={cn(
        'group relative flex flex-col transition-all duration-500 overflow-hidden',
        config.glowClass,
        'bg-white/[0.02] hover:bg-white/[0.05]'
      )}
    >
      {/* 
          Phần nội dung chính của vật phẩm:
          Bao gồm Tên, Mô tả và Icon đại diện.
      */}
      <div className="p-5">
        <div className="flex items-start justify-between gap-4 mb-4">
          <div className="min-w-0 space-y-1">
            <h3 className={cn('truncate text-base font-black tracking-tight tn-text-primary group-hover:tn-text-accent transition-colors')}>
              {text.name}
            </h3>
            <p className={cn('line-clamp-2 text-xs font-medium tn-text-secondary opacity-70 leading-relaxed')}>
              {text.description}
            </p>
          </div>
          
          {/* Icon vật phẩm: Được đặt trong một box kính nhỏ */}
          <div className={cn(
            'flex h-11 w-11 shrink-0 items-center justify-center rounded-xl border tn-border-soft bg-white/[0.03] transition-transform duration-500 group-hover:scale-110',
            item.rarity === 'legendary' ? 'border-amber-500/30' : 
            item.rarity === 'epic' ? 'border-purple-500/30' : ''
          )}>
            <Package2 className={cn('h-5 w-5 opacity-60')} />
          </div>
        </div>

        {/* 
            Phân bổ thông tin bổ sung:
            Độ hiếm dưới dạng Badge và số lượng vật phẩm hiện có.
        */}
        <div className="flex items-center justify-between">
          <Badge variant={config.badgeVariant} size="sm" className="font-black uppercase tracking-[0.05em]">
            {config.label}
          </Badge>
          
          <div className="flex items-center gap-1.5">
            <span className={cn('tn-text-muted text-[10px] font-black uppercase tracking-widest opacity-50')}>
              Số lượng:
            </span>
            <span className={cn('text-sm font-black tn-text-primary')}>
              x{item.quantity}
            </span>
          </div>
        </div>
      </div>
      
      {/* Hiệu ứng trang trí light-beam khi hover */}
      <div className="absolute inset-x-0 bottom-0 h-0.5 bg-gradient-to-r from-transparent via-current to-transparent opacity-0 transition-opacity group-hover:opacity-20" />
    </GlassCard>
  );
}

const InventoryItemCard = memo(InventoryItemCardComponent);
InventoryItemCard.displayName = 'InventoryItemCard';

export default InventoryItemCard;
