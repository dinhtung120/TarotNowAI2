'use client';

import { useEffect, useMemo, useRef, useState } from 'react';
import {
 Banknote,
 CreditCard,
 Gift,
 History,
 LayoutDashboard,
 Scale,
 SlidersHorizontal,
 ScrollText,
 Users,
} from 'lucide-react';
import { usePathname } from '@/i18n/routing';
import type { AdminLayoutLabels, MenuConfigItem } from './AdminLayout.types';

const MENU_CONFIG: MenuConfigItem[] = [
 { key: 'overview', href: '/admin', icon: LayoutDashboard },
 { key: 'users', href: '/admin/users', icon: Users },
 { key: 'deposits', href: '/admin/deposits', icon: CreditCard },
 { key: 'promotions', href: '/admin/promotions', icon: Gift },
 { key: 'readings', href: '/admin/readings', icon: History },
 { key: 'readerRequests', href: '/admin/reader-requests', icon: ScrollText },
 { key: 'withdrawals', href: '/admin/withdrawals', icon: Banknote },
 { key: 'disputes', href: '/admin/disputes', icon: Scale },
 { key: 'systemConfigs', href: '/admin/system-configs', icon: SlidersHorizontal },
];

export function useAdminLayoutShellState(labels: AdminLayoutLabels) {
 const pathname = usePathname();
 const [mobileNavOpen, setMobileNavOpen] = useState(false);
 const [desktopNavOpen, setDesktopNavOpen] = useState(false);
 const desktopSidebarRef = useRef<HTMLDivElement>(null);

 const menuItems = useMemo(
  () => MENU_CONFIG.map((item) => ({ ...item, name: labels.menu[item.key] })),
  [labels.menu]
 );

 useEffect(() => {
  if (!mobileNavOpen) return undefined;
  const previousOverflow = document.body.style.overflow;
  document.body.style.overflow = 'hidden';
  return () => {
   document.body.style.overflow = previousOverflow;
  };
 }, [mobileNavOpen]);

 useEffect(() => {
  if (!desktopNavOpen) return undefined;
  const handleClickOutside = (event: MouseEvent) => {
   if (desktopSidebarRef.current?.contains(event.target as Node)) return;
   setDesktopNavOpen(false);
  };
  document.addEventListener('mousedown', handleClickOutside);
  return () => document.removeEventListener('mousedown', handleClickOutside);
 }, [desktopNavOpen]);

 return {
  desktopNavOpen,
  desktopSidebarRef,
  menuItems,
  mobileNavOpen,
  pathname,
  setDesktopNavOpen,
  setMobileNavOpen,
 };
}
