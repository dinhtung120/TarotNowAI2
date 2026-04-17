import { routing } from '@/i18n/routing';

/**
 * Chuẩn hoá pathname client-side:
 * - bỏ query/hash
 * - bỏ trailing slash
 * - bỏ locale prefix (vi/en/zh)
 */
export function normalizePathname(pathname: string): string {
 const basePath = pathname.split('#')[0]?.split('?')[0] ?? pathname;
 const normalizedBasePath = basePath.replace(/\/+$/, '') || '/';
 const firstSegment = normalizedBasePath.split('/')[1];

 if (!firstSegment) {
  return normalizedBasePath;
 }

 if (!routing.locales.includes(firstSegment as (typeof routing.locales)[number])) {
  return normalizedBasePath;
 }

 const withoutLocale = normalizedBasePath.split('/').slice(2).join('/');
 return `/${withoutLocale}`.replace(/\/+$/, '') || '/';
}

/**
 * Home route check đã loại bỏ locale prefix.
 */
export function isHomePath(pathname: string): boolean {
 return normalizePathname(pathname) === '/';
}
