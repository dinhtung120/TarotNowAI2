import { cookies, headers } from 'next/headers';
import type { AuthResponse, UserProfile } from '@/features/auth/domain/types';
import { AUTH_COOKIE, AUTH_HEADER, AUTH_SESSION } from '@/shared/infrastructure/auth/authConstants';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

const ROLE_CLAIM_KEY = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
const SUBJECT_CLAIM_KEY = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
const DEFAULT_USER_ROLE = 'user';
const DEFAULT_USER_STATUS: UserProfile['status'] = 'Active';

type JwtPayload = Record<string, unknown> & { exp?: number };

function parseJwtPayload(token: string | undefined): JwtPayload | null {
 if (!token) {
  return null;
 }

 const parts = token.split('.');
 if (parts.length < 2 || !parts[1]) {
  return null;
 }

 try {
  return JSON.parse(Buffer.from(parts[1], 'base64url').toString('utf8')) as JwtPayload;
 } catch {
  return null;
 }
}

function parseJwtExp(token: string | undefined): number | undefined {
 const payload = parseJwtPayload(token);
 return typeof payload?.exp === 'number' ? payload.exp : undefined;
}

function isExpiringSoon(token: string | undefined): boolean {
 const exp = parseJwtExp(token);
 if (!exp) {
  return true;
 }

 const now = Math.floor(Date.now() / 1000);
 return exp <= now + Math.max(0, AUTH_SESSION.ACCESS_REFRESH_THRESHOLD_SECONDS);
}

async function getValidAccessTokenFromCookie(): Promise<string | undefined> {
 const cookieStore = await cookies();
 const accessToken = cookieStore.get(AUTH_COOKIE.ACCESS)?.value;
 if (!accessToken || isExpiringSoon(accessToken)) {
  return undefined;
 }

 return accessToken;
}

function normalizeHeaderValue(value: string | null | undefined): string {
 if (!value) {
  return '';
 }

 const trimmed = value.trim();
 return trimmed.length > 0 ? trimmed : '';
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

async function refreshServerAccessToken(
 refreshToken: string,
 deviceId: string,
 userAgent: string,
): Promise<string | undefined> {
 const result = await serverHttpRequest<AuthResponse>('/auth/refresh', {
  method: 'POST',
  headers: {
   Cookie: `${AUTH_COOKIE.REFRESH}=${refreshToken}`,
   [AUTH_HEADER.IDEMPOTENCY_KEY]: crypto.randomUUID(),
   [AUTH_HEADER.DEVICE_ID]: deviceId,
   [AUTH_HEADER.FORWARDED_USER_AGENT]: userAgent,
  },
  cache: 'no-store',
  fallbackErrorMessage: AUTH_ERROR.UNAUTHORIZED,
 });
 if (!result.ok) {
  return undefined;
 }

 const nextAccessToken = result.data.accessToken;
 if (!nextAccessToken || isExpiringSoon(nextAccessToken)) {
  return undefined;
 }

 return nextAccessToken;
}

interface ServerAuthTokenOptions {
 allowRefresh?: boolean;
}

export async function getServerAccessTokenOrRefresh(
 options: ServerAuthTokenOptions = {},
): Promise<string | undefined> {
 const accessToken = await getValidAccessTokenFromCookie();
 if (accessToken) {
  return accessToken;
 }

 if (options.allowRefresh === false) {
  return undefined;
 }

 const cookieStore = await cookies();
 const refreshToken = cookieStore.get(AUTH_COOKIE.REFRESH)?.value;
 if (!refreshToken) {
  return undefined;
 }

 const headerStore = await headers();
 const forwardedDeviceId = normalizeHeaderValue(headerStore.get(AUTH_HEADER.DEVICE_ID));
 const cookieDeviceId = normalizeHeaderValue(cookieStore.get(AUTH_COOKIE.DEVICE)?.value);
 const resolvedDeviceId = cookieDeviceId || forwardedDeviceId || 'server-auth';
 const userAgent = normalizeHeaderValue(headerStore.get('user-agent'));
 return refreshServerAccessToken(refreshToken, resolvedDeviceId, userAgent);
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

export async function getServerSessionSnapshot(
 options: ServerSessionOptions = {},
): Promise<ServerSessionSnapshot> {
 const accessToken = await getServerAccessTokenOrRefresh(options);
 if (!accessToken) {
  return { authenticated: false, user: null };
 }

 const profile = await serverHttpRequest<unknown>('/profile', {
  method: 'GET',
  token: accessToken,
  cache: 'no-store',
  fallbackErrorMessage: AUTH_ERROR.UNAUTHORIZED,
 });
 if (!profile.ok) {
  return { authenticated: false, user: null };
 }

 const normalizedProfile = normalizeProfileWithTokenRole(profile.data, accessToken);
 if (!normalizedProfile) {
  return { authenticated: false, user: null };
 }

 return { authenticated: true, user: normalizedProfile };
}
