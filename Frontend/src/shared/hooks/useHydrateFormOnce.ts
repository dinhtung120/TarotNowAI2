'use client';

import { useEffect, useRef } from 'react';
import type { FieldValues, UseFormReset } from 'react-hook-form';

interface UseHydrateFormOnceOptions<TFieldValues extends FieldValues> {
 enabled?: boolean;
 identity: string | number | null;
 reset: UseFormReset<TFieldValues>;
 values: TFieldValues | null;
}

export function useHydrateFormOnce<TFieldValues extends FieldValues>({
 enabled = true,
 identity,
 reset,
 values,
}: UseHydrateFormOnceOptions<TFieldValues>) {
 const lastHydratedIdentityRef = useRef<string | number | null>(null);

 useEffect(() => {
  if (!enabled || identity === null || values === null) {
   return;
  }

  if (lastHydratedIdentityRef.current === identity) {
   return;
  }

  reset(values, { keepDirtyValues: true });
  lastHydratedIdentityRef.current = identity;
 }, [enabled, identity, reset, values]);
}
