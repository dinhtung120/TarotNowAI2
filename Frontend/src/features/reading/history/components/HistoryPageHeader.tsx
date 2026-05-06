import { History } from 'lucide-react';
import { SectionHeader } from '@/shared/ui';
import { cn } from '@/lib/utils';
import { HistoryFilters } from './HistoryFilters';

interface HistoryPageHeaderProps {
 title: string;
 subtitle: string;
 tag: string;
 filterType: string;
 filterDate: string;
 labels: {
  all: string;
  daily: string;
  spread3: string;
  spread5: string;
  spread10: string;
 };
 onFilterTypeChange: (value: string) => void;
 onFilterDateChange: (value: string) => void;
}

export function HistoryPageHeader({
 title,
 subtitle,
 tag,
 filterType,
 filterDate,
 labels,
 onFilterTypeChange,
 onFilterDateChange,
}: HistoryPageHeaderProps) {
 return (
  <SectionHeader
   title={title}
   subtitle={subtitle}
   tag={tag}
   tagIcon={<History className={cn('w-3 h-3')} />}
   action={
    <HistoryFilters
     filterType={filterType}
     filterDate={filterDate}
     labels={labels}
     onFilterTypeChange={onFilterTypeChange}
     onFilterDateChange={onFilterDateChange}
    />
   }
   className={cn('mb-12')}
  />
 );
}
