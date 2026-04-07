import { useCallback, useEffect } from 'react';

interface UseModalLifecycleArgs {
  isOpen: boolean;
  onClose: () => void;
}

export function useModalLifecycle({ isOpen, onClose }: UseModalLifecycleArgs) {
  const handleEsc = useCallback(
    (event: KeyboardEvent) => {
      if (event.key === 'Escape') onClose();
    },
    [onClose],
  );

  useEffect(() => {
    if (!isOpen) return undefined;
    document.addEventListener('keydown', handleEsc);
    document.body.style.overflow = 'hidden';
    return () => {
      document.removeEventListener('keydown', handleEsc);
      document.body.style.overflow = '';
    };
  }, [handleEsc, isOpen]);
}
