import { NextRequest, NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { AUTH_ERROR } from '@/shared/models/authErrors';
import type { InitReadingRequest, InitReadingResponse } from '@/features/reading/shared/actions/types';
import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { invokeDomainCommand } from '@/shared/gateways/domainCommandRegistry';

export async function POST(request: NextRequest): Promise<NextResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 }

 let payload: InitReadingRequest;
 try {
  payload = (await request.json()) as InitReadingRequest;
 } catch {
  return buildProblemResponse(400, 'Invalid request payload.');
 }

 const result = await invokeDomainCommand<InitReadingResponse>('reading.session.init', {
  path: '/reading/init',
  token,
  json: payload,
  fallbackErrorMessage: 'Failed to initialize reading session.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data, { status: 200 });
}
