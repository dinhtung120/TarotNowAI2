import {
  BarChart3,
  Bell,
  CreditCard,
  Gamepad2,
  Gift,
  History,
  LayoutGrid,
  Package,
  MessageSquare,
  Sparkles,
  User,
  Users,
  Wallet,
  type LucideIcon,
} from "lucide-react";

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
    id: "tarot",
    labelKey: "groups.tarot",
    items: [
      { labelKey: "readings", href: "/reading", icon: Sparkles },
      { labelKey: "history", href: "/reading/history", icon: History },
      { labelKey: "collection", href: "/collection", icon: LayoutGrid },
    ],
  },
  {
    id: "social",
    labelKey: "groups.social",
    items: [
      { labelKey: "readers", href: "/readers", icon: Users },
      { labelKey: "chat", href: "/chat", icon: MessageSquare },
      { labelKey: "community", href: "/community", icon: Sparkles },
    ],
  },
  {
    id: "game",
    labelKey: "groups.game",
    items: [
      { labelKey: "gamification", href: "/gamification", icon: Gamepad2 },
      { labelKey: "inventory", href: "/inventory", icon: Package },
      { labelKey: "leaderboard", href: "/leaderboard", icon: BarChart3 },
      { labelKey: "gacha", href: "/gacha", icon: Gift },
      { labelKey: "gachaHistory", href: "/gacha/history", icon: History },
    ],
  },
  {
    id: "account",
    labelKey: "groups.account",
    items: [
      { labelKey: "profile", href: "/profile", icon: User },
      { labelKey: "wallet", href: "/wallet", icon: Wallet },
      { labelKey: "deposit", href: "/wallet/deposit", icon: CreditCard },
      { labelKey: "depositHistory", href: "/wallet/deposit/history", icon: History },
      { labelKey: "notifications", href: "/notifications", icon: Bell },
    ],
  },
];

function matchesUserSidebarPath(pathname: string, href: string): boolean {
  if (href === "/") return pathname === "/";
  return pathname === href || pathname.startsWith(`${href}/`);
}

export function getUserSidebarMostSpecificActiveHref(
  pathname: string,
): string | null {
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
