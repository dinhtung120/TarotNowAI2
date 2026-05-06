import { describe, expect, it } from 'vitest';
import {
 getLocalStorageItem,
 getSessionStorageNumber,
 removeLocalStorageItem,
 setLocalStorageItem,
 setSessionStorageItem,
} from '@/shared/storage/browserStorage';

describe('browser storage helpers', () => {
 it('reads and writes localStorage safely', () => {
  removeLocalStorageItem('tn:test');
  expect(getLocalStorageItem('tn:test', 'fallback')).toBe('fallback');

  setLocalStorageItem('tn:test', 'value');
  expect(getLocalStorageItem('tn:test', 'fallback')).toBe('value');

  removeLocalStorageItem('tn:test');
  expect(getLocalStorageItem('tn:test', 'fallback')).toBe('fallback');
 });

 it('parses integer from sessionStorage with fallback', () => {
  setSessionStorageItem('tn:number', '42');
  expect(getSessionStorageNumber('tn:number', 0)).toBe(42);

  setSessionStorageItem('tn:number', 'NaN');
  expect(getSessionStorageNumber('tn:number', 7)).toBe(7);
 });
});
