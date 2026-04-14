import { NextRequest, NextResponse } from 'next/server';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { clearAuthCookies } from '@/app/api/auth/_shared';

export async function POST(request: NextRequest): Promise<NextResponse> {
 const result = await serverHttpRequest<Record<string, unknown>>('/auth/logout', {
  method: 'POST',
  headers: {
   Cookie: request.headers.get('cookie') ?? '',
  },
  cache: 'no-store',
  fallbackErrorMessage: AUTH_ERROR.UNAUTHORIZED,
 });

 const response = NextResponse.json(
  result.ok ? { success: true } : { success: false, error: result.error },
  { status: result.ok ? 200 : result.status },
 );
 clearAuthCookies(response);
 return response;
}
