'use client';

import { useLocale, useTranslations } from 'next-intl';
import { SectionHeader } from '@/shared/components/ui';
import { useNotificationMarkRead } from '@/features/notifications/application/useNotificationMarkRead';
import { useNotificationsPage } from '@/features/notifications/application/useNotificationsPage';
import { useRelativeTimeNow } from '@/shared/application/hooks/useRelativeTimeNow';
import {
 NotificationFilters,
 NotificationsContent,
 formatRelativeNotificationTime,
 getLocalizedNotificationBody,
 getLocalizedNotificationTitle,
} from '@/features/notifications/presentation/components/notifications';
import { cn } from '@/lib/utils';

export default function NotificationsPage() {
 const t = useTranslations('Notifications');
 const locale = useLocale();
 const referenceNowMs = useRelativeTimeNow();
 const { data, loading, page, setPage, filterUnread, setFilterUnread, totalPages, markAsRead } = useNotificationsPage();
 const { handleMarkRead } = useNotificationMarkRead({ markAsRead });

 return (
  <div className={cn('space-y-8 max-w-3xl mx-auto px-4 sm:px-6 pt-24 pb-28')}>
   <SectionHeader tag={t('tag')} title={t('title')} subtitle={t('subtitle')} />

   <NotificationFilters
    filterUnread={filterUnread}
    allLabel={t('filter_all')}
    unreadLabel={t('filter_unread')}
    onFilterAll={() => {
     setFilterUnread(false);
     setPage(1);
    }}
    onFilterUnread={() => {
     setFilterUnread(true);
     setPage(1);
    }}
   />

   <NotificationsContent
    loading={loading}
    data={data}
    page={page}
    totalPages={totalPages}
    loadingLabel={t('loading')}
    emptyTitle={t('empty_title')}
    emptyDescription={t('empty_desc')}
    markReadLabel={t('mark_read')}
    getTitle={(item) => getLocalizedNotificationTitle(item, locale)}
    getBody={(item) => getLocalizedNotificationBody(item, locale)}
    getTimeLabel={(item) => formatRelativeNotificationTime(item.createdAt, t, referenceNowMs)}
    onPageChange={setPage}
    onMarkRead={handleMarkRead}
   />
  </div>
 );
}
