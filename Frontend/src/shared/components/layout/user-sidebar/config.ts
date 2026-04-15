 import {
  BarChart3,
  Bell,
 Gamepad2,
 Gift,
 History,
 Home,
 LayoutGrid,
 Package,
 MessageSquare,
 Sparkles,
 User,
 Users,
 Wallet,
 type LucideIcon,
} from 'lucide-react';

interface UserSidebarMenuItem {
 labelKey: string;
 href: string;
 icon: LucideIcon;
 badge?: number;
}

interface UserSidebarMenuGroup {
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
   { labelKey: 'inventory', href: '/inventory', icon: Package },
   { labelKey: 'history', href: '/reading/history', icon: History },
   { labelKey: 'gamification', href: '/gamification', icon: Gamepad2 },
   { labelKey: 'leaderboard', href: '/leaderboard', icon: BarChart3 },
   { labelKey: 'gacha', href: '/gacha', icon: Gift },
   { labelKey: 'gachaHistory', href: '/gacha/history', icon: History },
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

function matchesUserSidebarPath(pathname: string, href: string): boolean {
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
