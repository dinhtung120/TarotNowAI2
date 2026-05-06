import { AUTH_ERROR } from '@/shared/models/authErrors';

function pickErrorMessage(payload: unknown): string | undefined {
  if (!payload || typeof payload !== 'object') return undefined;

  const errorCode = (payload as Record<string, unknown>).errorCode;
  if (typeof errorCode === 'string' && errorCode.trim()) return errorCode;

  const candidates = ['message', 'error', 'detail', 'title'] as const;
  for (const key of candidates) {
    const value = (payload as Record<string, unknown>)[key];
    if (typeof value === 'string' && value.trim()) return value;
  }

  const errors = (payload as Record<string, unknown>).errors;
  if (Array.isArray(errors) && errors.length > 0) {
    const first = errors[0];
    if (typeof first === 'string' && first.trim()) return first;
  }

  return undefined;
}

export async function parseApiError(
  response: Response,
  fallbackMessage = `Request failed (${response.status})`
): Promise<string> {
  try {
    const data = (await response.clone().json()) as unknown;
    const payloadMessage = pickErrorMessage(data);
    if (payloadMessage) return payloadMessage;
  } catch {
  }

  if (response.status === 401) {
    return AUTH_ERROR.UNAUTHORIZED;
  }

  if (response.status === 429) {
    return AUTH_ERROR.RATE_LIMITED;
  }

  try {
    const text = await response.text();
    if (text.trim()) return text;
  } catch {
  }

  return fallbackMessage;
}
