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
 profile: ReaderProfile;
 ratingLabel: string;
 reviewsLabel: string;
 soulLinkLabel: string;
 specialtiesTitle: string;
 t: (key: string, values?: Record<string, string | number | Date>) => string;
}

export function ReaderProfileCard(props: ReaderProfileCardProps) {
 return (
  <GlassCard className={cn('relative overflow-hidden !p-0 !rounded-[3rem] tn-border')}>
   <div className={cn('absolute inset-0 bg-gradient-to-b from-[var(--purple-accent)]/20 via-transparent to-transparent opacity-50')} />
   <div className={cn('absolute top-0 right-0 p-10 opacity-[0.03] pointer-events-none')}>
    <Sparkles size={240} className={cn('text-[var(--purple-accent)]')} />
   </div>
   <div className={cn('relative z-10 p-6 sm:p-8 md:p-14 space-y-10 md:space-y-12')}>
    <ReaderProfileHero profile={props.profile} fallbackName={props.fallbackName} diamondSuffix={props.diamondSuffix} t={props.t} />
    <ReaderProfileStatsGrid profile={props.profile} ratingLabel={props.ratingLabel} reviewsLabel={props.reviewsLabel} />
    <ReaderProfileBioPanel bio={props.bio} profile={props.profile} soulLinkLabel={props.soulLinkLabel} specialtiesTitle={props.specialtiesTitle} />
   </div>
  </GlassCard>
 );
}
