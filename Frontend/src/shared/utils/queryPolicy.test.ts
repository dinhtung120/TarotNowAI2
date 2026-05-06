import { describe, expect, it, vi } from 'vitest';
import {
 createCancellableLoadTask,
 queryFnOrThrow,
 useDebouncedQueryInput,
} from '@/shared/utils/queryPolicy';
import { useDebouncedValue } from '@/shared/hooks/useDebouncedValue';

vi.mock('@/shared/hooks/useDebouncedValue', () => ({
 useDebouncedValue: vi.fn(),
}));

describe('queryPolicy', () => {
 it('throws the fallback error when query results are unsuccessful or empty', () => {
  expect(() => queryFnOrThrow({ success: false, error: 'boom' }, 'fallback')).toThrow('boom');
  expect(() => queryFnOrThrow({ success: true, data: null }, 'fallback')).toThrow('fallback');
  expect(queryFnOrThrow({ success: true, data: { ok: true } }, 'fallback')).toEqual({ ok: true });
 });

 it('delegates debounced query input to the shared debounce hook', () => {
  vi.mocked(useDebouncedValue).mockReturnValue('ready');
  expect(useDebouncedQueryInput('draft', 300)).toBe('ready');
  expect(useDebouncedValue).toHaveBeenCalledWith('draft', 300);
 });

 it('creates cancellable load tokens that invalidate prior requests', () => {
  const task = createCancellableLoadTask();
  const firstToken = task.createToken();
  const secondToken = task.createToken();

  expect(task.isCurrentToken(firstToken)).toBe(false);
  expect(task.isCurrentToken(secondToken)).toBe(true);

  task.cancel();
  expect(task.isCurrentToken(secondToken)).toBe(false);
 });
});
