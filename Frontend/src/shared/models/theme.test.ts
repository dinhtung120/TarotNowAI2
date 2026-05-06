import { describe, expect, it } from 'vitest';
import {
 DEFAULT_THEME,
 getThemeStylesheetHref,
 isValidTheme,
 resolveTheme,
} from '@/shared/models/theme';

describe('theme domain helpers', () => {
 it('accepts known theme ids', () => {
  expect(isValidTheme(DEFAULT_THEME)).toBe(true);
  expect(isValidTheme('prismatic-aurora')).toBe(true);
 });

 it('rejects unknown theme ids', () => {
  expect(isValidTheme('unknown-theme')).toBe(false);
  expect(isValidTheme(null)).toBe(false);
  expect(isValidTheme(undefined)).toBe(false);
 });

 it('resolves fallback when theme is invalid', () => {
  expect(resolveTheme('unknown-theme', 'zen-garden')).toBe('zen-garden');
  expect(resolveTheme(undefined, 'obsidian-gold')).toBe('obsidian-gold');
 });

 it('creates stylesheet href from theme id', () => {
  expect(getThemeStylesheetHref('prismatic-opaline')).toBe('/themes/prismatic-opaline.css');
 });
});
