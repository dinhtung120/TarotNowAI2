import { Link } from "@/i18n/routing";
import StreakBadge from "@/features/checkin/presentation/StreakBadge";
import { cn } from "@/lib/utils";
import WalletWidget from "@/shared/components/common/WalletWidget";
import type { NavbarNavItem } from "./config";

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
  <div className={cn("md:hidden absolute left-0 right-0 top-full bg-[var(--bg-glass)] border-b border-[var(--border-subtle)] animate-in slide-in-from-top duration-300 max-h-[calc(100dvh-3.5rem)] overflow-y-auto")}>
   <div className={cn("px-3 sm:px-4 py-4 space-y-1")}>
    {links.map((link) => {
     const Icon = link.icon;
     return (
      <Link
       key={link.href}
       href={link.href}
       className={cn("flex items-center gap-3 px-4 py-3 rounded-xl text-sm font-medium text-[var(--text-secondary)] hover:text-[var(--text-ink)] hover:bg-[var(--purple-50)] transition-colors")}
      >
       <Icon className={cn("w-4 h-4")} />
       {tNav(link.labelKey)}
      </Link>
     );
    })}
    <div className={cn("pt-3 border-t border-[var(--border-subtle)] flex items-center gap-2")}>
     <StreakBadge />
     <WalletWidget />
    </div>
   </div>
  </div>
 );
}
