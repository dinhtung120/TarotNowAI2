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
  <div className={cn('tn-admin-mobile-root fixed inset-0 z-40')}>
   <button type="button" className={cn('absolute inset-0 bg-black/45')} onClick={onClose} aria-label="Close menu" />
   <aside className={cn('absolute left-0 top-0 h-full tn-admin-mobile-drawer-shell border-r shadow-2xl flex flex-col')}>
    <div className={cn('px-4 pt-4')}>
     <button
      type="button"
      onClick={onClose}
      className={cn('ml-auto flex h-11 w-11 items-center justify-center rounded-xl tn-surface tn-mobile-topbar-trigger tn-text-secondary transition-colors')}
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
