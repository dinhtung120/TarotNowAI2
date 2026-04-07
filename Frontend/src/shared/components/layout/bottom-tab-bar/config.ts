import {
 Bell,
 Bookmark,
 Gamepad2,
 Gift,
 History,
 Home,
 MessageSquare,
 Sparkles,
 User,
 Users,
 Wallet,
 type LucideIcon,
} from 'lucide-react';
import type { ComponentProps } from 'react';
import { Link } from '@/i18n/routing';

export interface BottomTabSubItem {
 labelKey: string;
 href: string;
 icon: LucideIcon;
 matchPrefixes: string[];
}

export interface BottomTabGroup {
 id: string;
 labelKey: string;
 icon: LucideIcon;
 href?: string;
 matchPrefixes: string[];
 subItems?: BottomTabSubItem[];
}

export type BottomTabLinkHref = ComponentProps<typeof Link>['href'];

export const bottomTabs: BottomTabGroup[] = [
 {
  id: 'home',
  labelKey: 'home',
  href: '/',
  icon: Home,
  matchPrefixes: ['/'],
 },
 {
  id: 'tarot',
  labelKey: 'tarot',
  icon: Sparkles,
  matchPrefixes: ['/reading', '/collection', '/gamification', '/gacha'],
  subItems: [
   { labelKey: 'readings', href: '/reading', icon: Sparkles, matchPrefixes: ['/reading'] },
   { labelKey: 'collection', href: '/collection', icon: Bookmark, matchPrefixes: ['/collection'] },
   { labelKey: 'history', href: '/reading/history', icon: History, matchPrefixes: ['/reading/history'] },
   { labelKey: 'gamification', href: '/gamification', icon: Gamepad2, matchPrefixes: ['/gamification'] },
   { labelKey: 'gacha', href: '/gacha', icon: Gift, matchPrefixes: ['/gacha'] },
  ],
 },
 {
  id: 'social',
  labelKey: 'social',
  icon: Users,
  matchPrefixes: ['/chat', '/readers', '/community'],
  subItems: [
   { labelKey: 'community', href: '/community', icon: Sparkles, matchPrefixes: ['/community'] },
   { labelKey: 'chat', href: '/chat', icon: MessageSquare, matchPrefixes: ['/chat'] },
   { labelKey: 'readers', href: '/readers', icon: Users, matchPrefixes: ['/readers'] },
  ],
 },
 {
  id: 'account',
  labelKey: 'account',
  icon: User,
  matchPrefixes: ['/wallet', '/profile', '/notifications', '/reader'],
  subItems: [
   { labelKey: 'wallet', href: '/wallet', icon: Wallet, matchPrefixes: ['/wallet'] },
   { labelKey: 'notifications', href: '/notifications', icon: Bell, matchPrefixes: ['/notifications'] },
   { labelKey: 'profile', href: '/profile', icon: User, matchPrefixes: ['/profile', '/reader'] },
  ],
 },
];

export function matchesPath(candidatePath: string, prefix: string): boolean {
 if (prefix === '/') return candidatePath === '/';
 return candidatePath === prefix || candidatePath.startsWith(`${prefix}/`);
}
