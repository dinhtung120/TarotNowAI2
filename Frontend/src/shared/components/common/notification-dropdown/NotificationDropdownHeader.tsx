import { cn } from '@/lib/utils';

interface NotificationDropdownHeaderProps {
  isMarkingAll: boolean;
  unreadCount: number;
  title: string;
  onMarkAllRead: () => Promise<void>;
}

export default function NotificationDropdownHeader({ isMarkingAll, unreadCount, title, onMarkAllRead }: NotificationDropdownHeaderProps) {
  return (
    <div className={cn('flex items-center justify-between border-b border-[var(--border-subtle)] bg-[var(--bg-glass)] px-4 py-3')}>
      <h3 className={cn('text-sm font-black tracking-tight text-[var(--text-ink)]')}>{title}</h3>
      <button
        type="button"
        onClick={() => void onMarkAllRead()}
        disabled={isMarkingAll || unreadCount === 0}
        className={cn('text-[11px] font-bold text-[var(--purple-accent)] transition-opacity hover:underline disabled:cursor-not-allowed disabled:opacity-50')}
      >
        Đọc tất cả
      </button>
    </div>
  );
}
