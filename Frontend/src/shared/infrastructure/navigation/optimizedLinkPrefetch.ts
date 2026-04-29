import { useCallback, useMemo } from 'react';
import { isPrefetchBlocked, schedulePrefetch, shouldRunPrefetch } from '@/shared/infrastructure/navigation/prefetchPolicy';
import { normalizeNavigationPath } from '@/shared/infrastructure/navigation/routeQueryPrefetch';

const QUERY_PREFETCH_COOLDOWN_MS = 300_000;
const ROUTE_PREFETCH_COOLDOWN_MS = 240_000;

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

function isPrefetchBudgetAvailable(): boolean {
 if (typeof navigator === 'undefined') {
  return true;
 }

 const connection = (navigator as Navigator & {
  connection?: {
   saveData?: boolean;
   effectiveType?: string;
  };
 }).connection;
 if (!connection) {
  return true;
 }

 if (connection.saveData) {
  return false;
 }

 const effectiveType = connection.effectiveType?.toLowerCase() ?? '';
 return effectiveType !== 'slow-2g' && effectiveType !== '2g';
}

export function useOptimizedLinkPrefetch(options: UseOptimizedLinkPrefetchOptions) {
 const {
  href,
  locale,
  onQueryPrefetch,
  onRoutePrefetch,
  pathname,
  prefetch,
  prefetchQueries,
 } = options;
 const rawHref = useMemo(() => resolveHrefPath(href), [href]);
 const normalizedCurrentPath = useMemo(() => normalizeNavigationPath(pathname), [pathname]);
 const normalizedHref = useMemo(() => {
  if (!rawHref) {
   return null;
  }

  const normalizedPath = normalizeNavigationPath(rawHref);
  return normalizedPath.startsWith('/') ? normalizedPath : null;
 }, [rawHref]);

 const isBlockedPath = normalizedHref ? isPrefetchBlocked(normalizedHref) : true;
 const isSameRoute = Boolean(normalizedHref) && normalizedHref === normalizedCurrentPath;
 const canPrefetchQueries = Boolean(normalizedHref) && !isBlockedPath && !isSameRoute && prefetchQueries;
 const prefetchEnabled = prefetch !== false;
 const canPrefetchRouteOnIntent = Boolean(rawHref) && !isBlockedPath && !isSameRoute && prefetchEnabled;
 const routeTaskKey = normalizedHref ? `route:${locale}:${normalizedHref}` : '';
 const queryTaskKey = normalizedHref ? `query:${locale}:${normalizedHref}` : '';

 const runRoutePrefetch = useCallback(() => {
  if (
   !canPrefetchRouteOnIntent
   || !rawHref
   || !isPrefetchBudgetAvailable()
   || !shouldRunPrefetch(routeTaskKey, ROUTE_PREFETCH_COOLDOWN_MS)
  ) {
   return;
  }

  schedulePrefetch(routeTaskKey, async () => {
   try {
    await onRoutePrefetch(href);
   } catch {
    // Route prefetch remains best-effort and should never block navigation.
   }
  });
 }, [canPrefetchRouteOnIntent, href, onRoutePrefetch, rawHref, routeTaskKey]);

 const runQueryPrefetch = useCallback(() => {
  if (
   !canPrefetchQueries
   || !normalizedHref
   || !isPrefetchBudgetAvailable()
   || !shouldRunPrefetch(queryTaskKey, QUERY_PREFETCH_COOLDOWN_MS)
  ) {
   return;
  }

  schedulePrefetch(queryTaskKey, async () => {
   await onQueryPrefetch(normalizedHref);
  });
 }, [canPrefetchQueries, normalizedHref, onQueryPrefetch, queryTaskKey]);

 return {
  canPrefetchRouteOnIntent,
  canPrefetchQueries,
  runRoutePrefetch,
  runQueryPrefetch,
 };
}
