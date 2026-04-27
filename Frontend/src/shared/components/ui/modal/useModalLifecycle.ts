import { useCallback, useEffect, useRef, type RefObject } from 'react';

interface UseModalLifecycleArgs {
  isOpen: boolean;
  onClose: () => void;
  containerRef?: RefObject<HTMLElement | null>;
}

function resolveFocusableElements(container: HTMLElement): HTMLElement[] {
  const selector = [
    'a[href]',
    'button:not([disabled])',
    'input:not([disabled])',
    'textarea:not([disabled])',
    'select:not([disabled])',
    '[tabindex]:not([tabindex="-1"])',
  ].join(',');

  return Array.from(container.querySelectorAll<HTMLElement>(selector))
    .filter((element) => !element.hasAttribute('disabled') && element.getAttribute('aria-hidden') !== 'true');
}

export function useModalLifecycle({ isOpen, onClose, containerRef }: UseModalLifecycleArgs) {
  const previousActiveElementRef = useRef<HTMLElement | null>(null);

  const handleKeyDown = useCallback(
    (event: KeyboardEvent) => {
      if (event.key === 'Escape') {
        onClose();
        return;
      }

      if (event.key !== 'Tab' || !containerRef?.current) {
        return;
      }

      const focusableElements = resolveFocusableElements(containerRef.current);
      if (focusableElements.length === 0) {
        event.preventDefault();
        containerRef.current.focus();
        return;
      }

      const firstElement = focusableElements[0];
      const lastElement = focusableElements[focusableElements.length - 1];
      const activeElement = document.activeElement as HTMLElement | null;

      if (event.shiftKey && activeElement === firstElement) {
        event.preventDefault();
        lastElement.focus();
      } else if (!event.shiftKey && activeElement === lastElement) {
        event.preventDefault();
        firstElement.focus();
      }
    },
    [containerRef, onClose],
  );

  useEffect(() => {
    if (!isOpen) return undefined;
    previousActiveElementRef.current = document.activeElement instanceof HTMLElement
      ? document.activeElement
      : null;
    document.addEventListener('keydown', handleKeyDown);
    document.body.style.overflow = 'hidden';
    document.documentElement.style.overflow = 'hidden';

    const focusTimer = window.setTimeout(() => {
      const container = containerRef?.current;
      if (!container) {
        return;
      }

      const focusableElements = resolveFocusableElements(container);
      const nextFocusTarget = focusableElements[0] ?? container;
      nextFocusTarget.focus();
    }, 0);

    return () => {
      window.clearTimeout(focusTimer);
      document.removeEventListener('keydown', handleKeyDown);
      document.body.style.overflow = '';
      document.documentElement.style.overflow = '';
      previousActiveElementRef.current?.focus();
      previousActiveElementRef.current = null;
    };
  }, [containerRef, handleKeyDown, isOpen]);
}
