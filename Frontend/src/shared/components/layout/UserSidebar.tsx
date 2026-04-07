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
    <div ref={sidebarRef} className={cn('fixed tn-user-sidebar-anchor z-50')}>
      <button type="button" onClick={() => setIsOpen((value) => !value)} className={cn('flex h-10 w-10 items-center justify-center rounded-xl border tn-user-sidebar-toggle shadow-lg transition-all duration-300')} aria-label="Toggle Sidebar Menu">
        {isOpen ? <X className={cn('h-5 w-5 rotate-90 transition-transform duration-300')} /> : <Menu className={cn('h-5 w-5 transition-transform duration-300')} />}
      </button>
      <UserSidebarPanel activeHref={activeHref} isOpen={isOpen} tNav={tNav} tUserNav={tUserNav} onClose={() => setIsOpen(false)} />
    </div>
  );
}
