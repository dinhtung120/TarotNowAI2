const SIMPLE_EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

function normalize(value: string | null | undefined): string {
  return value?.trim() ?? '';
}

export function isValidAuthFlowEmail(value: string): boolean {
  return SIMPLE_EMAIL_REGEX.test(normalize(value));
}

export function resolveVerifyEmailPrefill(rawEmail: string | null): { email: string; isReadonly: boolean } {
  const normalizedEmail = normalize(rawEmail);
  if (!isValidAuthFlowEmail(normalizedEmail)) {
    return { email: '', isReadonly: false };
  }

  return { email: normalizedEmail, isReadonly: true };
}

export function resolveLoginIdentityPrefill(
  rawEmailQuery: string | null,
  rememberedIdentity: string | null
): string {
  const normalizedQueryEmail = normalize(rawEmailQuery);
  if (isValidAuthFlowEmail(normalizedQueryEmail)) {
    return normalizedQueryEmail;
  }

  return normalize(rememberedIdentity);
}
