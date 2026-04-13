'use client';

import { parseApiError } from '@/shared/infrastructure/error/parseApiError';
import { browserApiPath, getPublicApiBaseUrl } from '@/shared/infrastructure/http/apiUrl';
import { useAuthStore } from '@/store/authStore';

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
}

async function requestJsonToApiV1<TResponse, TRequest extends object>(
  pathFromV1Root: string,
  payload: TRequest,
  options: ClientJsonOptions,
): Promise<ClientJsonResult<TResponse>> {
  const token = useAuthStore.getState().token;
  if (!token) {
    return {
      ok: false,
      error: options.unauthorizedMessage ?? options.fallbackErrorMessage,
      status: 401,
    };
  }

  const url = resolveClientApiUrl(pathFromV1Root);
  const response = await fetch(url, {
    method: options.method ?? 'POST',
    headers: {
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(payload),
  });

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
