import { BriefcaseBusiness, Gem, Star } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ReaderDetailStatsProps {
 avgRating: number;
 totalReviews: number;
 diamondPerQuestion: number;
 yearsOfExperience: number;
 perQuestionSuffix: string;
 yearsExperienceLabel: string;
}

export function ReaderDetailStats(props: ReaderDetailStatsProps) {
 const {
  avgRating,
  totalReviews,
  diamondPerQuestion,
  yearsOfExperience,
  perQuestionSuffix,
  yearsExperienceLabel,
 } = props;

 return (
  <div className={cn('flex flex-wrap items-center gap-3')}>
   <div className={cn('flex items-center gap-1.5 rounded-lg border tn-border-soft tn-surface px-3 py-1.5')}>
    <Star className={cn('h-3.5 w-3.5 tn-text-warning')} fill="currentColor" />
    <span className={cn('text-xs font-black tn-text-primary')}>{avgRating > 0 ? avgRating.toFixed(1) : '--'}</span>
    <span className={cn('tn-text-10 font-bold tn-text-tertiary')}>({totalReviews})</span>
   </div>

   <div className={cn('flex items-center gap-1.5 rounded-lg border tn-border-accent-20 tn-bg-accent-10 px-3 py-1.5')}>
    <Gem className={cn('h-3.5 w-3.5 tn-text-accent')} />
    <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-accent')}>
     {diamondPerQuestion} {perQuestionSuffix}
    </span>
   </div>

   <div className={cn('flex items-center gap-1.5 rounded-lg border tn-border-soft tn-surface px-3 py-1.5')}>
    <BriefcaseBusiness className={cn('h-3.5 w-3.5 tn-text-secondary')} />
    <span className={cn('tn-text-10 font-black uppercase tn-text-secondary')}>
     {yearsOfExperience}+ {yearsExperienceLabel}
    </span>
   </div>
  </div>
 );
}
