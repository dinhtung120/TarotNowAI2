import { useCallback, useEffect, useRef, useState } from 'react';
import type { NotificationItem } from '@/features/notifications/application/actions';

interface UseNotificationDropdownStateArgs {
  locale: string;
  markAllAsRead: () => Promise<unknown>;
}

export function useNotificationDropdownState({ locale, markAllAsRead }: UseNotificationDropdownStateArgs) {
  const [isOpen, setIsOpen] = useState(false);
  const [isMarkingAll, setIsMarkingAll] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);
  const bellButtonRef = useRef<HTMLButtonElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node) && bellButtonRef.current && !bellButtonRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const toggleOpen = useCallback(() => setIsOpen((value) => !value), []);
  const close = useCallback(() => setIsOpen(false), []);

  const handleMarkAllRead = useCallback(async () => {
    if (isMarkingAll) return;
    setIsMarkingAll(true);
    try {
      await markAllAsRead();
    } finally {
      setIsMarkingAll(false);
    }
  }, [isMarkingAll, markAllAsRead]);

  const getTitle = useCallback(
    (item: NotificationItem) => (locale === 'vi' ? item.titleVi || item.titleEn : item.titleEn || item.titleVi),
    [locale],
  );

  return {
    bellButtonRef,
    close,
    dropdownRef,
    getTitle,
    handleMarkAllRead,
    isMarkingAll,
    isOpen,
    toggleOpen,
  };
}
