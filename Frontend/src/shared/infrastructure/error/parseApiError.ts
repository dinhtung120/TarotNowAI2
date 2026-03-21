function pickErrorMessage(payload: unknown): string | undefined {
  if (!payload || typeof payload !== 'object') return undefined;

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
    const message = pickErrorMessage(data);
    if (message) return message;
  } catch {
    // Ignore JSON parse errors and continue with text parsing.
  }

  try {
    const text = await response.text();
    if (text.trim()) return text;
  } catch {
    // Ignore text parsing errors and use fallback.
  }

  return fallbackMessage;
}
