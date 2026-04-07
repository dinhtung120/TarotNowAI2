'use client';

import { Sparkles, User } from 'lucide-react';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { cn } from '@/lib/utils';

interface ReaderProfileBioPanelProps {
 bio: string;
 profile: ReaderProfile;
 soulLinkLabel: string;
 specialtiesTitle: string;
}

export function ReaderProfileBioPanel({
 bio,
 profile,
 soulLinkLabel,
 specialtiesTitle,
}: ReaderProfileBioPanelProps) {
 return (
  <div className={cn('space-y-8 tn-overlay-soft p-8 rounded-3xl border tn-border-soft shadow-inner')}>
   <div className={cn('space-y-4')}>
    <div className={cn('flex items-center gap-2 text-[11px] font-black uppercase tracking-widest text-[var(--purple-accent)]')}>
     <User className={cn('w-4 h-4')} /> {soulLinkLabel}
    </div>
    <p className={cn('text-[15px] font-medium text-[var(--text-secondary)] leading-relaxed italic border-l-2 border-[var(--purple-accent)]/30 pl-4 py-1')}> &ldquo;{bio}&rdquo;</p>
   </div>
   {profile.specialties.length > 0 ? (
    <div className={cn('space-y-4 pt-6 border-t tn-border-soft')}>
     <div className={cn('flex items-center gap-2 text-[11px] font-black uppercase tracking-widest text-[var(--text-tertiary)]')}>
      <Sparkles className={cn('w-4 h-4')} /> {specialtiesTitle}
     </div>
     <div className={cn('flex flex-wrap gap-2.5')}>
      {profile.specialties.map((spec) => (
       <span key={spec} className={cn('px-4 py-2 rounded-xl tn-surface tn-text-primary text-[11px] font-black uppercase tracking-widest border tn-border shadow-sm hover:border-[var(--purple-accent)]/50 transition-colors cursor-default')}>{spec}</span>
      ))}
     </div>
    </div>
   ) : null}
  </div>
 );
}
