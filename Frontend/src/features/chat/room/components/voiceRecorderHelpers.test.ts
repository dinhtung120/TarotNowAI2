import { describe, expect, it } from 'vitest';
import { buildWaveformBars } from './voiceRecorderHelpers';

describe('buildWaveformBars', () => {
 it('returns finite zero bars for empty audio data', () => {
  const bars = buildWaveformBars(new Uint8Array(0), 8);

  expect(bars).toHaveLength(8);
  expect(bars.every((value) => Number.isFinite(value) && value >= 0 && value <= 1)).toBe(true);
  expect(bars.every((value) => value === 0)).toBe(true);
 });

 it('returns no bars for non-positive bar count', () => {
  expect(buildWaveformBars(new Uint8Array([1, 2, 3]), 0)).toEqual([]);
 });
});
