import { Menu, X } from 'lucide-react';
import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
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
      <div className={cn('flex items-center tn-gap-2-3-sm')}>
        <Link href="/login" className={cn('inline-flex min-h-11 items-center px-2 text-xs font-medium tn-text-secondary transition-colors')}>{tNav('login')}</Link>
        <Link href="/register" className={cn('inline-flex min-h-11 items-center rounded-xl bg-gradient-to-r from-violet-600 via-fuchsia-500 to-indigo-600 px-3 py-1.5 text-xs font-bold tn-text-ink shadow-lg transition-all')}>{tNav('register')}</Link>
      </div>
    );
  }

  return (
    <div className={cn('flex items-center tn-gap-2-3-sm')}>
      <StreakBadge />
      <div className={cn('tn-show-sm')}><WalletWidget /></div>
      <NotificationDropdown />
      <NavbarAvatarMenu user={user} menuItems={avatarMenuItems} open={avatarMenuOpen} avatarMenuRef={avatarMenuRef} tNav={tNav} onClose={onCloseAvatarMenu} onToggle={onToggleAvatarMenu} onLogout={onLogout} />
      <button type="button" onClick={onToggleMobileMenu} className={cn('tn-hide-md min-h-11 min-w-11 cursor-pointer items-center justify-center rounded-xl p-2.5 tn-text-secondary transition-colors tn-hover-surface-soft tn-hover-text-primary')} aria-label={tCommon('menu')}>
        {mobileMenuOpen ? <X className={cn('h-5 w-5')} /> : <Menu className={cn('h-5 w-5')} />}
      </button>
    </div>
  );
}
