import { History, Home, ShieldCheck, Sparkles, User, Users, Wallet } from "lucide-react";
import type { LucideIcon } from "lucide-react";

export interface NavbarNavItem {
 labelKey: string;
 href: string;
 icon: LucideIcon;
}

const AUTHLESS_PATHS = [
 "/login",
 "/register",
 "/forgot-password",
 "/reset-password",
 "/verify-email",
 "/admin",
] as const;

export const NAV_LINKS: NavbarNavItem[] = [
 { labelKey: "home", href: "/", icon: Home },
 { labelKey: "readings", href: "/reading", icon: Sparkles },
 { labelKey: "readers", href: "/readers", icon: Users },
];

export function getAvatarMenuItems(isAdmin: boolean): NavbarNavItem[] {
 return [
  { labelKey: "profile", href: "/profile", icon: User },
  { labelKey: "wallet", href: "/wallet", icon: Wallet },
  { labelKey: "history", href: "/reading/history", icon: History },
  ...(isAdmin
   ? [{ labelKey: "adminPortal", href: "/admin", icon: ShieldCheck }]
   : []),
 ];
}

export function shouldHideNavbar(pathname: string): boolean {
 return AUTHLESS_PATHS.some((path) => pathname.includes(path));
}
