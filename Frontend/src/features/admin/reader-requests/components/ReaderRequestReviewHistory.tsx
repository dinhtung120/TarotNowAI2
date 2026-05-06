'use client';

import { memo, useMemo } from 'react';
import type { AdminReaderRequestReviewHistoryEntry } from '@/features/admin/reader-requests/actions/reader-requests';
import { cn } from '@/lib/utils';

interface ReaderRequestReviewHistoryProps {
 locale: string;
 history: AdminReaderRequestReviewHistoryEntry[];
 labels: {
  title: string;
  empty: string;
  at: string;
  by: string;
  actionApprove: string;
  actionReject: string;
 };
}

const UNKNOWN_REVIEWER = 'N/A';

function resolveActionLabel(action: string, labels: ReaderRequestReviewHistoryProps['labels']): string {
 if (action === 'approve') return labels.actionApprove;
 if (action === 'reject') return labels.actionReject;
 return action;
}

export const ReaderRequestReviewHistory = memo(function ReaderRequestReviewHistory({ history, labels, locale }: ReaderRequestReviewHistoryProps) {
 const sortedHistory = useMemo(
  () => [...history].sort((left, right) => new Date(right.reviewedAt).getTime() - new Date(left.reviewedAt).getTime()),
  [history],
 );

 return (
  <div className={cn('space-y-3 p-4 rounded-2xl tn-panel-soft border tn-border-soft')}>
   <div className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-tertiary')}>{labels.title}</div>
   {sortedHistory.length === 0 ? <p className={cn('text-xs tn-text-muted')}>{labels.empty}</p> : null}
   {sortedHistory.map((entry, index) => (
    <div key={`${entry.reviewedAt}-${entry.action}-${index}`} className={cn('space-y-1 rounded-xl tn-surface p-3 border tn-border-soft')}>
     <div className={cn('flex flex-wrap items-center gap-2 text-xs tn-text-secondary')}>
      <span className={cn('font-black uppercase tn-text-accent')}>{resolveActionLabel(entry.action, labels)}</span>
      <span>{labels.at}: {new Date(entry.reviewedAt).toLocaleString(locale)}</span>
      <span>{labels.by}: {entry.reviewedBy || UNKNOWN_REVIEWER}</span>
     </div>
     {entry.adminNote ? <p className={cn('text-xs tn-text-secondary leading-relaxed')}>{entry.adminNote}</p> : null}
    </div>
   ))}
  </div>
 );
});
