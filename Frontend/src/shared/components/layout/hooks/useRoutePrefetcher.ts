'use client';

import { useEffect, useRef } from 'react';
import { NAV_LINKS } from '@/shared/components/common/navbar/config';
import { bottomTabs, matchesPath } from '@/shared/components/layout/bottom-tab-bar/config';
import { usePathname, useRouter } from '@/i18n/routing';

function stripLocalePrefix(pathname: string): string {
 const segments = pathname.split('/').filter(Boolean);
 if (segments.length === 0) {
  return '/';
 }
 const maybeLocale = segments[0];
 if (maybeLocale.length === 2 || maybeLocale.length === 5) {
  const rest = segments.slice(1).join('/');
  return rest ? `/${rest}` : '/';
 }
 return pathname.startsWith('/') ? pathname : `/${pathname}`;
}

function clusterHrefsForPath(pathWithoutLocale: string): string[] {
 const hrefs = new Set<string>();

 for (const tab of bottomTabs) {
  const inTab = tab.matchPrefixes.some((prefix) => matchesPath(pathWithoutLocale, prefix));
  if (!inTab) {
   continue;
  }
  if (tab.href) {
   hrefs.add(tab.href);
  }
  tab.subItems?.forEach((item) => hrefs.add(item.href));
  return [...hrefs];
 }

 return [];
}

function globalNavHrefs(): string[] {
 const hrefs = new Set<string>();
 NAV_LINKS.forEach((l) => hrefs.add(l.href));
 bottomTabs.forEach((tab) => {
  if (tab.href) {
   hrefs.add(tab.href);
  }
  tab.subItems?.forEach((item) => hrefs.add(item.href));
 });
 return [...hrefs];
}

/** Cụm tab: prefetch sớm hơn sau paint; tránh 0ms để không tranh CPU với hydration. */
const CLUSTER_DELAY_MS = 120;
/** Toàn nav: sau khi cụm xong ~1s+ để không dồn request. */
const GLOBAL_DELAY_MS = 1400;

export function useRoutePrefetcher() {
 const router = useRouter();
 const pathname = usePathname();
 const prefetchedRef = useRef<Set<string>>(new Set());

 useEffect(() => {
  const connection =
   typeof navigator !== 'undefined'
    ? (navigator as Navigator & { connection?: { saveData?: boolean; effectiveType?: string } }).connection
    : undefined;
  if (connection?.saveData) {
   return;
  }
  const slowNetworks = new Set(['slow-2g', '2g']);
  if (connection?.effectiveType && slowNetworks.has(connection.effectiveType)) {
   return;
  }

  const pathKey = stripLocalePrefix(pathname);

  const prefetchHref = (href: string) => {
   const isCurrent =
    href === '/' ? pathKey === '/' : pathKey === href || pathKey.startsWith(`${href}/`);
   if (isCurrent) {
    return;
   }
   if (prefetchedRef.current.has(href)) {
    return;
   }
   prefetchedRef.current.add(href);
   router.prefetch(href);
  };

  const tCluster = window.setTimeout(() => {
   clusterHrefsForPath(pathKey).forEach(prefetchHref);
  }, CLUSTER_DELAY_MS);

  const tGlobal = window.setTimeout(() => {
   globalNavHrefs().forEach(prefetchHref);
  }, GLOBAL_DELAY_MS);

  return () => {
   window.clearTimeout(tCluster);
   window.clearTimeout(tGlobal);
  };
 }, [pathname, router]);
}
