'use client';

/* 
 * Import hooks và các thành phần UI cần thiết.
 * - memo: Tối ưu hóa hiệu năng bằng cách tránh render lại nếu props không thay đổi.
 * - GlassCard: Tạo hiệu ứng kính mờ cho từng mục lịch sử.
 * - Badge: Hiển thị các nhãn (pool code, độ hiếm) một cách đẹp mắt.
 */
import { memo } from 'react';
import { cn } from '@/lib/utils';
import type { GachaHistoryEntry, GachaHistoryReward } from '@/shared/infrastructure/gacha/gachaTypes';
import Badge from '@/shared/components/ui/Badge';
import GlassCard from '@/shared/components/ui/GlassCard';

/**
 * Định nghĩa giao diện cho các nhãn ngôn ngữ.
 */
interface GachaHistoryTableLabels {
  empty: string;
  pityReset: string;
  pityStayed: string;
  pullCount: string;
}

/**
 * Thuộc tính của thành phần GachaHistoryTable.
 */
interface GachaHistoryTableProps {
  entries: GachaHistoryEntry[];
  locale: string;
  labels: GachaHistoryTableLabels;
}

/**
 * Hàm hỗ trợ bản địa hóa tên phần thưởng dựa trên locale hiện tại.
 */
function localizeRewardName(reward: GachaHistoryReward, locale: string): string {
  if (locale === 'en') return reward.nameEn;
  if (locale === 'zh') return reward.nameZh;
  return reward.nameVi;
}

/**
 * Hàm lấy variant của Badge dựa trên cấp độ hiếm (rarity).
 * Giúp người dùng dễ dàng nhận diện độ hiếm qua màu sắc.
 */
function getRarityBadgeVariant(rarity: string | number) {
  const r = String(rarity);
  if (r.includes('5')) return 'amber'; // Cực hiếm (Vàng)
  if (r.includes('4')) return 'purple'; // Hiếm (Tím)
  return 'default'; // Phổ thông (Xám/Trong suốt)
}

/**
 * GachaHistoryTableComponent - Hiển thị danh sách lịch sử quay gacha.
 * Thay vì sử dụng bảng (table) truyền thống, chúng tôi sử dụng danh sách các thẻ (cards) 
 * để tối ưu hóa hiển thị trên di động và mang lại cảm giác hiện đại.
 */
function GachaHistoryTableComponent({ entries, locale, labels }: GachaHistoryTableProps) {
  
  /* Trường hợp không có dữ liệu: Hiển thị trạng thái trống thẩm mỹ */
  if (!entries.length) {
    return (
      <div className={cn('flex flex-col items-center justify-center rounded-3xl border border-dashed tn-border-soft p-12 text-center')}>
        <div className="mb-4 h-16 w-16 rounded-full bg-slate-500/10 flex items-center justify-center">
          <span className="text-2xl text-slate-500 opacity-50">✦</span>
        </div>
        <p className={cn('tn-text-muted text-sm font-medium italic')}>
          {labels.empty}
        </p>
      </div>
    );
  }

  return (
    <div className={cn('grid grid-cols-1 gap-6')}>
      {entries.map((entry) => (
        <article key={entry.pullOperationId} className="group transition-all duration-300">
          <GlassCard 
            variant="elevated" 
            padding="none" 
            className="border-white/5 bg-white/[0.02] hover:bg-white/[0.04] transition-colors"
          >
            {/* Phần Header của Card: Pool Code và Thời gian */}
            <div className={cn('flex flex-wrap items-center justify-between gap-4 border-b tn-border-soft px-6 py-4')}>
              <div className="flex items-center gap-3">
                <Badge variant="purple" size="md" className="font-black tracking-tighter">
                  {entry.poolCode.toUpperCase()}
                </Badge>
                <span className={cn('tn-text-muted text-[10px] font-bold uppercase tracking-widest')}>
                  ID: {entry.pullOperationId.slice(0, 8)}...
                </span>
              </div>
              <time className={cn('tn-text-secondary text-[11px] font-bold opacity-60')}>
                {new Date(entry.createdAtUtc).toLocaleString(locale)}
              </time>
            </div>

            {/* Nội dung chi tiết về số lượt quay và Pity */}
            <div className="px-6 py-5">
              <div className="mb-6 flex flex-wrap items-center gap-x-6 gap-y-2">
                <div className="flex flex-col shrink-0">
                  <span className={cn('tn-text-muted text-[9px] font-black uppercase tracking-widest mb-1')}>
                    {labels.pullCount}
                  </span>
                  <span className={cn('tn-text-primary text-lg font-black tracking-tight')}>
                    {entry.pullCount}
                  </span>
                </div>
                
                <div className="h-8 w-px bg-white/5" />
                
                <div className="flex flex-col">
                  <span className={cn('tn-text-muted text-[9px] font-black uppercase tracking-widest mb-1')}>
                    Tiến trình Pity
                  </span>
                  <div className="flex items-center gap-2">
                    <span className="tn-text-secondary text-sm font-bold">{entry.pityBefore}</span>
                    <span className="tn-text-muted text-xs">→</span>
                    <span className={cn('text-sm font-bold', entry.wasPityReset ? 'tn-text-warning' : 'tn-text-primary')}>
                      {entry.pityAfter}
                    </span>
                  </div>
                </div>

                <div className="ml-auto">
                    <Badge variant={entry.wasPityReset ? 'warning' : 'default'} size="sm">
                        {entry.wasPityReset ? labels.pityReset : labels.pityStayed}
                    </Badge>
                </div>
              </div>

              {/* Danh sách phần thưởng nhận được */}
              <div className={cn('grid grid-cols-1 gap-3 sm:grid-cols-2 lg:grid-cols-3')}>
                {entry.rewards.map((reward, index) => (
                  <div
                    key={`${entry.pullOperationId}_${index}`}
                    className={cn(
                      'flex items-center gap-3 rounded-2xl border tn-border-soft bg-white/[0.02] p-3 transition-all',
                      'hover:border-white/10 hover:bg-white/[0.05]'
                    )}
                  >
                    {/* Visual Indicator cho độ hiếm */}
                    <div className={cn(
                      'h-10 w-10 shrink-0 rounded-xl flex items-center justify-center font-black text-xs',
                      reward.rarity.toString().includes('5') ? 'bg-amber-500/20 text-amber-500' : 
                      reward.rarity.toString().includes('4') ? 'bg-purple-500/20 text-purple-400' : 
                      'bg-slate-500/20 text-slate-400'
                    )}>
                      {reward.rarity}★
                    </div>

                    <div className="flex flex-col min-w-0">
                      <p className={cn('truncate text-sm font-black tn-text-primary tracking-tight')}>
                        {localizeRewardName(reward, locale)}
                      </p>
                      <p className={cn('tn-text-muted text-[10px] font-bold uppercase tracking-wider')}>
                        {reward.kind === 'currency' 
                          ? `${reward.amount ?? 0} ${reward.currency?.toUpperCase() ?? ''}` 
                          : `Số lượng: ${reward.quantityGranted}`}
                      </p>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </GlassCard>
        </article>
      ))}
    </div>
  );
}

const GachaHistoryTable = memo(GachaHistoryTableComponent);
GachaHistoryTable.displayName = 'GachaHistoryTable';

export default GachaHistoryTable;
