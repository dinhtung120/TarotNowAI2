import type { QueryClient } from '@tanstack/react-query';
import { routing } from '@/i18n/routing';
import { gachaQueryKeys } from '@/shared/infrastructure/gacha/gachaConstants';
import { fetchJsonOrThrow } from '@/shared/infrastructure/http/clientFetch';
import { inventoryQueryKeys } from '@/shared/infrastructure/inventory/inventoryConstants';
import { isPrefetchBlocked } from '@/shared/infrastructure/navigation/prefetchPolicy';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { useAuthStore } from '@/store/authStore';

interface RouteQuerySpec {
 queryKey: readonly unknown[];
 queryFn: () => Promise<unknown>;
 staleTime: number;
}

const PREFETCH_TIMEOUT_MS = 6_000;

const startsWithSegment = (pathname: string, segment: string): boolean =>
 pathname === segment || pathname.startsWith(`${segment}/`);

const stripQueryHash = (href: string): string => href.split('#')[0]?.split('?')[0] ?? href;

const stripLocalePrefix = (pathname: string): string => {
 const candidate = pathname.split('/')[1];
 if (!candidate) {
  return pathname;
 }

 if (!routing.locales.includes(candidate as (typeof routing.locales)[number])) {
  return pathname;
 }

 const rest = pathname.split('/').slice(2).join('/');
 return `/${rest}`.replace(/\/+$/, '') || '/';
};

async function fetchPrefetchJson<T>(apiPath: string, fallbackErrorMessage: string): Promise<T> {
 return fetchJsonOrThrow<T>(
  apiPath,
  {
   method: 'GET',
   credentials: 'include',
   cache: 'no-store',
  },
  fallbackErrorMessage,
  PREFETCH_TIMEOUT_MS,
 );
}

/** Normalize URL path (remove locale + query/hash) before route-query mapping. */
export function normalizeNavigationPath(href: string): string {
 const rawPath = stripQueryHash(href);
 if (!rawPath.startsWith('/')) {
  return rawPath;
 }

 const withoutLocale = stripLocalePrefix(rawPath);
 return withoutLocale.replace(/\/+$/, '') || '/';
}

function buildRouteQuerySpecs(pathname: string): RouteQuerySpec[] {
 const authState = useAuthStore.getState();
 const isAuthenticated = authState.isAuthenticated;
 const normalizedRole = authState.user?.role?.trim().toLowerCase() ?? '';
 if (!isAuthenticated) {
  return [];
 }

 if (pathname === '/inventory') {
  return [
   {
    queryKey: inventoryQueryKeys.mine(),
    queryFn: () => fetchPrefetchJson('/api/inventory', 'Failed to prefetch inventory.'),
    staleTime: 20_000,
   },
  ];
 }

 if (pathname === '/gacha') {
  return [
   {
    queryKey: gachaQueryKeys.pools(),
    queryFn: () => fetchPrefetchJson('/api/gacha/pools', 'Failed to prefetch gacha pools.'),
    staleTime: 20_000,
   },
  ];
 }

 if (pathname === '/reading') {
  if (normalizedRole === 'admin') {
   return [];
  }

  return [
   {
    queryKey: userStateQueryKeys.reading.setupSnapshot(),
    queryFn: () => fetchPrefetchJson('/api/reading/setup-snapshot', 'Failed to prefetch reading setup.'),
    staleTime: 45_000,
   },
  ];
 }

 if (startsWithSegment(pathname, '/wallet')) {
  return [
   {
    queryKey: userStateQueryKeys.wallet.balance(),
    queryFn: () => fetchPrefetchJson('/api/wallet/balance', 'Failed to prefetch wallet balance.'),
    staleTime: 20_000,
   },
  ];
 }

 return [];
}

export async function prefetchRouteQueries(queryClient: QueryClient, href: string): Promise<void> {
 const pathname = normalizeNavigationPath(href);
 if (!pathname.startsWith('/') || isPrefetchBlocked(pathname)) {
  return;
 }

 const specs = buildRouteQuerySpecs(pathname);
 if (specs.length === 0) {
  return;
 }

 await Promise.all(
  specs.map(async (spec) => {
   try {
    await queryClient.prefetchQuery({
     queryKey: spec.queryKey,
     queryFn: spec.queryFn,
     staleTime: spec.staleTime,
    });
   } catch {
    // Prefetch remains best-effort to keep navigation unblocked.
   }
  }),
 );
}
