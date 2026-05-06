import { cn } from '@/lib/utils';

interface NotificationFiltersProps {
 filterUnread: boolean;
 allLabel: string;
 unreadLabel: string;
 onFilterAll: () => void;
 onFilterUnread: () => void;
}

const filterButtonClass =
 'px-4 py-2 rounded-xl text-xs font-bold uppercase tracking-wider transition-all min-h-11';

export function NotificationFilters({
 filterUnread,
 allLabel,
 unreadLabel,
 onFilterAll,
 onFilterUnread,
}: NotificationFiltersProps) {
 return (
  <div className={cn('flex gap-2')}>
   <button
    type="button"
    onClick={onFilterAll}
    className={cn(
     filterButtonClass,
     !filterUnread
      ? 'bg-[var(--bg-elevated)] text-[var(--text-ink)] border border-[var(--border-hover)] shadow-[var(--glow-purple-sm)]'
      : 'text-[var(--text-muted)] hover:text-[var(--text-secondary)] border border-transparent hover:bg-[var(--bg-surface-hover)]',
    )}
   >
    {allLabel}
   </button>
   <button
    type="button"
    onClick={onFilterUnread}
    className={cn(
     filterButtonClass,
     filterUnread
      ? 'bg-[var(--bg-elevated)] text-[var(--text-ink)] border border-[var(--border-hover)] shadow-[var(--glow-purple-sm)]'
      : 'text-[var(--text-muted)] hover:text-[var(--text-secondary)] border border-transparent hover:bg-[var(--bg-surface-hover)]',
    )}
   >
    {unreadLabel}
   </button>
  </div>
 );
}
