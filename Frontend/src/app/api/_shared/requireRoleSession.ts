import { NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import {
 getServerAccessToken,
 getServerSessionSnapshot,
} from '@/shared/infrastructure/auth/serverAuth';
import type { UserProfile } from '@/features/auth/session/types';

function normalizeRole(role: string | undefined): string {
 return role?.trim().toLowerCase() ?? '';
}

export interface RequiredRoleSession {
 token: string;
 user: UserProfile;
}

export async function requireRoleSession(role: string): Promise<RequiredRoleSession | NextResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 }

 const session = await getServerSessionSnapshot({ allowRefresh: false });
 if (!session.authenticated || !session.user) {
  return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 }

 if (normalizeRole(session.user.role) !== normalizeRole(role)) {
  return buildProblemResponse(403, 'Forbidden');
 }

 return {
  token,
  user: session.user,
 };
}

export function requireAdminSession(): Promise<RequiredRoleSession | NextResponse> {
 return requireRoleSession('admin');
}
