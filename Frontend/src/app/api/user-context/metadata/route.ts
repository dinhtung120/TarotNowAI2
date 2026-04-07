import { NextResponse } from 'next/server';
import { getInitialMetadata } from '@/shared/application/actions/metadata';

export async function GET() {
  const result = await getInitialMetadata();

  if (!result.success) {
    const statusCode = result.error === 'Unauthorized' ? 401 : 500;
    return NextResponse.json(result, { status: statusCode });
  }

  return NextResponse.json(result, { status: 200 });
}
