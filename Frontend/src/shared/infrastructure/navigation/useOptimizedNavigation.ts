'use client';

import { useCallback } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { useRouter } from '@/i18n/routing';
import { normalizeNavigationPath, prefetchRouteQueries } from '@/shared/infrastructure/navigation/routeQueryPrefetch';

interface OptimizedNavigateOptions {
 useViewTransition?: boolean;
}

function runWithViewTransition(callback: () => void, enabled: boolean): void {
 if (!enabled || typeof document === 'undefined' || typeof window === 'undefined') {
  callback();
  return;
 }

 if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
  callback();
  return;
 }

 if (!('startViewTransition' in document)) {
  callback();
  return;
 }

 document.startViewTransition?.(() => {
  callback();
 });
}

/** Hook điều hướng tối ưu: prefetch route + query cache trước khi push/replace. */
export function useOptimizedNavigation() {
 const router = useRouter();
 const queryClient = useQueryClient();

 const prefetch = useCallback((href: string) => {
  const normalizedHref = normalizeNavigationPath(href);

  try {
   void router.prefetch(href);
  } catch {
   // Prefetch route tree thất bại không được chặn user navigation.
  }

  void prefetchRouteQueries(queryClient, normalizedHref);
 }, [queryClient, router]);

 const push = useCallback((href: string, options?: OptimizedNavigateOptions) => {
  prefetch(href);

  runWithViewTransition(() => {
   router.push(href);
  }, options?.useViewTransition ?? true);
 }, [prefetch, router]);

 const replace = useCallback((href: string, options?: OptimizedNavigateOptions) => {
  prefetch(href);

  runWithViewTransition(() => {
   router.replace(href);
  }, options?.useViewTransition ?? true);
 }, [prefetch, router]);

 return {
  prefetch,
  push,
  replace,
 };
}
