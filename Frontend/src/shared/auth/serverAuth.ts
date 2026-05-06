import { cookies } from 'next/headers';
import type { UserProfile } from '@/features/auth/session/types';
import { AUTH_COOKIE, AUTH_SESSION } from '@/shared/auth/authConstants';
import { AUTH_ERROR } from '@/shared/models/authErrors';
import { serverHttpRequest } from '@/shared/http/serverHttpClient';
import { verifyAccessToken } from '@/shared/server/auth/accessTokenVerifier';
import { AUTH_VERIFIER_POLICY, resolveAuthVerificationPolicy } from '@/shared/server/auth/authVerifierPolicy';

const ROLE_CLAIM_KEY = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
const SUBJECT_CLAIM_KEY = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
const DEFAULT_USER_ROLE = 'user';
const DEFAULT_USER_STATUS: UserProfile['status'] = 'Active';

type JwtPayload = Record<string, unknown> & { exp?: number | string };

function decodeJwtPart(part: string): Record<string, unknown> | null {
 if (!part) {
  return null;
 }

 try {
  const normalized = part.replace(/-/g, '+').replace(/_/g, '/');
  const padded = normalized.padEnd(normalized.length + ((4 - (normalized.length % 4)) % 4), '=');
  const decoded = typeof atob === 'function'
   ? atob(padded)
   : Buffer.from(padded, 'base64').toString('utf8');
  return JSON.parse(decoded) as Record<string, unknown>;
 } catch {
  return null;
 }
}

function parseJwtPayload(token: string | undefined): JwtPayload | null {
 if (!token) {
  return null;
 }

 const parts = token.split('.');
 if (parts.length !== 3 || !parts[1]) {
  return null;
 }

 return decodeJwtPart(parts[1]) as JwtPayload | null;
}

function resolveNumericExp(payload: JwtPayload | null | undefined): number | undefined {
 const rawExp = payload?.exp;
 if (typeof rawExp === 'number' && Number.isFinite(rawExp)) {
  return rawExp;
 }

 if (typeof rawExp === 'string' && rawExp.trim().length > 0) {
  const parsed = Number(rawExp);
  if (Number.isFinite(parsed)) {
   return parsed;
  }
 }

 return undefined;
}

function isExpiringSoon(payload: JwtPayload | null | undefined): boolean {
 const exp = resolveNumericExp(payload);
 if (!exp) {
  return true;
 }

 const now = Math.floor(Date.now() / 1000);
 return exp <= now + Math.max(0, AUTH_SESSION.ACCESS_REFRESH_THRESHOLD_SECONDS);
}

async function getValidAccessTokenFromCookie(): Promise<string | undefined> {
 const cookieStore = await cookies();
 const accessToken = cookieStore.get(AUTH_COOKIE.ACCESS)?.value;
 if (!accessToken) {
  return undefined;
 }

 const verification = await verifyAccessToken(accessToken);
 if (verification.valid) {
  if (isExpiringSoon(verification.payload as JwtPayload | null | undefined)) {
   return undefined;
  }
  return accessToken;
 }

 if (verification.reason === 'missing_verifier_config') {
  if (resolveAuthVerificationPolicy() === AUTH_VERIFIER_POLICY.FAIL_OPEN) {
   // Fallback by decoding payload when verifier config is unavailable in frontend runtime.
   // Sensitive authorization is still enforced by backend /profile response.
   const unverifiedPayload = parseJwtPayload(accessToken);
   if (isExpiringSoon(unverifiedPayload)) {
    return undefined;
   }
   return accessToken;
  }

  return undefined;
 }

 return undefined;
}

function toStringValue(value: unknown): string {
 return typeof value === 'string' ? value : '';
}

function toNumberValue(value: unknown): number {
 if (typeof value === 'number' && Number.isFinite(value)) {
  return value;
 }

 if (typeof value === 'string') {
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : 0;
 }

 return 0;
}

function resolveRoleFromPayload(payload: JwtPayload | null): string | undefined {
 if (!payload) {
  return undefined;
 }

 const directRole = payload.role;
 if (typeof directRole === 'string' && directRole.trim().length > 0) {
  return directRole.trim();
 }

 const claimRole = payload[ROLE_CLAIM_KEY];
 if (typeof claimRole === 'string' && claimRole.trim().length > 0) {
  return claimRole.trim();
 }

 if (Array.isArray(claimRole)) {
  const firstRole = claimRole.find((value) => typeof value === 'string' && value.trim().length > 0);
  if (firstRole) {
   return firstRole;
  }
 }

 return undefined;
}

function normalizeStatus(status: unknown): UserProfile['status'] {
 const normalizedStatus = toStringValue(status).trim().toLowerCase();
 if (normalizedStatus === 'pending') return 'Pending';
 if (normalizedStatus === 'suspended') return 'Suspended';
 if (normalizedStatus === 'banned') return 'Banned';
 if (normalizedStatus === 'active') return 'Active';
 return DEFAULT_USER_STATUS;
}

function normalizeProfileWithTokenRole(
 profile: unknown,
 accessToken: string,
): UserProfile | null {
 const source = (profile && typeof profile === 'object') ? profile as Record<string, unknown> : null;
 if (!source) {
  return null;
 }

 const payload = parseJwtPayload(accessToken);
 const roleFromJwt = resolveRoleFromPayload(payload) ?? DEFAULT_USER_ROLE;
 const subjectFromJwt = toStringValue(payload?.sub ?? payload?.[SUBJECT_CLAIM_KEY]);
 const id = toStringValue(source.id) || subjectFromJwt;
 const email = toStringValue(source.email);
 const username = toStringValue(source.username);
 const displayName = toStringValue(source.displayName) || username;
 if (!id || !email || !username || !displayName) {
  return null;
 }

 const normalized = {
  ...source,
  id,
  email,
  username,
  displayName,
  avatarUrl: source.avatarUrl === null ? null : toStringValue(source.avatarUrl) || null,
  level: toNumberValue(source.level),
  exp: toNumberValue(source.exp),
  role: toStringValue(source.role) || roleFromJwt,
  status: normalizeStatus(source.status),
 } as UserProfile;
 return normalized;
}

interface ServerAuthTokenOptions {
 allowRefresh?: boolean;
}

export async function getServerAccessTokenOrRefresh(
 _options: ServerAuthTokenOptions = {},
): Promise<string | undefined> {
 void _options;
 /*
  * Do not perform hidden refresh here.
  * Server-side contexts cannot reliably persist refresh-token rotation cookies.
  * Refresh flow must go through /api/auth/refresh where cookies are explicitly set.
  */
 return getValidAccessTokenFromCookie();
}

export async function getServerAccessToken(): Promise<string | undefined> {
 return getServerAccessTokenOrRefresh();
}

export interface ServerSessionSnapshot {
 authenticated: boolean;
 user: UserProfile | null;
}

interface ServerSessionOptions {
 allowRefresh?: boolean;
}

async function resolveProfileFromAccessToken(accessToken: string): Promise<UserProfile | null> {
 const profile = await serverHttpRequest<unknown>('/profile', {
  method: 'GET',
  token: accessToken,
  cache: 'no-store',
  fallbackErrorMessage: AUTH_ERROR.UNAUTHORIZED,
 });
 if (!profile.ok) {
  return null;
 }

 return normalizeProfileWithTokenRole(profile.data, accessToken);
}

export async function getServerSessionSnapshot(
 _options: ServerSessionOptions = {},
): Promise<ServerSessionSnapshot> {
 void _options;
 const accessToken = await getServerAccessTokenOrRefresh({ allowRefresh: false });
 if (!accessToken) {
  return { authenticated: false, user: null };
 }

 const normalizedProfile = await resolveProfileFromAccessToken(accessToken);
 if (!normalizedProfile) {
  return { authenticated: false, user: null };
 }

 return { authenticated: true, user: normalizedProfile };
}
