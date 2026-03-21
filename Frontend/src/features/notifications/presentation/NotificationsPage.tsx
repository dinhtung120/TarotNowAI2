/*
 * ===================================================================
 * FILE: (user)/notifications/page.tsx
 * BỐI CẢNH (CONTEXT):
 *   Trang hiển thị danh sách thông báo in-app của user.
 *
 * TÍNH NĂNG CHÍNH:
 *   - Hiển thị danh sách thông báo dạng card (icon theo type, title/body theo locale).
 *   - Filter tabs: Tất cả / Chưa đọc.
 *   - Nút "Đánh dấu đã đọc" cho từng thông báo chưa đọc.
 *   - Phân trang khi có nhiều thông báo.
 *   - Empty state khi chưa có thông báo.
 *   - Thông báo cũ tự xóa sau 30 ngày (TTL phía MongoDB).
 *
 * TẠI SAO DÙNG USE-EFFECT THAY VÌ SERVER COMPONENT?
 *   - Trang cần tương tác: click mark read, filter, pagination.
 *   - Dữ liệu thay đổi realtime → cần re-fetch khi user tương tác.
 *   - Server Component chỉ render 1 lần → không phù hợp.
 * ===================================================================
 */
'use client';

import { useTranslations, useLocale } from 'next-intl';
import {
  Bell,
  BellOff,
  CheckCheck,
  Info,
  Zap,
  Flame,
  Wallet,
  Star,
} from 'lucide-react';
import toast from 'react-hot-toast';

import { GlassCard, SectionHeader, EmptyState, Pagination } from '@/components/ui';
import {
  type NotificationItem,
} from '@/actions/notificationActions';
import { useNotificationsPage } from '@/features/notifications/application/useNotificationsPage';

/*
 * ICON MAP: ánh xạ loại thông báo → icon + màu phù hợp.
 * Giúp user phân biệt nhanh loại thông báo bằng mắt thường.
 * - system: icon Info (xanh dương) — thông tin hệ thống
 * - quest: icon Star (tím) — nhiệm vụ / achievement
 * - streak: icon Flame (cam) — chuỗi điểm danh
 * - escrow: icon Wallet (xanh lá) — tài chính / escrow
 * - payment: icon Zap (vàng) — thanh toán / nạp tiền
 */
const typeIconMap: Record<string, { icon: typeof Bell; colorClass: string }> = {
  system: { icon: Info, colorClass: 'text-blue-400' },
  quest: { icon: Star, colorClass: 'text-purple-400' },
  streak: { icon: Flame, colorClass: 'text-orange-400' },
  escrow: { icon: Wallet, colorClass: 'text-emerald-400' },
  payment: { icon: Zap, colorClass: 'text-yellow-400' },
};

/*
 * Hàm helper: format thời gian tương đối (relative time).
 * Hiển thị "Vừa xong", "5 phút trước", "3 giờ trước", "2 ngày trước"
 * thay vì datetime cụ thể → thân thiện UX hơn.
 */
function formatRelativeTime(
  dateStr: string,
  t: ReturnType<typeof useTranslations>
): string {
  const now = Date.now();
  const date = new Date(dateStr).getTime();
  const diffMs = now - date;
  const diffMinutes = Math.floor(diffMs / 60000);
  const diffHours = Math.floor(diffMs / 3600000);
  const diffDays = Math.floor(diffMs / 86400000);

  if (diffMinutes < 1) return t('time_just_now');
  if (diffMinutes < 60) return t('time_minutes_ago', { count: diffMinutes });
  if (diffHours < 24) return t('time_hours_ago', { count: diffHours });
  return t('time_days_ago', { count: diffDays });
}

export default function NotificationsPage() {
  const t = useTranslations('Notifications');
  const locale = useLocale();
  const {
    data,
    loading,
    page,
    setPage,
    filterUnread,
    setFilterUnread,
    totalPages,
    markAsRead,
  } = useNotificationsPage();

  /*
   * handleMarkRead: đánh dấu 1 thông báo đã đọc.
   * Gọi server action → nếu thành công, cập nhật local state (isRead=true)
   * thay vì re-fetch toàn bộ → nhanh hơn, giảm load server.
   */
  const handleMarkRead = async (id: string) => {
    const result = await markAsRead(id);
    if (result.success) {
      toast.success(t('mark_read_success'));
    } else {
      toast.error(t('mark_read_fail'));
    }
  };

  /*
   * getLocalizedText: chọn title/body theo locale hiện tại.
   * Fallback: nếu locale không có text → dùng tiếng Anh.
   * Hỗ trợ vi/en (zh chưa có field trong DTO nhưng có trong MongoDB).
   */
  const getTitle = (item: NotificationItem) => {
    if (locale === 'vi') return item.titleVi || item.titleEn;
    return item.titleEn || item.titleVi;
  };

  const getBody = (item: NotificationItem) => {
    if (locale === 'vi') return item.bodyVi || item.bodyEn;
    return item.bodyEn || item.bodyVi;
  };

  return (
    <div className="space-y-8 max-w-3xl mx-auto">
      {/* ===== HEADER ===== */}
      <SectionHeader tag={t('tag')} title={t('title')} subtitle={t('subtitle')} />

      {/* ===== FILTER TABS =====
        * Dùng 2 nút: "Tất cả" và "Chưa đọc".
        * Active tab có background nổi bật + border.
        * Khi chuyển tab: reset về trang 1, re-fetch dữ liệu.
        */}
      <div className="flex gap-2">
        <button
          onClick={() => { setFilterUnread(false); setPage(1); }}
          className={[
            'px-4 py-2 rounded-xl text-xs font-bold uppercase tracking-wider transition-all min-h-11',
            !filterUnread
              ? 'bg-[var(--bg-elevated)] text-[var(--text-ink)] border border-[var(--border-hover)] shadow-[var(--glow-purple-sm)]'
              : 'text-[var(--text-muted)] hover:text-[var(--text-secondary)] border border-transparent hover:bg-[var(--bg-surface-hover)]',
          ].join(' ')}
        >
          {t('filter_all')}
        </button>
        <button
          onClick={() => { setFilterUnread(true); setPage(1); }}
          className={[
            'px-4 py-2 rounded-xl text-xs font-bold uppercase tracking-wider transition-all min-h-11',
            filterUnread
              ? 'bg-[var(--bg-elevated)] text-[var(--text-ink)] border border-[var(--border-hover)] shadow-[var(--glow-purple-sm)]'
              : 'text-[var(--text-muted)] hover:text-[var(--text-secondary)] border border-transparent hover:bg-[var(--bg-surface-hover)]',
          ].join(' ')}
        >
          {t('filter_unread')}
        </button>
      </div>

      {/* ===== LOADING STATE ===== */}
      {loading && (
        <GlassCard className="p-8 text-center">
          <div className="flex items-center justify-center gap-3 tn-text-muted">
            <Bell className="w-5 h-5 animate-pulse" />
            <span className="text-sm font-medium">{t('loading')}</span>
          </div>
        </GlassCard>
      )}

      {/* ===== EMPTY STATE =====
        * Hiển thị khi không có thông báo nào.
        * Icon BellOff + thông điệp thân thiện.
        */}
      {!loading && data && data.items.length === 0 && (
        <EmptyState
          icon={<BellOff className="w-12 h-12" />}
          title={t('empty_title')}
          message={t('empty_desc')}
        />
      )}

      {/* ===== NOTIFICATION LIST =====
        * Mỗi notification là 1 GlassCard chứa:
        * - Icon theo type (bên trái)
        * - Title + Body (giữa)
        * - Relative time (bên phải)
        * - Nút "Đánh dấu đã đọc" (nếu chưa đọc)
        *
        * Chưa đọc: có border trái màu tím nổi bật + opacity full.
        * Đã đọc: opacity giảm → user nhận biết nhanh.
        */}
      {!loading && data && data.items.length > 0 && (
        <div className="space-y-3">
          {data.items.map((item) => {
            const typeConfig = typeIconMap[item.type] ?? typeIconMap.system;
            const Icon = typeConfig.icon;
            const title = getTitle(item);
            const body = getBody(item);

            return (
              <GlassCard
                key={item.id}
                className={[
                  'p-4 transition-all duration-300',
                  !item.isRead
                    ? 'border-l-2 border-l-[var(--purple-accent)]'
                    : 'opacity-70',
                ].join(' ')}
              >
                <div className="flex items-start gap-4">
                  {/* === Icon === */}
                  <div
                    className={[
                      'w-10 h-10 rounded-xl flex items-center justify-center shrink-0',
                      'bg-[var(--bg-glass)]',
                    ].join(' ')}
                  >
                    <Icon className={`w-5 h-5 ${typeConfig.colorClass}`} />
                  </div>

                  {/* === Content === */}
                  <div className="flex-1 min-w-0">
                    <div className="flex items-start justify-between gap-2">
                      <h3
                        className={[
                          'text-sm leading-tight',
                          !item.isRead
                            ? 'font-bold tn-text-primary'
                            : 'font-medium tn-text-secondary',
                        ].join(' ')}
                      >
                        {title}
                      </h3>
                      <span className="text-[10px] font-medium tn-text-muted whitespace-nowrap shrink-0">
                        {formatRelativeTime(item.createdAt, t)}
                      </span>
                    </div>

                    {body && (
                      <p className="mt-1 text-xs tn-text-secondary leading-relaxed line-clamp-2">
                        {body}
                      </p>
                    )}

                    {/* === Mark as read button (chỉ hiện khi chưa đọc) === */}
                    {!item.isRead && (
                      <button
                        onClick={() => handleMarkRead(item.id)}
                        className="mt-2 flex items-center gap-1.5 text-[10px] font-bold uppercase tracking-widest text-[var(--purple-muted)] hover:text-[var(--purple-accent)] transition-colors"
                      >
                        <CheckCheck className="w-3.5 h-3.5" />
                        {t('mark_read')}
                      </button>
                    )}
                  </div>
                </div>
              </GlassCard>
            );
          })}
        </div>
      )}

      {/* ===== PAGINATION =====
        * Sử dụng component Pagination chung.
        * Hiện khi có nhiều hơn 1 trang.
        */}
      {!loading && totalPages > 1 && (
        <Pagination
          currentPage={page}
          totalPages={totalPages}
          onPageChange={setPage}
        />
      )}
    </div>
  );
}
