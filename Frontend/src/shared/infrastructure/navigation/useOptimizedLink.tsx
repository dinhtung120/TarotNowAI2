'use client';

import { forwardRef, useCallback, useEffect, useMemo, useState } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { useLocale } from 'next-intl';
import type { ComponentPropsWithoutRef, FocusEvent, MouseEvent } from 'react';
import { Link as IntlLink, usePathname } from '@/i18n/routing';
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
 const locale = useLocale();
 const pathname = usePathname();
 const [isGateOpen, setIsGateOpen] = useState<boolean>(false);

 const normalizedHref = useMemo(() => {
  const hrefPath = resolveHrefPath(href);
  if (!hrefPath) {
   return null;
  }
  const normalizedPath = normalizeNavigationPath(hrefPath);
  return normalizedPath.startsWith('/') ? normalizedPath : null;
 }, [href]);

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
 const queryTaskKey = normalizedHref ? `query:${locale}:${normalizedHref}` : '';

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
  runQueryPrefetch();
 }, [onMouseEnter, runQueryPrefetch]);

 const handleFocus = useCallback((event: FocusEvent<HTMLAnchorElement>) => {
  onFocus?.(event);
  runQueryPrefetch();
 }, [onFocus, runQueryPrefetch]);

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
