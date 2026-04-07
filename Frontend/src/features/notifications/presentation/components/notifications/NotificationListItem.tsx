import { memo } from 'react';
import { CheckCheck } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import type { NotificationItem } from '@/features/notifications/application/actions';
import { cn } from '@/lib/utils';
import { notificationTypeIconMap } from './utils';

interface NotificationListItemProps {
 item: NotificationItem;
 title: string;
 body: string;
 timeLabel: string;
 markReadLabel: string;
 onMarkRead: (id: string) => Promise<void>;
}

export const NotificationListItem = memo(function NotificationListItem({
 item,
 title,
 body,
 timeLabel,
 markReadLabel,
 onMarkRead,
}: NotificationListItemProps) {
 const typeConfig = notificationTypeIconMap[item.type] ?? notificationTypeIconMap.system;
 const Icon = typeConfig.icon;

 return (
  <GlassCard
   className={cn(
    'p-4 transition-all duration-300',
    item.isRead ? 'opacity-70' : 'border-l-2 border-l-[var(--purple-accent)]',
   )}
  >
   <div className={cn('flex items-start gap-4')}>
    <div className={cn('w-10 h-10 rounded-xl flex items-center justify-center shrink-0 bg-[var(--bg-glass)]')}>
     <Icon className={cn('w-5 h-5', typeConfig.colorClass)} />
    </div>
    <div className={cn('flex-1 min-w-0')}>
     <div className={cn('flex items-start justify-between gap-2')}>
      <h3 className={cn('text-sm leading-tight', item.isRead ? 'font-medium tn-text-secondary' : 'font-bold tn-text-primary')}>
       {title}
      </h3>
      <span className={cn('text-[10px] font-medium tn-text-muted whitespace-nowrap shrink-0')}>{timeLabel}</span>
     </div>
     {body ? <p className={cn('mt-1 text-xs tn-text-secondary leading-relaxed line-clamp-2')}>{body}</p> : null}
     {!item.isRead ? (
      <button
       type="button"
       onClick={() => void onMarkRead(item.id)}
       className={cn('mt-2 flex items-center gap-1.5 text-[10px] font-bold uppercase tracking-widest text-[var(--purple-muted)] hover:text-[var(--purple-accent)] transition-colors')}
      >
       <CheckCheck className={cn('w-3.5 h-3.5')} />
       {markReadLabel}
      </button>
     ) : null}
    </div>
   </div>
  </GlassCard>
 );
});
