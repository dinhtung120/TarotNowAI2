import { NextResponse } from 'next/server';
import { getInitialMetadata } from '@/app/_shared/actions/metadata';
import { AUTH_ERROR } from '@/shared/models/authErrors';

export async function GET() {
  const result = await getInitialMetadata();

  if (!result.success) {
    const statusCode = result.error === AUTH_ERROR.UNAUTHORIZED ? 401 : 500;
    return NextResponse.json(result, { status: statusCode });
  }

  return NextResponse.json(result, { status: 200 });
}
