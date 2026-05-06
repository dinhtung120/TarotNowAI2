import { describe, expect, it } from 'vitest';
import {
 formatBirthdayDraft,
 formatIsoToDisplay,
 parseDisplayBirthday,
} from '@/features/auth/register/registerBirthdayInputValue';

describe('registerBirthdayInputValue', () => {
 it('formats and parses birthday values consistently', () => {
  expect(formatIsoToDisplay('2000-02-01')).toBe('01/02/2000');
  expect(formatIsoToDisplay(undefined)).toBe('');
  expect(formatBirthdayDraft('01022000')).toBe('01/02/2000');
  expect(formatBirthdayDraft('01/02/2000abc')).toBe('01/02/2000');
  expect(parseDisplayBirthday('01/02/2000')).toBe('2000-02-01');
  expect(parseDisplayBirthday('01/02')).toBe('');
 });
});
