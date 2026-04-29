'use client';

import { Bell } from 'lucide-react';
import type { useTranslations } from 'next-intl';
import { NotificationDropdownItem } from '@/shared/components/common/notification-dropdown/NotificationDropdownItem';
import { formatRelativeTime } from '@/shared/components/common/notification-dropdown/notificationTime';
import { useRelativeTimeNow } from '@/shared/application/hooks/useRelativeTimeNow';
import type { NotificationItem } from '@/features/notifications/application/actions';
import { cn } from '@/lib/utils';

interface NotificationDropdownContentProps {
  getTitle: (item: NotificationItem) => string;
  hasLoadError: boolean;
  loadErrorMessage: string;
  isLoading: boolean;
  notifications: NotificationItem[];
  onMarkRead: (notificationId: string) => Promise<unknown>;
  t: ReturnType<typeof useTranslations>;
}

export default function NotificationDropdownContent({ getTitle, hasLoadError, loadErrorMessage, isLoading, notifications, onMarkRead, t }: NotificationDropdownContentProps) {
  const referenceNowMs = useRelativeTimeNow();

  if (isLoading) return <div className={cn('flex h-24 items-center justify-center tn-text-muted')}><Bell className={cn('h-5 w-5 animate-pulse')} /></div>;
  if (hasLoadError) return <div className={cn('px-4 py-8 text-center text-xs font-semibold text-red-300')}>{loadErrorMessage}</div>;
  if (notifications.length === 0) return <div className={cn('px-4 py-8 text-center')}><div className={cn('mx-auto mb-3 flex h-12 w-12 items-center justify-center rounded-full tn-bg-surface-hover')}><Bell className={cn('h-5 w-5 tn-text-muted')} /></div><p className={cn('text-sm font-medium tn-text-secondary')}>{t('empty_desc')}</p></div>;

  return (
    <div className={cn('flex flex-col')}>
      {notifications.map((item) => (
        <NotificationDropdownItem
          key={item.id}
          item={item}
          title={getTitle(item)}
          timeLabel={formatRelativeTime(item.createdAt, t, referenceNowMs)}
          onMarkRead={onMarkRead}
        />
      ))}
    </div>
  );
}
