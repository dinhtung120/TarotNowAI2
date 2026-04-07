import { Loader2 } from 'lucide-react';
import { cn } from '@/lib/utils';

export function CollectionLoadingState() {
 return (
  <div className={cn('min-h-screen tn-surface flex items-center justify-center')}>
   <div className={cn('relative group')}>
    <div className={cn('absolute inset-x-0 top-0 h-40 w-40 tn-bg-accent-20 tn-blur-60 rounded-full animate-pulse')} />
    <Loader2 className={cn('w-12 h-12 animate-spin tn-text-accent relative z-10')} />
   </div>
  </div>
 );
}
