import { beforeEach, describe, expect, it, vi } from 'vitest';
import { getOrCreateDeviceId } from '@/shared/infrastructure/auth/deviceId';

describe('deviceId', () => {
 beforeEach(() => {
  window.localStorage.clear();
 });

 it('returns cached device id from localStorage', () => {
  window.localStorage.setItem('tarot-now-device-id', 'cached-device-id');

  const result = getOrCreateDeviceId();

  expect(result).toBe('cached-device-id');
 });

 it('creates and stores a new device id when missing', () => {
  const randomUuidSpy = vi.spyOn(crypto, 'randomUUID').mockReturnValue('generated-device-id');

  const result = getOrCreateDeviceId();

  expect(result).toBe('generated-device-id');
  expect(window.localStorage.getItem('tarot-now-device-id')).toBe('generated-device-id');
  randomUuidSpy.mockRestore();
 });
});
