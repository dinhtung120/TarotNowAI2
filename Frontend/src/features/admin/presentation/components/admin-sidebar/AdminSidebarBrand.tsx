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
  <div className={cn('mb-2 sm:mb-4', isDropdown ? 'p-4' : 'p-6 sm:p-8')}>
   <div className={cn('flex items-center gap-3 group px-4 py-3 rounded-2xl bg-[var(--bg-elevated)] border border-[var(--border-default)] shadow-[var(--shadow-card)] overflow-hidden relative')}>
    <div className={cn('absolute inset-0 bg-gradient-to-r from-[var(--purple-accent)]/10 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-500')} />
    <div className={cn('w-10 h-10 rounded-xl bg-[var(--purple-accent)]/10 flex items-center justify-center border border-[var(--purple-accent)]/20 group-hover:scale-110 transition-transform duration-500 shrink-0')}>
     <ShieldCheck className={cn('w-6 h-6 text-[var(--purple-accent)]')} />
    </div>
    <div className={cn('relative z-10 flex-1 min-w-0')}>
     <h2 className={cn('text-sm font-black text-[var(--text-ink)] tracking-widest uppercase italic truncate')}>{title}</h2>
     <div className={cn('text-[10px] font-bold text-[var(--purple-muted)] tracking-tighter uppercase leading-none truncate')}>
      {subtitle}
     </div>
    </div>
   </div>
  </div>
 );
}
