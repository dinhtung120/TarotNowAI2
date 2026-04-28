import { NextRequest, NextResponse } from 'next/server';
import { getSessionRouteResponse } from '@/app/api/auth/session/sessionRouteHandler';

export async function GET(request: NextRequest): Promise<NextResponse> {
 return getSessionRouteResponse(request);
}
