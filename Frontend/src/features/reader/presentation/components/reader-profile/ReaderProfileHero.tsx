'use client';

import { BriefcaseBusiness, Gem } from 'lucide-react';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { cn } from '@/lib/utils';
import { ReaderStatusBadge } from '../ReaderStatusBadge';
import ReaderSocialLinksInline from '@/features/reader/presentation/components/ReaderSocialLinksInline';

interface ReaderProfileHeroProps {
 diamondSuffix: string;
 fallbackName: string;
 yearsExperienceLabel: string;
 profile: ReaderProfile;
 t: (key: string, values?: Record<string, string | number | Date>) => string;
}

export function ReaderProfileHero({
 diamondSuffix,
 fallbackName,
 yearsExperienceLabel,
 profile,
 t,
}: ReaderProfileHeroProps) {
 return (
  <div className={cn('tn-flex-col-row-md items-center gap-8 tn-text-center-left-md')}>
   <div className={cn('relative w-28 h-28 shrink-0 group')}>
    <div className={cn('absolute inset-0 bg-gradient-to-br from-purple-500/50 to-amber-300/30 tn-rounded-2rem blur-xl opacity-60 transition-opacity duration-500')} />
    <div className={cn('w-full h-full tn-rounded-2rem tn-surface-strong border-2 tn-border flex items-center justify-center text-5xl font-black tn-text-primary relative z-10 shadow-2xl overflow-hidden')}>
     {profile.displayName?.charAt(0)?.toUpperCase() || '?'}
    </div>
   </div>
   <div className={cn('space-y-4')}>
    <h1 className={cn('tn-text-4-5-md font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-lg')}>{profile.displayName || fallbackName}</h1>
    <div className={cn('flex flex-wrap tn-justify-center-start-md gap-3')}>
     <ReaderStatusBadge status={profile.status} t={t} />
     <div className={cn('inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full tn-panel ')}>
      <Gem className={cn('w-3.5 h-3.5 tn-text-accent')} />
      <span className={cn('tn-text-overline tn-text-primary')}>{profile.diamondPerQuestion} {diamondSuffix}</span>
     </div>
     <div className={cn('inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full tn-panel ')}>
      <BriefcaseBusiness className={cn('w-3.5 h-3.5 tn-text-secondary')} />
      <span className={cn('tn-text-overline tn-text-primary')}>{profile.yearsOfExperience}+ {yearsExperienceLabel}</span>
     </div>
    </div>
    <ReaderSocialLinksInline
     facebookUrl={profile.facebookUrl}
     instagramUrl={profile.instagramUrl}
     tikTokUrl={profile.tikTokUrl}
    />
   </div>
  </div>
 );
}
