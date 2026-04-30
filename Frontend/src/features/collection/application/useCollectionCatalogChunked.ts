'use client';

import { useEffect, useMemo, useRef, useState } from 'react';
import { useQueries, useQuery } from '@tanstack/react-query';
import {
  type CollectionCatalogChunk,
  type CollectionCatalogChunkItem,
  type CollectionCatalogDetail,
  type CollectionCatalogManifest,
  fetchCollectionCatalogChunk,
  fetchCollectionCatalogDetail,
  fetchCollectionCatalogManifest,
} from '@/features/collection/application/collectionCatalogChunked';
import {
  readCollectionChunkCache,
  readCollectionDetailCache,
  readCollectionManifestCache,
  syncCollectionCatalogVersionHistory,
  writeCollectionChunkCache,
  writeCollectionDetailCache,
  writeCollectionManifestCache,
} from '@/features/collection/application/gateways/collectionCatalogCacheGateway';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';

const DEFAULT_VISIBLE_CHUNK_WINDOW = 1;
const CHUNK_WINDOW_EXPAND_COOLDOWN_MS = 600;

interface UseCollectionCatalogChunkedOptions {
  orderedCardIds: number[];
  selectedCardId: number | null;
}

export interface UseCollectionCatalogChunkedResult {
  manifest: CollectionCatalogManifest | null;
  visibleCardIds: number[];
  hasMoreVisibleCards: boolean;
  requestNextVisibleChunkWindow: () => void;
  cardSummaryById: Map<number, CollectionCatalogChunkItem>;
  selectedCardDetail: CollectionCatalogDetail | null;
  isManifestLoading: boolean;
  isChunkLoading: boolean;
  isDetailLoading: boolean;
}

export function useCollectionCatalogChunked({
  orderedCardIds,
  selectedCardId,
}: UseCollectionCatalogChunkedOptions): UseCollectionCatalogChunkedResult {
  const [bootstrapReady, setBootstrapReady] = useState(false);
  const [bootstrapManifest, setBootstrapManifest] = useState<CollectionCatalogManifest | null>(null);
  const [visibleChunkWindowState, setVisibleChunkWindowState] = useState({
    manifestVersion: '',
    chunkWindow: DEFAULT_VISIBLE_CHUNK_WINDOW,
  });
  const lastChunkWindowExpandAtMsRef = useRef(0);

  useEffect(() => {
    let alive = true;
    (async () => {
      const cachedManifest = await readCollectionManifestCache();
      if (!alive) return;
      setBootstrapManifest(cachedManifest);
      setBootstrapReady(true);
    })();
    return () => {
      alive = false;
    };
  }, []);

  const manifestQuery = useQuery({
    queryKey: userStateQueryKeys.collection.catalog.manifest(),
    enabled: bootstrapReady,
    initialData: bootstrapManifest ?? undefined,
    initialDataUpdatedAt: bootstrapManifest ? 0 : undefined,
    queryFn: async () => {
      const manifest = await fetchCollectionCatalogManifest();
      await writeCollectionManifestCache(manifest);
      await syncCollectionCatalogVersionHistory(manifest.version);
      return manifest;
    },
    staleTime: 1000 * 60 * 5,
    gcTime: 1000 * 60 * 60 * 12,
    refetchOnWindowFocus: false,
  });

  const manifest = manifestQuery.data ?? null;
  const manifestVersion = manifest?.version ?? '';
  const visibleChunkWindow = visibleChunkWindowState.manifestVersion === manifestVersion
    ? visibleChunkWindowState.chunkWindow
    : DEFAULT_VISIBLE_CHUNK_WINDOW;

  const visibleCardIds = useMemo(() => {
    if (!orderedCardIds.length) return [];
    const visibleCount = Math.min(
      orderedCardIds.length,
      visibleChunkWindow * (manifest?.chunkSize ?? 16),
    );
    return orderedCardIds.slice(0, visibleCount);
  }, [manifest?.chunkSize, orderedCardIds, visibleChunkWindow]);

  const hasMoreVisibleCards = visibleCardIds.length < orderedCardIds.length;

  const cardToChunkId = useMemo(() => {
    const map = new Map<number, number>();
    for (const chunk of manifest?.chunks ?? []) {
      for (const cardId of chunk.cardIds ?? []) {
        map.set(cardId, chunk.chunkId);
      }
    }
    return map;
  }, [manifest?.chunks]);
  const requestedChunkIds = useMemo(() => {
    if (!manifest) return [];
    const requiredChunkIds = new Set<number>([0]);
    for (const cardId of visibleCardIds) {
      const chunkId = cardToChunkId.get(cardId);
      if (typeof chunkId === 'number' && chunkId >= 0) {
        requiredChunkIds.add(chunkId);
      }
    }
    return [...requiredChunkIds].sort((left, right) => left - right);
  }, [cardToChunkId, manifest, visibleCardIds]);

  const chunkQueries = useQueries({
    queries: (manifest ? requestedChunkIds : []).map((chunkId) => ({
      queryKey: userStateQueryKeys.collection.catalog.chunk(manifest!.version, chunkId),
      queryFn: async (): Promise<CollectionCatalogChunk> => {
        const cached = await readCollectionChunkCache(manifest!.version, chunkId);
        if (cached) return cached;

        const chunk = await fetchCollectionCatalogChunk(manifest!.version, chunkId);
        await writeCollectionChunkCache(chunk);
        return chunk;
      },
      staleTime: Infinity,
      gcTime: 1000 * 60 * 60 * 24,
      refetchOnWindowFocus: false,
    })),
  });

  const cardSummaryById = useMemo(() => {
    const map = new Map<number, CollectionCatalogChunkItem>();
    for (const chunkQuery of chunkQueries) {
      const data = chunkQuery.data;
      if (!data) continue;
      for (const item of data.items) {
        map.set(item.id, item);
      }
    }
    return map;
  }, [chunkQueries]);

  const selectedCardDetailQuery = useQuery({
    queryKey: manifest && selectedCardId !== null
      ? userStateQueryKeys.collection.catalog.detail(manifest.version, selectedCardId)
      : ['collection', 'catalog', 'detail', 'none'],
    enabled: Boolean(manifest && selectedCardId !== null),
    queryFn: async () => {
      const cardId = selectedCardId!;
      const version = manifest!.version;
      const cached = await readCollectionDetailCache(version, cardId);
      if (cached) return cached;

      const detail = await fetchCollectionCatalogDetail(version, cardId);
      await writeCollectionDetailCache(detail);
      return detail;
    },
    staleTime: Infinity,
    gcTime: 1000 * 60 * 60 * 24,
    refetchOnWindowFocus: false,
  });

  const isChunkLoading = chunkQueries.some((query) => query.isLoading || query.isFetching);

  return {
    manifest,
    visibleCardIds,
    hasMoreVisibleCards,
    requestNextVisibleChunkWindow: () => {
      if (!hasMoreVisibleCards) return;
      if (isChunkLoading) return;
      const now = Date.now();
      if (now - lastChunkWindowExpandAtMsRef.current < CHUNK_WINDOW_EXPAND_COOLDOWN_MS) {
        return;
      }
      lastChunkWindowExpandAtMsRef.current = now;
      setVisibleChunkWindowState((current) => {
        const currentWindow = current.manifestVersion === manifestVersion
          ? current.chunkWindow
          : DEFAULT_VISIBLE_CHUNK_WINDOW;
        return {
          manifestVersion,
          chunkWindow: currentWindow + 1,
        };
      });
    },
    cardSummaryById,
    selectedCardDetail: selectedCardDetailQuery.data ?? null,
    isManifestLoading: manifestQuery.isLoading || !bootstrapReady,
    isChunkLoading,
    isDetailLoading: selectedCardDetailQuery.isLoading || selectedCardDetailQuery.isFetching,
  };
}
