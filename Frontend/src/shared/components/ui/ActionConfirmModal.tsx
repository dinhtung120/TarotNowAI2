import type { ReactNode } from 'react';
import Button from './Button';
import Modal from './Modal';
import { cn } from '@/lib/utils';

interface ActionConfirmModalProps {
 open: boolean;
 icon: ReactNode;
 title: string;
 description: string;
 confirmLabel: string;
 cancelLabel: string;
 onConfirm: () => void;
 onCancel: () => void;
 confirmVariant?: 'primary' | 'danger';
 confirmDisabled?: boolean;
 confirmLoading?: boolean;
}

export default function ActionConfirmModal({
 open,
 icon,
 title,
 description,
 confirmLabel,
 cancelLabel,
 onConfirm,
 onCancel,
 confirmVariant = 'primary',
 confirmDisabled = false,
 confirmLoading = false,
}: ActionConfirmModalProps) {
 return (
  <Modal isOpen={open} onClose={onCancel} title={title} description={description} size="sm">
   <div className={cn("space-y-8")}>
    <div className={cn("mx-auto flex h-16 w-16 items-center justify-center rounded-2xl border shadow-inner")}>
     {icon}
    </div>
    <div className={cn("flex gap-4")}>
     <Button variant="secondary" onClick={onCancel} className={cn("flex-1")}>
      {cancelLabel}
     </Button>
     <Button variant={confirmVariant} onClick={onConfirm} disabled={confirmDisabled} isLoading={confirmLoading} className={cn("flex-1")}>
      {confirmLabel}
     </Button>
    </div>
   </div>
  </Modal>
 );
}
