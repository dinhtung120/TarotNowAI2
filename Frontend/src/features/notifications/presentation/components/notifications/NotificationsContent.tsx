import { BellOff } from 'lucide-react';
import { EmptyState, Pagination } from '@/shared/components/ui';
import type { NotificationItem, NotificationListResponse } from '@/features/notifications/application/actions';
import { cn } from '@/lib/utils';
import { NotificationListItem } from './NotificationListItem';
import { NotificationLoadingState } from './NotificationLoadingState';

interface NotificationsContentProps {
 loading: boolean;
 data: NotificationListResponse | null | undefined;
 page: number;
 totalPages: number;
 loadingLabel: string;
 emptyTitle: string;
 emptyDescription: string;
 markReadLabel: string;
 getTitle: (item: NotificationItem) => string;
 getBody: (item: NotificationItem) => string;
 getTimeLabel: (item: NotificationItem) => string;
 onPageChange: (page: number) => void;
 onMarkRead: (id: string) => Promise<void>;
}

export function NotificationsContent({
 loading,
 data,
 page,
 totalPages,
 loadingLabel,
 emptyTitle,
 emptyDescription,
 markReadLabel,
 getTitle,
 getBody,
 getTimeLabel,
 onPageChange,
 onMarkRead,
}: NotificationsContentProps) {
 if (loading) {
  return <NotificationLoadingState label={loadingLabel} />;
 }

 if (!data || data.items.length === 0) {
  return <EmptyState icon={<BellOff className={cn('w-12 h-12')} />} title={emptyTitle} message={emptyDescription} />;
 }

 return (
  <>
   <div className={cn('space-y-3')}>
    {data.items.map((item) => (
     <NotificationListItem
      key={item.id}
      item={item}
      title={getTitle(item)}
      body={getBody(item)}
      timeLabel={getTimeLabel(item)}
      markReadLabel={markReadLabel}
      onMarkRead={onMarkRead}
     />
    ))}
   </div>
   {totalPages > 1 ? (
    <Pagination currentPage={page} totalPages={totalPages} onPageChange={onPageChange} />
   ) : null}
  </>
 );
}
