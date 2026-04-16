'use client';

import { forwardRef, useCallback, useEffect, useMemo, useState } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { useLocale } from 'next-intl';
import type { ComponentPropsWithoutRef, FocusEvent, MouseEvent } from 'react';
import { Link as IntlLink, usePathname, useRouter } from '@/i18n/routing';
import {
 isPrefetchBlocked,
 isPrefetchGateOpen,
 markRouteChanged,
 schedulePrefetch,
 shouldRunPrefetch,
 subscribePrefetchGate,
} from '@/shared/infrastructure/navigation/prefetchPolicy';
import { normalizeNavigationPath, prefetchRouteQueries } from '@/shared/infrastructure/navigation/routeQueryPrefetch';

type IntlLinkProps = ComponentPropsWithoutRef<typeof IntlLink>;

interface OptimizedLinkProps extends IntlLinkProps {
 prefetchQueries?: boolean;
}

const QUERY_PREFETCH_COOLDOWN_MS = 90_000;
const ROUTE_PREFETCH_COOLDOWN_MS = 90_000;

function resolveHrefPath(href: IntlLinkProps['href']): string | null {
 if (typeof href === 'string') {
  return href;
 }

 if (href && typeof href === 'object' && 'pathname' in href && typeof href.pathname === 'string') {
  return href.pathname;
 }

 return null;
}

/** Link wrapper: giữ route prefetch sau gate 500ms + query prefetch theo policy trung tâm. */
export const OptimizedLink = forwardRef<HTMLAnchorElement, OptimizedLinkProps>(function OptimizedLink(
 { href, onFocus, onMouseEnter, prefetch, prefetchQueries = true, ...rest },
 ref,
) {
 const queryClient = useQueryClient();
 const router = useRouter();
 const locale = useLocale();
 const pathname = usePathname();
 const [isGateOpen, setIsGateOpen] = useState<boolean>(false);
 const rawHref = useMemo(() => resolveHrefPath(href), [href]);

 const normalizedHref = useMemo(() => {
  const hrefPath = rawHref;
  if (!hrefPath) {
   return null;
  }
  const normalizedPath = normalizeNavigationPath(hrefPath);
  return normalizedPath.startsWith('/') ? normalizedPath : null;
 }, [rawHref]);

 useEffect(() => subscribePrefetchGate(() => setIsGateOpen(isPrefetchGateOpen())), []);

 useEffect(() => {
  if (!pathname) {
   return;
  }
  markRouteChanged(pathname);
 }, [pathname]);

 const isBlockedPath = normalizedHref ? isPrefetchBlocked(normalizedHref) : true;
 const shouldPrefetchRoute = Boolean(normalizedHref) && !isBlockedPath && (prefetch ?? true) && isGateOpen;
 const canPrefetchQueries = Boolean(normalizedHref) && !isBlockedPath && prefetchQueries;
 const canPrefetchRouteOnIntent = Boolean(rawHref) && !isBlockedPath && (prefetch ?? true);
 const routeTaskKey = normalizedHref ? `route:${locale}:${normalizedHref}` : '';
 const queryTaskKey = normalizedHref ? `query:${locale}:${normalizedHref}` : '';

 const runRoutePrefetch = useCallback(() => {
  if (!canPrefetchRouteOnIntent || !rawHref) {
   return;
  }

  if (!shouldRunPrefetch(routeTaskKey, ROUTE_PREFETCH_COOLDOWN_MS)) {
   return;
  }

  schedulePrefetch(routeTaskKey, async () => {
   try {
    await router.prefetch(rawHref);
   } catch {
    // Route prefetch remains best-effort and should never block navigation.
   }
  });
 }, [canPrefetchRouteOnIntent, rawHref, routeTaskKey, router]);

 const runQueryPrefetch = useCallback(() => {
  if (!canPrefetchQueries || !normalizedHref) {
   return;
  }

  if (!shouldRunPrefetch(queryTaskKey, QUERY_PREFETCH_COOLDOWN_MS)) {
   return;
  }

  schedulePrefetch(queryTaskKey, async () => {
   await prefetchRouteQueries(queryClient, normalizedHref);
  });
 }, [canPrefetchQueries, normalizedHref, queryClient, queryTaskKey]);

 const handleMouseEnter = useCallback((event: MouseEvent<HTMLAnchorElement>) => {
  onMouseEnter?.(event);
  runRoutePrefetch();
  runQueryPrefetch();
 }, [onMouseEnter, runQueryPrefetch, runRoutePrefetch]);

 const handleFocus = useCallback((event: FocusEvent<HTMLAnchorElement>) => {
  onFocus?.(event);
  runRoutePrefetch();
  runQueryPrefetch();
 }, [onFocus, runQueryPrefetch, runRoutePrefetch]);

 return (
  <IntlLink
   ref={ref}
   href={href}
   prefetch={shouldPrefetchRoute}
   onFocus={handleFocus}
   onMouseEnter={handleMouseEnter}
   {...rest}
  />
 );
});
