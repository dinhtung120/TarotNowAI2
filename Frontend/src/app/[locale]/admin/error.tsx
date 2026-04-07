'use client';

import { cn } from '@/lib/utils';
import { useErrorBoundaryLogger } from '@/shared/application/hooks/useErrorBoundaryLogger';

interface AdminErrorProps {
 error: Error & { digest?: string };
 reset: () => void;
}

export default function AdminError({ error, reset }: AdminErrorProps) {
 useErrorBoundaryLogger('AdminErrorBoundary', error);

 return (
  <div className={cn("flex tn-minh-50vh flex-col items-center justify-center gap-4 px-4 text-center")}>
   <h2 className={cn("text-xl font-bold tn-text-primary")}>Admin page failed to load</h2>
   <p className={cn("tn-text-secondary")}>Try reloading this section.</p>
   <button
    type="button"
    onClick={reset}
    className={cn("rounded-xl px-4 py-2 text-sm font-semibold tn-btn tn-btn-primary")}
   >
    Retry
   </button>
  </div>
 );
}
