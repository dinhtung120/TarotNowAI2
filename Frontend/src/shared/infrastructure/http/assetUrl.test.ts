import { describe, expect, it } from 'vitest';
import { resolveAvatarUrl } from './assetUrl';

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

  it('keeps blob and data urls unchanged', () => {
    expect(resolveAvatarUrl('blob:http://localhost:3000/abc')).toBe('blob:http://localhost:3000/abc');
    expect(resolveAvatarUrl('data:image/png;base64,AAA')).toBe('data:image/png;base64,AAA');
  });
});
