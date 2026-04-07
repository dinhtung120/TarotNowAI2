'use client';

import { Menu, X } from 'lucide-react';
import { useMemo } from 'react';
import { useTranslations } from 'next-intl';
import { usePathname } from '@/i18n/routing';
import UserSidebarPanel from '@/shared/components/layout/user-sidebar/UserSidebarPanel';
import { getUserSidebarMostSpecificActiveHref } from '@/shared/components/layout/user-sidebar/config';
import { useUserSidebarState } from '@/shared/components/layout/user-sidebar/useUserSidebarState';
import { cn } from '@/lib/utils';

export default function UserSidebar() {
  const pathname = usePathname();
  const activeHref = useMemo(() => getUserSidebarMostSpecificActiveHref(pathname), [pathname]);
  const tNav = useTranslations('Navigation');
  const tUserNav = useTranslations('UserNav');
  const { isOpen, setIsOpen, sidebarRef } = useUserSidebarState();

  return (
    <div ref={sidebarRef} className={cn('fixed left-4 top-[4.5rem] z-50 hidden md:block')}>
      <button type="button" onClick={() => setIsOpen((value) => !value)} className={cn('flex h-10 w-10 items-center justify-center rounded-xl border border-[var(--border-subtle)] bg-[var(--bg-glass)]/60 text-[var(--text-secondary)] shadow-lg transition-all duration-300 hover:border-[var(--border-hover)] hover:bg-[var(--bg-elevated)] hover:text-[var(--text-ink)]')} aria-label="Toggle Sidebar Menu">
        {isOpen ? <X className={cn('h-5 w-5 rotate-90 transition-transform duration-300')} /> : <Menu className={cn('h-5 w-5 transition-transform duration-300')} />}
      </button>
      <UserSidebarPanel activeHref={activeHref} isOpen={isOpen} tNav={tNav} tUserNav={tUserNav} onClose={() => setIsOpen(false)} />
    </div>
  );
}
