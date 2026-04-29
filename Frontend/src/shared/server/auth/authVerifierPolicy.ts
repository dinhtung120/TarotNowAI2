export const AUTH_VERIFIER_POLICY = {
 FAIL_OPEN: 'fail-open',
 FAIL_CLOSED: 'fail-closed',
} as const;

export type AuthVerifierPolicy =
 (typeof AUTH_VERIFIER_POLICY)[keyof typeof AUTH_VERIFIER_POLICY];

const AUTH_VERIFIER_POLICY_ENV = 'AUTH_VERIFIER_POLICY';

function normalizePolicy(value: string | undefined): AuthVerifierPolicy | null {
 if (!value) {
  return null;
 }

 const normalized = value.trim().toLowerCase();
 if (normalized === AUTH_VERIFIER_POLICY.FAIL_OPEN) {
  return AUTH_VERIFIER_POLICY.FAIL_OPEN;
 }
 if (normalized === AUTH_VERIFIER_POLICY.FAIL_CLOSED) {
  return AUTH_VERIFIER_POLICY.FAIL_CLOSED;
 }

 return null;
}

export function resolveAuthVerificationPolicy(): AuthVerifierPolicy {
 const configuredPolicy = normalizePolicy(process.env[AUTH_VERIFIER_POLICY_ENV]);
 const nodeEnv = process.env.NODE_ENV?.trim().toLowerCase();
 const isProduction = nodeEnv === 'production';

 if (isProduction) {
  return AUTH_VERIFIER_POLICY.FAIL_CLOSED;
 }

 if (configuredPolicy) {
  return configuredPolicy;
 }

 return AUTH_VERIFIER_POLICY.FAIL_OPEN;
}
