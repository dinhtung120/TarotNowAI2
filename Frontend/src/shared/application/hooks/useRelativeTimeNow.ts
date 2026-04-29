'use client';

import { useSyncExternalStore } from 'react';

const DEFAULT_TICK_MS = 60_000;
const SERVER_SNAPSHOT = Number.NaN;

function createNowSubscription(tickMs: number) {
 return (onStoreChange: () => void) => {
  const timer = window.setInterval(onStoreChange, tickMs);
  return () => {
   window.clearInterval(timer);
  };
 };
}

export function useRelativeTimeNow(tickMs = DEFAULT_TICK_MS): number {
 return useSyncExternalStore(
  createNowSubscription(tickMs),
  () => Date.now(),
  () => SERVER_SNAPSHOT,
 );
}
