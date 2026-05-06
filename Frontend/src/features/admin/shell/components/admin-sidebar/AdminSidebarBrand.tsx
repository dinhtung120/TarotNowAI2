'use client';

import { ShieldCheck } from 'lucide-react';
import { cn } from '@/lib/utils';

interface AdminSidebarBrandProps {
 isDropdown: boolean;
 subtitle: string;
 title: string;
}

export function AdminSidebarBrand({
 isDropdown,
 subtitle,
 title,
}: AdminSidebarBrandProps) {
 return (
  <div className={cn('tn-admin-sidebar-brand-wrap', isDropdown ? 'p-4' : 'tn-admin-sidebar-brand-pad')}>
   <div className={cn('flex items-center gap-3 group px-4 py-3 rounded-2xl tn-bg-elevated border tn-border tn-shadow-card overflow-hidden relative')}>
    <div className={cn('tn-admin-sidebar-brand-overlay absolute inset-0 opacity-0 transition-opacity duration-500')} />
    <div className={cn('tn-admin-sidebar-brand-icon w-10 h-10 rounded-xl tn-bg-accent-10 flex items-center justify-center border tn-border-accent-20 transition-transform duration-500 shrink-0')}>
     <ShieldCheck className={cn('w-6 h-6 tn-text-accent')} />
    </div>
    <div className={cn('relative z-10 flex-1 min-w-0')}>
     <h2 className={cn('text-sm font-black tn-text-ink tracking-widest uppercase italic truncate')}>{title}</h2>
     <div className={cn('tn-text-10 font-bold tn-text-accent-soft tracking-tighter uppercase leading-none truncate')}>
      {subtitle}
     </div>
    </div>
   </div>
  </div>
 );
}
