import type { ReaderProfile } from '@/features/reader/shared';
import { normalizeReaderStatus } from '@/features/reader/shared/readerStatus';

export interface HomeSnapshotDto {
 featuredReaders: ReaderProfile[];
 totalCount: number;
}

type RawHomeSnapshot = {
 featuredReaders?: unknown[];
 FeaturedReaders?: unknown[];
 totalCount?: number;
 TotalCount?: number;
};

function mapReaderProfile(raw: unknown): ReaderProfile {
 const source = typeof raw === 'object' && raw !== null ? (raw as Record<string, unknown>) : {};
 const asString = (v: unknown, fb = '') => (typeof v === 'string' ? v : fb);
 const asNum = (v: unknown, fb = 0) =>
  typeof v === 'number' && Number.isFinite(v) ? v : fb;
 const asStrArr = (v: unknown) =>
  Array.isArray(v) ? v.filter((x): x is string => typeof x === 'string') : [];

  return {
  id: asString(source.id ?? source.Id),
  userId: asString(source.userId ?? source.UserId),
  status: normalizeReaderStatus(asString(source.status ?? source.Status, 'offline')),
  diamondPerQuestion: asNum(source.diamondPerQuestion ?? source.DiamondPerQuestion),
  bioVi: asString(source.bioVi ?? source.BioVi),
  bioEn: asString(source.bioEn ?? source.BioEn),
  bioZh: asString(source.bioZh ?? source.BioZh),
  specialties: asStrArr(source.specialties ?? source.Specialties),
  yearsOfExperience: Math.max(0, Math.trunc(asNum(source.yearsOfExperience ?? source.YearsOfExperience))),
  facebookUrl: asString(source.facebookUrl ?? source.FacebookUrl) || null,
  instagramUrl: asString(source.instagramUrl ?? source.InstagramUrl) || null,
  tikTokUrl: asString(source.tikTokUrl ?? source.TikTokUrl) || null,
  avgRating: asNum(source.avgRating ?? source.AvgRating),
  totalReviews: Math.trunc(asNum(source.totalReviews ?? source.TotalReviews)),
  displayName: asString(source.displayName ?? source.DisplayName),
  avatarUrl: asString(source.avatarUrl ?? source.AvatarUrl) || null,
  createdAt: asString(source.createdAt ?? source.CreatedAt),
  updatedAt: asString(source.updatedAt ?? source.UpdatedAt) || null,
 };
}

export function mapReadersFromHomeSnapshot(raw: unknown): HomeSnapshotDto {
 const o = (typeof raw === 'object' && raw !== null ? raw : {}) as RawHomeSnapshot;
 const list = (o.featuredReaders ?? o.FeaturedReaders ?? []) as unknown[];
 const readers = list.map(mapReaderProfile);
 const total = typeof o.totalCount === 'number' ? o.totalCount : Number(o.TotalCount ?? readers.length);
 return {
  featuredReaders: readers,
  totalCount: Number.isFinite(total) ? total : readers.length,
 };
}
