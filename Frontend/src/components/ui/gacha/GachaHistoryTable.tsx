'use client';

/* 
 * Import các thành phần hỗ trợ UI.
 * - memo: Tránh render lại không cần thiết.
 * - GlassCard: Hiệu ứng kính mờ cho từng dòng lịch sử.
 * - Badge: Hiển thị tag Pool Code và cấp độ hiếm.
 */
import { memo } from 'react';
import { cn } from '@/lib/utils';
import type { GachaHistoryEntry, GachaHistoryReward } from '@/shared/infrastructure/gacha/gachaTypes';
import Badge from '@/shared/components/ui/Badge';
import GlassCard from '@/shared/components/ui/GlassCard';

/**
 * Interface cho các nhãn ngôn ngữ.
 */
interface GachaHistoryTableLabels {
    empty: string;
    pityReset: string;
    pityStayed: string;
    pullCount: string;
}

/**
 * Props của thành phần GachaHistoryTable.
 */
interface GachaHistoryTableProps {
    entries: GachaHistoryEntry[];
    locale: string;
    labels: GachaHistoryTableLabels;
}

/**
 * Bản địa hóa tên phần thưởng.
 */
function localizeRewardName(reward: GachaHistoryReward, locale: string): string {
    if (locale === 'en') return reward.nameEn;
    if (locale === 'zh') return reward.nameZh;
    return reward.nameVi;
}

/**
 * GachaHistoryTableComponent - Phiên bản tối giản.
 * Chỉ hiển thị: Thời gian, Loại vòng quay, và Danh sách phần thưởng trong 1-2 dòng.
 */
function GachaHistoryTableComponent({ entries, locale, labels }: GachaHistoryTableProps) {

    /* Xử lý khi không có dữ liệu */
    if (!entries.length) {
        return (
            <div className={cn('flex flex-col items-center justify-center rounded-3xl border border-dashed tn-border-soft p-12 text-center')}>
                <p className={cn('tn-text-muted text-sm font-medium italic')}>
                    {labels.empty}
                </p>
            </div>
        );
    }

    return (
        <div className={cn('flex flex-col gap-3')}>
            {entries.map((entry) => (
                <article key={entry.pullOperationId} className="group transition-all duration-300">
                    <GlassCard
                        variant="default"
                        padding="none"
                        className="border-white/5 bg-white/[0.01] hover:bg-white/[0.03] transition-colors overflow-hidden"
                    >
                        {/* 
                            Giao diện dòng đơn (Single Row Layout):
                            Kết hợp linh hoạt thông tin trên 1-2 dòng để tối ưu diện tích.
                        */}
                        <div className={cn('flex flex-col sm:flex-row sm:items-center justify-between gap-3 px-6 py-3')}>
                            
                            {/* Khối bên trái: Thời gian & Loại vòng quay */}
                            <div className="flex items-center gap-4 shrink-0">
                                <time className={cn('tn-text-secondary text-[10px] font-black uppercase tracking-widest opacity-50 min-w-[110px]')}>
                                    {new Date(entry.createdAtUtc).toLocaleString(locale, {
                                        month: 'numeric',
                                        day: 'numeric',
                                        hour: '2-digit',
                                        minute: '2-digit'
                                    })}
                                </time>
                                
                                <Badge variant="purple" size="sm" className="font-black tracking-tighter scale-90 origin-left">
                                    {entry.poolCode.toUpperCase()}
                                </Badge>
                            </div>

                            {/* Khối bên phải (hoặc dòng 2): Danh sách phần thưởng nhận được */}
                            <div className="flex flex-wrap items-center gap-2 sm:justify-end flex-1 min-w-0">
                                {entry.rewards.map((reward, index) => {
                                    const isRare = reward.rarity.toString().includes('5');
                                    const isEpic = reward.rarity.toString().includes('4');
                                    
                                    return (
                                        <div 
                                            key={`${entry.pullOperationId}_${index}`}
                                            className={cn(
                                                'inline-flex items-center gap-1.5 px-2 py-1 rounded-lg border tn-border-soft bg-white/[0.02]',
                                                isRare ? 'border-amber-500/30' : isEpic ? 'border-purple-500/30' : ''
                                            )}
                                        >
                                            <span className={cn(
                                                'text-[10px] font-black',
                                                isRare ? 'text-amber-500' : isEpic ? 'text-purple-400' : 'tn-text-muted'
                                            )}>
                                                {reward.rarity}★
                                            </span>
                                            <span className={cn(
                                                'text-xs font-bold truncate max-w-[120px]',
                                                isRare ? 'tn-text-warning' : isEpic ? 'text-purple-300' : 'tn-text-primary'
                                            )}>
                                                {localizeRewardName(reward, locale)}
                                            </span>
                                            <span className="tn-text-muted text-[10px]">
                                                {reward.kind === 'currency' 
                                                    ? `${reward.amount ?? 0}${reward.currency?.toUpperCase() ?? ''}` 
                                                    : `x${reward.quantityGranted}`}
                                            </span>
                                        </div>
                                    );
                                })}
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
