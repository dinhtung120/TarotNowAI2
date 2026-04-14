import { NextRequest, NextResponse } from 'next/server';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { getServerSessionSnapshot } from '@/shared/infrastructure/auth/serverAuth';

export async function GET(request: NextRequest): Promise<NextResponse> {
 const session = await getServerSessionSnapshot();
 if (!session.authenticated || !session.user) {
  return NextResponse.json(
   {
    success: false,
    authenticated: false,
    error: AUTH_ERROR.UNAUTHORIZED,
   },
   { status: 401 },
  );
 }

 return NextResponse.json(
  {
   success: true,
   authenticated: true,
   user: session.user,
  },
  { status: 200 },
 );
}
