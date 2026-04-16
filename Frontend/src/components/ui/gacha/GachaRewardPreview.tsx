'use client';

/* 
 * Import các thành phần hỗ trợ UI và tối ưu hóa.
 * - memo: Tránh render lại danh sách phần thưởng khi props không đổi.
 * - Image: Hiển thị icon phần thưởng tối ưu từ Next.js.
 * - GlassCard: Thành phần chủ đạo tạo giao diện kính mờ.
 * - Badge: Hiển thị độ hiếm (stars) một cách chuyên nghiệp.
 */
import { memo } from 'react';
import Image from 'next/image';
import { cn, formatCardStat } from '@/lib/utils';

import { gachaRewardKinds } from '@/shared/infrastructure/gacha/gachaConstants';
import type { GachaPoolRewardRate } from '@/shared/infrastructure/gacha/gachaTypes';
import Badge from '@/shared/components/ui/Badge';

/**
 * Props cho thành phần GachaRewardPreview.
 */
interface GachaRewardPreviewProps {
  rewards: GachaPoolRewardRate[];
  locale: string;
  emptyLabel: string;
}

/**
 * Hàm hỗ trợ bản địa hóa tên phần thưởng.
 */
function localizeRewardName(reward: GachaPoolRewardRate, locale: string): string {
  if (locale === 'en') return reward.nameEn;
  if (locale === 'zh') return reward.nameZh;
  return reward.nameVi;
}

/**
 * Hàm tạo nhãn giá trị (Số lượng hoặc Đơn vị tiền tệ) hiển thị cho phần thưởng.
 */
function rewardValueLabel(reward: GachaPoolRewardRate): string {
  if (reward.kind === gachaRewardKinds.currency) {
    return `${reward.amount ?? 0} ${reward.currency?.toUpperCase() ?? ''}`.trim();
  }
  return `x${reward.quantityGranted}`;
}

/**
 * GachaRewardPreviewComponent - Hiển thị danh sách các phần thưởng có thể nhận được và xác suất tương ứng.
 */
function GachaRewardPreviewComponent({ rewards, locale, emptyLabel }: GachaRewardPreviewProps) {
  
  /* Xử lý trạng thái trống: Hiển thị nếu không có dữ liệu phần thưởng */
  if (!rewards.length) {
    return (
      <div className={cn('rounded-3xl border border-dashed tn-border-soft p-10 text-center')}>
        <p className={cn('tn-text-muted text-sm font-medium italic opacity-60')}>
          {emptyLabel}
        </p>
      </div>
    );
  }

  return (
    <div className={cn('grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-3')}>
      {rewards.map((reward, index) => {
        const isRare = reward.rarity.toString().includes('5');
        const isEpic = reward.rarity.toString().includes('4');
        
        return (
          <article
            key={`${reward.kind}_${reward.rarity}_${reward.itemCode ?? reward.currency ?? index}`}
            className={cn(
              'group relative flex items-center justify-between rounded-2xl border tn-border-soft bg-white/[0.02] p-4 transition-all duration-300',
              'hover:border-white/10 hover:bg-white/[0.05] hover:shadow-[var(--shadow-card)]'
            )}
          >
            {/* 
                Khối thông tin bên trái: Bao gồm hình ảnh/icon và tên phần thưởng.
            */}
            <div className={cn('flex min-w-0 items-center gap-4')}>
              <div className={cn(
                  'relative h-12 w-12 shrink-0 overflow-hidden rounded-xl border tn-border-soft bg-slate-900/40 transition-transform duration-500 group-hover:scale-110',
                  isRare ? 'border-amber-500/30' : isEpic ? 'border-purple-500/30' : ''
              )}>
                {reward.iconUrl ? (
                  <Image
                    src={reward.iconUrl}
                    alt={localizeRewardName(reward, locale)}
                    fill
                    sizes="48px"
                    className={cn('object-cover p-1')}
                  />
                ) : (
                  <div className="flex h-full w-full items-center justify-center text-xs opacity-20">✦</div>
                )}
              </div>
              
              <div className={cn('min-w-0')}>
                <p className={cn('truncate text-sm font-black tn-text-primary tracking-tight mb-1')}>
                  {localizeRewardName(reward, locale)}
                </p>
                <div className="flex items-center gap-2">
                    <Badge variant={isRare ? 'amber' : isEpic ? 'purple' : 'default'} size="sm" className="px-1.5 py-0">
                        {reward.rarity}★
                    </Badge>
                    <span className={cn('tn-text-muted text-[10px] font-bold uppercase tracking-widest opacity-60')}>
                        {rewardValueLabel(reward)}
                    </span>
                </div>
              </div>
            </div>

            {/* 
                Khối thông tin bên phải: Hiển thị xác suất (Probability).
            */}
            <div className={cn('pl-4 text-right')}>
              <div className={cn('flex flex-col')}>
                <p className={cn('text-sm font-black tracking-tight', isRare ? 'tn-text-warning' : 'tn-text-primary')}>
                  {formatCardStat(reward.probabilityPercent)}%
                </p>
                <p className={cn('tn-text-muted text-[9px] font-bold uppercase tracking-[0.1em] opacity-50')}>
                  Xác suất
                </p>
              </div>
            </div>
          </article>
        );
      })}
    </div>
  );
}

const GachaRewardPreview = memo(GachaRewardPreviewComponent);
GachaRewardPreview.displayName = 'GachaRewardPreview';

export default GachaRewardPreview;
