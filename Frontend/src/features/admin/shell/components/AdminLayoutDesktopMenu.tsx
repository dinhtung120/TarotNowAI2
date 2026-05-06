'use client';

import type { RefObject } from 'react';
import { Menu, X } from 'lucide-react';
import { cn } from '@/lib/utils';
import { AdminSidebarContent } from './AdminSidebarContent';
import type { AdminLayoutLabels, AdminSidebarMenuItem } from './AdminLayout.types';

interface AdminLayoutDesktopMenuProps {
 desktopNavOpen: boolean;
 desktopSidebarRef: RefObject<HTMLDivElement | null>;
 labels: AdminLayoutLabels;
 menuItems: AdminSidebarMenuItem[];
 onClose: () => void;
 onToggle: () => void;
 pathname: string;
}

export function AdminLayoutDesktopMenu({
 desktopNavOpen,
 desktopSidebarRef,
 labels,
 menuItems,
 onClose,
 onToggle,
 pathname,
}: AdminLayoutDesktopMenuProps) {
 return (
  <div ref={desktopSidebarRef} className={cn('tn-admin-desktop-anchor fixed top-3 left-4 z-50')}>
   <button
    type="button"
    onClick={onToggle}
    className={cn('flex items-center justify-center w-12 h-12 rounded-2xl border tn-admin-desktop-toggle backdrop-blur-md shadow-lg transition-all duration-300')}
    aria-label="Toggle Admin Menu"
   >
    {desktopNavOpen ? <X className={cn('w-6 h-6 transition-transform duration-300 rotate-90')} /> : <Menu className={cn('w-6 h-6 transition-transform duration-300')} />}
   </button>
   <aside
    className={cn(
     'absolute top-14 left-0 tn-admin-desktop-aside border rounded-3xl flex-col backdrop-blur-2xl transition-all duration-300 origin-top-left overflow-hidden',
     desktopNavOpen ? 'flex opacity-100 scale-100 visible' : 'opacity-0 scale-95 invisible pointer-events-none'
    )}
   >
    <AdminSidebarContent
     labels={labels}
     menuItems={menuItems}
     pathname={pathname}
     isDropdown
     onClose={onClose}
    />
   </aside>
  </div>
 );
}
