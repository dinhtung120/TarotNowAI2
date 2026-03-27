const DEFAULT_API_ORIGIN = 'http://127.0.0.1:5037';
const API_VERSION_PATH = '/api/v1';

export function resolveApiBaseUrl(value: string | undefined): string {
 const rawValue = value ?? `${DEFAULT_API_ORIGIN}${API_VERSION_PATH}`;
 return normalizeApiBaseUrl(rawValue);
}

export function resolveApiOrigin(value: string | undefined): string {
 return apiOriginFromBase(resolveApiBaseUrl(value));
}

function normalizeApiBaseUrl(value: string): string {
 const trimmed = value.trim().replace(/\/+$/, '');
 if (!trimmed) return `${DEFAULT_API_ORIGIN}${API_VERSION_PATH}`;

 if (trimmed.endsWith(API_VERSION_PATH)) return trimmed;
 return `${trimmed}${API_VERSION_PATH}`;
}

function apiOriginFromBase(baseUrl: string): string {
 const trimmed = baseUrl.trim().replace(/\/+$/, '');
 if (trimmed.endsWith(API_VERSION_PATH)) return trimmed.slice(0, -API_VERSION_PATH.length);
 return trimmed;
}

const rawApiBaseUrl =
 process.env.NEXT_PUBLIC_API_URL ?? `${DEFAULT_API_ORIGIN}${API_VERSION_PATH}`;

export const API_BASE_URL = resolveApiBaseUrl(rawApiBaseUrl);
export const API_ORIGIN = resolveApiOrigin(rawApiBaseUrl);

export function apiUrl(path: string): string {
 const base = API_BASE_URL.replace(/\/+$/, '');
 if (!path) return base;
 if (/^https?:\/\//i.test(path)) return path;
 return `${base}${path.startsWith('/') ? path : `/${path}`}`;
}

export function apiOriginUrl(path: string): string {
 const base = API_ORIGIN.replace(/\/+$/, '');
 if (!path) return base;
 if (/^https?:\/\//i.test(path)) return path;
 return `${base}${path.startsWith('/') ? path : `/${path}`}`;
}
