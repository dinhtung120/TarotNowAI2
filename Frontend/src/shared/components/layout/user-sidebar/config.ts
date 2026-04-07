import {
 Bell,
 Gamepad2,
 Gift,
 History,
 Home,
 LayoutGrid,
 MessageSquare,
 Sparkles,
 User,
 Users,
 Wallet,
 type LucideIcon,
} from 'lucide-react';

export interface UserSidebarMenuItem {
 labelKey: string;
 href: string;
 icon: LucideIcon;
 badge?: number;
}

export interface UserSidebarMenuGroup {
 id: string;
 labelKey: string;
 items: UserSidebarMenuItem[];
}

export const userSidebarMenuGroups: UserSidebarMenuGroup[] = [
 {
  id: 'main',
  labelKey: 'groups.main',
  items: [{ labelKey: 'home', href: '/', icon: Home }],
 },
 {
  id: 'tarot',
  labelKey: 'groups.tarot',
  items: [
   { labelKey: 'readings', href: '/reading', icon: Sparkles },
   { labelKey: 'collection', href: '/collection', icon: LayoutGrid },
   { labelKey: 'history', href: '/reading/history', icon: History },
   { labelKey: 'gamification', href: '/gamification', icon: Gamepad2 },
   { labelKey: 'gacha', href: '/gacha', icon: Gift },
  ],
 },
 {
  id: 'social',
  labelKey: 'groups.social',
  items: [
   { labelKey: 'community', href: '/community', icon: Sparkles },
   { labelKey: 'chat', href: '/chat', icon: MessageSquare },
   { labelKey: 'readers', href: '/readers', icon: Users },
  ],
 },
 {
  id: 'account',
  labelKey: 'groups.account',
  items: [
   { labelKey: 'wallet', href: '/wallet', icon: Wallet },
   { labelKey: 'notifications', href: '/notifications', icon: Bell },
   { labelKey: 'profile', href: '/profile', icon: User },
  ],
 },
];

export function matchesUserSidebarPath(pathname: string, href: string): boolean {
 if (href === '/') return pathname === '/';
 return pathname === href || pathname.startsWith(`${href}/`);
}

export function getUserSidebarMostSpecificActiveHref(pathname: string): string | null {
 let activeHref: string | null = null;

 for (const group of userSidebarMenuGroups) {
  for (const item of group.items) {
   if (!matchesUserSidebarPath(pathname, item.href)) continue;
   if (!activeHref || item.href.length > activeHref.length) {
    activeHref = item.href;
   }
  }
 }

 return activeHref;
}
