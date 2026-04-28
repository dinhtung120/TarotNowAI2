'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { useHydrateFormOnce } from '@/shared/application/hooks/useHydrateFormOnce';

interface HarnessProps {
 enabled?: boolean;
 identity: string | number | null;
 values: { name: string } | null;
 reset: ReturnType<typeof vi.fn>;
}

function Harness({ enabled = true, identity, values, reset }: HarnessProps) {
 useHydrateFormOnce({
  enabled,
  identity,
  values,
  reset,
 });

 return null;
}

describe('useHydrateFormOnce', () => {
 let container: HTMLDivElement;
 let root: Root;

 beforeEach(() => {
  container = document.createElement('div');
  document.body.appendChild(container);
  root = createRoot(container);
 });

 afterEach(() => {
  act(() => root.unmount());
  container.remove();
 });

 it('hydrates once per identity and preserves dirty values', () => {
  const reset = vi.fn();
  const firstValues = { name: 'first' };
  const secondValues = { name: 'second' };

  act(() => {
   root.render(<Harness identity="reader-1" values={firstValues} reset={reset} />);
  });

  expect(reset).toHaveBeenCalledTimes(1);
  expect(reset).toHaveBeenCalledWith(firstValues, { keepDirtyValues: true });

  act(() => {
   root.render(<Harness identity="reader-1" values={secondValues} reset={reset} />);
  });
  expect(reset).toHaveBeenCalledTimes(1);

  act(() => {
   root.render(<Harness identity="reader-2" values={secondValues} reset={reset} />);
  });
  expect(reset).toHaveBeenCalledTimes(2);
  expect(reset).toHaveBeenLastCalledWith(secondValues, { keepDirtyValues: true });
 });

 it('skips hydration when disabled or missing identity', () => {
  const reset = vi.fn();

  act(() => {
   root.render(<Harness enabled={false} identity="reader-1" values={{ name: 'ignored' }} reset={reset} />);
  });
  act(() => {
   root.render(<Harness identity={null} values={{ name: 'ignored' }} reset={reset} />);
  });

  expect(reset).not.toHaveBeenCalled();
 });
});
