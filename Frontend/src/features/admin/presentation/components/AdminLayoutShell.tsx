'use client';

import dynamic from 'next/dynamic';
import type { ReactNode } from 'react';
import { cn } from '@/lib/utils';
import { AdminLayoutDesktopMenu } from './AdminLayoutDesktopMenu';
import { AdminLayoutMobileDrawer } from './AdminLayoutMobileDrawer';
import { AdminLayoutMobileTopBar } from './AdminLayoutMobileTopBar';
import type { AdminLayoutLabels } from './AdminLayout.types';
import { useAdminLayoutShellState } from './useAdminLayoutShellState';

export type { AdminLayoutLabels } from './AdminLayout.types';

const AstralBackground = dynamic(() => import('@/shared/components/layout/AstralBackground'), {
 ssr: false,
});

interface AdminLayoutShellProps {
 children: ReactNode;
 labels: AdminLayoutLabels;
}

export default function AdminLayoutShell({ children, labels }: AdminLayoutShellProps) {
 const {
  desktopNavOpen,
  desktopSidebarRef,
  menuItems,
  mobileNavOpen,
  pathname,
  setDesktopNavOpen,
  setMobileNavOpen,
 } = useAdminLayoutShellState(labels);

 return (
  <div className={cn('flex h-dvh bg-[var(--bg-void)] text-[var(--text-primary)] overflow-hidden')}>
   <AstralBackground variant="subtle" />

   <AdminLayoutDesktopMenu
    desktopNavOpen={desktopNavOpen}
    desktopSidebarRef={desktopSidebarRef}
    labels={labels}
    menuItems={menuItems}
    onToggle={() => setDesktopNavOpen((prev) => !prev)}
    onClose={() => setDesktopNavOpen(false)}
    pathname={pathname}
   />

   {mobileNavOpen ? <AdminLayoutMobileDrawer labels={labels} menuItems={menuItems} onClose={() => setMobileNavOpen(false)} pathname={pathname} /> : null}

   <main className={cn('relative z-10 flex-1 min-w-0 min-h-0 overflow-y-auto custom-scrollbar')}>
    <AdminLayoutMobileTopBar label={labels.title} sectionMain={labels.sectionMain} onOpenMenu={() => setMobileNavOpen(true)} />
    <div className={cn('min-h-full p-4 sm:p-6 md:p-8 lg:p-12 animate-in fade-in duration-700')}>{children}</div>
   </main>
  </div>
 );
}
