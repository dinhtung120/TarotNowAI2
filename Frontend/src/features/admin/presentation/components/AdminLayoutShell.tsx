'use client';

import dynamic from 'next/dynamic';
import { type ReactNode, useEffect, useMemo, useState, useRef } from 'react';
import {
 Users,
 CreditCard,
 Gift,
 History,
 LayoutDashboard,
 ShieldCheck,
 ChevronRight,
 LogOut,
 ScrollText,
 Banknote,
 Scale,
 Menu,
 X,
 type LucideIcon,
} from 'lucide-react';
import { Link, usePathname } from '@/i18n/routing';
import { cn } from '@/shared/utils/cn';

const AstralBackground = dynamic(() => import('@/shared/components/layout/AstralBackground'), {
 ssr: false,
});

export interface AdminLayoutLabels {
 title: string;
 subtitle: string;
 sectionMain: string;
 exitPortal: string;
 menu: {
  overview: string;
  users: string;
  deposits: string;
  promotions: string;
  readings: string;
  readerRequests: string;
  withdrawals: string;
  disputes: string;
 };
}

interface AdminLayoutShellProps {
 children: ReactNode;
 labels: AdminLayoutLabels;
}

interface MenuConfigItem {
 key: keyof AdminLayoutLabels['menu'];
 href: string;
 icon: LucideIcon;
}

const MENU_CONFIG: MenuConfigItem[] = [
 { key: 'overview', href: '/admin', icon: LayoutDashboard },
 { key: 'users', href: '/admin/users', icon: Users },
 { key: 'deposits', href: '/admin/deposits', icon: CreditCard },
 { key: 'promotions', href: '/admin/promotions', icon: Gift },
 { key: 'readings', href: '/admin/readings', icon: History },
 { key: 'readerRequests', href: '/admin/reader-requests', icon: ScrollText },
 { key: 'withdrawals', href: '/admin/withdrawals', icon: Banknote },
 { key: 'disputes', href: '/admin/disputes', icon: Scale },
];

const isRouteActive = (pathname: string, href: string) => {
 if (href === '/admin') return pathname === href;
 return pathname.startsWith(href);
};

export default function AdminLayoutShell({ children, labels }: AdminLayoutShellProps) {
 const pathname = usePathname();
 const [mobileNavOpen, setMobileNavOpen] = useState(false);
 const [desktopNavOpen, setDesktopNavOpen] = useState(false);
 const desktopSidebarRef = useRef<HTMLDivElement>(null);

 const menuItems = useMemo(
  () =>
   MENU_CONFIG.map((item) => ({
    ...item,
    name: labels.menu[item.key],
   })),
  [labels.menu]
 );

 // Prevent scrolling when mobile nav is open
 useEffect(() => {
  if (!mobileNavOpen) return undefined;
  const previousOverflow = document.body.style.overflow;
  document.body.style.overflow = 'hidden';
  return () => {
   document.body.style.overflow = previousOverflow;
  };
 }, [mobileNavOpen]);

 // Close desktop nav when clicking outside
 useEffect(() => {
  const handleClickOutside = (event: MouseEvent) => {
   if (desktopSidebarRef.current && !desktopSidebarRef.current.contains(event.target as Node)) {
    setDesktopNavOpen(false);
   }
  };
  if (desktopNavOpen) {
   document.addEventListener('mousedown', handleClickOutside);
  }
  return () => {
   document.removeEventListener('mousedown', handleClickOutside);
  };
 }, [desktopNavOpen]);

 // Close all navs on route change
 useEffect(() => {
  setDesktopNavOpen(false);
  setMobileNavOpen(false);
 }, [pathname]);

 const renderSidebarContent = (mobile = false, isDropdown = false) => (
  <>
   <div className={cn("mb-2 sm:mb-4", isDropdown ? "p-4" : "p-6 sm:p-8")}>
    <div className="flex items-center gap-3 group px-4 py-3 rounded-2xl bg-[var(--bg-elevated)] border border-[var(--border-default)] shadow-[var(--shadow-card)] overflow-hidden relative">
     <div className="absolute inset-0 bg-gradient-to-r from-[var(--purple-accent)]/10 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-500" />
     <div className="w-10 h-10 rounded-xl bg-[var(--purple-accent)]/10 flex items-center justify-center border border-[var(--purple-accent)]/20 group-hover:scale-110 transition-transform duration-500 shrink-0">
      <ShieldCheck className="w-6 h-6 text-[var(--purple-accent)]" />
     </div>
     <div className="relative z-10 flex-1 min-w-0">
      <h2 className="text-sm font-black text-[var(--text-ink)] tracking-widest uppercase italic truncate">
       {labels.title}
      </h2>
      <div className="text-[10px] font-bold text-[var(--purple-muted)] tracking-tighter uppercase leading-none truncate">
       {labels.subtitle}
      </div>
     </div>
    </div>
   </div>

   <nav className={cn("flex-1 px-4 space-y-2 overflow-y-auto no-scrollbar", isDropdown && "max-h-[60vh] custom-scrollbar")}>
    <div className={cn("mb-4", isDropdown ? "px-2" : "px-6")}>
     <span className="text-[9px] font-black uppercase tracking-[0.3em] text-[var(--text-muted)]">
      {labels.sectionMain}
     </span>
    </div>

    {menuItems.map((item) => {
     const active = isRouteActive(pathname, item.href);
     const Icon = item.icon;

     return (
      <Link
       key={item.href}
       href={item.href}
       onClick={() => {
        if (mobile) setMobileNavOpen(false);
       }}
       className={cn(
        'group flex items-center justify-between px-5 sm:px-6 py-4 rounded-2xl transition-all duration-300 relative overflow-hidden',
        active
         ? 'bg-[var(--bg-elevated)] text-[var(--text-ink)] shadow-[var(--glow-purple-sm)] border border-[var(--border-hover)]'
         : 'hover:bg-[var(--bg-surface-hover)] hover:text-[var(--text-ink)] text-[var(--text-secondary)] border border-transparent'
       )}
      >
       <div className="flex items-center gap-4 relative z-10 font-bold">
        <Icon
         className={cn(
          'w-5 h-5 transition-all duration-300',
          active ? 'text-[var(--purple-accent)] scale-110' : 'group-hover:text-[var(--text-ink)]'
         )}
        />
        <span
         className={cn('text-[11px] uppercase tracking-widest', active ? 'font-black' : 'font-bold')}
        >
         {item.name}
        </span>
       </div>

       {active && (
        <div className="relative z-10">
         <ChevronRight className="w-4 h-4 text-[var(--purple-muted)]" />
        </div>
       )}

       {active && (
        <div className="absolute left-0 top-1/4 bottom-1/4 w-1 bg-[var(--purple-accent)] rounded-r-full shadow-[0_0_10px_var(--purple-accent)]" />
       )}
      </Link>
     );
    })}
   </nav>

   <div className={cn("border-t border-[var(--border-subtle)] shrink-0", isDropdown ? "p-4" : "p-6 sm:p-8")}>
    <Link
     href="/"
     onClick={() => {
      if (mobile) setMobileNavOpen(false);
      else setDesktopNavOpen(false);
     }}
     className="flex items-center justify-center gap-3 px-6 py-4 rounded-2xl bg-[var(--bg-elevated)] border border-[var(--border-subtle)] hover:bg-[var(--danger)]/10 hover:border-[var(--danger)]/20 hover:text-[var(--danger)] transition-all group"
    >
     <LogOut className="w-5 h-5 text-[var(--text-secondary)] group-hover:text-[var(--danger)] transition-colors" />
     <span className="text-[10px] font-black uppercase tracking-widest">{labels.exitPortal}</span>
    </Link>
   </div>
  </>
 );

 return (
  <div className="flex h-dvh bg-[var(--bg-void)] text-[var(--text-primary)] overflow-hidden">
   <AstralBackground variant="subtle" />

   {/* Desktop Dropdown Sidebar Toggle */}
   <div ref={desktopSidebarRef} className="hidden lg:block fixed top-3 left-4 z-50">
    <button
     onClick={() => setDesktopNavOpen(!desktopNavOpen)}
     className="flex items-center justify-center w-12 h-12 rounded-2xl bg-[var(--bg-glass)]/80 border border-[var(--border-subtle)] backdrop-blur-md shadow-lg transition-all duration-300 hover:bg-[var(--bg-elevated)] hover:border-[var(--border-hover)] text-[var(--text-secondary)] hover:text-[var(--text-ink)]"
     aria-label="Toggle Admin Menu"
    >
     {desktopNavOpen ? (
      <X className="w-6 h-6 transition-transform duration-300 rotate-90" />
     ) : (
      <Menu className="w-6 h-6 transition-transform duration-300" />
     )}
    </button>

    {/* Dropdown Navigation Menu */}
    <aside
     className={cn(
      "absolute top-14 left-0 w-[19rem] bg-[var(--bg-overlay)]/95 border border-[var(--border-subtle)] rounded-3xl flex-col backdrop-blur-2xl shadow-[var(--glow-purple-lg)] transition-all duration-300 origin-top-left overflow-hidden",
      desktopNavOpen ? "flex opacity-100 scale-100 visible" : "opacity-0 scale-95 invisible pointer-events-none"
     )}
    >
     {renderSidebarContent(false, true)}
    </aside>
   </div>

   {mobileNavOpen && (
    <div className="lg:hidden fixed inset-0 z-40">
     <button
      type="button"
      className="absolute inset-0 bg-black/45"
      onClick={() => setMobileNavOpen(false)}
      aria-label="Close menu"
     />
     <aside className="absolute left-0 top-0 h-full w-[min(22rem,86vw)] bg-[var(--bg-glass)] border-r border-[var(--border-subtle)] shadow-2xl flex flex-col">
      <div className="px-4 pt-4">
       <button
        type="button"
        onClick={() => setMobileNavOpen(false)}
        className="ml-auto flex h-11 w-11 items-center justify-center rounded-xl tn-surface hover:tn-surface-strong tn-text-secondary hover:tn-text-primary transition-colors"
        aria-label="Close menu"
       >
        <X className="w-4 h-4" />
       </button>
      </div>
      {renderSidebarContent(true)}
     </aside>
    </div>
   )}

   <main className="relative z-10 flex-1 min-w-0 min-h-0 overflow-y-auto custom-scrollbar">
    <div className="lg:hidden sticky top-0 z-30 bg-[var(--bg-glass)]/95 backdrop-blur border-b border-[var(--border-subtle)] px-4 py-3 flex items-center justify-between">
     <button
      type="button"
      onClick={() => setMobileNavOpen(true)}
      className="inline-flex items-center gap-2 px-3 py-2 rounded-xl tn-surface hover:tn-surface-strong tn-text-secondary hover:tn-text-primary transition-colors min-h-11"
     >
      <Menu className="w-4 h-4" />
      <span className="text-[10px] font-black uppercase tracking-widest">{labels.sectionMain}</span>
     </button>
     <span className="text-[11px] font-black uppercase tracking-widest tn-text-muted">
      {labels.title}
     </span>
    </div>
    <div className="min-h-full p-4 sm:p-6 md:p-8 lg:p-12 animate-in fade-in duration-700">{children}</div>
   </main>
  </div>
 );
}
