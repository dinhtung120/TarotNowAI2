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
  className?: string; // Bổ sung className để hỗ trợ custom style và animation
  onClose: () => void;
}

const sizeStyles: Record<ModalSize, string> = { sm: 'max-w-md', md: 'max-w-lg', lg: 'max-w-2xl' };

export default function Modal({ 
    children, 
    description, 
    isOpen, 
    onClose, 
    title, 
    size = 'md', 
    showCloseButton = true,
    className // Lấy className từ props
}: ModalProps) {
  useModalLifecycle({ isOpen, onClose });
  if (!isOpen) return null;

  return createPortal(
    <div className={cn('fixed inset-0 tn-z-9999 flex items-center justify-center p-4 pt-24 pb-28 md:p-8')} role="dialog" aria-modal="true" aria-label={title}>
      <div className={cn('absolute inset-0 animate-in fade-in tn-modal-backdrop duration-200')} onClick={onClose} />
      <div className={cn(
        'relative z-10 w-full max-h-full flex flex-col rounded-3xl tn-modal-shell animate-in fade-in zoom-in-95 duration-300', 
        sizeStyles[size],
        className // Áp dụng các class truyền từ ngoài vào (như hiệu ứng rung shake)
      )}>
        <ModalHeader title={title} description={description} showCloseButton={showCloseButton} onClose={onClose} />
        <div className={cn('p-6 overflow-y-auto custom-scrollbar')}>{children}</div>
      </div>
    </div>,
    document.body,
  );
}
