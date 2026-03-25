'use client';

import { useEffect } from 'react';

interface AdminErrorProps {
 error: Error & { digest?: string };
 reset: () => void;
}

export default function AdminError({ error, reset }: AdminErrorProps) {
 useEffect(() => {
  console.error('[AdminErrorBoundary]', error);
 }, [error]);

 return (
  <div className="flex min-h-[50vh] flex-col items-center justify-center gap-4 px-4 text-center">
   <h2 className="text-xl font-bold tn-text-primary">Admin page failed to load</h2>
   <p className="tn-text-secondary">Try reloading this section.</p>
   <button
    type="button"
    onClick={reset}
    className="rounded-xl px-4 py-2 text-sm font-semibold tn-btn tn-btn-primary"
   >
    Retry
   </button>
  </div>
 );
}
