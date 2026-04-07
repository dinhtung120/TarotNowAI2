'use client';

import { X } from 'lucide-react';
import { cn } from '@/lib/utils';
import { AdminSidebarContent } from './AdminSidebarContent';
import type { AdminLayoutLabels, AdminSidebarMenuItem } from './AdminLayout.types';

interface AdminLayoutMobileDrawerProps {
 labels: AdminLayoutLabels;
 menuItems: AdminSidebarMenuItem[];
 onClose: () => void;
 pathname: string;
}

export function AdminLayoutMobileDrawer({
 labels,
 menuItems,
 onClose,
 pathname,
}: AdminLayoutMobileDrawerProps) {
 return (
  <div className={cn('lg:hidden fixed inset-0 z-40')}>
   <button type="button" className={cn('absolute inset-0 bg-black/45')} onClick={onClose} aria-label="Close menu" />
   <aside className={cn('absolute left-0 top-0 h-full w-[min(22rem,86vw)] bg-[var(--bg-glass)] border-r border-[var(--border-subtle)] shadow-2xl flex flex-col')}>
    <div className={cn('px-4 pt-4')}>
     <button
      type="button"
      onClick={onClose}
      className={cn('ml-auto flex h-11 w-11 items-center justify-center rounded-xl tn-surface hover:tn-surface-strong tn-text-secondary hover:tn-text-primary transition-colors')}
      aria-label="Close menu"
     >
      <X className={cn('w-4 h-4')} />
     </button>
    </div>
    <AdminSidebarContent labels={labels} menuItems={menuItems} pathname={pathname} onClose={onClose} />
   </aside>
  </div>
 );
}
