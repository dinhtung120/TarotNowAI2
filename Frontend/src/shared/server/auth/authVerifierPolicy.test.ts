// @vitest-environment node

import { afterEach, describe, expect, it } from 'vitest';
import {
 AUTH_VERIFIER_POLICY,
 resolveAuthVerificationPolicy,
} from '@/shared/server/auth/authVerifierPolicy';

const originalNodeEnv = process.env.NODE_ENV;
const originalVerifierPolicy = process.env.AUTH_VERIFIER_POLICY;

describe('resolveAuthVerificationPolicy', () => {
 afterEach(() => {
  process.env.NODE_ENV = originalNodeEnv;
  if (typeof originalVerifierPolicy === 'string') {
   process.env.AUTH_VERIFIER_POLICY = originalVerifierPolicy;
  } else {
   delete process.env.AUTH_VERIFIER_POLICY;
  }
 });

 it('always resolves fail-closed in production', () => {
  process.env.NODE_ENV = 'production';
  process.env.AUTH_VERIFIER_POLICY = AUTH_VERIFIER_POLICY.FAIL_OPEN;

  expect(resolveAuthVerificationPolicy()).toBe(AUTH_VERIFIER_POLICY.FAIL_CLOSED);
 });

 it('defaults to fail-open in non-production without explicit override', () => {
  process.env.NODE_ENV = 'development';
  delete process.env.AUTH_VERIFIER_POLICY;

  expect(resolveAuthVerificationPolicy()).toBe(AUTH_VERIFIER_POLICY.FAIL_OPEN);
 });

 it('supports explicit non-production override to fail-closed', () => {
  process.env.NODE_ENV = 'test';
  process.env.AUTH_VERIFIER_POLICY = AUTH_VERIFIER_POLICY.FAIL_CLOSED;

  expect(resolveAuthVerificationPolicy()).toBe(AUTH_VERIFIER_POLICY.FAIL_CLOSED);
 });
});
