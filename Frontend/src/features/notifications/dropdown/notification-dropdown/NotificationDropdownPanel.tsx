import type { useTranslations } from 'next-intl';
import type { NotificationItem } from '@/features/notifications/shared/actions';
import NotificationDropdownContent from '@/features/notifications/dropdown/notification-dropdown/NotificationDropdownContent';
import NotificationDropdownFooter from '@/features/notifications/dropdown/notification-dropdown/NotificationDropdownFooter';
import NotificationDropdownHeader from '@/features/notifications/dropdown/notification-dropdown/NotificationDropdownHeader';
import { cn } from '@/lib/utils';

interface NotificationDropdownPanelProps {
  dropdownRef: React.RefObject<HTMLDivElement | null>;
  getTitle: (item: NotificationItem) => string;
  hasLoadError: boolean;
  loadErrorMessage: string;
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

export default function NotificationDropdownPanel({ dropdownRef, getTitle, hasLoadError, loadErrorMessage, isLoading, isMarkingAll, notifications, unreadCount, viewAllLabel, title, t, onMarkAllRead, onMarkRead, onViewAll }: NotificationDropdownPanelProps) {
  return (
    <div ref={dropdownRef} className={cn('tn-notification-panel animate-in slide-in-from-top-2 fade-in duration-200')}>
      <NotificationDropdownHeader title={title} unreadCount={unreadCount} isMarkingAll={isMarkingAll} onMarkAllRead={onMarkAllRead} />
      <div className={cn('tn-notification-panel-content')}>
        <NotificationDropdownContent t={t} isLoading={isLoading} hasLoadError={hasLoadError} loadErrorMessage={loadErrorMessage} notifications={notifications} getTitle={getTitle} onMarkRead={onMarkRead} />
      </div>
      <NotificationDropdownFooter ctaLabel={viewAllLabel} onViewAll={onViewAll} />
    </div>
  );
}
