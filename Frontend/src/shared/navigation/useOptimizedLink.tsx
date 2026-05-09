'use client';

import { forwardRef, useCallback } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { useLocale } from 'next-intl';
import type { ComponentPropsWithoutRef, FocusEvent, MouseEvent } from 'react';
import { Link as IntlLink, usePathname, useRouter } from '@/i18n/routing';
import { prefetchRouteQueries } from '@/shared/navigation/routeQueryPrefetch';
import { useOptimizedLinkPrefetch } from '@/shared/navigation/optimizedLinkPrefetch';

type IntlLinkProps = ComponentPropsWithoutRef<typeof IntlLink>;
type RouterPrefetchHref = Parameters<ReturnType<typeof useRouter>['prefetch']>[0];

interface OptimizedLinkProps extends IntlLinkProps {
 prefetchQueries?: boolean;
}

/** Link wrapper: giữ route prefetch sau gate 500ms + query prefetch theo policy trung tâm. */
export const OptimizedLink = forwardRef<HTMLAnchorElement, OptimizedLinkProps>(function OptimizedLink(
 { href, onFocus, onMouseEnter, prefetch, prefetchQueries = false, ...rest },
 ref,
) {
 const queryClient = useQueryClient();
 const router = useRouter();
 const pathname = usePathname();
 const locale = useLocale();
 const { runRoutePrefetch, runQueryPrefetch } = useOptimizedLinkPrefetch({
  href,
  locale,
  pathname,
  prefetch,
  prefetchQueries,
  onQueryPrefetch: async (normalizedHref) => prefetchRouteQueries(queryClient, normalizedHref),
  onRoutePrefetch: async (rawHref) => {
   if (typeof rawHref === 'string') {
    await router.prefetch(rawHref);
    return;
   }

   if (typeof rawHref.pathname === 'string') {
    const prefetchHref: RouterPrefetchHref = {
     ...rawHref,
     pathname: rawHref.pathname,
    };
    await router.prefetch(prefetchHref);
   }
  },
 });

 const prefetchDisabled = prefetch === false;

 const handleMouseEnter = useCallback((event: MouseEvent<HTMLAnchorElement>) => {
  onMouseEnter?.(event);
  if (prefetchDisabled) return;
  runRoutePrefetch();
  runQueryPrefetch();
 }, [onMouseEnter, prefetchDisabled, runQueryPrefetch, runRoutePrefetch]);

 const handleFocus = useCallback((event: FocusEvent<HTMLAnchorElement>) => {
  onFocus?.(event);
  if (prefetchDisabled) return;
  runRoutePrefetch();
  runQueryPrefetch();
 }, [onFocus, prefetchDisabled, runQueryPrefetch, runRoutePrefetch]);

 const nativePrefetchEnabled = prefetch === true;

 return (
  <IntlLink
   ref={ref}
   href={href}
   prefetch={nativePrefetchEnabled}
   onFocus={handleFocus}
   onMouseEnter={handleMouseEnter}
   {...rest}
  />
 );
});
