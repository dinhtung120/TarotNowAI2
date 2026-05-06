import { NextRequest, NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { requireAdminSession } from '@/app/api/admin/gamification/_shared';
import type { AdminTitleDefinition } from '@/features/admin/gamification/types/adminGamification.types';
import { serverHttpRequest } from '@/shared/http/serverHttpClient';

export async function GET(): Promise<NextResponse> {
 const sessionOrResponse = await requireAdminSession();
 if (sessionOrResponse instanceof NextResponse) {
  return sessionOrResponse;
 }

 const result = await serverHttpRequest<AdminTitleDefinition[]>('/admin/gamification/titles', {
  method: 'GET',
  token: sessionOrResponse.token,
  fallbackErrorMessage: 'Failed to load admin titles.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data ?? [], { status: 200 });
}

export async function POST(request: NextRequest): Promise<NextResponse> {
 const sessionOrResponse = await requireAdminSession();
 if (sessionOrResponse instanceof NextResponse) {
  return sessionOrResponse;
 }

 let payload: AdminTitleDefinition;
 try {
  payload = (await request.json()) as AdminTitleDefinition;
 } catch {
  return buildProblemResponse(400, 'Invalid request payload.');
 }

 const result = await serverHttpRequest<{ message: string }>('/admin/gamification/titles', {
  method: 'POST',
  token: sessionOrResponse.token,
  json: payload,
  fallbackErrorMessage: 'Failed to save admin title.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data ?? { message: 'Saved.' }, { status: 200 });
}
