import { describe, expect, it } from 'vitest';
import { actionFail, actionOk } from '@/shared/domain/actionResult';

describe('actionResult helpers', () => {
 it('creates success result with data', () => {
  const result = actionOk({ id: '1' });
  expect(result.success).toBe(true);
  expect(result.data).toEqual({ id: '1' });
 });

 it('creates success result without data', () => {
  const result = actionOk();
  expect(result.success).toBe(true);
  expect(result.data).toBeUndefined();
 });

 it('creates failure result', () => {
  const result = actionFail('Unauthorized');
  expect(result.success).toBe(false);
  if (!result.success) {
   expect(result.error).toBe('Unauthorized');
  }
 });
});
