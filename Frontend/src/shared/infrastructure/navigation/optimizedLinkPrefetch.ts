import { useCallback, useMemo } from 'react';
import { isPrefetchBlocked, schedulePrefetch, shouldRunPrefetch } from '@/shared/infrastructure/navigation/prefetchPolicy';
import { normalizeNavigationPath } from '@/shared/infrastructure/navigation/routeQueryPrefetch';

const QUERY_PREFETCH_COOLDOWN_MS = 90_000;
const ROUTE_PREFETCH_COOLDOWN_MS = 90_000;

type HrefLike = { pathname?: string | null } | string;

export function resolveHrefPath(href: HrefLike): string | null {
 if (typeof href === 'string') {
  return href;
 }

 return typeof href?.pathname === 'string' ? href.pathname : null;
}

interface UseOptimizedLinkPrefetchOptions {
 href: HrefLike;
 locale: string;
 onQueryPrefetch: (normalizedHref: string) => Promise<void>;
 onRoutePrefetch: (rawHref: HrefLike) => Promise<void>;
 pathname: string;
 prefetch?: boolean | 'auto' | null;
 prefetchQueries: boolean;
}

export function useOptimizedLinkPrefetch(options: UseOptimizedLinkPrefetchOptions) {
 const rawHref = useMemo(() => resolveHrefPath(options.href), [options.href]);
 const normalizedCurrentPath = useMemo(() => normalizeNavigationPath(options.pathname), [options.pathname]);
 const normalizedHref = useMemo(() => {
  if (!rawHref) {
   return null;
  }

  const normalizedPath = normalizeNavigationPath(rawHref);
  return normalizedPath.startsWith('/') ? normalizedPath : null;
 }, [rawHref]);

 const isBlockedPath = normalizedHref ? isPrefetchBlocked(normalizedHref) : true;
 const isSameRoute = Boolean(normalizedHref) && normalizedHref === normalizedCurrentPath;
 const canPrefetchQueries = Boolean(normalizedHref) && !isBlockedPath && !isSameRoute && options.prefetchQueries;
 const prefetchEnabled = options.prefetch !== false;
 const canPrefetchRouteOnIntent = Boolean(rawHref) && !isBlockedPath && !isSameRoute && prefetchEnabled;
 const routeTaskKey = normalizedHref ? `route:${options.locale}:${normalizedHref}` : '';
 const queryTaskKey = normalizedHref ? `query:${options.locale}:${normalizedHref}` : '';

 const runRoutePrefetch = useCallback(() => {
  if (!canPrefetchRouteOnIntent || !rawHref || !shouldRunPrefetch(routeTaskKey, ROUTE_PREFETCH_COOLDOWN_MS)) {
   return;
  }

  schedulePrefetch(routeTaskKey, async () => {
   try {
    await options.onRoutePrefetch(options.href);
   } catch {
    // Route prefetch remains best-effort and should never block navigation.
   }
  });
 }, [canPrefetchRouteOnIntent, options, rawHref, routeTaskKey]);

 const runQueryPrefetch = useCallback(() => {
  if (!canPrefetchQueries || !normalizedHref || !shouldRunPrefetch(queryTaskKey, QUERY_PREFETCH_COOLDOWN_MS)) {
   return;
  }

  schedulePrefetch(queryTaskKey, async () => {
   await options.onQueryPrefetch(normalizedHref);
  });
 }, [canPrefetchQueries, normalizedHref, options, queryTaskKey]);

 return {
  runRoutePrefetch,
  runQueryPrefetch,
 };
}
