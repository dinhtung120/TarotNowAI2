import { describe, expect, it } from 'vitest';
import { resolveAvatarUrl, shouldUseUnoptimizedImage } from './assetUrl';

describe('resolveAvatarUrl', () => {
  it('returns null for null or blank inputs', () => {
    expect(resolveAvatarUrl(null)).toBeNull();
    expect(resolveAvatarUrl(undefined)).toBeNull();
    expect(resolveAvatarUrl('   ')).toBeNull();
  });

  it('keeps uploads-relative url unchanged', () => {
    expect(resolveAvatarUrl('/uploads/avatars/a.webp')).toBe('/uploads/avatars/a.webp');
  });

  it('normalizes uploads url without leading slash', () => {
    expect(resolveAvatarUrl('uploads/avatars/a.webp')).toBe('/uploads/avatars/a.webp');
  });

  it('normalizes api-version uploads url', () => {
    expect(resolveAvatarUrl('/api/v1/uploads/avatars/a.webp')).toBe('/uploads/avatars/a.webp');
    expect(resolveAvatarUrl('api/v1/uploads/avatars/a.webp')).toBe('/uploads/avatars/a.webp');
  });

  it('rewrites absolute uploads url to same-origin uploads path', () => {
    expect(resolveAvatarUrl('http://127.0.0.1:5037/uploads/avatars/a.webp?x=1')).toBe('/uploads/avatars/a.webp?x=1');
  });

  it('keeps non-uploads absolute url unchanged', () => {
    expect(resolveAvatarUrl('https://cdn.example.com/avatar.png')).toBe('https://cdn.example.com/avatar.png');
  });

  it('drops legacy media avatar urls that cause optimized image 404s', () => {
    expect(resolveAvatarUrl('https://media.tarotnow.xyz/avatars/missing.webp')).toBeNull();
  });

  it('keeps blob and data urls unchanged', () => {
    expect(resolveAvatarUrl('blob:http://localhost:3000/abc')).toBe('blob:http://localhost:3000/abc');
    expect(resolveAvatarUrl('data:image/png;base64,AAA')).toBe('data:image/png;base64,AAA');
  });
});

describe('shouldUseUnoptimizedImage', () => {
  it('returns true for empty values', () => {
    expect(shouldUseUnoptimizedImage(null)).toBe(true);
    expect(shouldUseUnoptimizedImage(undefined)).toBe(true);
    expect(shouldUseUnoptimizedImage('   ')).toBe(true);
  });

  it('keeps local static assets optimized', () => {
    expect(shouldUseUnoptimizedImage('/images/collection/back-card.svg')).toBe(false);
  });

  it('keeps collection image proxy urls unoptimized', () => {
    expect(
      shouldUseUnoptimizedImage(
        '/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Fa.avif%3Fiv%3Dabc%26variant%3Dthumb&iv=abc',
      ),
    ).toBe(true);
    expect(
      shouldUseUnoptimizedImage(
        'https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Fa.avif&iv=abc',
      ),
    ).toBe(true);
  });

  it('keeps allowlisted remote hosts optimized', () => {
    expect(shouldUseUnoptimizedImage('https://media.tarotnow.xyz/uploads/a.webp')).toBe(false);
  });

  it('bypasses optimization for community CDN images and small item icons', () => {
    expect(shouldUseUnoptimizedImage('https://media.tarotnow.xyz/community/a.webp')).toBe(true);
    expect(shouldUseUnoptimizedImage('https://media.tarotnow.xyz/icon/free_draw_ticket_50_20260416_165452.avif')).toBe(true);
  });

  it('falls back to unoptimized for non-allowlisted remote hosts', () => {
    expect(shouldUseUnoptimizedImage('https://cdn.example.com/avatar.png')).toBe(true);
  });
});
