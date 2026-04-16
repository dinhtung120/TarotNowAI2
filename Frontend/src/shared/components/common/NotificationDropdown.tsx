'use client';

import { useLocale, useTranslations } from 'next-intl';
import { useNotificationDropdown } from '@/features/notifications/application/useNotificationDropdown';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';
import NotificationBellButton from '@/shared/components/common/notification-dropdown/NotificationBellButton';
import NotificationDropdownPanel from '@/shared/components/common/notification-dropdown/NotificationDropdownPanel';
import { useNotificationDropdownState } from '@/shared/components/common/notification-dropdown/useNotificationDropdownState';
import { cn } from '@/lib/utils';

export default function NotificationDropdown() {
  const t = useTranslations('Notifications');
  const tCommon = useTranslations('Common');
  const locale = useLocale();
  const navigation = useOptimizedNavigation();
  const { notifications, unreadCount, isLoading, markAsRead, markAllAsRead } = useNotificationDropdown();
  const { bellButtonRef, close, dropdownRef, getTitle, handleMarkAllRead, isMarkingAll, isOpen, toggleOpen } = useNotificationDropdownState({ locale, markAllAsRead });

  return (
    <div className={cn('relative inline-flex items-center')}>
      <NotificationBellButton buttonRef={bellButtonRef} isOpen={isOpen} unreadCount={unreadCount} onToggle={toggleOpen} ariaLabel={tCommon('notifications')} />
      {isOpen ? (
        <NotificationDropdownPanel
          dropdownRef={dropdownRef}
          t={t}
          title={t('title')}
          viewAllLabel={t('filter_all')}
          notifications={notifications}
          unreadCount={unreadCount}
          isLoading={isLoading}
          isMarkingAll={isMarkingAll}
          getTitle={getTitle}
          onMarkRead={markAsRead}
          onMarkAllRead={handleMarkAllRead}
          onViewAll={() => {
            close();
            navigation.push('/notifications');
          }}
        />
      ) : null}
    </div>
  );
}
