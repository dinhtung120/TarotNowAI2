'use client';

import { LogOut } from 'lucide-react';
import { Link } from '@/i18n/routing';
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
  <div className={cn('border-t border-[var(--border-subtle)] shrink-0', isDropdown ? 'p-4' : 'p-6 sm:p-8')}>
   <Link
    href="/"
    onClick={onClose}
    className={cn('flex items-center justify-center gap-3 px-6 py-4 rounded-2xl bg-[var(--bg-elevated)] border border-[var(--border-subtle)] hover:bg-[var(--danger)]/10 hover:border-[var(--danger)]/20 hover:text-[var(--danger)] transition-all group')}
   >
    <LogOut className={cn('w-5 h-5 text-[var(--text-secondary)] group-hover:text-[var(--danger)] transition-colors')} />
    <span className={cn('text-[10px] font-black uppercase tracking-widest')}>{label}</span>
   </Link>
  </div>
 );
}
