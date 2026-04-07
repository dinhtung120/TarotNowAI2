'use client';

import { CheckCircle2, Clock, Filter, XCircle } from 'lucide-react';
import { FilterTabs } from '@/shared/components/ui';
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
    { value: 'pending', label: pendingLabel, icon: <Clock className={cn('w-4 h-4')} />, activeClassName: 'bg-[var(--warning)]/10 border border-[var(--warning)]/30 text-[var(--warning)] shadow-md' },
    { value: 'approved', label: approvedLabel, icon: <CheckCircle2 className={cn('w-4 h-4')} />, activeClassName: 'bg-[var(--success)]/10 border border-[var(--success)]/30 text-[var(--success)] shadow-md' },
    { value: 'rejected', label: rejectedLabel, icon: <XCircle className={cn('w-4 h-4')} />, activeClassName: 'bg-[var(--danger)]/10 border border-[var(--danger)]/30 text-[var(--danger)] shadow-md' },
    { value: '', label: allLabel, icon: <Filter className={cn('w-4 h-4')} />, activeClassName: 'bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/30 text-[var(--purple-accent)] shadow-md' },
   ]}
   onChange={onChange}
  />
 );
}
