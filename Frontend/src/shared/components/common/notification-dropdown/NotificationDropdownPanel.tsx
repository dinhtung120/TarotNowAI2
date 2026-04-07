import type { useTranslations } from 'next-intl';
import type { NotificationItem } from '@/features/notifications/application/actions';
import NotificationDropdownContent from '@/shared/components/common/notification-dropdown/NotificationDropdownContent';
import NotificationDropdownFooter from '@/shared/components/common/notification-dropdown/NotificationDropdownFooter';
import NotificationDropdownHeader from '@/shared/components/common/notification-dropdown/NotificationDropdownHeader';
import { cn } from '@/lib/utils';

interface NotificationDropdownPanelProps {
  dropdownRef: React.RefObject<HTMLDivElement | null>;
  getTitle: (item: NotificationItem) => string;
  isLoading: boolean;
  isMarkingAll: boolean;
  notifications: NotificationItem[];
  unreadCount: number;
  viewAllLabel: string;
  title: string;
  t: ReturnType<typeof useTranslations>;
  onMarkAllRead: () => Promise<void>;
  onMarkRead: (notificationId: string) => Promise<unknown>;
  onViewAll: () => void;
}

export default function NotificationDropdownPanel({ dropdownRef, getTitle, isLoading, isMarkingAll, notifications, unreadCount, viewAllLabel, title, t, onMarkAllRead, onMarkRead, onViewAll }: NotificationDropdownPanelProps) {
  return (
    <div ref={dropdownRef} className={cn('fixed left-0 right-0 top-[72px] z-[100] mx-auto w-[calc(100vw-32px)] max-w-[360px] animate-in slide-in-from-top-2 fade-in overflow-hidden rounded-2xl border border-[var(--border-subtle)] bg-[var(--bg-elevated)] shadow-[var(--shadow-elevated)] duration-200 sm:absolute sm:left-auto sm:right-0 sm:top-[calc(100%+8px)] sm:mx-0 sm:w-[380px] sm:max-w-none')}>
      <NotificationDropdownHeader title={title} unreadCount={unreadCount} isMarkingAll={isMarkingAll} onMarkAllRead={onMarkAllRead} />
      <div className={cn('max-h-[360px] overflow-y-auto')}>
        <NotificationDropdownContent t={t} isLoading={isLoading} notifications={notifications} getTitle={getTitle} onMarkRead={onMarkRead} />
      </div>
      <NotificationDropdownFooter ctaLabel={viewAllLabel} onViewAll={onViewAll} />
    </div>
  );
}
