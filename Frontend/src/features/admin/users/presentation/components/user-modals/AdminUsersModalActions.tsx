'use client';

import type { LucideIcon } from 'lucide-react';
import { Loader2 } from 'lucide-react';
import { Button } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface AdminUsersModalActionsProps {
 cancelLabel: string;
 confirmLabel: string;
 confirmIcon: LucideIcon;
 isLoading: boolean;
 onCancel: () => void;
 onConfirm: () => void;
}

export function AdminUsersModalActions({
 cancelLabel,
 confirmLabel,
 confirmIcon: ConfirmIcon,
 isLoading,
 onCancel,
 onConfirm,
}: AdminUsersModalActionsProps) {
 return (
  <div className={cn('flex gap-4 p-8 pt-0')}>
  <Button variant="secondary" onClick={onCancel} disabled={isLoading} className={cn('flex-1 py-5 shadow-sm')}>
   {cancelLabel}
  </Button>
  <Button variant="primary" onClick={onConfirm} disabled={isLoading} className={cn('flex-1 py-5 tn-btn-primary-shadow')}>
   {isLoading ? <Loader2 className={cn('w-5 h-5 animate-spin mx-auto')} /> : <span className={cn('flex items-center justify-center gap-2')}>{confirmLabel} <ConfirmIcon className={cn('w-4 h-4 ml-1')} /></span>}
  </Button>
  </div>
 );
}
