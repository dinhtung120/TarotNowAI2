import { BriefcaseBusiness, Gem, Star } from 'lucide-react';
import type { ReaderProfile } from '@/features/reader/shared';
import type { ReaderDirectoryCardLabels } from '@/features/reader/directory/components/readers-directory/types';
import { cn } from '@/lib/utils';

interface ReaderDirectoryCardStatsProps {
 labels: ReaderDirectoryCardLabels;
 reader: ReaderProfile;
}

export default function ReaderDirectoryCardStats({
 labels,
 reader,
}: ReaderDirectoryCardStatsProps) {
 return (
  <div className={cn('grid grid-cols-1 gap-2 sm:grid-cols-3')}>
   <div className={cn('tn-border-soft tn-surface flex items-center gap-1.5 rounded-lg border px-2.5 py-1.5')}>
    <Star className={cn('h-3.5 w-3.5 tn-text-warning')} fill="currentColor" />
    <span className={cn('tn-text-primary text-xs font-black')}>
     {reader.avgRating > 0 ? reader.avgRating.toFixed(1) : '--'}
    </span>
    <span className={cn('tn-text-10 font-bold tn-text-tertiary')}>
     ({reader.totalReviews})
    </span>
   </div>

   <div className={cn('flex items-center gap-1.5 rounded-lg border tn-border-accent-20 tn-bg-accent-10 px-2.5 py-1.5')}>
    <Gem className={cn('h-3.5 w-3.5 tn-text-accent')} />
    <span className={cn('tn-text-10 font-black tracking-widest tn-text-accent uppercase')}>
     {reader.diamondPerQuestion} {labels.perQuestionSuffix}
    </span>
   </div>

   <div className={cn('tn-border-soft tn-surface flex items-center gap-1.5 rounded-lg border px-2.5 py-1.5')}>
    <BriefcaseBusiness className={cn('h-3.5 w-3.5 tn-text-secondary')} />
    <span className={cn('tn-text-10 font-black tn-text-secondary uppercase')}>
     {reader.yearsOfExperience}+ {labels.yearsExperienceLabel}
    </span>
   </div>
  </div>
 );
}
