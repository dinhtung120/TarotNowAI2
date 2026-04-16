'use client';

import { forwardRef, useCallback } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import type { ComponentPropsWithoutRef, FocusEvent, MouseEvent } from 'react';
import { Link as IntlLink } from '@/i18n/routing';
import { normalizeNavigationPath, prefetchRouteQueries } from '@/shared/infrastructure/navigation/routeQueryPrefetch';

type IntlLinkProps = ComponentPropsWithoutRef<typeof IntlLink>;

interface OptimizedLinkProps extends IntlLinkProps {
 prefetchQueries?: boolean;
}

/**
 * Link wrapper: giữ Next prefetch và bổ sung TanStack query prefetch theo route đích.
 */
export const OptimizedLink = forwardRef<HTMLAnchorElement, OptimizedLinkProps>(
 function OptimizedLink({ href, onFocus, onMouseEnter, prefetch, prefetchQueries = true, ...rest }, ref) {
  const queryClient = useQueryClient();

  const runQueryPrefetch = useCallback(() => {
   if (!prefetchQueries || typeof href !== 'string') {
    return;
   }

   const normalizedHref = normalizeNavigationPath(href);
   void prefetchRouteQueries(queryClient, normalizedHref);
  }, [href, prefetchQueries, queryClient]);

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
    prefetch={prefetch ?? true}
    onFocus={handleFocus}
    onMouseEnter={handleMouseEnter}
    {...rest}
   />
  );
 },
);
