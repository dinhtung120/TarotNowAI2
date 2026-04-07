'use client';

import type { LucideIcon } from 'lucide-react';
import { X } from 'lucide-react';
import { cn } from '@/lib/utils';

interface AdminUsersModalHeaderProps {
 cancelLabel: string;
 Icon: LucideIcon;
 iconClassName: string;
 onClose: () => void;
 subtitle: string;
 title: string;
}

export function AdminUsersModalHeader({
 cancelLabel,
 Icon,
 iconClassName,
 onClose,
 subtitle,
 title,
}: AdminUsersModalHeaderProps) {
 return (
  <div className={cn('p-8 border-b tn-border-soft tn-surface flex items-center justify-between')}>
   <div className={cn('flex items-center gap-4')}>
   <div className={cn('w-12 h-12 rounded-2xl border flex items-center justify-center shadow-inner', iconClassName)}>
     <Icon className={cn('w-6 h-6')} />
    </div>
    <div className={cn('text-left')}>
     <h2 className={cn('text-xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md')}>{title}</h2>
     <p className={cn('tn-text-9 font-black tn-text-tertiary uppercase tracking-widest')}>{subtitle}</p>
    </div>
   </div>
   <button type="button" onClick={onClose} className={cn('w-10 h-10 rounded-full tn-surface flex items-center justify-center tn-admin-modal-close-btn transition-all shadow-xl border')} aria-label={cancelLabel}>
    <X className={cn('w-5 h-5')} />
   </button>
  </div>
 );
}
