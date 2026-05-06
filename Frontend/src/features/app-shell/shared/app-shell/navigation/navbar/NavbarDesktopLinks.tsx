import { OptimizedLink as Link } from '@/shared/navigation/useOptimizedLink';
import { cn } from "@/lib/utils";
import type { NavbarNavItem } from "@/shared/app-shell/navigation/navbar/config";

interface NavbarDesktopLinksProps {
 links: NavbarNavItem[];
 pathname: string;
 tNav: (key: string) => string;
}

export function NavbarDesktopLinks({ links, pathname, tNav }: NavbarDesktopLinksProps) {
 return (
  <div className={cn("tn-hidden-flex-md items-center gap-1")}> 
   {links.map((link) => {
    const isActive = link.href === "/" ? pathname === "/" : pathname.startsWith(link.href);
    const Icon = link.icon;
    return (
     <Link
      key={link.href}
      href={link.href}
      className={cn(
       "flex items-center gap-2 px-3 py-2 rounded-xl text-sm font-medium transition-all duration-300 min-h-11",
       isActive
        ? "text-[var(--text-ink)] bg-[var(--bg-elevated)] border border-[var(--border-default)] shadow-[var(--shadow-card)]"
        : "text-[var(--text-secondary)] hover:text-[var(--text-ink)] hover:bg-[var(--purple-50)]",
      )}
     >
      <Icon className={cn("w-4 h-4")} />
      <span className={cn("text-xs font-bold tracking-wide")}>{tNav(link.labelKey)}</span>
     </Link>
    );
   })}
  </div>
 );
}
