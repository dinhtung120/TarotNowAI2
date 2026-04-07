import { X } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

interface ModalHeaderProps {
  description?: string;
  showCloseButton: boolean;
  title?: string;
  onClose: () => void;
}

export default function ModalHeader({ description, showCloseButton, title, onClose }: ModalHeaderProps) {
  const t = useTranslations('Common');

  if (!title && !showCloseButton) return null;

  return (
    <div className={cn('flex items-start justify-between p-6 pb-0')}>
      <div className={cn('space-y-1')}>
        {title ? <h2 className={cn('text-lg font-black tracking-tight text-[var(--text-ink)]')}>{title}</h2> : null}
        {description ? <p className={cn('text-sm font-medium text-[var(--text-secondary)]')}>{description}</p> : null}
      </div>
      {showCloseButton ? (
        <button type="button" onClick={onClose} className={cn('cursor-pointer rounded-xl p-2 text-[var(--text-secondary)] transition-colors hover:bg-[var(--purple-50)] hover:text-[var(--text-primary)]')} aria-label={t('close_modal')}>
          <X className={cn('h-5 w-5')} />
        </button>
      ) : null}
    </div>
  );
}
