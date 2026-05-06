'use client';

import { useMemo } from 'react';
import { useShallow } from 'zustand/react/shallow';
import { useTranslations } from 'next-intl';
import { usePathname } from '@/i18n/routing';
import { useAuthStore } from '@/features/auth/session/authStore';
import { useNavbarMenuState } from '@/features/app-shell/shared/hooks/useNavbarMenuState';
import { useChatRealtimeSync } from '@/features/chat/realtime/useChatRealtimeSync';
import { useChatUnreadNotifications } from '@/features/chat/inbox/hooks/useChatUnreadNotifications';
import NavbarBrandSection from '@/features/app-shell/shared/app-shell/navigation/navbar/NavbarBrandSection';
import { NavbarMobileMenu } from '@/features/app-shell/navigation/navbar/NavbarMobileMenu';
import NavbarRightSection from '@/features/app-shell/navigation/navbar/NavbarRightSection';
import { getAvatarMenuItems, NAV_LINKS, shouldHideNavbar } from '@/shared/app-shell/navigation/navbar/config';
import { useNavbarLogout } from '@/features/app-shell/shared/app-shell/navigation/navbar/useNavbarLogout';
import { useOptimizedNavigation } from '@/shared/navigation/useOptimizedNavigation';
import {
 normalizePathname,
 shouldEnableRealtimeForPath,
} from '@/shared/navigation/normalizePathname';
import { cn } from '@/lib/utils';

interface NavbarProps {
  onLogout?: () => Promise<unknown> | unknown;
}

function shouldEnableGlobalNavbarRealtime(): boolean {
  return true;
}

export default function Navbar({ onLogout }: NavbarProps = {}) {
  const navigation = useOptimizedNavigation();
  const pathname = usePathname();
  const tNav = useTranslations('Navigation');
  const tCommon = useTranslations('Common');
  const { user, isAuthenticated } = useAuthStore(useShallow((state) => ({ user: state.user, isAuthenticated: state.isAuthenticated })));
  const { avatarMenuOpen, mobileMenuOpen, avatarMenuRef, closeAvatarMenu, toggleAvatarMenu, toggleMobileMenu } = useNavbarMenuState(pathname);
  const normalizedPath = normalizePathname(pathname);
  const allowRealtime = shouldEnableRealtimeForPath(pathname);
  const isChatRoomPath = /\/chat\/[^/]+/.test(normalizedPath);
  const allowNavbarRealtime = allowRealtime && shouldEnableGlobalNavbarRealtime();
  const enableGlobalChatSync = allowNavbarRealtime && !isChatRoomPath;
  const enableMessageSync = allowNavbarRealtime;
  const enableNotificationSync = allowNavbarRealtime;

  useChatUnreadNotifications({ enabled: enableGlobalChatSync });
  useChatRealtimeSync({ enabled: enableGlobalChatSync });

  const avatarMenuItems = useMemo(() => getAvatarMenuItems(user?.role === 'admin'), [user?.role]);
  const handleLogout = useNavbarLogout({
   closeAvatarMenu,
   navigateToLogin: () => navigation.push('/login'),
   onLogout,
  });
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
          messagesEnabled={enableMessageSync}
          mobileMenuOpen={mobileMenuOpen}
          notificationsEnabled={enableNotificationSync}
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
