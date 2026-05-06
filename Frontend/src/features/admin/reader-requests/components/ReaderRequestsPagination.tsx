'use client';

import { StepPagination } from '@/shared/ui';
import { cn } from '@/lib/utils';

interface ReaderRequestsPaginationProps {
 canNext: boolean;
 canPrev: boolean;
 currentLabel: string;
 onNext: () => void;
 onPrev: () => void;
}

export function ReaderRequestsPagination({
 canNext,
 canPrev,
 currentLabel,
 onNext,
 onPrev,
}: ReaderRequestsPaginationProps) {
 return (
  <StepPagination
   className={cn('flex items-center justify-center gap-4 pt-6')}
   currentLabel={currentLabel}
   canPrev={canPrev}
   canNext={canNext}
   onPrev={onPrev}
   onNext={onNext}
  />
 );
}
