import { LogOut } from 'lucide-react';
import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import type { UserProfile } from '@/features/auth/domain/types';
import type { NavbarNavItem } from '@/shared/components/common/navbar/config';
import NavbarAvatarDropdownHeader from '@/shared/components/common/navbar/avatar-menu/NavbarAvatarDropdownHeader';
import { cn } from '@/lib/utils';

interface NavbarAvatarDropdownProps {
  menuItems: NavbarNavItem[];
  tNav: (key: string) => string;
  user: UserProfile | null;
  onClose: () => void;
  onLogout: () => Promise<void>;
}

export default function NavbarAvatarDropdown({ menuItems, tNav, user, onClose, onLogout }: NavbarAvatarDropdownProps) {
  return (
    <div className={cn('absolute right-0 top-full z-50 mt-2 w-56 overflow-hidden rounded-2xl border tn-border tn-bg-elevated tn-shadow-elevated animate-in fade-in slide-in-from-top-2 duration-200')}>
      <NavbarAvatarDropdownHeader tNav={tNav} user={user} />
      <div className={cn('py-1')}>
        {menuItems.map((item) => {
          const Icon = item.icon;
          return <Link key={item.href} href={item.href} onClick={onClose} className={cn('tn-navbar-dropdown-link flex min-h-11 items-center gap-3 px-4 py-2.5 text-xs font-medium tn-text-secondary transition-colors')}><Icon className={cn('h-4 w-4')} />{tNav(item.labelKey)}</Link>;
        })}
      </div>
      <div className={cn('border-t tn-border-soft py-1')}>
        <button type="button" onClick={() => void onLogout()} className={cn('tn-navbar-logout-btn flex min-h-11 w-full cursor-pointer items-center gap-3 px-4 py-2.5 text-xs font-medium tn-text-danger transition-colors')}><LogOut className={cn('h-4 w-4')} />{tNav('logout')}</button>
      </div>
    </div>
  );
}
