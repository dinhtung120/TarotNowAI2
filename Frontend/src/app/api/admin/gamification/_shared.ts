import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';

export async function requireAdminAccessToken() {
 const token = await getServerAccessToken();
 if (!token) {
  return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 }

 return token;
}
