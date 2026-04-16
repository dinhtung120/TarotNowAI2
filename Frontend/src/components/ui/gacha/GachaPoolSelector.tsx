'use client';

/* 
 * Import các thành phần hỗ trợ tối ưu hiệu năng và giao diện.
 * - memo: Đảm bảo thành phần chỉ render lại khi props thay đổi thực sự.
 * - GlassCard: Thành phần chủ đạo tạo hiệu ứng kính mờ.
 * - Badge: Hiển thị thông tin trạng thái và giá cả.
 * - Button: Nút bấm thực hiện hành động chính.
 */
import { memo } from 'react';
import { cn } from '@/lib/utils';
import Button from '@/shared/components/ui/Button';
import GlassCard from '@/shared/components/ui/GlassCard';
import Badge from '@/shared/components/ui/Badge';
import type { GachaPool } from '@/shared/infrastructure/gacha/gachaTypes';

/**
 * Định nghĩa các nhãn ngôn ngữ cho bộ chọn Pool.
 */
interface GachaPoolSelectorLabels {
  pull: string;
  pulling: string;
  pity: string;
  pityRuleHint: string;
}

/**
 * Props cho thành phần GachaPoolSelector.
 */
interface GachaPoolSelectorProps {
  pools: GachaPool[];
  locale: string;
  selectedPoolCode: string;
  isPulling: boolean;
  labels: GachaPoolSelectorLabels;
  onSelectPool: (poolCode: string) => void;
  onPull: () => void;
}

/**
 * Các hàm hỗ trợ bản địa hóa tên và mô tả Pool.
 */
function localizePoolName(pool: GachaPool, locale: string): string {
  if (locale === 'en') return pool.nameEn;
  if (locale === 'zh') return pool.nameZh;
  return pool.nameVi;
}

function localizePoolDescription(pool: GachaPool, locale: string): string {
  if (locale === 'en') return pool.descriptionEn;
  if (locale === 'zh') return pool.descriptionZh;
  return pool.descriptionVi;
}

/**
 * GachaPoolSelectorComponent - Cho phép người dùng chọn các chiến dịch quay gacha (Pools) khác nhau.
 * Mỗi Pool được hiển thị dưới dạng một Interactive Glass Card.
 */
function GachaPoolSelectorComponent({
  pools,
  locale,
  selectedPoolCode,
  isPulling,
  labels,
  onSelectPool,
  onPull,
}: GachaPoolSelectorProps) {
  return (
    <div className={cn('grid grid-cols-1 gap-6 lg:grid-cols-3')}>
      {pools.map((pool) => {
        const isSelected = selectedPoolCode === pool.code;
        
        return (
          <GlassCard
            key={pool.code}
            variant={isSelected ? 'elevated' : 'interactive'}
            padding="none"
            onClick={() => onSelectPool(pool.code)}
            className={cn(
              'relative flex flex-col transition-all duration-500',
              isSelected ? 'ring-2 ring-emerald-500/40 shadow-[0_0_30px_rgba(16,185,129,0.1)]' : 'tn-panel-soft'
            )}
          >
            {/* 
                Header của thẻ Pool: Tên Pool và Badge chi phí.
            */}
            <div className={cn('flex flex-col px-6 pt-6')}>
              <div className="mb-2 flex items-center justify-between gap-2">
                <h3 className={cn('text-lg font-black tracking-tight tn-text-primary')}>
                  {localizePoolName(pool, locale)}
                </h3>
                <Badge variant={isSelected ? 'success' : 'default'} size="sm">
                  {`${pool.costAmount} ${pool.costCurrency.toUpperCase()}`}
                </Badge>
              </div>
              
              {/* Mô tả Pool: Cung cấp thông tin chi tiết về nội dung của Pool */}
              <p className={cn('tn-text-secondary text-xs font-medium leading-relaxed line-clamp-2 mb-4')}>
                {localizePoolDescription(pool, locale)}
              </p>
            </div>

            {/* 
                Phần hiển thị Pity (Tiến trình bảo hiểm):
                Sử dụng các thanh tiến trình mini hoặc chỉ số để thông báo cho người dùng.
            */}
            <div className={cn('mt-auto bg-white/[0.03] p-6 pt-4 border-t tn-border-soft')}>
              <div className="mb-3 flex items-center justify-between">
                <span className={cn('tn-text-muted text-[10px] font-black uppercase tracking-widest')}>
                  {labels.pity}
                </span>
                <span className={cn('font-bold text-sm', isSelected ? 'tn-text-success' : 'tn-text-primary')}>
                  {pool.userCurrentPity} / {pool.hardPityCount}
                </span>
              </div>
              
              {/* Thanh tiến trình Pity trực quan */}
              <div className="mb-4 h-1.5 w-full overflow-hidden rounded-full bg-slate-900/50">
                <div 
                  className={cn(
                    'h-full transition-all duration-700 ease-out',
                    isSelected ? 'bg-emerald-500 shadow-[0_0_10px_rgba(16,185,129,0.5)]' : 'bg-slate-500/40'
                  )}
                  style={{ width: `${(pool.userCurrentPity / pool.hardPityCount) * 100}%` }}
                />
              </div>

              {/* Lời nhắc/Gợi ý về luật Pity */}
              <p className={cn('mb-4 tn-text-muted text-[10px] font-medium italic opacity-60')}>
                {labels.pityRuleHint}
              </p>

              {/* 
                  Nút Quay: Chỉ hiển thị khi Pool đang được chọn.
                  Sử dụng variant 'brand' để thu hút sự chú ý.
              */}
              {isSelected && (
                <Button 
                  variant="brand" 
                  size="md"
                  fullWidth
                  className="animate-in fade-in zoom-in duration-300"
                  onClick={(e) => {
                    e.stopPropagation(); // Ngăn chặn trigger chọn pool lại
                    onPull();
                  }} 
                  isLoading={isPulling}
                >
                  {isPulling ? labels.pulling : labels.pull}
                </Button>
              )}
            </div>
          </GlassCard>
        );
      })}
    </div>
  );
}

const GachaPoolSelector = memo(GachaPoolSelectorComponent);
GachaPoolSelector.displayName = 'GachaPoolSelector';

export default GachaPoolSelector;
