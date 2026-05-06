import {
  BarChart3,
  Bell,
  Bookmark,
  CreditCard,
  Gamepad2,
  Gift,
  History,
  Home,
  MessageSquare,
  Package,
  Sparkles,
  User,
  Users,
  Wallet,
  type LucideIcon,
} from "lucide-react";
import type { ComponentProps } from "react";
import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';

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

export type BottomTabLinkHref = ComponentProps<typeof Link>["href"];

export const bottomTabs: BottomTabGroup[] = [
  {
    id: "home",
    labelKey: "home",
    href: "/",
    icon: Home,
    matchPrefixes: ["/"],
  },
  {
    id: "tarot",
    labelKey: "tarot",
    icon: Sparkles,
    matchPrefixes: ["/reading", "/collection"],
    subItems: [
      {
        labelKey: "readings",
        href: "/reading",
        icon: Sparkles,
        matchPrefixes: ["/reading"],
      },
      {
        labelKey: "history",
        href: "/reading/history",
        icon: History,
        matchPrefixes: ["/reading/history"],
      },
      {
        labelKey: "collection",
        href: "/collection",
        icon: Bookmark,
        matchPrefixes: ["/collection"],
      },
    ],
  },
  {
    id: "social",
    labelKey: "social",
    icon: Users,
    matchPrefixes: ["/chat", "/readers", "/community"],
    subItems: [
      {
        labelKey: "readers",
        href: "/readers",
        icon: Users,
        matchPrefixes: ["/readers"],
      },
      {
        labelKey: "chat",
        href: "/chat",
        icon: MessageSquare,
        matchPrefixes: ["/chat"],
      },
      {
        labelKey: "community",
        href: "/community",
        icon: Sparkles,
        matchPrefixes: ["/community"],
      },
    ],
  },
  {
    id: "game",
    labelKey: "game",
    icon: Gamepad2,
    matchPrefixes: ["/gamification", "/inventory", "/leaderboard", "/gacha"],
    subItems: [
      {
        labelKey: "gamification",
        href: "/gamification",
        icon: Gamepad2,
        matchPrefixes: ["/gamification"],
      },
      {
        labelKey: "inventory",
        href: "/inventory",
        icon: Package,
        matchPrefixes: ["/inventory"],
      },
      {
        labelKey: "leaderboard",
        href: "/leaderboard",
        icon: BarChart3,
        matchPrefixes: ["/leaderboard"],
      },
      {
        labelKey: "gacha",
        href: "/gacha",
        icon: Gift,
        matchPrefixes: ["/gacha"],
      },
      {
        labelKey: "gachaHistory",
        href: "/gacha/history",
        icon: History,
        matchPrefixes: ["/gacha/history"],
      },
    ],
  },
  {
    id: "account",
    labelKey: "account",
    icon: User,
    matchPrefixes: ["/wallet", "/profile", "/notifications", "/reader"],
    subItems: [
      {
        labelKey: "profile",
        href: "/profile",
        icon: User,
        matchPrefixes: ["/profile", "/reader"],
      },
      {
        labelKey: "wallet",
        href: "/wallet",
        icon: Wallet,
        matchPrefixes: ["/wallet"],
      },
      {
        labelKey: "deposit",
        href: "/wallet/deposit",
        icon: CreditCard,
        matchPrefixes: ["/wallet/deposit"],
      },
      {
        labelKey: "notifications",
        href: "/notifications",
        icon: Bell,
        matchPrefixes: ["/notifications"],
      },
    ],
  },
];

export function matchesPath(candidatePath: string, prefix: string): boolean {
  if (prefix === "/") return candidatePath === "/";
  return candidatePath === prefix || candidatePath.startsWith(`${prefix}/`);
}
