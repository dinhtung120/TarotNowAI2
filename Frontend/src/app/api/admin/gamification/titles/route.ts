import { NextRequest, NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { requireAdminAccessToken } from '@/app/api/admin/gamification/_shared';
import type { AdminTitleDefinition } from '@/features/gamification/admin/application/adminGamification.types';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

export async function GET(): Promise<NextResponse> {
 const tokenOrResponse = await requireAdminAccessToken();
 if (tokenOrResponse instanceof NextResponse) {
  return tokenOrResponse;
 }

 const result = await serverHttpRequest<AdminTitleDefinition[]>('/admin/gamification/titles', {
  method: 'GET',
  token: tokenOrResponse,
  fallbackErrorMessage: 'Failed to load admin titles.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data ?? [], { status: 200 });
}

export async function POST(request: NextRequest): Promise<NextResponse> {
 const tokenOrResponse = await requireAdminAccessToken();
 if (tokenOrResponse instanceof NextResponse) {
  return tokenOrResponse;
 }

 let payload: AdminTitleDefinition;
 try {
  payload = (await request.json()) as AdminTitleDefinition;
 } catch {
  return buildProblemResponse(400, 'Invalid request payload.');
 }

 const result = await serverHttpRequest<{ message: string }>('/admin/gamification/titles', {
  method: 'POST',
  token: tokenOrResponse,
  json: payload,
  fallbackErrorMessage: 'Failed to save admin title.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data ?? { message: 'Saved.' }, { status: 200 });
}
