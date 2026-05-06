'use client';

import { CheckCircle2, Clock, Filter, XCircle } from 'lucide-react';
import { FilterTabs } from '@/shared/ui';
import { cn } from '@/lib/utils';

interface ReaderRequestsFilterTabsProps {
 allLabel: string;
 approvedLabel: string;
 onChange: (value: string) => void;
 pendingLabel: string;
 rejectedLabel: string;
 value: string;
}

export function ReaderRequestsFilterTabs({
 allLabel,
 approvedLabel,
 onChange,
 pendingLabel,
 rejectedLabel,
 value,
}: ReaderRequestsFilterTabsProps) {
 return (
  <FilterTabs
   value={value}
   options={[
    { value: 'pending', label: pendingLabel, icon: <Clock className={cn('w-4 h-4')} />, activeClassName: 'tn-bg-warning-10 border tn-border-warning-30 tn-text-warning shadow-md' },
    { value: 'approved', label: approvedLabel, icon: <CheckCircle2 className={cn('w-4 h-4')} />, activeClassName: 'tn-bg-success-10 border tn-border-success-30 tn-text-success shadow-md' },
    { value: 'rejected', label: rejectedLabel, icon: <XCircle className={cn('w-4 h-4')} />, activeClassName: 'tn-bg-danger-soft border tn-border-danger tn-text-danger shadow-md' },
    { value: '', label: allLabel, icon: <Filter className={cn('w-4 h-4')} />, activeClassName: 'tn-bg-accent-10 border tn-border-accent-30 tn-text-accent shadow-md' },
   ]}
   onChange={onChange}
  />
 );
}
