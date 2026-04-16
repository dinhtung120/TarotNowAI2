import { routing } from '@/i18n/routing';

const blockedPrefixes = ['/admin'] as const;
const blockedExactPaths = new Set([
 '/wallet/withdraw',
 '/gacha/history',
 '/profile/mfa',
 '/readers/[id]',
 '/reading/history/[id]',
]);
const blockedRegexPatterns = [
 /^\/readers\/[^/]+$/,
 /^\/reading\/history\/[^/]+$/,
] as const;

const ROUTE_CHANGE_DELAY_MS = 500;

let prefetchReadyAt = 0;
let lastMarkedPathname = '';
let gateOpenTimerId: number | null = null;

const pendingPrefetches = new Map<string, number>();
const inFlightPrefetches = new Set<string>();
const lastPrefetchTimestamps = new Map<string, number>();
const gateListeners = new Set<() => void>();

function normalizePathname(pathname: string): string {
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

function matchesBlockedPrefix(pathname: string): boolean {
 return blockedPrefixes.some((prefix) => pathname === prefix || pathname.startsWith(`${prefix}/`));
}

function clearPendingPrefetches(): void {
 for (const timeoutId of pendingPrefetches.values()) {
  clearTimeout(timeoutId);
 }
 pendingPrefetches.clear();
}

function notifyGateListeners(): void {
 for (const listener of gateListeners) {
  listener();
 }
}

function scheduleGateOpenNotification(): void {
 if (typeof window === 'undefined') {
  return;
 }

 if (gateOpenTimerId !== null) {
  clearTimeout(gateOpenTimerId);
 }

 const delayMs = Math.max(0, prefetchReadyAt - Date.now());
 gateOpenTimerId = window.setTimeout(() => {
  gateOpenTimerId = null;
  notifyGateListeners();
 }, delayMs);

 // Báo ngay trạng thái "gate đang đóng/mở" cho subscriber để cập nhật prefetch prop.
 notifyGateListeners();
}

function syncRouteChangeGateFromBrowserPath(): void {
 if (typeof window === 'undefined') {
  return;
 }

 const browserPathname = normalizePathname(window.location.pathname);
 if (browserPathname === lastMarkedPathname) {
  return;
 }

 lastMarkedPathname = browserPathname;
 prefetchReadyAt = Date.now() + ROUTE_CHANGE_DELAY_MS;
 clearPendingPrefetches();
 scheduleGateOpenNotification();
}

/**
 * Kiểm tra route có thuộc danh sách cấm prefetch hay không.
 */
export function isPrefetchBlocked(pathname: string): boolean {
 const normalizedPath = normalizePathname(pathname);

 if (blockedExactPaths.has(normalizedPath)) {
  return true;
 }

 if (matchesBlockedPrefix(normalizedPath)) {
  return true;
 }

 return blockedRegexPatterns.some((pattern) => pattern.test(normalizedPath));
}

/**
 * Đánh dấu route vừa đổi; mọi prefetch mới sẽ mở sau 500ms.
 */
export function markRouteChanged(pathname: string): void {
 const normalizedPath = normalizePathname(pathname);
 if (normalizedPath === lastMarkedPathname) {
  return;
 }

 lastMarkedPathname = normalizedPath;
 prefetchReadyAt = Date.now() + ROUTE_CHANGE_DELAY_MS;

 // Hủy các tác vụ queue cũ để tránh chạy nhầm prefetch của route trước.
 clearPendingPrefetches();
 scheduleGateOpenNotification();
}

/**
 * Kiểm tra prefetch gate đã mở hay chưa (>= readyAt).
 */
export function isPrefetchGateOpen(): boolean {
 syncRouteChangeGateFromBrowserPath();
 return Date.now() >= prefetchReadyAt;
}

/**
 * Đăng ký listener để nhận cập nhật gate open/close.
 */
export function subscribePrefetchGate(listener: () => void): () => void {
 gateListeners.add(listener);
 return () => {
  gateListeners.delete(listener);
 };
}

/**
 * Kiểm soát cooldown + dedupe trước khi enqueue prefetch.
 */
export function shouldRunPrefetch(taskKey: string, cooldownMs: number): boolean {
 syncRouteChangeGateFromBrowserPath();

 if (pendingPrefetches.has(taskKey) || inFlightPrefetches.has(taskKey)) {
  return false;
 }

 const lastRun = lastPrefetchTimestamps.get(taskKey);
 if (!lastRun) {
  return true;
 }

 return Date.now() - lastRun >= cooldownMs;
}

/**
 * Enqueue prefetch theo prefetch gate hiện tại (không block UI).
 */
export function schedulePrefetch(taskKey: string, task: () => Promise<void> | void): void {
 syncRouteChangeGateFromBrowserPath();

 if (pendingPrefetches.has(taskKey) || inFlightPrefetches.has(taskKey)) {
  return;
 }

 const runTask = async () => {
  pendingPrefetches.delete(taskKey);
  inFlightPrefetches.add(taskKey);

  try {
   await task();
  } finally {
   lastPrefetchTimestamps.set(taskKey, Date.now());
   inFlightPrefetches.delete(taskKey);
  }
 };

 if (typeof window === 'undefined') {
  void runTask();
  return;
 }

 const delayMs = Math.max(0, prefetchReadyAt - Date.now());
 const timeoutId = window.setTimeout(() => {
  void runTask();
 }, delayMs);

 pendingPrefetches.set(taskKey, timeoutId);
}
