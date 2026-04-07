'use client';

import { Menu } from 'lucide-react';
import { cn } from '@/lib/utils';

interface AdminLayoutMobileTopBarProps {
 label: string;
 sectionMain: string;
 onOpenMenu: () => void;
}

export function AdminLayoutMobileTopBar({
 label,
 sectionMain,
 onOpenMenu,
}: AdminLayoutMobileTopBarProps) {
 return (
  <div className={cn('lg:hidden sticky top-0 z-30 bg-[var(--bg-glass)]/95 backdrop-blur border-b border-[var(--border-subtle)] px-4 py-3 flex items-center justify-between')}>
   <button type="button" onClick={onOpenMenu} className={cn('inline-flex items-center gap-2 px-3 py-2 rounded-xl tn-surface hover:tn-surface-strong tn-text-secondary hover:tn-text-primary transition-colors min-h-11')}>
    <Menu className={cn('w-4 h-4')} />
    <span className={cn('text-[10px] font-black uppercase tracking-widest')}>{sectionMain}</span>
   </button>
   <span className={cn('text-[11px] font-black uppercase tracking-widest tn-text-muted')}>{label}</span>
  </div>
 );
}
