import type { RefObject } from 'react';
import type { UserProfile } from '@/features/auth/session/types';
import NavbarAvatarDropdown from '@/features/app-shell/navigation/navbar/avatar-menu/NavbarAvatarDropdown';
import NavbarAvatarTrigger from '@/features/app-shell/navigation/navbar/avatar-menu/NavbarAvatarTrigger';
import type { NavbarNavItem } from '@/shared/app-shell/navigation/navbar/config';
import { cn } from '@/lib/utils';

interface NavbarAvatarMenuProps {
  avatarMenuRef: RefObject<HTMLDivElement | null>;
  menuItems: NavbarNavItem[];
  open: boolean;
  tNav: (key: string) => string;
  user: UserProfile | null;
  onClose: () => void;
  onLogout: () => Promise<void>;
  onToggle: () => void;
}

export function NavbarAvatarMenu({ avatarMenuRef, menuItems, open, tNav, user, onClose, onLogout, onToggle }: NavbarAvatarMenuProps) {
  return (
    <div ref={avatarMenuRef} className={cn('relative')}>
      <NavbarAvatarTrigger open={open} tNav={tNav} user={user} onToggle={onToggle} />
      {open ? <NavbarAvatarDropdown menuItems={menuItems} tNav={tNav} user={user} onClose={onClose} onLogout={onLogout} /> : null}
    </div>
  );
}
