import type { ReactNode } from 'react';
import Button from './Button';
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
 if (!open) {
  return null;
 }

 return (
  <div className={cn("fixed inset-0 tn-z-150 flex items-center justify-center p-6 animate-in fade-in duration-300")}>
   <div className={cn("absolute inset-0 tn-overlay-strong")} onClick={onCancel} />
   <div className={cn("relative z-10 w-full max-w-sm tn-panel tn-rounded-2_5xl p-10 shadow-2xl animate-in zoom-in-95 duration-300")}>
    <div className={cn("w-16 h-16 rounded-2xl mx-auto mb-6 flex items-center justify-center border shadow-inner")}>{icon}</div>
    <h3 className={cn("text-xl font-black tn-text-primary uppercase italic tracking-tighter text-center mb-2")}>{title}</h3>
    <p className={cn("text-xs tn-text-secondary font-medium text-center leading-relaxed mb-8")}>{description}</p>
    <div className={cn("flex gap-4")}>
     <Button variant="secondary" onClick={onCancel} className={cn("flex-1")}>
      {cancelLabel}
     </Button>
     <Button variant={confirmVariant} onClick={onConfirm} disabled={confirmDisabled} className={cn("flex-1")}>
      {confirmLoading ? '...' : null}
      {confirmLabel}
     </Button>
    </div>
   </div>
  </div>
 );
}
