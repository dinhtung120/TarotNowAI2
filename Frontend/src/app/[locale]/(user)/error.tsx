'use client';

import { useEffect } from 'react';
import { Link } from '@/i18n/routing';
import { cn } from '@/lib/utils';

interface UserErrorProps {
 error: Error & { digest?: string };
 reset: () => void;
}

export default function UserSegmentError({ error, reset }: UserErrorProps) {
 useEffect(() => {
  console.error('[UserSegmentErrorBoundary]', error);
 }, [error]);

 return (
  <div className={cn("mx-auto flex min-h-[55vh] max-w-xl flex-col items-center justify-center gap-5 px-6 text-center")}>
   <h2 className={cn("text-2xl font-bold tn-text-primary")}>Unable to load this page</h2>
   <p className={cn("tn-text-secondary")}>Please retry, or return to your home page.</p>
   <div className={cn("flex flex-wrap items-center justify-center gap-3")}>
    <button
     type="button"
     onClick={reset}
     className={cn("rounded-xl px-4 py-2 text-sm font-semibold tn-btn tn-btn-primary")}
    >
     Retry
    </button>
    <Link
     href="/"
     className={cn("rounded-xl border px-4 py-2 text-sm font-semibold tn-border tn-text-secondary hover:tn-text-primary")}
    >
     Home
    </Link>
   </div>
  </div>
 );
}
