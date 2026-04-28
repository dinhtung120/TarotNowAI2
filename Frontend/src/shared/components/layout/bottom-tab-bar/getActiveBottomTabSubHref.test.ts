import { describe, expect, it } from 'vitest';
import { getActiveBottomTabSubHref } from '@/shared/components/layout/bottom-tab-bar/getActiveBottomTabSubHref';

describe('getActiveBottomTabSubHref', () => {
 it('prefers the deepest matching prefix and the longest href tie-breaker', () => {
  const items = [
   { href: '/chat', matchPrefixes: ['/chat'] },
   { href: '/chat/requests', matchPrefixes: ['/chat/requests'] },
   { href: '/chat/requests/detail', matchPrefixes: ['/chat/requests'] },
  ] as never;

  const result = getActiveBottomTabSubHref(items, '/chat/requests/detail', (pathname, prefix) => pathname.startsWith(prefix));
  expect(result).toBe('/chat/requests/detail');
 });
});
