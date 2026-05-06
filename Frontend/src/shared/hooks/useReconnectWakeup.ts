'use client';

import { useCallback, useEffect, useRef, useState } from 'react';

export function useReconnectWakeup() {
 const timerRef = useRef<number | null>(null);
 const [wakeupVersion, setWakeupVersion] = useState(0);

 const cancelWakeup = useCallback(() => {
  if (timerRef.current === null) {
   return;
  }

  window.clearTimeout(timerRef.current);
  timerRef.current = null;
 }, []);

 const scheduleWakeup = useCallback((delayMs: number) => {
  cancelWakeup();
  const safeDelayMs = Math.max(0, Math.floor(delayMs));

  timerRef.current = window.setTimeout(() => {
   timerRef.current = null;
   setWakeupVersion((current) => current + 1);
  }, safeDelayMs);
 }, [cancelWakeup]);

 useEffect(() => cancelWakeup, [cancelWakeup]);

 return {
  wakeupVersion,
  scheduleWakeup,
  cancelWakeup,
 };
}
