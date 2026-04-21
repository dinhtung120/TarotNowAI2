'use client';

import { Sparkles } from 'lucide-react';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import { ReaderProfileBioPanel } from './ReaderProfileBioPanel';
import { ReaderProfileHero } from './ReaderProfileHero';
import { ReaderProfileStatsGrid } from './ReaderProfileStatsGrid';

interface ReaderProfileCardProps {
 bio: string;
 diamondSuffix: string;
 fallbackName: string;
 yearsExperienceLabel: string;
 profile: ReaderProfile;
 ratingLabel: string;
 reviewsLabel: string;
 soulLinkLabel: string;
 specialtiesTitle: string;
 t: (key: string, values?: Record<string, string | number | Date>) => string;
}

export function ReaderProfileCard(props: ReaderProfileCardProps) {
 return (
  <GlassCard className={cn('relative overflow-hidden !p-0 !tn-rounded-3xl tn-border')}>
   <div className={cn('tn-reader-profile-card-overlay absolute inset-0 opacity-50')} />
   <div className={cn('absolute top-0 right-0 p-10 tn-opacity-03 pointer-events-none')}>
    <Sparkles size={240} className={cn('tn-text-accent')} />
   </div>
   <div className={cn('tn-reader-profile-card-content relative z-10')}>
    <ReaderProfileHero
     profile={props.profile}
     fallbackName={props.fallbackName}
     diamondSuffix={props.diamondSuffix}
     yearsExperienceLabel={props.yearsExperienceLabel}
     t={props.t}
    />
    <ReaderProfileStatsGrid profile={props.profile} ratingLabel={props.ratingLabel} reviewsLabel={props.reviewsLabel} />
    <ReaderProfileBioPanel bio={props.bio} profile={props.profile} soulLinkLabel={props.soulLinkLabel} specialtiesTitle={props.specialtiesTitle} />
   </div>
  </GlassCard>
 );
}
