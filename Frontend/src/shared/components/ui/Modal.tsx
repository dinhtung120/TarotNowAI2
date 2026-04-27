'use client';

import type { ReactNode } from 'react';
import { useId, useRef } from 'react';
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
  const panelRef = useRef<HTMLDivElement | null>(null);
  const titleId = useId();
  useModalLifecycle({ isOpen, onClose, containerRef: panelRef });
  if (!isOpen) return null;

  return createPortal(
    <div className={cn('fixed inset-0 tn-z-9999 overflow-y-auto custom-scrollbar flex flex-col pointer-events-none')} role="dialog" aria-modal="true" aria-labelledby={title ? titleId : undefined} aria-label={title ? undefined : 'Dialog'}>
      <button type="button" aria-label="Close dialog" className={cn('fixed inset-0 animate-in fade-in tn-modal-backdrop duration-200 pointer-events-auto touch-none')} onClick={onClose} />
      <div className={cn('relative z-10 flex min-h-full w-full flex-col p-4 pt-24 pb-28 md:p-8 pointer-events-auto')}>
        <div className={cn(
          'relative m-auto w-full flex flex-col rounded-3xl tn-modal-shell animate-in fade-in zoom-in-95 duration-300', 
          sizeStyles[size],
          className
        )} ref={panelRef} tabIndex={-1}>
          <ModalHeader title={title} description={description} showCloseButton={showCloseButton} onClose={onClose} titleId={title ? titleId : undefined} />
          <div className={cn('p-6')}>{children}</div>
        </div>
      </div>
    </div>,
    document.body,
  );
}
