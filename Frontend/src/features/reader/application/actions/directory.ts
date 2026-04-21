'use server';

import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
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
 yearsOfExperience: number;
 facebookUrl?: string | null;
 instagramUrl?: string | null;
 tikTokUrl?: string | null;
 avgRating: number;
 totalReviews: number;
 displayName: string;
 avatarUrl?: string | null;
 createdAt: string;
 updatedAt?: string | null;
}

interface ListReadersResponse {
 readers: ReaderProfile[];
 totalCount: number;
}

type ReaderApi = {
 readers?: ReaderProfile[];
 Readers?: ReaderProfile[];
 totalCount?: number;
 TotalCount?: number;
};

type ReaderSource = Record<string, unknown>;

const FAIL_LIST_READERS = 'Failed to list readers';
const FAIL_LIST_FEATURED = 'Failed to list featured readers';
const FAIL_GET_READER = 'Failed to get reader profile';

const asString = (value: unknown, fallback = '') => (typeof value === 'string' ? value : fallback);
const asNumber = (value: unknown, fallback = 0) => {
 if (typeof value === 'number' && Number.isFinite(value)) {
  return value;
 }

 if (typeof value === 'string') {
  const converted = Number(value);
  return Number.isFinite(converted) ? converted : fallback;
 }

 return fallback;
};

const asStringArray = (value: unknown) =>
 Array.isArray(value) ? value.filter((item): item is string => typeof item === 'string') : [];

const asBoolean = (value: unknown) => {
 if (typeof value === 'boolean') return value;
 if (typeof value !== 'string') return null;
 const normalized = value.trim().toLowerCase();
 if (normalized === 'true' || normalized === '1') return true;
 if (normalized === 'false' || normalized === '0') return false;
 return null;
};

function resolveStatus(source: ReaderSource): string {
 const rawStatus = source.status ?? source.Status ?? source.readerStatus ?? source.ReaderStatus ?? source.onlineStatus ?? source.OnlineStatus;
 if (typeof rawStatus === 'string' && rawStatus.trim().length > 0) return normalizeReaderStatus(rawStatus);
 if (typeof rawStatus === 'object' && rawStatus !== null) {
  const nested = rawStatus as ReaderSource;
  const nestedValue = nested.value ?? nested.Value ?? nested.code ?? nested.Code ?? nested.name ?? nested.Name;
  if (typeof nestedValue === 'string' && nestedValue.trim().length > 0) return normalizeReaderStatus(nestedValue);
 }
 if (asBoolean(source.isAway ?? source.IsAway) === true) return 'away';
 const isOnline = asBoolean(source.isOnline ?? source.IsOnline ?? source.online ?? source.Online ?? source.isConnected ?? source.IsConnected);
 return isOnline === true ? 'online' : 'offline';
}

function mapReaderProfile(raw: unknown): ReaderProfile {
 const source = typeof raw === 'object' && raw !== null ? (raw as ReaderSource) : {};
 return {
  id: asString(source.id ?? source.Id),
  userId: asString(source.userId ?? source.UserId),
  status: resolveStatus(source),
  diamondPerQuestion: asNumber(source.diamondPerQuestion ?? source.DiamondPerQuestion),
  bioVi: asString(source.bioVi ?? source.BioVi),
  bioEn: asString(source.bioEn ?? source.BioEn),
  bioZh: asString(source.bioZh ?? source.BioZh),
  specialties: asStringArray(source.specialties ?? source.Specialties),
  yearsOfExperience: Math.max(0, Math.trunc(asNumber(source.yearsOfExperience ?? source.YearsOfExperience))),
  facebookUrl: asString(source.facebookUrl ?? source.FacebookUrl) || null,
  instagramUrl: asString(source.instagramUrl ?? source.InstagramUrl) || null,
  tikTokUrl: asString(source.tikTokUrl ?? source.TikTokUrl) || null,
  avgRating: asNumber(source.avgRating ?? source.AvgRating),
  totalReviews: Math.trunc(asNumber(source.totalReviews ?? source.TotalReviews)),
  displayName: asString(source.displayName ?? source.DisplayName),
  avatarUrl: asString(source.avatarUrl ?? source.AvatarUrl) || null,
  createdAt: asString(source.createdAt ?? source.CreatedAt),
  updatedAt: asString(source.updatedAt ?? source.UpdatedAt) || null,
 };
}

function mapReaderArray(data: ReaderApi | { readers?: ReaderProfile[]; Readers?: ReaderProfile[] }) {
 return (data.readers || data.Readers || []).map(mapReaderProfile);
}

export async function listReaders(
 page = 1,
 pageSize = 12,
 specialty = '',
 status = '',
 searchTerm = '',
): Promise<ActionResult<ListReadersResponse>> {
 try {
  const params = new URLSearchParams({ page: page.toString(), pageSize: pageSize.toString() });
  if (specialty) params.append('specialty', specialty);
  if (status) params.append('status', status);
  if (searchTerm) params.append('searchTerm', searchTerm);

  const result = await serverHttpRequest<ReaderApi>(`/readers?${params.toString()}`, {
   method: 'GET',
   cache: 'no-store',
   fallbackErrorMessage: FAIL_LIST_READERS,
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
   return actionFail(result.error || FAIL_LIST_READERS);
  }

  const data = result.data;
  return actionOk({ readers: mapReaderArray(data), totalCount: data.totalCount ?? data.TotalCount ?? 0 });
 } catch (error) {
  logger.error('[ReaderAction] listReaders', error, { page, pageSize, specialty, statusFilter: status, searchTerm });
  return actionFail(FAIL_LIST_READERS);
 }
}

export async function listFeaturedReaders(limit = 4): Promise<ActionResult<ReaderProfile[]>> {
 try {
  const result = await serverHttpRequest<{ readers?: ReaderProfile[]; Readers?: ReaderProfile[] }>(`/readers?page=1&pageSize=${limit}`, {
   method: 'GET',
   next: { revalidate: 120 },
   fallbackErrorMessage: FAIL_LIST_FEATURED,
  });

  if (!result.ok) {
   logger.error('[ReaderAction] listFeaturedReaders', result.error, { status: result.status, limit });
   return actionFail(result.error || FAIL_LIST_FEATURED);
  }

  return actionOk(mapReaderArray(result.data));
 } catch (error) {
  logger.error('[ReaderAction] listFeaturedReaders', error, { limit });
  return actionFail(FAIL_LIST_FEATURED);
 }
}

export async function getReaderProfile(userId: string): Promise<ActionResult<ReaderProfile>> {
 try {
  const result = await serverHttpRequest<ReaderProfile>(`/reader/profile/${userId}`, {
   method: 'GET',
   cache: 'no-store',
   fallbackErrorMessage: FAIL_GET_READER,
  });

  if (!result.ok) {
   logger.error('[ReaderAction] getReaderProfile', result.error, { status: result.status, userId });
   return actionFail(result.error || FAIL_GET_READER);
  }

  return actionOk(mapReaderProfile(result.data));
 } catch (error) {
  logger.error('[ReaderAction] getReaderProfile', error, { userId });
  return actionFail(FAIL_GET_READER);
 }
}
