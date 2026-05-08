import { fetchJsonOrThrow } from '@/shared/gateways/clientFetch';

const CATALOG_MANIFEST_ENDPOINT = '/api/v1/reading/cards-catalog/manifest';
const CHUNK_ENDPOINT_PREFIX = '/api/v1/reading/cards-catalog/chunks';
const DETAIL_ENDPOINT_PREFIX = '/api/v1/reading/cards-catalog/details';
const DEFAULT_TIMEOUT_MS = 8_000;

export interface CollectionCatalogChunkRef {
  chunkId: number;
  startIndex: number;
  endIndex: number;
  count: number;
  cardIds: number[];
}

export interface CollectionCatalogManifest {
  version: string;
  chunkSize: number;
  totalCards: number;
  updatedAtUtc: string;
  chunks: CollectionCatalogChunkRef[];
}

export interface CollectionCatalogChunkItem {
  id: number;
  name: string;
  arcana: string;
  suit: string | null;
  rarity: string;
  thumbUrl: string | null;
  fullUrl: string | null;
}

export interface CollectionCatalogChunk {
  version: string;
  chunkId: number;
  items: CollectionCatalogChunkItem[];
}

export interface CollectionCatalogDetail {
  version: string;
  id: number;
  name: string;
  arcana: string;
  suit: string | null;
  rarity: string;
  thumbUrl: string | null;
  fullUrl: string | null;
  uprightKeywords: string[];
  uprightDescription: string;
  reversedKeywords: string[];
  reversedDescription: string;
}

export function buildCollectionCatalogChunkKey(version: string, chunkId: number): string {
  return `collection:chunk:${version}:${chunkId}`;
}

export function buildCollectionCatalogDetailKey(version: string, cardId: number): string {
  return `collection:detail:${version}:${cardId}`;
}

export const COLLECTION_MANIFEST_CACHE_KEY = 'collection:manifest:v1';
export const COLLECTION_VERSION_HISTORY_KEY = 'collection:version-history:v1';

export function toCollectionImageProxyUrl(
  sourceUrl: string | null | undefined,
  version: string,
  mode: 'thumb' | 'full' = 'full',
): string | null {
  if (!sourceUrl) return null;
  const raw = sourceUrl.trim();
  if (!raw) return null;

  if (mode === 'thumb') {
    try {
      const parsed = new URL(raw);
      if (parsed.hostname.toLowerCase() === 'img.tarotnow.xyz') {
        return raw;
      }
    } catch {
      // fall through to proxy path
    }
  }

  const params = new URLSearchParams({
    src: raw,
    iv: version,
  });
  return `/api/collection/card-image/${encodeURIComponent(version)}.avif?${params.toString()}`;
}

export async function fetchCollectionCatalogManifest(): Promise<CollectionCatalogManifest> {
  return fetchJsonOrThrow<CollectionCatalogManifest>(
    CATALOG_MANIFEST_ENDPOINT,
    {
      method: 'GET',
      credentials: 'include',
      cache: 'no-store',
    },
    'Failed to load collection catalog manifest.',
    DEFAULT_TIMEOUT_MS,
  );
}

export async function fetchCollectionCatalogChunk(
  version: string,
  chunkId: number,
): Promise<CollectionCatalogChunk> {
  return fetchJsonOrThrow<CollectionCatalogChunk>(
    `${CHUNK_ENDPOINT_PREFIX}/${chunkId}?v=${encodeURIComponent(version)}`,
    {
      method: 'GET',
      credentials: 'include',
      cache: 'force-cache',
    },
    `Failed to load collection catalog chunk ${chunkId}.`,
    DEFAULT_TIMEOUT_MS,
  );
}

export async function fetchCollectionCatalogDetail(
  version: string,
  cardId: number,
): Promise<CollectionCatalogDetail> {
  return fetchJsonOrThrow<CollectionCatalogDetail>(
    `${DETAIL_ENDPOINT_PREFIX}/${cardId}?v=${encodeURIComponent(version)}`,
    {
      method: 'GET',
      credentials: 'include',
      cache: 'force-cache',
    },
    `Failed to load collection catalog detail ${cardId}.`,
    DEFAULT_TIMEOUT_MS,
  );
}
