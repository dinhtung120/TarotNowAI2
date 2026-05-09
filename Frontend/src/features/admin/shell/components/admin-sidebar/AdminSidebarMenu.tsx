'use client';

import { ChevronRight } from 'lucide-react';
import { OptimizedLink as Link } from '@/shared/navigation/useOptimizedLink';
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
  <nav className={cn('flex-1 px-4 space-y-2 overflow-y-auto no-scrollbar', isDropdown && 'tn-maxh-60vh custom-scrollbar')}>
   <div className={cn('mb-4', isDropdown ? 'px-2' : 'px-6')}>
    <span className={cn('tn-text-9 font-black uppercase tn-tracking-03 tn-text-muted')}>{sectionMain}</span>
   </div>
   {menuItems.map((item) => {
    const active = isRouteActive(pathname, item.href);
    const Icon = item.icon;
    return (
     <Link key={item.href} href={item.href} prefetch={false} onClick={onClose} className={cn('group flex items-center justify-between tn-px-5-6-sm py-4 rounded-2xl transition-all duration-300 relative overflow-hidden', active ? 'tn-bg-elevated tn-text-ink tn-shadow-glow-purple-sm border tn-border-strong' : 'tn-hover-surface-hover tn-hover-text-ink tn-text-secondary border border-transparent')}>
      <div className={cn('flex items-center gap-4 relative z-10 font-bold')}>
       <Icon className={cn('w-5 h-5 transition-all duration-300', active ? 'tn-text-accent scale-110' : 'tn-group-text-ink')} />
       <span className={cn('tn-text-11 uppercase tracking-widest', active ? 'font-black' : 'font-bold')}>{item.name}</span>
      </div>
      {active ? <ChevronRight className={cn('w-4 h-4 tn-text-accent-soft relative z-10')} /> : null}
      {active ? <div className={cn('absolute left-0 top-1/4 bottom-1/4 w-1 tn-bg-accent rounded-r-full tn-shadow-glow-accent-soft')} /> : null}
     </Link>
    );
   })}
  </nav>
 );
}
