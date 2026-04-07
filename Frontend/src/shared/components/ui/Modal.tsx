'use client';

import type { ReactNode } from 'react';
import { createPortal } from 'react-dom';
import ModalHeader from '@/shared/components/ui/modal/ModalHeader';
import { useModalLifecycle } from '@/shared/components/ui/modal/useModalLifecycle';
import { cn } from '@/lib/utils';

type ModalSize = 'sm' | 'md' | 'lg';

interface ModalProps {
  children: ReactNode;
  description?: string;
  isOpen: boolean;
  showCloseButton?: boolean;
  size?: ModalSize;
  title?: string;
  onClose: () => void;
}

const sizeStyles: Record<ModalSize, string> = { sm: 'max-w-md', md: 'max-w-lg', lg: 'max-w-2xl' };

export default function Modal({ children, description, isOpen, onClose, title, size = 'md', showCloseButton = true }: ModalProps) {
  useModalLifecycle({ isOpen, onClose });
  if (!isOpen) return null;

  return createPortal(
    <div className={cn('fixed inset-0 z-[9999] flex items-center justify-center p-4')} role="dialog" aria-modal="true" aria-label={title}>
      <div className={cn('absolute inset-0 animate-in fade-in bg-[color:var(--c-70-53-105-28)] duration-200')} onClick={onClose} />
      <div className={cn('relative z-10 w-full rounded-3xl border border-[var(--border-default)] bg-[var(--bg-elevated)] shadow-[var(--shadow-elevated)] ring-1 ring-[color:var(--c-224-224-255-60)] animate-in fade-in zoom-in-95 duration-300', sizeStyles[size])}>
        <ModalHeader title={title} description={description} showCloseButton={showCloseButton} onClose={onClose} />
        <div className={cn('p-6')}>{children}</div>
      </div>
    </div>,
    document.body,
  );
}
