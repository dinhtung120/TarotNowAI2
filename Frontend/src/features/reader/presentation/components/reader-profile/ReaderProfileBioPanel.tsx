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
    <div className={cn('flex items-center gap-2 tn-text-11 font-black uppercase tracking-widest tn-text-accent')}>
     <User className={cn('w-4 h-4')} /> {soulLinkLabel}
    </div>
    <p className={cn('tn-text-15 font-medium tn-text-secondary leading-relaxed italic border-l-2 tn-border-accent-30 pl-4 py-1')}> &ldquo;{bio}&rdquo;</p>
   </div>
   {profile.specialties.length > 0 ? (
    <div className={cn('space-y-4 pt-6 border-t tn-border-soft')}>
     <div className={cn('flex items-center gap-2 tn-text-11 font-black uppercase tracking-widest tn-text-tertiary')}>
      <Sparkles className={cn('w-4 h-4')} /> {specialtiesTitle}
     </div>
     <div className={cn('flex flex-wrap gap-2.5')}>
      {profile.specialties.map((spec) => (
       <span key={spec} className={cn('px-4 py-2 rounded-xl tn-surface tn-text-primary tn-text-11 font-black uppercase tracking-widest border tn-border shadow-sm tn-hover-border-accent-50 transition-colors cursor-default')}>{spec}</span>
      ))}
     </div>
    </div>
   ) : null}
  </div>
 );
}
