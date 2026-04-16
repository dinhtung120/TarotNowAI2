'use client';

/* 
 * Import các hooks và thư viện cốt lõi.
 * - useCallback: Tối ưu hóa hàm xử lý sự kiện quay gacha để tránh render lại không cần thiết.
 * - useMemo: Quản lý và xử lý lỗi từ nhiều nguồn dữ liệu API tập trung.
 * - useState: Quản lý trạng thái Pool được chọn và hiển thị kết quả.
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
 * Import hạ tầng dữ liệu và Mutation xử lý Gacha.
 */
import { useGacha } from '@/shared/infrastructure/gacha/useGacha';
import { usePullGacha } from '@/shared/infrastructure/gacha/usePullGacha';
import type { PullGachaResult } from '@/shared/infrastructure/gacha/gachaTypes';

/**
 * Hàm hỗ trợ bản địa hóa văn bản dựa trên ngôn ngữ người dùng (VI/EN/ZH).
 */
function localizeText(value: { nameVi: string; nameEn: string; nameZh: string }, locale: string): string {
  if (locale === 'en') return value.nameEn;
  if (locale === 'zh') return value.nameZh;
  return value.nameVi;
}

/**
 * GachaPageClient - Thành phần chính điều khiển logic trang Quay thưởng (Gacha).
 * Phiên bản cập nhật hỗ trợ quay 10 lượt và quy tắc Pity dành riêng cho vật phẩm Legendary/Mythic.
 */
export default function GachaPageClient() {
  const locale = useLocale();
  const t = useTranslations('Gacha');
  
  /* Quản lý Pool người dùng đang chủ động chọn */
  const [selectedPoolCode, setSelectedPoolCode] = useState('');
  
  /* Quản lý dữ liệu kết quả sau khi quay (thưởng nhận được) để hiển thị lên Modal */
  const [resultModalData, setResultModalData] = useState<PullGachaResult | null>(null);
  const [isResultOpen, setIsResultOpen] = useState(false);

  /* 
   * Truy vấn toàn bộ dữ liệu cần thiết: Danh sách Pool, Tỷ lệ rơi, và Lịch sử preview.
   */
  const { poolsQuery, poolOddsQuery, historyPreviewQuery, resolvedPoolCode } = useGacha({
    selectedPoolCode,
    historyPreviewSize: 6,
  });

  /* Hook thực hiện hành động quay Gacha (POST request lên Server) */
  const pullMutation = usePullGacha();
  
  /* Pool đang được chọn (ưu tiên pool người dùng nhấn, fallback về pool mặc định) */
  const activePoolCode = selectedPoolCode || resolvedPoolCode;

  const pools = poolsQuery.data ?? [];
  const rewards = poolOddsQuery.data?.rewards ?? [];
  const historyItems = historyPreviewQuery.data?.items ?? [];

  /**
   * onPull - Xử lý sự kiện quay Gacha.
   * @param count - Số lượt quay người dùng chọn (1 hoặc 10). 
   * Khi quay 10 lượt, hệ thống sẽ tự động áp dụng mức giá ưu đãi ở phía Selector.
   */
  const onPull = useCallback(async (count: number = 1) => {
    if (!activePoolCode) return;
    try {
      const result = await pullMutation.mutateAsync({ poolCode: activePoolCode, count });
      setResultModalData(result);
      setIsResultOpen(true);
    } catch (error) {
      console.error('Lỗi khi thực hiện hành động quay:', error);
    }
  }, [activePoolCode, pullMutation]);

  /**
   * queryError - Tổng hợp các lỗi từ các API Query để thông báo cho người dùng.
   */
  const queryError = useMemo(() => {
    const candidates = [poolsQuery.error, poolOddsQuery.error, historyPreviewQuery.error];
    return candidates.find((candidate) => candidate instanceof Error) as Error | undefined;
  }, [historyPreviewQuery.error, poolOddsQuery.error, poolsQuery.error]);

  return (
    <div className={cn('mx-auto w-full max-w-6xl space-y-10 px-4 pb-24 pt-28 sm:px-6')}>
      
      {/* 
          Tiêu đề trang Gacha: Sử dụng font chữ Metallic tạo cảm giác huyền bí.
      */}
      <header className={cn('space-y-3')}>
        <h1 className={cn('lunar-metallic-text text-3xl font-black uppercase tracking-[0.2em] sm:text-5xl')}>
          {t('title')}
        </h1>
        <p className={cn('tn-text-secondary text-base font-medium max-w-2xl leading-relaxed')}>
          {t('subtitle')}
        </p>
      </header>

      {/* Hiển thị thông báo lỗi hệ thống nếu việc truy xuất dữ liệu thất bại */}
      {queryError ? (
        <div className={cn('rounded-2xl border border-red-500/20 bg-red-500/5 px-6 py-4 text-sm tn-text-danger')}>
          <p className="font-bold uppercase tracking-widest mb-1">Hệ thống ghi nhận lỗi:</p>
          {queryError.message}
        </div>
      ) : null}

      {/* 
          GachaPoolSelector: Khu vực chọn Pool và thực hiện lệnh Pull.
          Đã được cập nhật để bổ sung nút quay 10x và dấu "!" giải thích Pity.
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
            pityRuleHint: '' // Không hiển thị dòng chữ mô tả Pity cũ theo yêu cầu người dùng
          }}
          onSelectPool={setSelectedPoolCode}
          onPull={onPull}
        />
      </section>

      {/* 
          Khu vực hiển thị tỷ lệ rơi (Odds Preview).
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
          Lịch sử rút thưởng gần đây (Recent History).
          Tại đây, chúng tôi đã loại bỏ dòng trạng thái Reset Pity để giao diện gọn gàng hơn.
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
                        {/* Reset Pity được loại bỏ tại đây để đơn giản hóa giao diện */}
                    </div>
                </div>
              </GlassCard>
            );
          })}
        </div>
      </section>

      {/* Modal hiển thị khi có phần thưởng rơi ra */}
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
