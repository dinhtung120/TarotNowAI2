import { describe, expect, it } from 'vitest';
import { parseWholeWithdrawAmount } from './withdrawAmount';

describe('parseWholeWithdrawAmount', () => {
 it.each([
  '1000.5',
  '1e3',
  '',
  '  ',
  '-1',
 ])('rejects non-whole amount %s', (value) => {
  expect(parseWholeWithdrawAmount(value)).toBeNull();
 });

 it.each([
  ['0010', 10],
  [' 1000 ', 1000],
 ] as const)('parses whole amount %s', (value, expected) => {
  expect(parseWholeWithdrawAmount(value)).toBe(expected);
 });
});
