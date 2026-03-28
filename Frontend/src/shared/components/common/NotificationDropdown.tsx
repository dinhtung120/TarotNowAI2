'use client';

/*
 * ===================================================================
 * COMPONENT: NotificationDropdown
 * BỐI CẢNH: 
 *   Hiển thị icon chuông thông báo trên Navbar kèm theo Dropdown list 
 *   các thông báo hệ thống mới nhất (tối đa 10).
 * ===================================================================
 */

import { useState, useRef, useEffect, useCallback, memo } from 'react';
import { Bell, Info, Star, Flame, Wallet, Zap, CheckCheck } from 'lucide-react';
import { useTranslations, useLocale } from 'next-intl';
import { Link, useRouter } from '@/i18n/routing';
import { cn } from '@/shared/utils/cn';
import { useNotificationDropdown } from '@/features/notifications/application/useNotificationDropdown';
import { type NotificationItem } from '@/features/notifications/application/actions';

const typeIconMap: Record<string, { icon: typeof Bell; colorClass: string }> = {
  system: { icon: Info, colorClass: 'text-blue-400' },
  quest: { icon: Star, colorClass: 'text-purple-400' },
  streak: { icon: Flame, colorClass: 'text-orange-400' },
  escrow: { icon: Wallet, colorClass: 'text-emerald-400' },
  payment: { icon: Zap, colorClass: 'text-yellow-400' },
};

function formatRelativeTime(dateStr: string, t: ReturnType<typeof useTranslations>): string {
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

interface DropdownItemProps {
  item: NotificationItem;
  title: string;
  body: string;
  timeLabel: string;
  onMarkRead: (id: string) => Promise<any>;
}

const DropdownItem = memo(function DropdownItem({
  item,
  title,
  body,
  timeLabel,
  onMarkRead,
}: DropdownItemProps) {
  const typeConfig = typeIconMap[item.type] ?? typeIconMap.system;
  const Icon = typeConfig.icon;
  const [isMarking, setIsMarking] = useState(false);

  const handleClick = async () => {
    if (!item.isRead && !isMarking) {
      setIsMarking(true);
      await onMarkRead(item.id);
      setIsMarking(false);
    }
  };

  return (
    <div
      onClick={handleClick}
      className={cn(
        'group flex items-start gap-3 px-4 py-3 cursor-pointer transition-colors',
        !item.isRead
          ? 'bg-[var(--purple-accent)]/5 hover:bg-[var(--purple-accent)]/10 border-l-[3px] border-l-[var(--purple-accent)]'
          : 'bg-transparent hover:bg-[var(--bg-surface-hover)] opacity-80 border-l-[3px] border-l-transparent'
      )}
    >
      <div className="w-8 h-8 rounded-full bg-[var(--bg-glass)] flex items-center justify-center shrink-0 border border-[var(--border-subtle)] group-hover:border-[var(--border-default)] transition-colors">
        <Icon className={cn('w-4 h-4', typeConfig.colorClass)} />
      </div>

      <div className="flex-1 min-w-0">
        <div className="flex items-start justify-between gap-2">
          <p
            className={cn(
              'text-[13px] leading-tight line-clamp-2',
              !item.isRead ? 'font-bold text-[var(--text-ink)]' : 'font-medium text-[var(--text-secondary)]'
            )}
          >
            {title}
          </p>
        </div>
        <p className="mt-1 text-[11px] font-medium text-[var(--purple-muted)]">
          {timeLabel}
        </p>
      </div>

      {!item.isRead && (
        <div className="w-2 h-2 rounded-full bg-[var(--danger)] shrink-0 self-center" />
      )}
    </div>
  );
});

export default function NotificationDropdown() {
  const t = useTranslations('Notifications');
  const tCommon = useTranslations('Common');
  const locale = useLocale();
  const router = useRouter();
  
  const [isOpen, setIsOpen] = useState(false);
  const [isMarkingAll, setIsMarkingAll] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);
  const BellIconRef = useRef<HTMLButtonElement>(null);

  const { notifications, unreadCount, isLoading, markAsRead, markAllAsRead } = useNotificationDropdown();

  // Handle click outside to close
  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(e.target as Node) &&
        BellIconRef.current &&
        !BellIconRef.current.contains(e.target as Node)
      ) {
        setIsOpen(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const getTitle = useCallback((item: NotificationItem) => {
    if (locale === 'vi') return item.titleVi || item.titleEn;
    return item.titleEn || item.titleVi;
  }, [locale]);

  const getBody = useCallback((item: NotificationItem) => {
    if (locale === 'vi') return item.bodyVi || item.bodyEn;
    return item.bodyEn || item.bodyVi;
  }, [locale]);

  return (
    <div className="relative inline-flex items-center">
      {/* Nút Chuông */}
      <button
        ref={BellIconRef}
        onClick={() => setIsOpen(!isOpen)}
        className={cn(
          "inline-flex relative items-center justify-center p-2 rounded-xl transition-all cursor-pointer min-h-11 min-w-11",
          isOpen
            ? "bg-[var(--bg-elevated)] text-[var(--text-ink)] border border-[var(--border-default)] shadow-[var(--shadow-card)]"
            : "bg-transparent text-[var(--text-secondary)] hover:bg-[var(--purple-50)] hover:text-[var(--text-ink)]"
        )}
        aria-label={tCommon('notifications')}
      >
        <Bell className="w-5 h-5" />
        {unreadCount > 0 && (
          <span className="absolute top-1.5 right-1.5 min-w-[18px] h-[18px] px-1 rounded-full bg-[var(--danger)] text-white text-[9px] font-black flex items-center justify-center shadow-md animate-in zoom-in">
            {unreadCount > 99 ? '99+' : unreadCount}
          </span>
        )}
      </button>

      {/* Dropdown Box */}
      {isOpen && (
        <div
          ref={dropdownRef}
          className="absolute right-0 top-[calc(100%+8px)] w-[320px] sm:w-[380px] bg-[var(--bg-elevated)] border border-[var(--border-subtle)] rounded-2xl shadow-[var(--shadow-elevated)] overflow-hidden z-[100] animate-in slide-in-from-top-2 fade-in duration-200"
        >
          {/* Header */}
          <div className="flex items-center justify-between px-4 py-3 border-b border-[var(--border-subtle)] bg-[var(--bg-glass)]">
            <h3 className="text-sm font-black text-[var(--text-ink)] tracking-tight">
              {t('title')}
            </h3>
            <button
              onClick={async () => {
                if (isMarkingAll) return;
                setIsMarkingAll(true);
                await markAllAsRead();
                setIsMarkingAll(false);
              }}
              disabled={isMarkingAll || unreadCount === 0}
              className="text-[11px] font-bold text-[var(--purple-accent)] hover:underline disabled:opacity-50 disabled:cursor-not-allowed transition-opacity"
            >
              Đọc tất cả
            </button>
          </div>

          {/* List items */}
          <div className="max-h-[360px] overflow-y-auto">
            {isLoading ? (
              <div className="flex justify-center items-center h-24 text-[var(--text-muted)]">
                <Bell className="w-5 h-5 animate-pulse" />
              </div>
            ) : notifications.length > 0 ? (
              <div className="flex flex-col">
                {notifications.map((item) => (
                  <DropdownItem
                    key={item.id}
                    item={item}
                    title={getTitle(item)}
                    body={getBody(item)}
                    timeLabel={formatRelativeTime(item.createdAt, t)}
                    onMarkRead={markAsRead}
                  />
                ))}
              </div>
            ) : (
              <div className="py-8 px-4 text-center">
                <div className="w-12 h-12 rounded-full bg-[var(--bg-surface-hover)] flex items-center justify-center mx-auto mb-3">
                  <Bell className="w-5 h-5 text-[var(--text-muted)]" />
                </div>
                <p className="text-sm font-medium text-[var(--text-secondary)]">{t('empty_desc')}</p>
              </div>
            )}
          </div>

          {/* Footer - Nút Xem Tất Cả Dưới Cùng */}
          <div className="p-2 border-t border-[var(--border-subtle)] bg-[var(--bg-glass)]">
            <button
              onClick={() => {
                setIsOpen(false);
                router.push('/notifications');
              }}
              className="w-full py-2.5 rounded-lg text-xs font-bold text-[var(--text-ink)] hover:bg-[var(--bg-surface-hover)] transition-colors"
            >
              {t('filter_all')}
            </button>
          </div>
        </div>
      )}
    </div>
  );
}
