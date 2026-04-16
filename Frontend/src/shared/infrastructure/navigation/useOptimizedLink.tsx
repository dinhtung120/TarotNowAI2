'use client';

import { forwardRef, useCallback, useMemo } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { useLocale } from 'next-intl';
import type { ComponentPropsWithoutRef, FocusEvent, MouseEvent } from 'react';
import { Link as IntlLink, useRouter } from '@/i18n/routing';
import { isPrefetchBlocked, schedulePrefetch, shouldRunPrefetch } from '@/shared/infrastructure/navigation/prefetchPolicy';
import { normalizeNavigationPath, prefetchRouteQueries } from '@/shared/infrastructure/navigation/routeQueryPrefetch';

type IntlLinkProps = ComponentPropsWithoutRef<typeof IntlLink>;

interface OptimizedLinkProps extends IntlLinkProps {
 prefetchQueries?: boolean;
}

const ROUTE_PREFETCH_COOLDOWN_MS = 90_000;
const QUERY_PREFETCH_COOLDOWN_MS = 90_000;

/**
 * Link wrapper tối ưu prefetch:
 * - Route/query prefetch đều đi qua policy trung tâm (block + gate + dedupe).
 * - Tắt prefetch mặc định của Next Link để tránh bypass policy.
 */
export const OptimizedLink = forwardRef<HTMLAnchorElement, OptimizedLinkProps>(
 function OptimizedLink({ href, onFocus, onMouseEnter, prefetch, prefetchQueries = true, ...rest }, ref) {
  const queryClient = useQueryClient();
  const router = useRouter();
  const locale = useLocale();

  const hrefString = typeof href === 'string' ? href : null;
  const normalizedHref = useMemo(() => {
   if (!hrefString) {
    return null;
   }
   const normalizedPath = normalizeNavigationPath(hrefString);
   return normalizedPath.startsWith('/') ? normalizedPath : null;
  }, [hrefString]);

  const isBlockedPath = normalizedHref ? isPrefetchBlocked(normalizedHref) : false;
  const canPrefetchRoute = Boolean(hrefString && normalizedHref) && !isBlockedPath && prefetch !== false;
  const canPrefetchQueries = Boolean(normalizedHref) && !isBlockedPath && prefetchQueries;

  const routeTaskKey = normalizedHref ? `route:${locale}:${normalizedHref}` : '';
  const queryTaskKey = normalizedHref ? `query:${locale}:${normalizedHref}` : '';

  const runPrefetch = useCallback(() => {
   if (!normalizedHref) {
    return;
   }

   if (canPrefetchRoute && shouldRunPrefetch(routeTaskKey, ROUTE_PREFETCH_COOLDOWN_MS)) {
    schedulePrefetch(routeTaskKey, async () => {
     if (!hrefString) {
      return;
     }
     try {
      await router.prefetch(hrefString);
     } catch {
      // Route prefetch là best-effort để không ảnh hưởng tương tác.
     }
    });
   }

   if (canPrefetchQueries && shouldRunPrefetch(queryTaskKey, QUERY_PREFETCH_COOLDOWN_MS)) {
    schedulePrefetch(queryTaskKey, async () => {
     await prefetchRouteQueries(queryClient, normalizedHref);
    });
   }
  }, [canPrefetchQueries, canPrefetchRoute, hrefString, normalizedHref, queryClient, queryTaskKey, routeTaskKey, router]);

  const handleMouseEnter = useCallback((event: MouseEvent<HTMLAnchorElement>) => {
   onMouseEnter?.(event);
   runPrefetch();
  }, [onMouseEnter, runPrefetch]);

  const handleFocus = useCallback((event: FocusEvent<HTMLAnchorElement>) => {
   onFocus?.(event);
   runPrefetch();
  }, [onFocus, runPrefetch]);

  return (
   <IntlLink
    ref={ref}
    href={href}
    prefetch={false}
    onFocus={handleFocus}
    onMouseEnter={handleMouseEnter}
    {...rest}
   />
  );
 },
);
