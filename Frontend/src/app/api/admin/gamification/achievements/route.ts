import { NextRequest, NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { requireAdminSession } from '@/app/api/admin/gamification/_shared';
import type { AdminAchievementDefinition } from '@/features/admin/gamification/types/adminGamification.types';
import { serverHttpRequest } from '@/shared/http/serverHttpClient';

export async function GET(): Promise<NextResponse> {
 const sessionOrResponse = await requireAdminSession();
 if (sessionOrResponse instanceof NextResponse) {
  return sessionOrResponse;
 }

 const result = await serverHttpRequest<AdminAchievementDefinition[]>('/admin/gamification/achievements', {
  method: 'GET',
  token: sessionOrResponse.token,
  fallbackErrorMessage: 'Failed to load admin achievements.',
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

 let payload: AdminAchievementDefinition;
 try {
  payload = (await request.json()) as AdminAchievementDefinition;
 } catch {
  return buildProblemResponse(400, 'Invalid request payload.');
 }

 const result = await serverHttpRequest<{ message: string }>('/admin/gamification/achievements', {
  method: 'POST',
  token: sessionOrResponse.token,
  json: payload,
  fallbackErrorMessage: 'Failed to save admin achievement.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data ?? { message: 'Saved.' }, { status: 200 });
}
