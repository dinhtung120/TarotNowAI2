'use client';

/* 
 * Import các hooks và thư viện cốt lõi.
 * - useCallback: Tối ưu hóa hàm xử lý sự kiện quay gacha.
 * - useMemo: Quản lý và xử lý lỗi từ nhiều nguồn dữ liệu.
 * - useState: Quản lý Pool được chọn và trạng thái Modal kết quả.
 */
import { useCallback, useMemo, useState } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { cn } from '@/lib/utils';

/* 
 * Import các thành phần giao diện (UI Components).
 */
import GlassCard from '@/shared/components/ui/GlassCard';
import SectionHeader from '@/shared/components/ui/SectionHeader';
import GachaPoolSelector from '@/components/ui/gacha/GachaPoolSelector';
import GachaRewardPreview from '@/components/ui/gacha/GachaRewardPreview';
import GachaResultModal from '@/components/ui/gacha/GachaResultModal';

/* 
 * Import hạ tầng dữ liệu Gacha.
 */
import { useGacha } from '@/shared/infrastructure/gacha/useGacha';
import { usePullGacha } from '@/shared/infrastructure/gacha/usePullGacha';
import type { PullGachaResult } from '@/shared/infrastructure/gacha/gachaTypes';

/**
 * Hàm hỗ trợ bản địa hóa văn bản dựa trên ngôn ngữ người dùng.
 */
function localizeText(value: { nameVi: string; nameEn: string; nameZh: string }, locale: string): string {
  if (locale === 'en') return value.nameEn;
  if (locale === 'zh') return value.nameZh;
  return value.nameVi;
}

/**
 * GachaPageClient - Thành phần chính điều khiển trải nghiệm quay Gacha.
 */
export default function GachaPageClient() {
  const locale = useLocale();
  const t = useTranslations('Gacha');
  
  /* Quản lý Pool người dùng đang chủ động chọn */
  const [selectedPoolCode, setSelectedPoolCode] = useState('');
  
  /* Quản lý dữ liệu kết quả sau khi quay để hiển thị lên Modal */
  const [resultModalData, setResultModalData] = useState<PullGachaResult | null>(null);
  const [isResultOpen, setIsResultOpen] = useState(false);

  /* 
   * Lấy toàn bộ dữ liệu cần thiết cho trang Gacha: 
   * - Danh sách Pools.
   * - Tỷ lệ rơi (Odds) của Pool đang chọn.
   * - Xem trước lịch sử (History Preview).
   */
  const { poolsQuery, poolOddsQuery, historyPreviewQuery, resolvedPoolCode } = useGacha({
    selectedPoolCode,
    historyPreviewSize: 6,
  });

  /* Hook thực hiện hành động quay Gacha (Mutation) */
  const pullMutation = usePullGacha();
  
  /* Xác định Pool Code đang hoạt động (ưu tiên pool người dùng chọn, nếu không lấy pool mặc định từ hệ thống) */
  const activePoolCode = selectedPoolCode || resolvedPoolCode;

  const pools = poolsQuery.data ?? [];
  const rewards = poolOddsQuery.data?.rewards ?? [];
  const historyItems = historyPreviewQuery.data?.items ?? [];

  /**
   * onPull - Xử lý sự kiện khi người dùng nhấn nút Quay.
   * Sử dụng mutateAsync để đợi kết quả trước khi mở Modal hiển thị.
   */
  const onPull = useCallback(async () => {
    if (!activePoolCode) return;
    try {
      const result = await pullMutation.mutateAsync({ poolCode: activePoolCode, count: 1 });
      setResultModalData(result);
      setIsResultOpen(true);
    } catch (error) {
      console.error('Lỗi khi thực hiện quay gacha:', error);
    }
  }, [activePoolCode, pullMutation]);

  /**
   * queryError - Tập hợp lỗi từ các API Query để hiển thị tập trung.
   */
  const queryError = useMemo(() => {
    const candidates = [poolsQuery.error, poolOddsQuery.error, historyPreviewQuery.error];
    return candidates.find((candidate) => candidate instanceof Error) as Error | undefined;
  }, [historyPreviewQuery.error, poolOddsQuery.error, poolsQuery.error]);

  return (
    <div className={cn('mx-auto w-full max-w-6xl space-y-10 px-4 pb-24 pt-28 sm:px-6')}>
      
      {/* 
          Phần Header trang chủ Gacha.
          Sử dụng hiệu ứng Metallic cho tiêu đề chính để tạo điểm nhấn Magic.
      */}
      <header className={cn('space-y-3')}>
        <h1 className={cn('lunar-metallic-text text-3xl font-black uppercase tracking-[0.2em] sm:text-5xl')}>
          {t('title')}
        </h1>
        <p className={cn('tn-text-secondary text-base font-medium max-w-2xl leading-relaxed')}>
          {t('subtitle')}
        </p>
      </header>

      {/* Hiển thị lỗi hệ thống nếu có thông qua một thông báo tinh tế */}
      {queryError ? (
        <div className={cn('rounded-2xl border border-red-500/20 bg-red-500/5 px-6 py-4 text-sm tn-text-danger')}>
          <p className="font-bold uppercase tracking-widest mb-1">Thông báo hệ thống:</p>
          {queryError.message}
        </div>
      ) : null}

      {/* 
          GachaPoolSelector: Khu vực chọn Pool và thực hiện hành động quay.
          Đây là trung tâm tương tác của trang.
      */}
      <section className="relative">
        <div className="absolute -left-20 -top-20 h-64 w-64 rounded-full bg-emerald-500/5 blur-[100px]" />
        <GachaPoolSelector
          pools={pools}
          locale={locale}
          selectedPoolCode={activePoolCode}
          isPulling={pullMutation.isPending}
          labels={{ 
            pull: t('pull1x'), 
            pulling: t('pulling'), 
            pity: t('pityProgress'), 
            pityRuleHint: t('pityRuleHint') 
          }}
          onSelectPool={setSelectedPoolCode}
          onPull={onPull}
        />
      </section>

      {/* 
          Khu vực hiển thị tỷ lệ rơi (Odds).
          Bao bọc trong GlassCard để tạo sự phân cấp giao diện.
      */}
      <GlassCard variant="default" padding="lg" className="space-y-6">
        <SectionHeader 
            title={t('oddsTitle')} 
            className="mb-0"
        />
        <GachaRewardPreview 
            rewards={rewards} 
            locale={locale} 
            emptyLabel={t('emptyOdds')} 
        />
      </GlassCard>

      {/* 
          Khu vực hiển thị lịch sử quay gần đây.
          Cung cấp cái nhìn nhanh về các kết quả mà người dùng đã nhận được.
      */}
      <section className={cn('space-y-6')}>
        <div className={cn('flex items-center justify-between gap-4')}>
          <SectionHeader title={t('historyTitle')} className="mb-0" />
          <Link 
            href="/gacha/history" 
            className={cn('text-xs font-black uppercase tracking-[0.15em] tn-text-accent hover:brightness-110 transition-all')}
          >
            {t('viewAllHistory')} →
          </Link>
        </div>
        
        <div className={cn('grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3')}>
          {historyItems.map((item) => {
            const firstReward = item.rewards[0];
            const rewardLabel = firstReward ? localizeText(firstReward, locale) : t('emptyHistory');
            const isRare = firstReward?.rarity?.toString().includes('5');
            const isEpic = firstReward?.rarity?.toString().includes('4');

            return (
              <GlassCard 
                key={item.pullOperationId} 
                variant="elevated" 
                padding="sm" 
                className={cn(
                    'group relative overflow-hidden transition-all duration-500 hover:-translate-y-1',
                    isRare ? 'border-amber-500/20 shadow-[0_0_20px_rgba(245,158,11,0.05)]' : 
                    isEpic ? 'border-purple-500/20' : 'tn-border-soft'
                )}
              >
                <div className="flex flex-col">
                    <p className={cn(
                        'truncate text-sm font-black tracking-tight mb-1',
                        isRare ? 'text-amber-500' : isEpic ? 'text-purple-400' : 'tn-text-primary'
                    )}>
                        {rewardLabel}
                    </p>
                    <div className={cn('flex items-center gap-2 tn-text-muted text-[10px] font-bold uppercase tracking-wider')}>
                        <span className="opacity-70">{item.poolCode}</span>
                        <span className="h-1 w-1 rounded-full bg-current opacity-30" />
                        <span>{item.pullCount}x</span>
                        <span className="h-1 w-1 rounded-full bg-current opacity-30" />
                        <span className={cn(item.wasPityReset ? 'tn-text-warning' : '')}>
                            {item.wasPityReset ? t('historyPityResetShort') : t('historyPityStayedShort')}
                        </span>
                    </div>
                </div>
              </GlassCard>
            );
          })}
        </div>
      </section>

      {/* Modal hiển thị kết quả quay - Được tích hợp sẵn các hiệu ứng hoạt họa */}
      <GachaResultModal
        isOpen={isResultOpen}
        locale={locale}
        result={resultModalData}
        labels={{
          title: t('pullResultTitle'),
          pityTriggered: t('pityTriggered'),
          close: t('close'),
          rareDropAnimation: t('rareDropAnimation'),
        }}
        onClose={() => setIsResultOpen(false)}
      />
    </div>
  );
}
