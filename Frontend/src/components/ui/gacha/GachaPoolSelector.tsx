'use client';

/* 
 * Import các hooks và thành phần UI cần thiết.
 * - useState: Quản lý trạng thái hiển thị thông báo ghi chú Pity.
 * - Info: Icon dấu chấm hỏi/thông tin từ thư viện lucide-react.
 */
import { memo, useState } from 'react';
import { Info } from 'lucide-react';
import { cn } from '@/lib/utils';
import Button from '@/shared/components/ui/Button';
import GlassCard from '@/shared/components/ui/GlassCard';
import Badge from '@/shared/components/ui/Badge';
import type { GachaPool } from '@/shared/infrastructure/gacha/gachaTypes';

/**
 * Định nghĩa các nhãn ngôn ngữ.
 */
interface GachaPoolSelectorLabels {
  pull: string;
  pull10x: string;
  pulling: string;
  pity: string;
  pityRuleHint: string;
}

/**
 * Thuộc tính của thành phần GachaPoolSelector.
 * Lưu ý: onPull giờ đây nhận tham số 'count' (số lượt quay).
 */
interface GachaPoolSelectorProps {
  pools: GachaPool[];
  locale: string;
  selectedPoolCode: string;
  isPulling: boolean;
  labels: GachaPoolSelectorLabels;
  onSelectPool: (poolCode: string) => void;
  onPull: (count: number) => void;
}

/**
 * Các hàm hỗ trợ bản địa hóa.
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
 * GachaPoolSelectorComponent - Cho phép người dùng chọn Pool và thực hiện lệnh Pull 1x hoặc 10x.
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
  /* State lưu trữ việc có đang hiển thị ghi chú Pity hay không */
  const [showPityNote, setShowPityNote] = useState<string | null>(null);

  return (
    <div className={cn('grid grid-cols-1 gap-6 lg:grid-cols-3')}>
      {pools.map((pool) => {
        const isSelected = selectedPoolCode === pool.code;
        const poolName = localizePoolName(pool, locale);
        
        /* Tính toán giá quay 10 lần với ưu đãi giảm giá 10% (x10 * 0.9 = x9) */
        const cost10x = Math.floor(pool.costAmount * 10 * 0.9);
        
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
            {/* Header: Tên và Chi phí cơ bản */}
            <div className={cn('flex flex-col px-6 pt-6')}>
              <div className="mb-2 flex items-center justify-between gap-2">
                <h3 className={cn('text-lg font-black tracking-tight tn-text-primary')}>
                  {poolName}
                </h3>
                <Badge variant={isSelected ? 'success' : 'default'} size="sm">
                  {`${pool.costAmount} ${pool.costCurrency.toUpperCase()}`}
                </Badge>
              </div>
              <p className={cn('tn-text-secondary text-xs font-medium leading-relaxed line-clamp-2 mb-4')}>
                {localizePoolDescription(pool, locale)}
              </p>
            </div>

            {/* Phần hiển thị Pity và Nút quay */}
            <div className={cn('mt-auto bg-white/[0.03] p-6 pt-4 border-t tn-border-soft')}>
              <div className="mb-3 flex items-center justify-between">
                <div className="flex items-center gap-1.5">
                  <span className={cn('tn-text-muted text-[10px] font-black uppercase tracking-widest')}>
                    {labels.pity}
                  </span>
                  
                  {/* 
                      Dấu "!" (Info icon) dùng để hiển thị ghi chú Pity.
                      Người dùng click vào để xem quy tắc chắc chắn rơi Legendary/Mythic.
                  */}
                  <button 
                    type="button"
                    onClick={(e) => {
                        e.stopPropagation();
                        setShowPityNote(showPityNote === pool.code ? null : pool.code);
                    }}
                    className={cn(
                        'flex h-4 w-4 items-center justify-center rounded-full transition-colors',
                        showPityNote === pool.code ? 'bg-emerald-500/20 text-emerald-400' : 'tn-text-muted hover:tn-text-primary'
                    )}
                  >
                    <Info size={12} strokeWidth={3} />
                  </button>
                </div>
                
                <span className={cn('font-bold text-sm', isSelected ? 'tn-text-success' : 'tn-text-primary')}>
                  {pool.userCurrentPity} / {pool.hardPityCount}
                </span>
              </div>

              {/* Ghi chú Pity hiển thị linh hoạt khi người dùng nhấn dấu ! */}
              {showPityNote === pool.code && (
                <div className="animate-in fade-in slide-in-from-top-1 duration-300 mb-4 rounded-xl bg-emerald-500/10 p-3 border border-emerald-500/20">
                  <p className="tn-text-success text-[10px] font-bold leading-relaxed">
                    Khi đầy tiến trình chắc chắn rớt item Legendary/Mythic. 
                    Nhận vật phẩm Legendary/Mythic sẽ đặt lại tiến trình này.
                  </p>
                </div>
              )}
              
              <div className="mb-4 h-1.5 w-full overflow-hidden rounded-full bg-slate-900/50">
                <div 
                  className={cn(
                    'h-full transition-all duration-700 ease-out',
                    isSelected ? 'bg-emerald-500 shadow-[0_0_10px_rgba(16,185,129,0.5)]' : 'bg-slate-500/40'
                  )}
                  style={{ width: `${(pool.userCurrentPity / pool.hardPityCount) * 100}%` }}
                />
              </div>

              {/* 
                  Khu vực Nút Quay: Đã cập nhật hỗ trợ Quay 1x và Quay 10x.
                  Hiển thị giá tiền trực tiếp trên nút để người dùng dễ quan sát.
              */}
              {isSelected && (
                <div className="grid grid-cols-2 gap-3 animate-in fade-in zoom-in duration-300">
                  {/* Nút Quay 1 lần */}
                  <Button 
                    variant="secondary" 
                    size="md"
                    className="flex flex-col gap-0 min-h-[54px]"
                    onClick={(e) => {
                      e.stopPropagation();
                      onPull(1);
                    }} 
                    isLoading={isPulling}
                  >
                    <span className="text-[11px]">{labels.pull}</span>
                    <span className="text-[9px] opacity-60 font-bold uppercase tracking-widest mt-0.5">
                      {pool.costAmount} {pool.costCurrency}
                    </span>
                  </Button>

                  {/* Nút Quay 10 lần với ưu đãi giảm giá 10% */}
                  <Button 
                    variant="brand" 
                    size="md"
                    className="flex flex-col gap-0 min-h-[54px] relative overflow-hidden"
                    onClick={(e) => {
                      e.stopPropagation();
                      onPull(10);
                    }} 
                    isLoading={isPulling}
                  >
                    <span className="text-[11px]">{labels.pull10x}</span>
                    <span className="text-[9px] opacity-90 font-bold uppercase tracking-widest mt-0.5">
                      {cost10x} {pool.costCurrency}
                    </span>
                    {/* Badge giảm giá nhỏ để kích thích người dùng */}
                    <div className="absolute -right-5 -top-1 rotate-45 bg-amber-400 px-5 py-0.5 shadow-lg">
                        <span className="text-[8px] font-black text-black">-10%</span>
                    </div>
                  </Button>
                </div>
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
