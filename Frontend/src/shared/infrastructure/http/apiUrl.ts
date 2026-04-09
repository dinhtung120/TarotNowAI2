const API_VERSION_PATH = '/api/v1';
const API_BASE_URL_ENV_NAME = 'NEXT_PUBLIC_API_URL';

function resolveApiBaseUrl(value: string | undefined): string {
 return normalizeApiBaseUrl(requireEnvValue(value, API_BASE_URL_ENV_NAME));
}

export function resolveApiOrigin(value: string | undefined): string {
 return apiOriginFromBase(resolveApiBaseUrl(value));
}

function normalizeApiBaseUrl(value: string): string {
 const trimmed = value.trim().replace(/\/+$/, '');
 if (!trimmed) {
  throw new Error(`${API_BASE_URL_ENV_NAME} must not be empty.`);
 }

 if (trimmed.endsWith(API_VERSION_PATH)) return trimmed;
 if (trimmed.endsWith('/api')) return `${trimmed}/v1`;
 return `${trimmed}${API_VERSION_PATH}`;
}

function requireEnvValue(value: string | undefined, envName: string): string {
 const trimmed = value?.trim();
 if (!trimmed) {
  throw new Error(`Missing required environment variable ${envName}. Configure it in Frontend/.env.local.`);
 }

 return trimmed;
}

function apiOriginFromBase(baseUrl: string): string {
 const trimmed = baseUrl.trim().replace(/\/+$/, '');
 if (trimmed.endsWith(API_VERSION_PATH)) return trimmed.slice(0, -API_VERSION_PATH.length);
 return trimmed;
}

const rawApiBaseUrl = requireEnvValue(process.env.NEXT_PUBLIC_API_URL, API_BASE_URL_ENV_NAME);

export const API_BASE_URL = resolveApiBaseUrl(rawApiBaseUrl);
export const API_ORIGIN = resolveApiOrigin(rawApiBaseUrl);

export function apiUrl(path: string): string {
 const base = API_BASE_URL.replace(/\/+$/, '');
 if (!path) return base;
 if (/^https?:\/\//i.test(path)) return path;
 return `${base}${path.startsWith('/') ? path : `/${path}`}`;
}
