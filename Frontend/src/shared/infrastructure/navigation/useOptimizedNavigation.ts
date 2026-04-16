'use client';

import { useCallback, useEffect } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { useLocale } from 'next-intl';
import { usePathname, useRouter } from '@/i18n/routing';
import { isPrefetchBlocked, markRouteChanged, schedulePrefetch, shouldRunPrefetch } from '@/shared/infrastructure/navigation/prefetchPolicy';
import { normalizeNavigationPath, prefetchRouteQueries } from '@/shared/infrastructure/navigation/routeQueryPrefetch';

interface OptimizedNavigateOptions {
 useViewTransition?: boolean;
}

const ROUTE_PREFETCH_COOLDOWN_MS = 90_000;
const QUERY_PREFETCH_COOLDOWN_MS = 90_000;

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
 const pathname = usePathname();
 const locale = useLocale();

 useEffect(() => {
  if (!pathname) {
   return;
  }
  markRouteChanged(pathname);
 }, [pathname]);

 const prefetch = useCallback((href: string) => {
  const normalizedHref = normalizeNavigationPath(href);
  if (!normalizedHref.startsWith('/') || isPrefetchBlocked(normalizedHref)) {
   return;
  }

  const routeTaskKey = `route:${locale}:${normalizedHref}`;
  if (shouldRunPrefetch(routeTaskKey, ROUTE_PREFETCH_COOLDOWN_MS)) {
   schedulePrefetch(routeTaskKey, async () => {
    try {
     await router.prefetch(href);
    } catch {
     // Prefetch route tree là best-effort, tuyệt đối không chặn navigation.
    }
   });
  }

  const queryTaskKey = `query:${locale}:${normalizedHref}`;
  if (shouldRunPrefetch(queryTaskKey, QUERY_PREFETCH_COOLDOWN_MS)) {
   schedulePrefetch(queryTaskKey, async () => {
    await prefetchRouteQueries(queryClient, normalizedHref);
   });
  }
 }, [locale, queryClient, router]);

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

 return { prefetch, push, replace };
}
