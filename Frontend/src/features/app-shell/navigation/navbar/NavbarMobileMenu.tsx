import { OptimizedLink as Link } from '@/shared/navigation/useOptimizedLink';
import StreakBadge from "@/features/checkin/streak/StreakBadge";
import { cn } from "@/lib/utils";
import { WalletWidget } from "@/features/wallet/public";
import type { NavbarNavItem } from "@/shared/app-shell/navigation/navbar/config";

interface NavbarMobileMenuProps {
 open: boolean;
 links: NavbarNavItem[];
 tNav: (key: string) => string;
}

export function NavbarMobileMenu({ open, links, tNav }: NavbarMobileMenuProps) {
 if (!open) {
  return null;
 }

 return (
  <div className={cn("tn-navbar-mobile-menu-shell animate-in slide-in-from-top duration-300")}>
   <div className={cn("tn-navbar-mobile-menu-inner space-y-1")}>
    {links.map((link) => {
     const Icon = link.icon;
     return (
      <Link
       key={link.href}
       href={link.href}
       className={cn("flex items-center gap-3 px-4 py-3 rounded-xl text-sm font-medium tn-navbar-mobile-link")}
      >
       <Icon className={cn("w-4 h-4")} />
       {tNav(link.labelKey)}
      </Link>
     );
    })}
    <div className={cn("pt-3 border-t tn-border-soft flex items-center gap-2")}>
     <StreakBadge />
     <WalletWidget />
    </div>
   </div>
  </div>
 );
}
