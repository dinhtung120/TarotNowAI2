const isBrowser = () => typeof window !== 'undefined';

function safeGet(storage: Storage, key: string): string | null {
 try {
  return storage.getItem(key);
 } catch {
  return null;
 }
}

function safeSet(storage: Storage, key: string, value: string): void {
 try {
  storage.setItem(key, value);
 } catch {
 }
}

function safeRemove(storage: Storage, key: string): void {
 try {
  storage.removeItem(key);
 } catch {
 }
}

export function getLocalStorageItem(key: string, fallback = ''): string {
 if (!isBrowser()) return fallback;
 return safeGet(window.localStorage, key) ?? fallback;
}

export function setLocalStorageItem(key: string, value: string): void {
 if (!isBrowser()) return;
 safeSet(window.localStorage, key, value);
}

export function removeLocalStorageItem(key: string): void {
 if (!isBrowser()) return;
 safeRemove(window.localStorage, key);
}

export function getSessionStorageItem(key: string, fallback = ''): string {
 if (!isBrowser()) return fallback;
 return safeGet(window.sessionStorage, key) ?? fallback;
}

export function getSessionStorageNumber(key: string, fallback = 0): number {
 const value = getSessionStorageItem(key, '');
 const parsed = Number.parseInt(value, 10);
 return Number.isFinite(parsed) ? parsed : fallback;
}

export function setSessionStorageItem(key: string, value: string): void {
 if (!isBrowser()) return;
 safeSet(window.sessionStorage, key, value);
}

export function removeSessionStorageItem(key: string): void {
 if (!isBrowser()) return;
 safeRemove(window.sessionStorage, key);
}
