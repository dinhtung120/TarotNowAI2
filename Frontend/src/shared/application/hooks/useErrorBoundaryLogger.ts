'use client';

import { useEffect } from 'react';

export function useErrorBoundaryLogger(scope: string, error: Error & { digest?: string }) {
  useEffect(() => {
    console.error(`[${scope}]`, error);
  }, [error, scope]);
}
