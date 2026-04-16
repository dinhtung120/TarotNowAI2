'use client';

import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { cn } from '@/lib/utils';
import { useErrorBoundaryLogger } from '@/shared/application/hooks/useErrorBoundaryLogger';

interface LocaleErrorProps {
 error: Error & { digest?: string };
 reset: () => void;
}

export default function LocaleError({ error, reset }: LocaleErrorProps) {
 useErrorBoundaryLogger('LocaleErrorBoundary', error);

 return (
  <div className={cn("mx-auto flex tn-min-h-60vh max-w-2xl flex-col items-center justify-center gap-6 px-6 text-center")}>
   <h1 className={cn("text-2xl font-bold tn-text-primary")}>Something went wrong</h1>
   <p className={cn("tn-text-secondary")}>
    We could not load this page right now. Please retry or return to the homepage.
   </p>
   <div className={cn("flex flex-wrap items-center justify-center gap-3")}>
    <button
     type="button"
     onClick={reset}
     className={cn("rounded-xl px-4 py-2 text-sm font-semibold tn-btn tn-btn-primary")}
    >
     Try again
    </button>
    <Link
     href="/"
     className={cn("rounded-xl border px-4 py-2 text-sm font-semibold tn-border tn-text-secondary tn-hover-text-primary")}
    >
     Back to home
    </Link>
   </div>
  </div>
 );
}
