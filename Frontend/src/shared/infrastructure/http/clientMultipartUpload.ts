'use client';

import { parseApiError } from '@/shared/infrastructure/error/parseApiError';
import { browserApiPath, getPublicApiBaseUrl } from '@/shared/infrastructure/http/apiUrl';
import { useAuthStore } from '@/store/authStore';

/**
 * URL upload multipart từ browser:
 * - Production (NEXT_PUBLIC_API_URL cùng origin với tab): /api/v1/... → nginx → backend.
 * - Dev (API ở host/port khác, ví dụ localhost:5037): URL đầy đủ từ env + CORS.
 * Tránh Server Action + FormData (Next serialize file) và tránh bắt buộc INTERNAL_API_URL cho upload.
 */
function resolveClientMultipartUrl(pathFromV1Root: string): string {
  const rel = pathFromV1Root.startsWith('/') ? pathFromV1Root : `/${pathFromV1Root}`;

  let publicBase: string;
  try {
    publicBase = getPublicApiBaseUrl().replace(/\/+$/, '');
  } catch {
    return browserApiPath(rel);
  }

  if (typeof window !== 'undefined') {
    try {
      const apiOrigin = new URL(publicBase).origin;
      if (apiOrigin === window.location.origin) {
        return browserApiPath(rel);
      }
    } catch {
      /* dùng URL đầy đủ */
    }
    return `${publicBase}${rel}`;
  }

  return browserApiPath(rel);
}

export type ClientMultipartResult<T> = { ok: true; data: T } | { ok: false; error: string; status: number };

export async function postFormDataToApiV1<T>(
  pathFromV1Root: string,
  formData: FormData,
  options: { fallbackErrorMessage: string; unauthorizedMessage?: string },
): Promise<ClientMultipartResult<T>> {
  const token = useAuthStore.getState().token;
  if (!token) {
    return {
      ok: false,
      error: options.unauthorizedMessage ?? options.fallbackErrorMessage,
      status: 401,
    };
  }

  const url = resolveClientMultipartUrl(pathFromV1Root);
  const response = await fetch(url, {
    method: 'POST',
    headers: { Authorization: `Bearer ${token}` },
    body: formData,
  });

  if (!response.ok) {
    const error = await parseApiError(response, options.fallbackErrorMessage);
    return { ok: false, error, status: response.status };
  }

  const data = (await response.json()) as T;
  return { ok: true, data };
}
