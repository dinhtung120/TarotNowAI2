import { describe, expect, it } from 'vitest';

import { toCollectionImageProxyUrl } from './collectionCatalogChunked';

describe('toCollectionImageProxyUrl', () => {
  it('uses a cacheable image-like path for collection image proxy urls', () => {
    const proxyUrl = toCollectionImageProxyUrl(
      'https://img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=abc&variant=thumb',
      'abc',
    );

    expect(proxyUrl).toBe(
      '/api/collection/card-image/abc.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3Dabc%26variant%3Dthumb&iv=abc',
    );
  });

  it('returns null for empty source urls', () => {
    expect(toCollectionImageProxyUrl(null, 'abc')).toBeNull();
    expect(toCollectionImageProxyUrl('   ', 'abc')).toBeNull();
  });
});
