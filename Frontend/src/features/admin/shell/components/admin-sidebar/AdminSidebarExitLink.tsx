'use client';

import { LogOut } from 'lucide-react';
import { OptimizedLink as Link } from '@/shared/navigation/useOptimizedLink';
import { cn } from '@/lib/utils';

interface AdminSidebarExitLinkProps {
 isDropdown: boolean;
 label: string;
 onClose: () => void;
}

export function AdminSidebarExitLink({
 isDropdown,
 label,
 onClose,
}: AdminSidebarExitLinkProps) {
 return (
  <div className={cn('border-t tn-border-soft shrink-0', isDropdown ? 'p-4' : 'tn-admin-sidebar-brand-pad')}>
   <Link
    href="/"
    onClick={onClose}
    className={cn('tn-admin-exit-link flex items-center justify-center gap-3 px-6 py-4 rounded-2xl tn-bg-elevated border tn-border-soft transition-all group')}
   >
    <LogOut className={cn('tn-group-text-danger w-5 h-5 tn-text-secondary transition-colors')} />
    <span className={cn('tn-text-10 font-black uppercase tracking-widest')}>{label}</span>
   </Link>
  </div>
 );
}
