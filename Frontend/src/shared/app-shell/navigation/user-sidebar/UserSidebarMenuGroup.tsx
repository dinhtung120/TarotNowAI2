import type { NavbarNavItem } from '@/shared/app-shell/navigation/navbar/config';
import UserSidebarMenuItem from '@/shared/app-shell/navigation/user-sidebar/UserSidebarMenuItem';
import { cn } from '@/lib/utils';

interface UserSidebarMenuGroupProps {
  activeHref: string | null;
  group: { id: string; items: (NavbarNavItem & { badge?: number })[]; labelKey: string };
  tNav: (key: string) => string;
  tUserNav: (key: string) => string;
  onClose: () => void;
}

export default function UserSidebarMenuGroup({ activeHref, group, tNav, tUserNav, onClose }: UserSidebarMenuGroupProps) {
  return (
    <div>
      <span className={cn('mb-2 block px-4 tn-text-9 font-black uppercase tn-tracking-03 tn-text-muted')}>{tUserNav(group.labelKey)}</span>
      <div className={cn('space-y-1')}>
        {group.items.map((item) => (
          <UserSidebarMenuItem key={item.href} item={item} active={item.href === activeHref} label={tNav(item.labelKey)} onClick={onClose} />
        ))}
      </div>
    </div>
  );
}
