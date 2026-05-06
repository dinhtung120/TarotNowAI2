import { cn } from '@/lib/utils';

interface NotificationDropdownHeaderProps {
  isMarkingAll: boolean;
  unreadCount: number;
  title: string;
  onMarkAllRead: () => Promise<void>;
}

export default function NotificationDropdownHeader({ isMarkingAll, unreadCount, title, onMarkAllRead }: NotificationDropdownHeaderProps) {
  return (
    <div className={cn('flex items-center justify-between border-b tn-border-soft tn-bg-glass px-4 py-3')}>
      <h3 className={cn('text-sm font-black tracking-tight tn-text-ink')}>{title}</h3>
      <button
        type="button"
        onClick={() => void onMarkAllRead()}
        disabled={isMarkingAll || unreadCount === 0}
        className={cn('tn-notification-mark-all font-bold transition-opacity tn-disabled-not-allowed tn-disabled-opacity-50')}
      >
        Đọc tất cả
      </button>
    </div>
  );
}
