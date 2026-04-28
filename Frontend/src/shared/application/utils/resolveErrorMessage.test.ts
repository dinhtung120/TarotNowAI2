import { describe, expect, it } from 'vitest';
import { resolveErrorMessage } from '@/shared/application/utils/resolveErrorMessage';

describe('resolveErrorMessage', () => {
 it('returns the original error message when available', () => {
  expect(resolveErrorMessage(new Error('Request failed.'), 'fallback')).toBe('Request failed.');
 });

 it('falls back when the input is not an Error instance', () => {
  expect(resolveErrorMessage('boom', 'fallback')).toBe('fallback');
 });
});
