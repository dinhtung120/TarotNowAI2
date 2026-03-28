import { apiUrl } from '@/shared/infrastructure/http/apiUrl';
import { parseApiError } from '@/shared/infrastructure/error/parseApiError';

export interface ServerHttpResultOk<T> {
  ok: true;
  status: number;
  headers: Headers;
  data: T;
}

export interface ServerHttpResultErr {
  ok: false;
  status: number;
  headers: Headers;
  error: string;
}

export type ServerHttpResult<T> = ServerHttpResultOk<T> | ServerHttpResultErr;

export interface ServerHttpRequestOptions extends Omit<RequestInit, 'body' | 'headers'> {
  token?: string;
  headers?: HeadersInit;
  json?: unknown;
  formData?: FormData;
  fallbackErrorMessage?: string;
}

function buildHeaders(
  token?: string,
  rawHeaders?: HeadersInit,
  hasJsonBody?: boolean
): Headers {
  const headers = new Headers(rawHeaders);
  if (token) headers.set('Authorization', `Bearer ${token}`);
  if (hasJsonBody && !headers.has('Content-Type')) {
    headers.set('Content-Type', 'application/json');
  }
  return headers;
}

function isEmptyResponse(response: Response): boolean {
  if (response.status === 204 || response.status === 205) return true;
  const length = response.headers.get('content-length');
  return length === '0';
}

function hasNextRevalidate(nextConfig: RequestInit['next']): boolean {
  if (!nextConfig || typeof nextConfig !== 'object') return false;
  const value = (nextConfig as { revalidate?: unknown }).revalidate;
  return typeof value === 'number';
}

async function parseResponseBody<T>(response: Response): Promise<T> {
  if (isEmptyResponse(response)) {
    return undefined as T;
  }

  const contentType = response.headers.get('content-type') ?? '';
  if (contentType.includes('application/json')) {
    return (await response.json()) as T;
  }

  return (await response.text()) as T;
}

export async function serverHttpRequest<T>(
  path: string,
  options: ServerHttpRequestOptions = {}
): Promise<ServerHttpResult<T>> {
  const { token, headers: rawHeaders, json, formData, fallbackErrorMessage, ...rest } = options;
  const resolvedCache = rest.cache ?? (hasNextRevalidate(rest.next) ? undefined : 'no-store');
  
  // Xác định body: ưu tiên formData nếu có, sau đó đến json
  let body: BodyInit | undefined;
  if (formData !== undefined) {
    body = formData;
  } else if (json !== undefined) {
    body = JSON.stringify(json);
  }

  const response = await fetch(apiUrl(path), {
    ...rest,
    ...(resolvedCache ? { cache: resolvedCache } : {}),
    headers: buildHeaders(token, rawHeaders, json !== undefined),
    body,
  });

  if (!response.ok) {
    const error = await parseApiError(response, fallbackErrorMessage);
    return { ok: false, status: response.status, headers: response.headers, error };
  }

  const data = await parseResponseBody<T>(response);
  return {
    ok: true,
    status: response.status,
    headers: response.headers,
    data,
  };
}
