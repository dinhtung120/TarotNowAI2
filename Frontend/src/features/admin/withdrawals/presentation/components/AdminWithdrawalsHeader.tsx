'use client';

import { Sparkles } from 'lucide-react';
import { SectionHeader } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface AdminWithdrawalsHeaderProps {
 subtitle: string;
 tag: string;
 title: string;
}

export function AdminWithdrawalsHeader({
 subtitle,
 tag,
 title,
}: AdminWithdrawalsHeaderProps) {
 return (
  <div className={cn('flex flex-col md:flex-row md:items-end justify-between gap-6')}>
   <SectionHeader
    tag={tag}
    tagIcon={<Sparkles className={cn('w-3 h-3 text-[var(--success)]')} />}
    title={title}
    subtitle={subtitle}
    className={cn('mb-0 text-left items-start')}
   />
  </div>
 );
}
