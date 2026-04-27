import { getLocalStorageItem, setLocalStorageItem } from '@/shared/infrastructure/storage/browserStorage';

const DEVICE_ID_STORAGE_KEY = 'tarot-now-device-id';

function generateDeviceId(): string {
 if (typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function') {
  return crypto.randomUUID();
 }

 return `${Date.now()}-${Math.random().toString(16).slice(2)}`;
}

export function getOrCreateDeviceId(): string {
 if (typeof window === 'undefined') {
  return generateDeviceId();
 }

 const existing = getLocalStorageItem(DEVICE_ID_STORAGE_KEY, '');
 if (existing && existing.trim().length > 0) {
  return existing;
 }

 const created = generateDeviceId();
 setLocalStorageItem(DEVICE_ID_STORAGE_KEY, created);
 return created;
}
