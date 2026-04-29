import { headers } from 'next/headers';

function headerEquals(headersList: Headers, key: string, expected: string): boolean {
 const value = headersList.get(key);
 if (!value) {
  return false;
 }

 return value.trim().toLowerCase() === expected;
}

function hasPrefetchSignal(headersList: Headers): boolean {
 if (headersList.has('next-router-prefetch')) {
  return true;
 }

 if (headerEquals(headersList, 'purpose', 'prefetch')) {
  return true;
 }

 if (headerEquals(headersList, 'sec-purpose', 'prefetch')) {
  return true;
 }

 if (headerEquals(headersList, 'x-middleware-prefetch', '1')) {
  return true;
 }

 return false;
}

function hasRscSignal(headersList: Headers): boolean {
 if (headerEquals(headersList, 'rsc', '1')) {
  return true;
 }

 const accept = (headersList.get('accept') ?? '').toLowerCase();
 return accept.includes('text/x-component');
}

/**
 * True only for top-level document navigation requests.
 * Prefetch/RSC/data-flight requests are excluded.
 */
export async function isServerDocumentNavigationRequest(): Promise<boolean> {
 const headersList = await headers();

 if (hasPrefetchSignal(headersList) || hasRscSignal(headersList)) {
  return false;
 }

 const fetchDest = (headersList.get('sec-fetch-dest') ?? '').toLowerCase();
 if (fetchDest.length > 0 && fetchDest !== 'document') {
  return false;
 }

 const fetchMode = (headersList.get('sec-fetch-mode') ?? '').toLowerCase();
 if (fetchMode.length > 0 && fetchMode !== 'navigate') {
  return false;
 }

 const accept = (headersList.get('accept') ?? '').toLowerCase();
 return accept.includes('text/html');
}
