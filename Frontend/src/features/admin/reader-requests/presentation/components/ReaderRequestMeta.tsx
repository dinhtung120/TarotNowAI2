'use client';

import { Clock, Users } from 'lucide-react';
import type { AdminReaderRequest } from '@/features/admin/application/actions';
import { cn } from '@/lib/utils';
import { ReaderRequestStatusBadge } from './ReaderRequestStatusBadge';

interface ReaderRequestMetaProps {
 labels: {
  statusPending: string;
  statusApproved: string;
  statusRejected: string;
  userPrefix: (id: string) => string;
 };
 locale: string;
 request: AdminReaderRequest;
}

export function ReaderRequestMeta({ labels, locale, request }: ReaderRequestMetaProps) {
 return (
  <div className={cn('flex flex-col justify-between gap-4 border-b tn-border-soft pb-4')}>
   <div className={cn('flex items-center gap-4')}>
    <ReaderRequestStatusBadge status={request.status} labels={{ pending: labels.statusPending, approved: labels.statusApproved, rejected: labels.statusRejected }} />
    <span className={cn('text-xs font-bold tn-text-secondary flex items-center gap-2')}>
     <Users className={cn('w-4 h-4')} />
     {labels.userPrefix(request.userId.substring(0, 8))}
    </span>
   </div>
   <div className={cn('flex items-center gap-2 tn-text-10 font-black tn-text-tertiary uppercase tracking-tighter tn-surface px-3 py-1.5 rounded-lg shadow-inner')}>
    <Clock className={cn('w-3.5 h-3.5')} />
    {new Date(request.createdAt).toLocaleString(locale)}
   </div>
  </div>
 );
}
