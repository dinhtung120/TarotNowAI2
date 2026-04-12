/** Tiền tố REST v1; dùng cho URL tương đối cùng origin (nginx → backend). */
export const API_V1_PREFIX = '/api/v1';
const API_VERSION_PATH = API_V1_PREFIX;
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
 * Hàm hỗ trợ cho next.config.ts và các script build-time.
 */
export function resolveApiOrigin(value: string | undefined): string {
  const base = normalizeApiBaseUrl(value || '', API_BASE_URL_ENV_NAME);
  return apiOriginFromBase(base);
}

/**
 * Origin backend cho Next.js rewrites (uploads, chat, presence) khi chạy trong Docker.
 * Dùng INTERNAL_API_URL để tránh proxy ra URL công khai (hairpin / ECONNRESET qua Cloudflare).
 */
export function resolveRewriteBackendOrigin(): string {
  const internal = process.env.INTERNAL_API_URL?.trim();
  if (internal) {
    try {
      return new URL(internal).origin;
    } catch {
      /* fallback public */
    }
  }
  return resolveApiOrigin(process.env.NEXT_PUBLIC_API_URL);
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

/**
 * Đường dẫn API v1 trên cùng host với trang (ví dụ /api/v1/profile).
 */
export function browserApiPath(pathFromV1Root: string): string {
  const rel = pathFromV1Root.startsWith('/') ? pathFromV1Root : `/${pathFromV1Root}`;
  return `${API_V1_PREFIX}${rel}`;
}

// Giữ lại các export cũ để tránh lỗi compile, nhưng sẽ refactor dần.
export const getApiBaseUrl = getPublicApiBaseUrl;
export const getApiOrigin = getPublicApiOrigin;
