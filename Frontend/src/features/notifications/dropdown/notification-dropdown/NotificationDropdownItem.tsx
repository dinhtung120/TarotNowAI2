'use client';

import { memo } from 'react';
import { cn } from '@/lib/utils';
import type { NotificationItem } from '@/features/notifications/shared/actions';
import { useNotificationDropdownItem } from '@/features/notifications/dropdown/notification-dropdown/useNotificationDropdownItem';

interface NotificationDropdownItemProps {
 item: NotificationItem;
 title: string;
 timeLabel: string;
 onMarkRead: (id: string) => Promise<unknown>;
}

export const NotificationDropdownItem = memo(function NotificationDropdownItem({
 item,
 title,
 timeLabel,
 onMarkRead,
}: NotificationDropdownItemProps) {
 const vm = useNotificationDropdownItem({ item, onMarkRead });
 const Icon = vm.typeConfig.icon;

 return (
  <div
   onClick={() => void vm.handleClick()}
   className={cn(
    'group flex items-start gap-3 px-4 py-3 cursor-pointer transition-colors',
    !item.isRead
     ? 'tn-notification-item-unread'
     : 'tn-notification-item-read',
   )}
  >
   <div className={cn('w-8 h-8 rounded-full tn-bg-glass flex items-center justify-center shrink-0 border tn-border-soft transition-colors', item.isRead ? 'tn-notification-item-icon-read' : '')}>
    <Icon className={cn('w-4 h-4', vm.typeConfig.colorClass)} />
   </div>
   <div className={cn('flex-1 min-w-0')}>
    <p className={cn('tn-text-13 leading-tight line-clamp-2', !item.isRead ? 'font-bold tn-text-ink' : 'font-medium tn-text-secondary')}>
     {title}
    </p>
    <p className={cn('mt-1 tn-text-11 font-medium tn-text-accent-soft')}>{timeLabel}</p>
   </div>
   {!item.isRead ? <div className={cn('w-2 h-2 rounded-full tn-bg-danger shrink-0 self-center')} /> : null}
  </div>
 );
});
