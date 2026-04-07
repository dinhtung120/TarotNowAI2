'use client';

import { useEffect } from 'react';
import { useRouter } from '@/i18n/routing';

const PREFETCH_ROUTES = [
  '/',
  '/reading',
  '/chat',
  '/wallet',
  '/profile',
  '/readers',
  '/collection',
  '/gamification',
  '/subscription',
];

const PrefetchDelayMs = 2000;

export function useRoutePrefetcher() {
  const router = useRouter();

  useEffect(() => {
    const timer = window.setTimeout(() => {
      PREFETCH_ROUTES.forEach((route) => {
        router.prefetch(route);
      });
    }, PrefetchDelayMs);

    return () => window.clearTimeout(timer);
  }, [router]);
}
