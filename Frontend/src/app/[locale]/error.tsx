'use client';

import { useEffect } from 'react';
import { Link } from '@/i18n/routing';

interface LocaleErrorProps {
 error: Error & { digest?: string };
 reset: () => void;
}

export default function LocaleError({ error, reset }: LocaleErrorProps) {
 useEffect(() => {
  console.error('[LocaleErrorBoundary]', error);
 }, [error]);

 return (
  <div className="mx-auto flex min-h-[60vh] max-w-2xl flex-col items-center justify-center gap-6 px-6 text-center">
   <h1 className="text-2xl font-bold tn-text-primary">Something went wrong</h1>
   <p className="tn-text-secondary">
    We could not load this page right now. Please retry or return to the homepage.
   </p>
   <div className="flex flex-wrap items-center justify-center gap-3">
    <button
     type="button"
     onClick={reset}
     className="rounded-xl px-4 py-2 text-sm font-semibold tn-btn tn-btn-primary"
    >
     Try again
    </button>
    <Link
     href="/"
     className="rounded-xl border px-4 py-2 text-sm font-semibold tn-border tn-text-secondary hover:tn-text-primary"
    >
     Back to home
    </Link>
   </div>
  </div>
 );
}
