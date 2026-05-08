import { describe, expect, it } from 'vitest';

import { toCollectionImageProxyUrl } from './collectionCatalogChunked';

describe('toCollectionImageProxyUrl', () => {
  it('uses direct CDN urls for collection thumbnails from the image CDN', () => {
    const sourceUrl = 'https://img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=abc&variant=thumb';

    expect(toCollectionImageProxyUrl(sourceUrl, 'abc', 'thumb')).toBe(sourceUrl);
  });

  it('keeps proxy urls for full collection images', () => {
    const proxyUrl = toCollectionImageProxyUrl(
      'https://img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=abc&variant=full',
      'abc',
      'full',
    );

    expect(proxyUrl).toBe(
      '/api/collection/card-image/abc.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3Dabc%26variant%3Dfull&iv=abc',
    );
  });

  it('returns null for empty source urls', () => {
    expect(toCollectionImageProxyUrl(null, 'abc')).toBeNull();
    expect(toCollectionImageProxyUrl('   ', 'abc')).toBeNull();
  });
});
