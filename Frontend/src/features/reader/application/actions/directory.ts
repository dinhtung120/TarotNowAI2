'use server';

import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { normalizeReaderStatus } from '@/features/reader/domain/readerStatus';

export interface ReaderProfile {
 id: string;
 userId: string;
 status: string;
 diamondPerQuestion: number;
 bioVi: string;
 bioEn: string;
 bioZh: string;
 specialties: string[];
 avgRating: number;
 totalReviews: number;
 displayName: string;
 avatarUrl?: string | null;
 createdAt: string;
 updatedAt?: string | null;
}

export interface ListReadersResponse {
 readers: ReaderProfile[];
 totalCount: number;
}

function asString(value: unknown, fallback = ''): string {
 if (typeof value !== 'string') return fallback;
 return value;
}

function asNumber(value: unknown, fallback = 0): number {
 if (typeof value === 'number' && Number.isFinite(value)) return value;
 if (typeof value === 'string') {
  const parsed = Number(value);
  if (Number.isFinite(parsed)) return parsed;
 }
 return fallback;
}

function asStringArray(value: unknown): string[] {
 if (!Array.isArray(value)) return [];
 return value.filter((item): item is string => typeof item === 'string');
}

function asBoolean(value: unknown): boolean | null {
 if (typeof value === 'boolean') return value;
 if (typeof value === 'string') {
  const normalized = value.trim().toLowerCase();
  if (normalized === 'true' || normalized === '1') return true;
  if (normalized === 'false' || normalized === '0') return false;
 }
 return null;
}

function resolveStatus(source: Record<string, unknown>): string {
 const rawStatus =
  source.status
  ?? source.Status
  ?? source.readerStatus
  ?? source.ReaderStatus
  ?? source.onlineStatus
  ?? source.OnlineStatus;

 if (typeof rawStatus === 'string' && rawStatus.trim().length > 0) {
  return normalizeReaderStatus(rawStatus);
 }

 if (typeof rawStatus === 'object' && rawStatus !== null) {
  const nested = rawStatus as Record<string, unknown>;
  const nestedValue = nested.value ?? nested.Value ?? nested.code ?? nested.Code ?? nested.name ?? nested.Name;
  if (typeof nestedValue === 'string' && nestedValue.trim().length > 0) {
   return normalizeReaderStatus(nestedValue);
  }
 }

 const isAway = asBoolean(source.isAway ?? source.IsAway);
 if (isAway === true) {
  return 'away';
 }

 const isOnline = asBoolean(
  source.isOnline
  ?? source.IsOnline
  ?? source.online
  ?? source.Online
  ?? source.isConnected
  ?? source.IsConnected
 );
 if (isOnline === true) {
  return 'online';
 }

 return 'offline';
}

function mapReaderProfile(raw: unknown): ReaderProfile {
 const source = typeof raw === 'object' && raw !== null
  ? raw as Record<string, unknown>
  : {};

 return {
  id: asString(source.id ?? source.Id),
  userId: asString(source.userId ?? source.UserId),
  status: resolveStatus(source),
  diamondPerQuestion: asNumber(source.diamondPerQuestion ?? source.DiamondPerQuestion, 0),
  bioVi: asString(source.bioVi ?? source.BioVi),
  bioEn: asString(source.bioEn ?? source.BioEn),
  bioZh: asString(source.bioZh ?? source.BioZh),
  specialties: asStringArray(source.specialties ?? source.Specialties),
  avgRating: asNumber(source.avgRating ?? source.AvgRating, 0),
  totalReviews: Math.trunc(asNumber(source.totalReviews ?? source.TotalReviews, 0)),
  displayName: asString(source.displayName ?? source.DisplayName),
  avatarUrl: asString(source.avatarUrl ?? source.AvatarUrl) || null,
  createdAt: asString(source.createdAt ?? source.CreatedAt),
  updatedAt: asString(source.updatedAt ?? source.UpdatedAt) || null,
 };
}

export async function listReaders(
 page = 1,
 pageSize = 12,
 specialty = '',
 status = '',
 searchTerm = ''
): Promise<ActionResult<ListReadersResponse>> {
 try {
  const params = new URLSearchParams({
   page: page.toString(),
   pageSize: pageSize.toString(),
  });
  if (specialty) params.append('specialty', specialty);
  if (status) params.append('status', status);
  if (searchTerm) params.append('searchTerm', searchTerm);

  const result = await serverHttpRequest<{
   readers?: ReaderProfile[];
   Readers?: ReaderProfile[];
   totalCount?: number;
   TotalCount?: number;
  }>(`/readers?${params.toString()}`, {
   method: 'GET',
   cache: 'no-store',
   fallbackErrorMessage: 'Failed to list readers',
  });

  if (!result.ok) {
   logger.error('[ReaderAction] listReaders', result.error, {
    status: result.status,
    page,
    pageSize,
    specialty,
    statusFilter: status,
    searchTerm,
   });
   return actionFail(result.error || 'Failed to list readers');
  }

  const data = result.data;
  const readers = (data.readers || data.Readers || []).map(mapReaderProfile);
  return actionOk({
   readers,
   totalCount: data.totalCount ?? data.TotalCount ?? 0,
  });
 } catch (error) {
  logger.error('[ReaderAction] listReaders', error, {
   page,
   pageSize,
   specialty,
   statusFilter: status,
   searchTerm,
  });
  return actionFail('Failed to list readers');
 }
}

export async function listFeaturedReaders(limit = 4): Promise<ActionResult<ReaderProfile[]>> {
 try {
  const result = await serverHttpRequest<{ readers?: ReaderProfile[]; Readers?: ReaderProfile[] }>(
   `/readers?page=1&pageSize=${limit}`,
   {
    method: 'GET',
    next: { revalidate: 120 },
    fallbackErrorMessage: 'Failed to list featured readers',
   }
  );

  if (!result.ok) {
   logger.error('[ReaderAction] listFeaturedReaders', result.error, {
    status: result.status,
    limit,
   });
   return actionFail(result.error || 'Failed to list featured readers');
  }

  const data = result.data;
  const readers = (data.readers || data.Readers || []).map(mapReaderProfile);
  return actionOk(readers);
 } catch (error) {
  logger.error('[ReaderAction] listFeaturedReaders', error, { limit });
  return actionFail('Failed to list featured readers');
 }
}

export async function getReaderProfile(userId: string): Promise<ActionResult<ReaderProfile>> {
 try {
  const result = await serverHttpRequest<ReaderProfile>(`/reader/profile/${userId}`, {
   method: 'GET',
   cache: 'no-store',
   fallbackErrorMessage: 'Failed to get reader profile',
  });

  if (!result.ok) {
   logger.error('[ReaderAction] getReaderProfile', result.error, {
    status: result.status,
    userId,
   });
   return actionFail(result.error || 'Failed to get reader profile');
  }

  return actionOk(mapReaderProfile(result.data));
 } catch (error) {
  logger.error('[ReaderAction] getReaderProfile', error, { userId });
  return actionFail('Failed to get reader profile');
 }
}
