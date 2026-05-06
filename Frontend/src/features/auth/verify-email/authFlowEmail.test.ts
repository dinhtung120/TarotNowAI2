import { describe, expect, it } from 'vitest';
import {
  isValidAuthFlowEmail,
  resolveLoginIdentityPrefill,
  resolveVerifyEmailPrefill,
} from '@/features/auth/verify-email/authFlowEmail';

describe('authFlowEmail helpers', () => {
  it('detects valid email format for auth flow query', () => {
    expect(isValidAuthFlowEmail('reader@example.com')).toBe(true);
    expect(isValidAuthFlowEmail('bad-email')).toBe(false);
    expect(isValidAuthFlowEmail('')).toBe(false);
  });

  it('prefills verify-email and locks field when query email is valid', () => {
    expect(resolveVerifyEmailPrefill('reader@example.com')).toEqual({
      email: 'reader@example.com',
      isReadonly: true,
    });
  });

  it('falls back to manual input on verify-email when query is missing or invalid', () => {
    expect(resolveVerifyEmailPrefill(null)).toEqual({
      email: '',
      isReadonly: false,
    });

    expect(resolveVerifyEmailPrefill('not-an-email')).toEqual({
      email: '',
      isReadonly: false,
    });
  });

  it('prefers query email over remembered identity on login', () => {
    const resolved = resolveLoginIdentityPrefill('query@example.com', 'remembered@example.com');
    expect(resolved).toBe('query@example.com');
  });

  it('uses remembered identity on login when query is missing or invalid', () => {
    expect(resolveLoginIdentityPrefill(null, 'remembered@example.com')).toBe('remembered@example.com');
    expect(resolveLoginIdentityPrefill('invalid-email', 'remembered@example.com')).toBe('remembered@example.com');
  });
});
