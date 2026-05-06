'use client';

import { parseApiError } from '@/shared/error/parseApiError';
import { browserApiPath, getPublicApiBaseUrl } from '@/shared/http/apiUrl';
import { tryRefreshClientSide } from '@/shared/auth/refreshClient';

function resolveClientApiUrl(pathFromV1Root: string): string {
  const rel = pathFromV1Root.startsWith('/') ? pathFromV1Root : `/${pathFromV1Root}`;

  if (typeof window !== 'undefined') {
    return browserApiPath(rel);
  }

  try {
    const publicBase = getPublicApiBaseUrl().replace(/\/+$/, '');
    return `${publicBase}${rel}`;
  } catch {
    return browserApiPath(rel);
  }
}

export type ClientJsonResult<T> = { ok: true; data: T } | { ok: false; error: string; status: number };

interface ClientJsonOptions {
  fallbackErrorMessage: string;
  unauthorizedMessage?: string;
  method?: 'POST' | 'PUT' | 'PATCH' | 'DELETE';
  timeoutMs?: number;
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

async function requestJsonToApiV1<TResponse, TRequest extends object>(
  pathFromV1Root: string,
  payload: TRequest,
  options: ClientJsonOptions,
): Promise<ClientJsonResult<TResponse>> {
  const url = resolveClientApiUrl(pathFromV1Root);
  const timeoutMs = Math.max(1_000, options.timeoutMs ?? 15_000);
  const requestInit: RequestInit = {
    method: options.method ?? 'POST',
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(payload),
  };
  let response: Response;
  try {
    response = await fetchWithTimeout(url, requestInit, timeoutMs);
  } catch {
    return { ok: false, error: options.fallbackErrorMessage, status: 503 };
  }

  if (response.status === 401) {
    const refreshed = await tryRefreshClientSide();
    if (!refreshed) {
      return {
        ok: false,
        error: options.unauthorizedMessage ?? options.fallbackErrorMessage,
        status: 401,
      };
    }

    try {
      response = await fetchWithTimeout(url, requestInit, timeoutMs);
    } catch {
      return { ok: false, error: options.fallbackErrorMessage, status: 503 };
    }
  }

  if (!response.ok) {
    const error = await parseApiError(response, options.fallbackErrorMessage);
    return { ok: false, error, status: response.status };
  }

  const data = (await response.json()) as TResponse;
  return { ok: true, data };
}

export async function postJsonToApiV1<TResponse, TRequest extends object>(
  pathFromV1Root: string,
  payload: TRequest,
  options: Omit<ClientJsonOptions, 'method'>,
): Promise<ClientJsonResult<TResponse>> {
  return requestJsonToApiV1<TResponse, TRequest>(pathFromV1Root, payload, { ...options, method: 'POST' });
}
