import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';

describe('apiUrl', () => {
  const originalEnv = process.env;

  beforeEach(() => {
    vi.resetModules();
    process.env = { ...originalEnv };
  });

  afterEach(() => {
    process.env = originalEnv;
    vi.unstubAllGlobals();
  });

  async function importModule() {
    return await import('./apiUrl');
  }

  describe('Server-side environment', () => {
    beforeEach(() => {
      vi.stubGlobal('window', undefined);
    });

    it('prefers INTERNAL_API_URL over NEXT_PUBLIC_API_URL on server', async () => {
      process.env.INTERNAL_API_URL = 'http://backend-private:5037/api/v1';
      process.env.NEXT_PUBLIC_API_URL = 'https://tarotnow.ai/api/v1';

      const { API_BASE_URL } = await importModule();
      expect(API_BASE_URL).toBe('http://backend-private:5037/api/v1');
    });

    it('falls back to NEXT_PUBLIC_API_URL on server if INTERNAL_API_URL is missing', async () => {
      delete process.env.INTERNAL_API_URL;
      process.env.NEXT_PUBLIC_API_URL = 'https://tarotnow.ai/api/v1';

      const { API_BASE_URL } = await importModule();
      expect(API_BASE_URL).toBe('https://tarotnow.ai/api/v1');
    });

    it('throws error if both are missing on server', async () => {
      delete process.env.INTERNAL_API_URL;
      delete process.env.NEXT_PUBLIC_API_URL;

      await expect(importModule()).rejects.toThrow();
    });
  });

  describe('Client-side environment', () => {
    beforeEach(() => {
      vi.stubGlobal('window', {});
    });

    it('always uses NEXT_PUBLIC_API_URL on client even if INTERNAL_API_URL exists', async () => {
      process.env.INTERNAL_API_URL = 'http://backend-private:5037/api/v1';
      process.env.NEXT_PUBLIC_API_URL = 'https://tarotnow.ai/api/v1';

      const { API_BASE_URL } = await importModule();
      expect(API_BASE_URL).toBe('https://tarotnow.ai/api/v1');
    });

    it('throws error if NEXT_PUBLIC_API_URL is missing on client', async () => {
      delete process.env.NEXT_PUBLIC_API_URL;
      process.env.INTERNAL_API_URL = 'http://backend-private:5037/api/v1';

      await expect(importModule()).rejects.toThrow();
    });
  });

  describe('apiUrl() helper', () => {
    it('appends path correctly to base url', async () => {
      process.env.NEXT_PUBLIC_API_URL = 'https://tarotnow.ai/api/v1';
      const { apiUrl } = await importModule();

      expect(apiUrl('/chat')).toBe('https://tarotnow.ai/api/v1/chat');
      expect(apiUrl('chat')).toBe('https://tarotnow.ai/api/v1/chat');
    });

    it('returns base url if path is empty', async () => {
      process.env.NEXT_PUBLIC_API_URL = 'https://tarotnow.ai/api/v1';
      const { apiUrl } = await importModule();

      expect(apiUrl('')).toBe('https://tarotnow.ai/api/v1');
    });

    it('returns absolute path unchanged if it starts with http', async () => {
      process.env.NEXT_PUBLIC_API_URL = 'https://tarotnow.ai/api/v1';
      const { apiUrl } = await importModule();

      expect(apiUrl('https://external.com/api')).toBe('https://external.com/api');
    });
  });
});
