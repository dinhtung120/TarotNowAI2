import { afterEach, beforeEach, describe, expect, it } from 'vitest';
import {
 apiUrl,
 browserApiPath,
 getInternalApiBaseUrl,
 getPublicApiBaseUrl,
 internalApiUrl,
 resolveApiOrigin,
 resolveRewriteBackendOrigin,
} from '@/shared/infrastructure/http/apiUrl';

describe('apiUrl helpers', () => {
 const originalEnv = { ...process.env };

 beforeEach(() => {
  process.env.NEXT_PUBLIC_API_URL = 'https://api.example.com/api/v1';
  process.env.INTERNAL_API_URL = 'http://internal-api:8080/api/v1';
 });

 afterEach(() => {
  process.env = { ...originalEnv };
 });

 it('returns normalized public base url', () => {
  process.env.NEXT_PUBLIC_API_URL = 'https://api.example.com/api';
  expect(getPublicApiBaseUrl()).toBe('https://api.example.com/api/v1');
  expect(resolveApiOrigin(process.env.NEXT_PUBLIC_API_URL)).toBe('https://api.example.com');
 });

 it('prefers INTERNAL_API_URL for server internal requests', () => {
  // Vitest runs in jsdom (window exists), so internal helper returns public URL in browser context.
  expect(getInternalApiBaseUrl()).toBe('https://api.example.com/api/v1');
  expect(resolveRewriteBackendOrigin()).toBe('http://internal-api:8080');
 });

 it('builds public and internal paths correctly', () => {
  expect(apiUrl('/profile')).toBe('https://api.example.com/api/v1/profile');
  expect(internalApiUrl('/wallet/balance')).toBe('https://api.example.com/api/v1/wallet/balance');
  expect(browserApiPath('chat')).toBe('/api/v1/chat');
 });
});
