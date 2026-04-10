const API_VERSION_PATH = '/api/v1';
const API_BASE_URL_ENV_NAME = 'NEXT_PUBLIC_API_URL';
const INTERNAL_API_BASE_URL_ENV_NAME = 'INTERNAL_API_URL';

/**
 * Chuẩn hóa URL API để đảm bảo luôn có suffix /api/v1.
 * Luồng: Xóa slash cuối -> kiểm tra suffix -> bổ sung nếu thiếu.
 */
function normalizeApiBaseUrl(value: string | undefined, envName: string): string {
  const trimmed = value?.trim().replace(/\/+$/, '');
  if (!trimmed) {
    throw new Error(`Missing required environment variable ${envName}.`);
  }

  if (trimmed.endsWith(API_VERSION_PATH)) return trimmed;
  if (trimmed.endsWith('/api')) return `${trimmed}/v1`;
  return `${trimmed}${API_VERSION_PATH}`;
}

/**
 * Lấy Origin từ Base URL (xóa phần /api/v1).
 */
function apiOriginFromBase(baseUrl: string): string {
  const trimmed = baseUrl.trim().replace(/\/+$/, '');
  if (trimmed.endsWith(API_VERSION_PATH)) return trimmed.slice(0, -API_VERSION_PATH.length).replace(/\/+$/, '');
  return trimmed;
}

/**
 * URL API CÔNG KHAI: Dùng cho Browser và Render giao diện (để tránh Hydration Mismatch).
 * Luôn sử dụng NEXT_PUBLIC_API_URL.
 */
export function getPublicApiBaseUrl(): string {
  return normalizeApiBaseUrl(process.env.NEXT_PUBLIC_API_URL, API_BASE_URL_ENV_NAME);
}

/**
 * ORIGIN API CÔNG KHAI: Dùng cho CSP và SignalR trên Client.
 */
export function getPublicApiOrigin(): string {
  return apiOriginFromBase(getPublicApiBaseUrl());
}

/**
 * URL API NỘI BỘ: Chỉ dùng trên Server (BFF, Server Actions) để tối ưu tốc độ gọi Backend.
 * Ưu tiên INTERNAL_API_URL, fallback về public nếu thiếu.
 */
export function getInternalApiBaseUrl(): string {
  const isServer = typeof window === 'undefined';
  if (!isServer) return getPublicApiBaseUrl();

  const internalUrl = process.env.INTERNAL_API_URL || process.env.NEXT_PUBLIC_API_URL;
  return normalizeApiBaseUrl(internalUrl, INTERNAL_API_BASE_URL_ENV_NAME);
}

/**
 * Tạo URL API đầy đủ cho mục đích HIỂN THỊ hoặc gọi từ Browser.
 * Đảm bảo tính nhất quán giữa Server và Client để tránh lỗi Hydration.
 */
export function apiUrl(path: string): string {
  const base = getPublicApiBaseUrl().replace(/\/+$/, '');
  if (!path) return base;
  if (/^https?:\/\//i.test(path)) return path;
  return `${base}${path.startsWith('/') ? path : `/${path}`}`;
}

/**
 * Tạo URL API đầy đủ cho mục đích gọi ngầm từ Server-to-Server.
 * Chỉ sử dụng trong Server Actions hoặc Route Handlers.
 */
export function internalApiUrl(path: string): string {
  const base = getInternalApiBaseUrl().replace(/\/+$/, '');
  if (!path) return base;
  if (/^https?:\/\//i.test(path)) return path;
  return `${base}${path.startsWith('/') ? path : `/${path}`}`;
}

// Giữ lại các export cũ để tránh lỗi compile, nhưng sẽ refactor dần.
export const getApiBaseUrl = getPublicApiBaseUrl;
export const getApiOrigin = getPublicApiOrigin;
