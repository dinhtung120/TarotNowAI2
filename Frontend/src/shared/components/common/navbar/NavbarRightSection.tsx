import { Menu, X } from 'lucide-react';
import { Link } from '@/i18n/routing';
import StreakBadge from '@/features/checkin/presentation/StreakBadge';
import NotificationDropdown from '@/shared/components/common/NotificationDropdown';
import WalletWidget from '@/shared/components/common/WalletWidget';
import { NavbarAvatarMenu } from '@/shared/components/common/navbar/NavbarAvatarMenu';
import type { NavbarNavItem } from '@/shared/components/common/navbar/config';
import type { UserProfile } from '@/features/auth/domain/types';
import { cn } from '@/lib/utils';

interface NavbarRightSectionProps {
  avatarMenuItems: NavbarNavItem[];
  avatarMenuOpen: boolean;
  avatarMenuRef: React.RefObject<HTMLDivElement | null>;
  isAuthenticated: boolean;
  mobileMenuOpen: boolean;
  tCommon: (key: string) => string;
  tNav: (key: string) => string;
  user: UserProfile | null;
  onCloseAvatarMenu: () => void;
  onLogout: () => Promise<void>;
  onToggleAvatarMenu: () => void;
  onToggleMobileMenu: () => void;
}

export default function NavbarRightSection({ avatarMenuItems, avatarMenuOpen, avatarMenuRef, isAuthenticated, mobileMenuOpen, tCommon, tNav, user, onCloseAvatarMenu, onLogout, onToggleAvatarMenu, onToggleMobileMenu }: NavbarRightSectionProps) {
  if (!isAuthenticated) {
    return (
      <div className={cn('flex items-center gap-2 sm:gap-3')}>
        <Link href="/login" className={cn('inline-flex min-h-11 items-center px-2 text-xs font-medium text-[var(--text-secondary)] transition-colors hover:text-[var(--text-ink)] sm:text-sm')}>{tNav('login')}</Link>
        <Link href="/register" className={cn('inline-flex min-h-11 items-center rounded-xl bg-gradient-to-r from-[var(--purple-gradient-from)] via-[var(--purple-gradient-via)] to-[var(--purple-gradient-to)] px-3 py-1.5 text-xs font-bold text-[var(--text-ink)] shadow-[var(--glow-purple-sm)] transition-all hover:brightness-105 sm:px-4 sm:py-2 sm:text-sm')}>{tNav('register')}</Link>
      </div>
    );
  }

  return (
    <div className={cn('flex items-center gap-2 sm:gap-3')}>
      <StreakBadge />
      <div className={cn('hidden sm:block')}><WalletWidget /></div>
      <NotificationDropdown />
      <NavbarAvatarMenu user={user} menuItems={avatarMenuItems} open={avatarMenuOpen} avatarMenuRef={avatarMenuRef} tNav={tNav} onClose={onCloseAvatarMenu} onToggle={onToggleAvatarMenu} onLogout={onLogout} />
      <button type="button" onClick={onToggleMobileMenu} className={cn('inline-flex min-h-11 min-w-11 cursor-pointer items-center justify-center rounded-xl p-2.5 text-[var(--text-secondary)] transition-colors hover:bg-[var(--purple-50)] hover:text-[var(--text-ink)] md:hidden')} aria-label={tCommon('menu')}>
        {mobileMenuOpen ? <X className={cn('h-5 w-5')} /> : <Menu className={cn('h-5 w-5')} />}
      </button>
    </div>
  );
}
