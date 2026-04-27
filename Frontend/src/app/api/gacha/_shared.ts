import { NextResponse } from 'next/server';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
export { buildProblemResponse };

export async function requireServerAccessToken(): Promise<string | NextResponse> {
 const token = await getServerAccessToken();
 if (token) {
  return token;
 }

 return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
}
