'use client';

import { cn } from '@/lib/utils';

interface InventoryQueryStateProps {
 isLoading: boolean;
 errorMessage?: string;
 loadingLabel: string;
}

export default function InventoryQueryState({
 isLoading,
 errorMessage,
 loadingLabel,
}: InventoryQueryStateProps) {
 return (
  <>
   {isLoading ? <p className={cn('text-sm text-slate-600 dark:text-slate-300')}>{loadingLabel}</p> : null}
   {errorMessage ? (
    <p className={cn('mb-4 rounded-xl border border-rose-300 bg-rose-50 px-3 py-2 text-sm text-rose-700 dark:border-rose-700 dark:bg-rose-900/20 dark:text-rose-300')}>
     {errorMessage}
    </p>
   ) : null}
  </>
 );
}
