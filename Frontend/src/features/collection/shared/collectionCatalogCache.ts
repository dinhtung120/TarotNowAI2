import type {
  CollectionCatalogChunk,
  CollectionCatalogDetail,
  CollectionCatalogManifest,
} from '@/features/collection/cards/collectionCatalogChunked';
import {
  buildCollectionCatalogChunkKey,
  buildCollectionCatalogDetailKey,
  COLLECTION_MANIFEST_CACHE_KEY,
  COLLECTION_VERSION_HISTORY_KEY,
} from '@/features/collection/cards/collectionCatalogChunked';

const DB_NAME = 'tarotnow-collection-cache';
const DB_VERSION = 1;
const STORE_NAME = 'kv';
const MAX_VERSION_HISTORY = 2;

type StoredEnvelope<T> = {
  savedAtMs: number;
  value: T;
};

function isIndexedDbAvailable(): boolean {
  return typeof window !== 'undefined' && typeof window.indexedDB !== 'undefined';
}

function openDatabase(): Promise<IDBDatabase | null> {
  if (!isIndexedDbAvailable()) {
    return Promise.resolve(null);
  }

  return new Promise((resolve) => {
    const request = window.indexedDB.open(DB_NAME, DB_VERSION);
    request.onupgradeneeded = () => {
      const db = request.result;
      if (!db.objectStoreNames.contains(STORE_NAME)) {
        db.createObjectStore(STORE_NAME);
      }
    };
    request.onsuccess = () => resolve(request.result);
    request.onerror = () => resolve(null);
    request.onblocked = () => resolve(null);
  });
}

function runRequest<T>(request: IDBRequest<T>): Promise<T | null> {
  return new Promise((resolve) => {
    request.onsuccess = () => resolve(request.result);
    request.onerror = () => resolve(null);
  });
}

async function readRaw<T>(key: string): Promise<StoredEnvelope<T> | null> {
  const db = await openDatabase();
  if (!db) return null;

  const tx = db.transaction(STORE_NAME, 'readonly');
  const store = tx.objectStore(STORE_NAME);
  const result = await runRequest(store.get(key));
  db.close();

  if (!result || typeof result !== 'object') return null;
  return result as StoredEnvelope<T>;
}

async function writeRaw<T>(key: string, value: T): Promise<void> {
  const db = await openDatabase();
  if (!db) return;

  const tx = db.transaction(STORE_NAME, 'readwrite');
  const store = tx.objectStore(STORE_NAME);
  store.put({ savedAtMs: Date.now(), value } satisfies StoredEnvelope<T>, key);

  await new Promise<void>((resolve) => {
    tx.oncomplete = () => resolve();
    tx.onerror = () => resolve();
    tx.onabort = () => resolve();
  });
  db.close();
}

async function deleteRaw(key: string): Promise<void> {
  const db = await openDatabase();
  if (!db) return;

  const tx = db.transaction(STORE_NAME, 'readwrite');
  tx.objectStore(STORE_NAME).delete(key);
  await new Promise<void>((resolve) => {
    tx.oncomplete = () => resolve();
    tx.onerror = () => resolve();
    tx.onabort = () => resolve();
  });
  db.close();
}

async function readAllKeys(): Promise<string[]> {
  const db = await openDatabase();
  if (!db) return [];

  const tx = db.transaction(STORE_NAME, 'readonly');
  const keys = await runRequest(tx.objectStore(STORE_NAME).getAllKeys());
  db.close();

  return (Array.isArray(keys) ? keys : [])
    .map((key) => String(key))
    .filter((key) => key.length > 0);
}

async function deleteByPrefix(prefix: string): Promise<void> {
  const keys = await readAllKeys();
  await Promise.all(keys.filter((key) => key.startsWith(prefix)).map((key) => deleteRaw(key)));
}

export async function readCollectionManifestCache(): Promise<CollectionCatalogManifest | null> {
  const cached = await readRaw<CollectionCatalogManifest>(COLLECTION_MANIFEST_CACHE_KEY);
  return cached?.value ?? null;
}

export async function writeCollectionManifestCache(manifest: CollectionCatalogManifest): Promise<void> {
  await writeRaw(COLLECTION_MANIFEST_CACHE_KEY, manifest);
}

export async function readCollectionChunkCache(version: string, chunkId: number): Promise<CollectionCatalogChunk | null> {
  const cached = await readRaw<CollectionCatalogChunk>(buildCollectionCatalogChunkKey(version, chunkId));
  return cached?.value ?? null;
}

export async function writeCollectionChunkCache(chunk: CollectionCatalogChunk): Promise<void> {
  await writeRaw(buildCollectionCatalogChunkKey(chunk.version, chunk.chunkId), chunk);
}

export async function readCollectionDetailCache(version: string, cardId: number): Promise<CollectionCatalogDetail | null> {
  const cached = await readRaw<CollectionCatalogDetail>(buildCollectionCatalogDetailKey(version, cardId));
  return cached?.value ?? null;
}

export async function writeCollectionDetailCache(detail: CollectionCatalogDetail): Promise<void> {
  await writeRaw(buildCollectionCatalogDetailKey(detail.version, detail.id), detail);
}

export async function syncCollectionCatalogVersionHistory(currentVersion: string): Promise<void> {
  if (!currentVersion) return;

  const rawHistory = await readRaw<string[]>(COLLECTION_VERSION_HISTORY_KEY);
  const history = Array.isArray(rawHistory?.value) ? rawHistory?.value : [];
  const nextHistory = [currentVersion, ...history.filter((entry) => entry !== currentVersion)].slice(0, MAX_VERSION_HISTORY);
  await writeRaw(COLLECTION_VERSION_HISTORY_KEY, nextHistory);

  const staleVersions = history.filter((entry) => !nextHistory.includes(entry));
  await Promise.all(staleVersions.map((version) => purgeCollectionCatalogVersion(version)));
}

export async function purgeCollectionCatalogVersion(version: string): Promise<void> {
  if (!version) return;
  await Promise.all([
    deleteByPrefix(`collection:chunk:${version}:`),
    deleteByPrefix(`collection:detail:${version}:`),
  ]);
}

export async function clearCollectionManifestCache(): Promise<void> {
  await deleteRaw(COLLECTION_MANIFEST_CACHE_KEY);
}
