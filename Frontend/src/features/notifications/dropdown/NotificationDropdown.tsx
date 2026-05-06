'use client';

import { useEffect, useRef } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { useNotificationDropdown } from '@/features/notifications/dropdown/hooks/useNotificationDropdown';
import { useOptimizedNavigation } from '@/shared/navigation/useOptimizedNavigation';
import NotificationBellButton from '@/features/notifications/dropdown/notification-dropdown/NotificationBellButton';
import NotificationDropdownPanel from '@/features/notifications/dropdown/notification-dropdown/NotificationDropdownPanel';
import { useNotificationDropdownState } from '@/features/notifications/dropdown/notification-dropdown/useNotificationDropdownState';
import { cn } from '@/lib/utils';

interface NotificationDropdownProps {
  enabled?: boolean;
}

export default function NotificationDropdown({ enabled = true }: NotificationDropdownProps) {
  const t = useTranslations('Notifications');
  const tCommon = useTranslations('Common');
  const locale = useLocale();
  const navigation = useOptimizedNavigation();
  const markAllAsReadRef = useRef<() => Promise<unknown>>(async () => undefined);
  const { bellButtonRef, close, dropdownRef, getTitle, handleMarkAllRead, isMarkingAll, isOpen, toggleOpen } = useNotificationDropdownState({
    locale,
    markAllAsRead: async () => markAllAsReadRef.current(),
  });
  const {
    notifications,
    unreadCount,
    isLoading,
    hasLoadError,
    loadErrorMessage,
    markAsRead,
    markAllAsRead,
    refreshDropdown,
    refreshUnreadCount,
  } = useNotificationDropdown({ enabled, open: isOpen });

  useEffect(() => {
    markAllAsReadRef.current = markAllAsRead;
  }, [markAllAsRead]);

  useEffect(() => {
    if (!isOpen) {
      return;
    }

    void refreshUnreadCount();
    void refreshDropdown();
  }, [isOpen, refreshDropdown, refreshUnreadCount]);

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
          hasLoadError={hasLoadError}
          loadErrorMessage={loadErrorMessage || tCommon('error')}
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
