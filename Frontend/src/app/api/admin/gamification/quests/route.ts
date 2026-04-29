import { NextRequest, NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { requireAdminSession } from '@/app/api/admin/gamification/_shared';
import type { AdminQuestDefinition } from '@/features/gamification/admin/application/adminGamification.types';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

export async function GET(): Promise<NextResponse> {
 const sessionOrResponse = await requireAdminSession();
 if (sessionOrResponse instanceof NextResponse) {
  return sessionOrResponse;
 }

 const result = await serverHttpRequest<AdminQuestDefinition[]>('/admin/gamification/quests', {
  method: 'GET',
  token: sessionOrResponse.token,
  fallbackErrorMessage: 'Failed to load admin quests.',
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

 let payload: AdminQuestDefinition;
 try {
  payload = (await request.json()) as AdminQuestDefinition;
 } catch {
  return buildProblemResponse(400, 'Invalid request payload.');
 }

 const result = await serverHttpRequest<{ message: string }>('/admin/gamification/quests', {
  method: 'POST',
  token: sessionOrResponse.token,
  json: payload,
  fallbackErrorMessage: 'Failed to save admin quest.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data ?? { message: 'Saved.' }, { status: 200 });
}
