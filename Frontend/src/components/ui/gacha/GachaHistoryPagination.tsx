'use client';

import { cn } from '@/lib/utils';
import Button from '@/shared/components/ui/Button';

interface GachaHistoryPaginationProps {
 page: number;
 totalPages: number;
 isLoading: boolean;
 previousLabel: string;
 nextLabel: string;
 onPrevious: () => void;
 onNext: () => void;
}

export function GachaHistoryPagination(props: GachaHistoryPaginationProps) {
 return (
  <div className={cn('flex items-center gap-4')}>
   <Button variant="secondary" size="md" disabled={props.page <= 1 || props.isLoading} onClick={props.onPrevious}>
    {props.previousLabel}
   </Button>
   <Button variant="secondary" size="md" disabled={props.page >= props.totalPages || props.isLoading} onClick={props.onNext}>
    {props.nextLabel}
   </Button>
  </div>
 );
}
