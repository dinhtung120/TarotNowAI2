'use client';

import { describe, expect, it } from 'vitest';
import {
 RewardFallbackIcon,
 gachaResultRarityConfig,
 resolveGachaResultName,
 resolveGachaResultSummary,
} from '@/components/ui/gacha/gachaResultItemView';

const REWARD = {
 nameVi: 'Thuong',
 nameEn: 'Reward',
 nameZh: 'Jiang li',
 rarity: 'legendary',
 quantityGranted: 2,
 amount: 10,
 currency: 'gold',
} as never;

describe('gachaResultItemView', () => {
 it('maps rarity, locale, and summary helpers for gacha rewards', () => {
  expect(resolveGachaResultName(REWARD, 'en')).toBe('Reward');
  expect(resolveGachaResultName(REWARD, 'zh')).toBe('Jiang li');
  expect(resolveGachaResultName(REWARD, 'vi')).toBe('Thuong');
  expect(resolveGachaResultSummary(REWARD)).toBe('10 GOLD');
  expect(resolveGachaResultSummary({ ...REWARD, currency: null })).toBe('x2');
  expect(gachaResultRarityConfig.legendary.text).toBe('text-amber-400');
 });

 it('renders a fallback icon element for rewards without artwork', () => {
  const element = RewardFallbackIcon({ reward: REWARD });
  expect(element.props.className).toContain('opacity-40');
 });
});
