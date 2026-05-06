import { routing } from '@/i18n/routing';

const AUTHLESS_PATHS = [
 '/login',
 '/register',
 '/forgot-password',
 '/reset-password',
 '/verify-email',
] as const;

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

/**
 * Legal routes thường là content tĩnh, không cần realtime/auth sync nền.
 */
export function isLegalPath(pathname: string): boolean {
 const normalizedPath = normalizePathname(pathname);
 return normalizedPath === '/legal' || normalizedPath.startsWith('/legal/');
}

/**
 * Route public/auth không cần realtime connection.
 */
export function isAuthlessPath(pathname: string): boolean {
 const normalizedPath = normalizePathname(pathname);
 return AUTHLESS_PATHS.some((path) => normalizedPath === path || normalizedPath.startsWith(`${path}/`));
}

/**
 * Bật realtime cho toàn bộ route người dùng đã xác thực (trừ auth/admin/legal).
 */
export function shouldEnableRealtimeForPath(pathname: string): boolean {
 const normalizedPath = normalizePathname(pathname);
 if (isAuthlessPath(normalizedPath)) return false;
 if (normalizedPath === '/admin' || normalizedPath.startsWith('/admin/')) return false;
 if (isLegalPath(normalizedPath)) return false;

 return true;
}
