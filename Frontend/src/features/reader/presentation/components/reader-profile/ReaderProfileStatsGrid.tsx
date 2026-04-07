'use client';

import { MessageCircle, Star } from 'lucide-react';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { cn } from '@/lib/utils';

interface ReaderProfileStatsGridProps {
 profile: ReaderProfile;
 ratingLabel: string;
 reviewsLabel: string;
}

export function ReaderProfileStatsGrid({
 profile,
 ratingLabel,
 reviewsLabel,
}: ReaderProfileStatsGridProps) {
 return (
  <div className={cn('tn-grid-cols-1-2-sm gap-4')}>
   <div className={cn('p-6 rounded-3xl tn-panel-soft text-center space-y-2 tn-hover-surface-strong transition-colors shadow-inner')}>
    <Star className={cn('w-6 h-6 tn-text-warning mx-auto mb-3')} fill="currentColor" />
    <div className={cn('text-3xl font-black tn-text-primary italic drop-shadow-md')}>{profile.avgRating > 0 ? profile.avgRating.toFixed(1) : '--'}</div>
    <div className={cn('tn-text-overline tn-text-tertiary')}>{ratingLabel}</div>
   </div>
   <div className={cn('p-6 rounded-3xl tn-panel-soft text-center space-y-2 tn-hover-surface-strong transition-colors shadow-inner')}>
    <MessageCircle className={cn('w-6 h-6 tn-text-accent mx-auto mb-3')} />
    <div className={cn('text-3xl font-black tn-text-primary italic drop-shadow-md')}>{profile.totalReviews}</div>
    <div className={cn('tn-text-overline tn-text-tertiary')}>{reviewsLabel}</div>
   </div>
  </div>
 );
}
