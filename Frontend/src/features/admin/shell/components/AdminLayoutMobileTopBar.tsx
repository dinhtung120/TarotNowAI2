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
  <div className={cn('tn-lg-hidden sticky top-0 z-30 tn-mobile-topbar-shell backdrop-blur border-b tn-border-soft px-4 py-3 flex items-center justify-between')}>
   <button type="button" onClick={onOpenMenu} className={cn('inline-flex items-center gap-2 px-3 py-2 rounded-xl tn-surface tn-text-secondary tn-mobile-topbar-trigger transition-colors min-h-11')}>
    <Menu className={cn('w-4 h-4')} />
    <span className={cn('tn-text-10 font-black uppercase tracking-widest')}>{sectionMain}</span>
   </button>
   <span className={cn('tn-text-11 font-black uppercase tracking-widest tn-text-muted')}>{label}</span>
  </div>
 );
}
