'use client';

import { Gem } from 'lucide-react';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { cn } from '@/lib/utils';
import { ReaderStatusBadge } from '../ReaderStatusBadge';

interface ReaderProfileHeroProps {
 diamondSuffix: string;
 fallbackName: string;
 profile: ReaderProfile;
 t: (key: string, values?: Record<string, string | number | Date>) => string;
}

export function ReaderProfileHero({
 diamondSuffix,
 fallbackName,
 profile,
 t,
}: ReaderProfileHeroProps) {
 return (
  <div className={cn('flex flex-col md:flex-row items-center gap-8 text-center md:text-left')}>
   <div className={cn('relative w-28 h-28 shrink-0 group')}>
    <div className={cn('absolute inset-0 bg-gradient-to-br from-[var(--purple-accent)]/50 to-[var(--warning)]/30 rounded-[2rem] blur-xl opacity-60 group-hover:opacity-100 transition-opacity duration-500')} />
    <div className={cn('w-full h-full rounded-[2rem] tn-surface-strong border-2 tn-border flex items-center justify-center text-5xl font-black tn-text-primary relative z-10 shadow-2xl overflow-hidden')}>
     {profile.displayName?.charAt(0)?.toUpperCase() || '?'}
    </div>
   </div>
   <div className={cn('space-y-4')}>
    <h1 className={cn('text-4xl md:text-5xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-lg')}>{profile.displayName || fallbackName}</h1>
    <div className={cn('flex flex-wrap justify-center md:justify-start gap-3')}>
     <ReaderStatusBadge status={profile.status} t={t} />
     <div className={cn('inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full tn-panel ')}>
      <Gem className={cn('w-3.5 h-3.5 text-[var(--purple-accent)]')} />
      <span className={cn('text-[10px] font-black uppercase tracking-widest tn-text-primary')}>{profile.diamondPerQuestion} {diamondSuffix}</span>
     </div>
    </div>
   </div>
  </div>
 );
}
