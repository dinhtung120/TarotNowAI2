import { decodeProtectedHeader, jwtVerify, type JWTPayload } from 'jose';

const ACCESS_TOKEN_ALGORITHM = 'HS256';
const CLOCK_SKEW_SECONDS = 5;

export interface AccessTokenVerificationResult {
 valid: boolean;
 payload: JWTPayload | null;
 reason:
  | 'verified'
  | 'missing_token'
  | 'malformed_token'
  | 'unsupported_algorithm'
  | 'missing_verifier_config'
  | 'invalid_token';
}

interface JwtVerifierConfig {
 secretKey: Uint8Array;
 issuer?: string;
 audience?: string;
}

function resolveJwtVerifierConfig(): JwtVerifierConfig | null {
 const rawSecret = process.env.JWT_SECRETKEY?.trim() ?? '';
 if (!rawSecret) {
  return null;
 }

 const issuer = process.env.JWT_ISSUER?.trim() || undefined;
 const audience = process.env.JWT_AUDIENCE?.trim() || undefined;

 return {
  secretKey: new TextEncoder().encode(rawSecret),
  issuer,
  audience,
 };
}

function hasValidStructure(token: string): boolean {
 const parts = token.split('.');
 return parts.length === 3 && parts.every((part) => part.trim().length > 0);
}

export async function verifyAccessToken(
 token: string | undefined,
): Promise<AccessTokenVerificationResult> {
 if (!token || token.trim().length === 0) {
  return { valid: false, payload: null, reason: 'missing_token' };
 }

 if (!hasValidStructure(token)) {
  return { valid: false, payload: null, reason: 'malformed_token' };
 }

 try {
  const header = decodeProtectedHeader(token);
  if (header.alg !== ACCESS_TOKEN_ALGORITHM) {
   return { valid: false, payload: null, reason: 'unsupported_algorithm' };
  }
 } catch {
  return { valid: false, payload: null, reason: 'malformed_token' };
 }

 const verifierConfig = resolveJwtVerifierConfig();
 if (!verifierConfig) {
  return { valid: false, payload: null, reason: 'missing_verifier_config' };
 }

 try {
  const { payload } = await jwtVerify(token, verifierConfig.secretKey, {
   algorithms: [ACCESS_TOKEN_ALGORITHM],
   issuer: verifierConfig.issuer,
   audience: verifierConfig.audience,
   clockTolerance: CLOCK_SKEW_SECONDS,
  });
  return { valid: true, payload, reason: 'verified' };
 } catch {
  return { valid: false, payload: null, reason: 'invalid_token' };
 }
}
