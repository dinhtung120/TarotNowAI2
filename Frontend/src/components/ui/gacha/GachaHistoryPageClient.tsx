'use client';

/* 
 * Import các hooks và thư viện cần thiết.
 * - useMemo: Tối ưu hóa việc tính toán tổng số trang.
 * - useState: Quản lý số trang hiện tại để phân trang.
 * - useLocale, useTranslations: Quản lý đa ngôn ngữ (i18n).
 */
import { useMemo, useState } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

/* 
 * Import các thành phần giao diện (UI Components).
 * - Button: Nút bấm được thiết kế riêng cho hệ thống.
 * - GlassCard: Thành phần tạo hiệu ứng lớp kính mờ (Glassmorphism), đặc trưng của TarotNow.
 * - GachaHistoryTable: Thành phần hiển thị danh sách lịch sử quay.
 */
import Button from '@/shared/components/ui/Button';
import GlassCard from '@/shared/components/ui/GlassCard';
import GachaHistoryTable from '@/components/ui/gacha/GachaHistoryTable';

/* 
 * Danh mục infrastructure (hạ tầng) để lấy dữ liệu.
 */
import { useGachaHistory } from '@/shared/infrastructure/gacha/useGachaHistory';

/**
 * Hằng số định nghĩa số lượng bản ghi trên một trang.
 * Việc sử dụng hằng số giúp dễ dàng thay đổi cấu hình hiển thị sau này.
 */
const DEFAULT_PAGE_SIZE = 20;

/**
 * GachaHistoryPageClient - Đại diện cho toàn bộ logic và giao diện của trang lịch sử quay Gacha.
 * @returns JSX.Element
 */
export default function GachaHistoryPageClient() {
  const locale = useLocale();
  const t = useTranslations('Gacha');
  
  /* Quản lý trạng thái trang hiện tại (mặc định là trang 1) */
  const [page, setPage] = useState(1);
  
  /* 
   * Gọi Hook useGachaHistory để lấy dữ liệu từ API.
   * Dữ liệu sẽ tự động được làm mới khi 'page' thay đổi nhờ React Query.
   */
  const historyQuery = useGachaHistory({ page, pageSize: DEFAULT_PAGE_SIZE });

  /**
   * Tính toán tổng số trang dựa trên 'totalCount' từ API.
   * Sử dụng useMemo để đảm bảo chỉ tính toán lại khi dữ liệu API thay đổi.
   */
  const totalPages = useMemo(() => {
    const totalCount = historyQuery.data?.totalCount ?? 0;
    return Math.max(1, Math.ceil(totalCount / DEFAULT_PAGE_SIZE));
  }, [historyQuery.data?.totalCount]);

  /**
   * Giao diện chính của trang.
   * Toàn bộ nội dung được bọc trong một container có khoảng cách (margin/padding) hợp lý.
   */
  return (
    <div className={cn('mx-auto w-full max-w-6xl space-y-8 px-4 pb-24 pt-28 sm:px-6')}>
      
      {/* 
          Sử dụng GlassCard để tạo một khối nội dung nổi bật trên nền ứng dụng.
          'variant="default"' sử dụng panel tiêu chuẩn của hệ thống.
          'padding="lg"' tạo không gian rộng rãi cho các phần tử bên trong.
      */}
      <GlassCard variant="default" padding="lg" className="relative overflow-hidden">
        
        {/* Lớp nền trang trí tạo hiệu ứng ánh sáng gradient phía sau tiêu đề */}
        <div className="absolute -right-24 -top-24 h-80 w-80 rounded-full bg-purple-500/5 blur-[120px]" />
        
        <header className={cn('relative z-10 mb-12 space-y-3')}>
          {/* 
              Tiêu đề chính: Sử dụng 'lunar-metallic-text' để tạo hiệu ứng chữ kim loại đổ màu.
              Font 'black' và 'uppercase' tăng tính thẩm mỹ và độ chuyên nghiệp.
          */}
          <h1 className={cn('lunar-metallic-text text-3xl font-black uppercase tracking-[0.2em] sm:text-4xl')}>
            {t('historyPageTitle')}
          </h1>
          
          {/* Phụ đề: Sử dụng 'tn-text-secondary' để giảm mức độ ưu tiên thị giác so với tiêu đề chính */}
          <p className={cn('tn-text-secondary text-sm font-medium sm:text-base max-w-2xl leading-relaxed')}>
            {t('historyPageSubtitle')}
          </p>
        </header>

        {/* 
            Kiểm tra lỗi: Nếu có lỗi từ phía Server, hiển thị thông báo đỏ đậm tính thẩm mỹ.
        */}
        {historyQuery.error instanceof Error ? (
          <div className={cn('mb-8 rounded-2xl border border-red-500/20 bg-red-500/5 px-6 py-4 text-sm tn-text-danger')}>
            <p className="font-bold uppercase tracking-wider mb-1">Cảnh báo hệ thống:</p>
            {historyQuery.error.message}
          </div>
        ) : null}

        {/* 
            GachaHistoryTable: Hiển thị danh sách các mục lịch sử.
            Thành phần này sẽ được tái thiết kế để sử dụng cấu trúc Card thanh thoát hơn.
        */}
        <div className="relative z-10 min-h-[400px]">
          <GachaHistoryTable
            entries={historyQuery.data?.items ?? []}
            locale={locale}
            labels={{
              empty: t('emptyHistory'),
              pityReset: t('historyPityReset'),
              pityStayed: t('historyPityStayed'),
              pullCount: t('historyPullCount'),
            }}
          />
        </div>

        {/* 
            Footer trang: Bao gồm bộ phân trang.
            Kẻ một đường line mờ (tn-border-soft) để tách biệt với danh sách ở trên.
        */}
        <div className={cn('mt-12 flex flex-col items-center justify-between gap-4 border-t tn-border-soft pt-10 sm:flex-row')}>
          
          {/* Chỉ số trang hiện tại / tổng số trang */}
          <div className={cn('tn-text-secondary text-xs font-black uppercase tracking-[0.25em] opacity-70')}>
            {t('historyPageIndicator', { page, totalPages })}
          </div>
          
          {/* Cụm nút điều hướng: Sử dụng variant 'secondary' để không làm rối mắt người dùng */}
          <div className={cn('flex items-center gap-4')}>
            <Button 
              variant="secondary" 
              size="md" 
              disabled={page <= 1 || historyQuery.isLoading} 
              onClick={() => {
                setPage((currentPage) => currentPage - 1);
                window.scrollTo({ top: 0, behavior: 'smooth' }); // Cuộn lên đầu khi chuyển trang
              }}
            >
              {t('historyPrev')}
            </Button>
            
            <Button 
              variant="secondary" 
              size="md" 
              disabled={page >= totalPages || historyQuery.isLoading} 
              onClick={() => {
                setPage((currentPage) => currentPage + 1);
                window.scrollTo({ top: 0, behavior: 'smooth' }); // Cuộn lên đầu khi chuyển trang
              }}
            >
              {t('historyNext')}
            </Button>
          </div>
        </div>
      </GlassCard>
    </div>
  );
}
