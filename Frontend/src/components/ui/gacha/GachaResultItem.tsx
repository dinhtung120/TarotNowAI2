'use client';

/* 
 * Import các thành phần hỗ trợ UI.
 * - Package, Coins, Gem: Các icon dự phòng nếu vật phẩm thiếu ảnh.
 * - Image: Tối ưu hóa việc hiển thị ảnh từ server.
 */
import { memo, useState } from 'react';
import { Package, Coins, Gem } from 'lucide-react';
import Image from 'next/image';
import { cn } from '@/lib/utils';
import type { PullGachaReward } from '@/shared/infrastructure/gacha/gachaTypes';

/**
 * Props của thành phần GachaResultItem.
 */
interface GachaResultItemProps {
  reward: PullGachaReward;
  locale: string;
}

/**
 * Bản đồ cấu hình màu sắc và hiệu ứng dựa trên phẩm cấp (Rarity).
 */
const rarityConfig: Record<string, { shadow: string; border: string; bg: string; text: string }> = {
  mythic:    { shadow: 'shadow-[0_0_20px_rgba(236,72,153,0.3)]', border: 'border-pink-500/50', bg: 'bg-pink-500/10', text: 'text-pink-400' },
  legendary: { shadow: 'shadow-[0_0_20px_rgba(245,158,11,0.3)]', border: 'border-amber-500/50', bg: 'bg-amber-500/10', text: 'text-amber-400' },
  '5':       { shadow: 'shadow-[0_0_20px_rgba(245,158,11,0.3)]', border: 'border-amber-500/50', bg: 'bg-amber-500/10', text: 'text-amber-400' },
  epic:      { shadow: 'shadow-[0_0_15px_rgba(168,85,247,0.2)]', border: 'border-purple-500/50', bg: 'bg-purple-500/10', text: 'text-purple-400' },
  '4':       { shadow: 'shadow-[0_0_15px_rgba(168,85,247,0.2)]', border: 'border-purple-500/50', bg: 'bg-purple-500/10', text: 'text-purple-400' },
  rare:      { shadow: 'shadow-[0_0_10px_rgba(14,165,233,0.1)]', border: 'border-sky-500/30',  bg: 'bg-sky-500/5',  text: 'text-sky-400' },
  '3':       { shadow: 'shadow-[0_0_10px_rgba(14,165,233,0.1)]', border: 'border-sky-500/30',  bg: 'bg-sky-500/5',  text: 'text-sky-400' },
};

/**
 * Thành phần hiển thị phần thưởng Gacha đơn lẻ với hiệu ứng Premium.
 */
function GachaResultItemComponent({ reward, locale }: GachaResultItemProps) {
  const [imageError, setImageError] = useState(false);
  
  const normalizedRarity = String(reward.rarity).toLowerCase();
  const config = rarityConfig[normalizedRarity] || { shadow: '', border: 'tn-border-soft', bg: 'bg-white/[0.02]', text: 'tn-text-primary' };

  /* Xác định tên phần thưởng theo ngôn ngữ */
  const name = locale === 'en' ? reward.nameEn : locale === 'zh' ? reward.nameZh : reward.nameVi;
  
  /* Xác định nhãn hiển thị số lượng hoặc trị giá */
  const summary = reward.currency 
    ? `${reward.amount ?? 0} ${reward.currency.toUpperCase()}` 
    : `x${reward.quantityGranted}`;

  return (
    <article className={cn(
      'group relative flex items-center gap-4 rounded-3xl border p-4 transition-all duration-500 overflow-hidden',
      config.border,
      config.bg,
      config.shadow,
      'hover:bg-white/[0.05] hover:scale-[1.02]'
    )}>
      {/* 
          Vùng hiển thị Icon vật phẩm.
          Kết hợp cơ chế dự phòng (fallback) nếu ảnh từ server không tải được.
      */}
      <div className={cn(
        'relative flex h-16 w-16 shrink-0 items-center justify-center rounded-2xl border tn-border-soft bg-black/20 overflow-hidden',
        config.border
      )}>
        {reward.iconUrl && !imageError ? (
          <Image
            src={reward.iconUrl}
            alt={name}
            fill
            className="object-cover transition-transform duration-700 group-hover:scale-125"
            onError={() => setImageError(true)}
            unoptimized
          />
        ) : (
          /* Biểu tượng dự phòng tùy theo loại phần thưởng */
          <div className="opacity-40">
            {reward.currency?.toLowerCase() === 'gold' ? <Coins size={28} /> :
             reward.currency?.toLowerCase() === 'diamond' ? <Gem size={28} /> :
             <Package size={28} />}
          </div>
        )}
      </div>

      {/* Thông tin chi tiết vật phẩm: Tên, Phẩm cấp và Số lượng */}
      <div className="min-w-0 flex-1 space-y-1">
        <h4 className={cn('truncate text-sm font-black tracking-tight tn-text-primary')}>
          {name}
        </h4>
        <div className="flex items-center gap-2">
          <span className={cn('text-[10px] font-black uppercase tracking-widest', config.text)}>
            {reward.rarity} Star
          </span>
          <span className="h-1 w-1 rounded-full bg-white/20" />
          <span className="tn-text-secondary text-[11px] font-bold">
            {summary}
          </span>
        </div>
      </div>
      
      {/* Hiệu ứng trang trí light-beam huyền bí khi hover */}
      <div className="absolute inset-x-0 bottom-0 h-[1px] bg-gradient-to-r from-transparent via-current to-transparent opacity-0 transition-opacity duration-500 group-hover:opacity-30" />
    </article>
  );
}

export const GachaResultItem = memo(GachaResultItemComponent);
