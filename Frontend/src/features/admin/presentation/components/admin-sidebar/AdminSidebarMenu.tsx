'use client';

import { ChevronRight } from 'lucide-react';
import { Link } from '@/i18n/routing';
import { cn } from '@/lib/utils';
import type { AdminSidebarMenuItem } from '../AdminLayout.types';
import { isRouteActive } from './isRouteActive';

interface AdminSidebarMenuProps {
 isDropdown: boolean;
 menuItems: AdminSidebarMenuItem[];
 onClose: () => void;
 pathname: string;
 sectionMain: string;
}

export function AdminSidebarMenu({
 isDropdown,
 menuItems,
 onClose,
 pathname,
 sectionMain,
}: AdminSidebarMenuProps) {
 return (
  <nav className={cn('flex-1 px-4 space-y-2 overflow-y-auto no-scrollbar', isDropdown && 'max-h-[60vh] custom-scrollbar')}>
   <div className={cn('mb-4', isDropdown ? 'px-2' : 'px-6')}>
    <span className={cn('text-[9px] font-black uppercase tracking-[0.3em] text-[var(--text-muted)]')}>{sectionMain}</span>
   </div>
   {menuItems.map((item) => {
    const active = isRouteActive(pathname, item.href);
    const Icon = item.icon;
    return (
     <Link key={item.href} href={item.href} onClick={onClose} className={cn('group flex items-center justify-between px-5 sm:px-6 py-4 rounded-2xl transition-all duration-300 relative overflow-hidden', active ? 'bg-[var(--bg-elevated)] text-[var(--text-ink)] shadow-[var(--glow-purple-sm)] border border-[var(--border-hover)]' : 'hover:bg-[var(--bg-surface-hover)] hover:text-[var(--text-ink)] text-[var(--text-secondary)] border border-transparent')}>
      <div className={cn('flex items-center gap-4 relative z-10 font-bold')}>
       <Icon className={cn('w-5 h-5 transition-all duration-300', active ? 'text-[var(--purple-accent)] scale-110' : 'group-hover:text-[var(--text-ink)]')} />
       <span className={cn('text-[11px] uppercase tracking-widest', active ? 'font-black' : 'font-bold')}>{item.name}</span>
      </div>
      {active ? <ChevronRight className={cn('w-4 h-4 text-[var(--purple-muted)] relative z-10')} /> : null}
      {active ? <div className={cn('absolute left-0 top-1/4 bottom-1/4 w-1 bg-[var(--purple-accent)] rounded-r-full shadow-[0_0_10px_var(--purple-accent)]')} /> : null}
     </Link>
    );
   })}
  </nav>
 );
}
