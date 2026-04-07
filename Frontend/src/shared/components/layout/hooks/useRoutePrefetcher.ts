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
  '/leaderboard',
  '/subscription',
];

const PrefetchDelayMs = 2000;
const PREFETCH_SESSION_KEY = 'tn-route-prefetch-done-v1';

export function useRoutePrefetcher() {
  const router = useRouter();

  useEffect(() => {
    if (process.env.NODE_ENV !== 'production') return;

    const hasPrefetched = window.sessionStorage.getItem(PREFETCH_SESSION_KEY);
    if (hasPrefetched === '1') return;
    window.sessionStorage.setItem(PREFETCH_SESSION_KEY, '1');

    const timer = window.setTimeout(() => {
      PREFETCH_ROUTES.forEach((route) => {
        router.prefetch(route);
      });
    }, PrefetchDelayMs);

    return () => window.clearTimeout(timer);
  }, [router]);
}
