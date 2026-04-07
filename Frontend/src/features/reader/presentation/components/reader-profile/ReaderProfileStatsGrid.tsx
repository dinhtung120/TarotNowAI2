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
  <div className={cn('grid grid-cols-1 sm:grid-cols-2 gap-4')}>
   <div className={cn('p-6 rounded-3xl tn-panel-soft text-center space-y-2 hover:tn-surface-strong transition-colors shadow-inner')}>
    <Star className={cn('w-6 h-6 text-[var(--warning)] mx-auto mb-3')} fill="currentColor" />
    <div className={cn('text-3xl font-black tn-text-primary italic drop-shadow-md')}>{profile.avgRating > 0 ? profile.avgRating.toFixed(1) : '--'}</div>
    <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]')}>{ratingLabel}</div>
   </div>
   <div className={cn('p-6 rounded-3xl tn-panel-soft text-center space-y-2 hover:tn-surface-strong transition-colors shadow-inner')}>
    <MessageCircle className={cn('w-6 h-6 text-[var(--purple-accent)] mx-auto mb-3')} />
    <div className={cn('text-3xl font-black tn-text-primary italic drop-shadow-md')}>{profile.totalReviews}</div>
    <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]')}>{reviewsLabel}</div>
   </div>
  </div>
 );
}
