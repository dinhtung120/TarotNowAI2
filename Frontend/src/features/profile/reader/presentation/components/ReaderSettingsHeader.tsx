'use client';

import { Sparkles } from 'lucide-react';
import { SectionHeader } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface ReaderSettingsHeaderProps {
 subtitle: string;
 tag: string;
 title: string;
}

export function ReaderSettingsHeader({
 subtitle,
 tag,
 title,
}: ReaderSettingsHeaderProps) {
 return (
  <SectionHeader
   tag={tag}
   tagIcon={<Sparkles className={cn('w-3 h-3 tn-text-warning')} />}
   title={title}
   subtitle={subtitle}
  />
 );
}
