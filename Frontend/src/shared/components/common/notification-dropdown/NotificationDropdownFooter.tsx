import { cn } from '@/lib/utils';

interface NotificationDropdownFooterProps {
  ctaLabel: string;
  onViewAll: () => void;
}

export default function NotificationDropdownFooter({ ctaLabel, onViewAll }: NotificationDropdownFooterProps) {
  return (
    <div className={cn('border-t border-[var(--border-subtle)] bg-[var(--bg-glass)] p-2')}>
      <button type="button" onClick={onViewAll} className={cn('w-full rounded-lg py-2.5 text-xs font-bold text-[var(--text-ink)] transition-colors hover:bg-[var(--bg-surface-hover)]')}>
        {ctaLabel}
      </button>
    </div>
  );
}
