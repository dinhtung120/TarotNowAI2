import { describe, expect, it } from 'vitest';
import {
 getSafeFollowupQuestion,
 getSafeStreamLanguage,
 isValidStreamSessionId,
} from '@/app/[locale]/api/reading/sessions/[sessionId]/streamRouteGuards';

describe('streamRouteGuards', () => {
 it('normalizes supported language values', () => {
  expect(getSafeStreamLanguage('EN-us')).toBe('en');
  expect(getSafeStreamLanguage('zh-CN')).toBe('zh');
  expect(getSafeStreamLanguage('vi')).toBe('vi');
 });

 it('falls back to english for unsupported language values', () => {
  expect(getSafeStreamLanguage('fr')).toBe('en');
  expect(getSafeStreamLanguage('   ')).toBe('en');
  expect(getSafeStreamLanguage(null)).toBe('en');
 });

 it('sanitizes follow-up question text', () => {
  expect(getSafeFollowupQuestion('\u0000  hello\r\nworld  ')).toBe('hello\nworld');
  expect(getSafeFollowupQuestion(null)).toBeUndefined();
  expect(getSafeFollowupQuestion('   ')).toBeUndefined();
  expect(getSafeFollowupQuestion('a'.repeat(2050))?.length).toBe(2000);
 });

 it('accepts only bounded session identifiers', () => {
  expect(isValidStreamSessionId('session-123')).toBe(true);
  expect(isValidStreamSessionId('bad/session')).toBe(false);
  expect(isValidStreamSessionId('a'.repeat(65))).toBe(false);
 });
});
