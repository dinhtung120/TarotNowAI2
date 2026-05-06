'use client';

import { act, useEffect } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { useReconnectWakeup } from '@/shared/hooks/useReconnectWakeup';

describe('useReconnectWakeup', () => {
 let container: HTMLDivElement;
 let root: Root;

 beforeEach(() => {
  vi.useFakeTimers();
  container = document.createElement('div');
  document.body.appendChild(container);
  root = createRoot(container);
 });

 afterEach(() => {
  act(() => root.unmount());
  container.remove();
  vi.useRealTimers();
 });

 it('bumps wakeupVersion when the scheduled wake-up fires', async () => {
  let latestHook: ReturnType<typeof useReconnectWakeup> | null = null;

  function Harness({ onChange }: { onChange: (value: ReturnType<typeof useReconnectWakeup>) => void }) {
   const hook = useReconnectWakeup();

   useEffect(() => {
    onChange(hook);
   }, [hook, onChange]);

   return null;
  }

  act(() => root.render(<Harness onChange={(value) => {
   latestHook = value;
  }} />));
  act(() => latestHook?.scheduleWakeup(50));

  await act(async () => {
   await vi.advanceTimersByTimeAsync(49);
  });
  expect(latestHook?.wakeupVersion).toBe(0);

  await act(async () => {
   await vi.advanceTimersByTimeAsync(1);
  });
  expect(latestHook?.wakeupVersion).toBe(1);
 });

 it('cancels the pending timer before it fires', async () => {
  let latestHook: ReturnType<typeof useReconnectWakeup> | null = null;

  function Harness({ onChange }: { onChange: (value: ReturnType<typeof useReconnectWakeup>) => void }) {
   const hook = useReconnectWakeup();

   useEffect(() => {
    onChange(hook);
   }, [hook, onChange]);

   return null;
  }

  act(() => root.render(<Harness onChange={(value) => {
   latestHook = value;
  }} />));
  act(() => {
   latestHook?.scheduleWakeup(50);
   latestHook?.cancelWakeup();
  });

  await act(async () => {
   await vi.advanceTimersByTimeAsync(50);
  });
  expect(latestHook?.wakeupVersion).toBe(0);
 });
});
