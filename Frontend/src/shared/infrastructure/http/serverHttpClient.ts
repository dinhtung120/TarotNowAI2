import { internalApiUrl } from '@/shared/infrastructure/http/apiUrl';
import { parseApiError } from '@/shared/infrastructure/error/parseApiError';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';
import { getRuntimePolicyStoreSnapshot } from '@/shared/config/runtimePolicyStore';

interface ServerHttpResultOk<T> {
  ok: true;
  status: number;
  headers: Headers;
  data: T;
}

interface ServerHttpResultErr {
  ok: false;
  status: number;
  headers: Headers;
  error: string;
}

type ServerHttpResult<T> = ServerHttpResultOk<T> | ServerHttpResultErr;

interface ServerHttpRequestOptions extends Omit<RequestInit, 'body' | 'headers'> {
  token?: string;
  headers?: HeadersInit;
  json?: unknown;
  formData?: FormData;
  fallbackErrorMessage?: string;
  timeoutMs?: number;
}

const DEFAULT_SERVER_TIMEOUT_MS = RUNTIME_POLICY_FALLBACKS.http.serverTimeoutMs;
const MIN_SERVER_TIMEOUT_MS = RUNTIME_POLICY_FALLBACKS.http.minTimeoutMs;

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

async function fetchWithTimeout(url: string, init: RequestInit, timeoutMs: number): Promise<Response> {
  const controller = new AbortController();
  const timeoutHandle = setTimeout(() => controller.abort(), timeoutMs);
  try {
    return await fetch(url, { ...init, signal: controller.signal });
  } finally {
    clearTimeout(timeoutHandle);
  }
}

function resolveTimeout(timeoutMs: number | undefined): number {
  const runtimePolicy = getRuntimePolicyStoreSnapshot();
  const defaultTimeoutMs = runtimePolicy.http.serverTimeoutMs || DEFAULT_SERVER_TIMEOUT_MS;
  const minTimeoutMs = runtimePolicy.http.minTimeoutMs || MIN_SERVER_TIMEOUT_MS;

  if (!timeoutMs || !Number.isFinite(timeoutMs)) {
    return defaultTimeoutMs;
  }

  return Math.max(minTimeoutMs, Math.floor(timeoutMs));
}

function isAbortError(error: unknown): boolean {
  if (error instanceof DOMException) {
    return error.name === 'AbortError';
  }

  if (error instanceof Error) {
    return error.name === 'AbortError';
  }

  return false;
}

export async function serverHttpRequest<T>(
  path: string,
  options: ServerHttpRequestOptions = {}
): Promise<ServerHttpResult<T>> {
  const { token, headers: rawHeaders, json, formData, fallbackErrorMessage, timeoutMs, ...rest } = options;
  const resolvedCache = rest.cache ?? (hasNextRevalidate(rest.next) ? undefined : 'no-store');
  const resolvedTimeoutMs = resolveTimeout(timeoutMs);
  
  let body: BodyInit | undefined;
  if (formData !== undefined) {
    body = formData;
  } else if (json !== undefined) {
    body = JSON.stringify(json);
  }

  let response: Response;
  try {
    response = await fetchWithTimeout(
      internalApiUrl(path),
      {
        ...rest,
        ...(resolvedCache ? { cache: resolvedCache } : {}),
        headers: buildHeaders(token, rawHeaders, json !== undefined),
        body,
      },
      resolvedTimeoutMs,
    );
  } catch (error) {
    if (isAbortError(error)) {
      return {
        ok: false,
        status: 504,
        headers: new Headers(),
        error: fallbackErrorMessage ?? 'Upstream request timed out.',
      };
    }

    return {
      ok: false,
      status: 503,
      headers: new Headers(),
      error: fallbackErrorMessage ?? 'Upstream request failed.',
    };
  }

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
