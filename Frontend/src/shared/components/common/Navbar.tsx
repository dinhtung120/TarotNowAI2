'use client';

import { useMemo } from 'react';
import { useShallow } from 'zustand/react/shallow';
import { useTranslations } from 'next-intl';
import { usePathname, useRouter } from '@/i18n/routing';
import { useAuthStore } from '@/store/authStore';
import { useNavbarMenuState } from '@/shared/application/hooks/useNavbarMenuState';
import { useChatRealtimeSync } from '@/shared/application/hooks/useChatRealtimeSync';
import { useChatUnreadNotifications } from '@/shared/application/hooks/useChatUnreadNotifications';
import NavbarBrandSection from '@/shared/components/common/navbar/NavbarBrandSection';
import { NavbarMobileMenu } from '@/shared/components/common/navbar/NavbarMobileMenu';
import NavbarRightSection from '@/shared/components/common/navbar/NavbarRightSection';
import { getAvatarMenuItems, NAV_LINKS, shouldHideNavbar } from '@/shared/components/common/navbar/config';
import { useNavbarLogout } from '@/shared/components/common/navbar/useNavbarLogout';
import { cn } from '@/lib/utils';

interface NavbarProps {
  onLogout?: () => Promise<unknown> | unknown;
}

export default function Navbar({ onLogout }: NavbarProps = {}) {
  const router = useRouter();
  const pathname = usePathname();
  const tNav = useTranslations('Navigation');
  const tCommon = useTranslations('Common');
  const { user, isAuthenticated } = useAuthStore(useShallow((state) => ({ user: state.user, isAuthenticated: state.isAuthenticated })));
  const { avatarMenuOpen, mobileMenuOpen, avatarMenuRef, closeAvatarMenu, toggleAvatarMenu, toggleMobileMenu } = useNavbarMenuState(pathname);

  useChatUnreadNotifications();
  useChatRealtimeSync();

  const avatarMenuItems = useMemo(() => getAvatarMenuItems(user?.role === 'admin'), [user?.role]);
  const handleLogout = useNavbarLogout({ closeAvatarMenu, onLogout, router });
  if (shouldHideNavbar(pathname)) return null;

  return (
    <nav className={cn('fixed left-0 right-0 top-0 z-50 animate-in fade-in slide-in-from-top border-b tn-border-soft tn-bg-glass tn-navbar-padding tn-navbar-shadow duration-500')}>
      <div className={cn('mx-auto flex max-w-7xl items-center justify-between')}>
        <NavbarBrandSection pathname={pathname} tNav={tNav} />
        <NavbarRightSection
          avatarMenuItems={avatarMenuItems}
          avatarMenuOpen={avatarMenuOpen}
          avatarMenuRef={avatarMenuRef}
          isAuthenticated={isAuthenticated}
          mobileMenuOpen={mobileMenuOpen}
          tCommon={tCommon}
          tNav={tNav}
          user={user}
          onCloseAvatarMenu={closeAvatarMenu}
          onLogout={handleLogout}
          onToggleAvatarMenu={toggleAvatarMenu}
          onToggleMobileMenu={toggleMobileMenu}
        />
      </div>
      <NavbarMobileMenu open={mobileMenuOpen} links={NAV_LINKS} tNav={tNav} />
    </nav>
  );
}
