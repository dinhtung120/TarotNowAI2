'use client';

import { Eye } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { SectionHeader } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface AdminReadingsHeaderProps {
 totalCount: number;
}

export function AdminReadingsHeader({ totalCount }: AdminReadingsHeaderProps) {
 const t = useTranslations('Admin');

 return (
  <div className={cn('flex flex-col tn-md-flex-row tn-md-items-end justify-between gap-6')}>
   <SectionHeader
    tag={t('readings.header.tag')}
    tagIcon={<Eye className={cn('w-3 h-3 tn-text-accent')} />}
    title={t('readings.header.title')}
    subtitle={t('readings.header.subtitle', { count: totalCount })}
    className={cn('mb-0 text-left items-start')}
   />
  </div>
 );
}
