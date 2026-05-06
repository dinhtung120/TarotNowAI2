import { ChevronRight, LayoutGrid } from 'lucide-react';
import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { cn } from '@/lib/utils';

interface CollectionEmptyStateProps {
 title: string;
 description: string;
 cta: string;
}

export function CollectionEmptyState({ title, description, cta }: CollectionEmptyStateProps) {
 return (
  <div className={cn('text-center py-24 tn-surface-soft border tn-border-soft tn-rounded-3xl animate-in fade-in duration-1000')}>
   <div className={cn('w-16 h-16 tn-surface rounded-3xl flex items-center justify-center mx-auto mb-6')}>
    <LayoutGrid className={cn('w-8 h-8 tn-text-muted')} />
   </div>
   <h3 className={cn('text-lg font-bold tn-text-secondary mb-2')}>{title}</h3>
   <p className={cn('tn-text-muted mb-8 text-sm font-medium')}>{description}</p>
   <Link
    href="/reading"
    className={cn('group relative px-8 py-3 tn-surface-strong tn-text-ink rounded-full font-black text-xs uppercase tracking-widest tn-hover-scale-105-active-95 shadow-xl')}
   >
    <span className={cn('flex items-center gap-2')}>
     {cta} <ChevronRight className={cn('w-4 h-4')} />
    </span>
   </Link>
  </div>
 );
}
