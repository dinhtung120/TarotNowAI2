import { cn } from '@/lib/utils';

interface NotificationDropdownFooterProps {
  ctaLabel: string;
  onViewAll: () => void;
}

export default function NotificationDropdownFooter({ ctaLabel, onViewAll }: NotificationDropdownFooterProps) {
  return (
    <div className={cn('border-t tn-border-soft tn-bg-glass p-2')}>
      <button type="button" onClick={onViewAll} className={cn('w-full rounded-lg py-2.5 text-xs font-bold tn-text-ink transition-colors tn-hover-surface-strong')}>
        {ctaLabel}
      </button>
    </div>
  );
}
