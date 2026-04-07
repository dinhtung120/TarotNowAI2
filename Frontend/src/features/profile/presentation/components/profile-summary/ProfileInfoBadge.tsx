'use client';

import { cn } from '@/lib/utils';

interface ProfileInfoBadgeProps {
 dotClass: string;
 label: string;
 value: string;
}

export function ProfileInfoBadge({
 dotClass,
 label,
 value,
}: ProfileInfoBadgeProps) {
 return (
  <div className={cn('flex items-center gap-2 px-3 py-1.5 rounded-xl tn-panel hover:tn-surface-strong transition-all duration-300')}>
   <div className={cn('w-2 h-2 rounded-full', dotClass)} />
   <span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]')}>{label}</span>
   <span className={cn('text-xs font-black tn-text-primary ml-0.5')}>{value}</span>
  </div>
 );
}
