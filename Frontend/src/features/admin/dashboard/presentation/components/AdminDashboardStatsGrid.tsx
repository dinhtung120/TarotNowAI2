'use client';

import { cn } from '@/lib/utils';
import type { AdminStatCard } from './types';

interface AdminDashboardStatsGridProps {
 cards: AdminStatCard[];
 locale: string;
 onNavigate: (href: AdminStatCard['href']) => void;
}

export function AdminDashboardStatsGrid({
 cards,
 locale,
 onNavigate,
}: AdminDashboardStatsGridProps) {
 return (
  <div className={cn('grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6')}>
   {cards.map((stat) => (
    <button
     key={stat.name}
     type="button"
     onClick={() => onNavigate(stat.href)}
     className={cn(
      'group relative h-44 tn-surface hover:tn-surface-strong rounded-[2.5rem] border tn-border-soft p-8 text-left transition-all duration-500 shadow-xl overflow-hidden ring-1 ring-transparent',
      stat.hoverRing
     )}
    >
     <div className={cn('absolute top-0 right-0 p-6 opacity-[0.05] group-hover:opacity-[0.08] transition-all duration-500 group-hover:scale-110 group-hover:rotate-12 translate-x-4 -translate-y-4')}>
      <stat.icon size={120} />
     </div>
     <div className={cn('relative z-10 h-full flex flex-col justify-between')}>
      <div className={cn('w-10 h-10 rounded-xl border tn-border-soft flex items-center justify-center transition-transform group-hover:scale-110', stat.bg)}>
       <stat.icon className={cn('w-5 h-5', stat.color)} />
      </div>
      <div>
       <div className={cn('text-4xl font-black tn-text-primary italic tracking-tighter mb-1 drop-shadow-md')}>
        {stat.value.toLocaleString(locale)}
       </div>
       <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]')}>
        {stat.name}
       </div>
      </div>
     </div>
    </button>
   ))}
  </div>
 );
}
