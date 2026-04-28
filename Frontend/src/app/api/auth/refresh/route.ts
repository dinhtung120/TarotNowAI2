import { NextRequest, NextResponse } from 'next/server';
import { executeRefreshRoute } from '@/app/api/auth/refresh/refreshRouteHandler';

export async function POST(request: NextRequest): Promise<NextResponse> {
 return executeRefreshRoute(request);
}
