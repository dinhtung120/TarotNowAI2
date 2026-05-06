'use client';

import { useCallback, useMemo, useState } from 'react';
import { resolveErrorMessage } from '@/shared/application/utils/resolveErrorMessage';
import { useGacha } from '@/features/gacha/shared/useGacha';
import { usePullGacha } from '@/features/gacha/shared/usePullGacha';
import type { PullGachaResult } from '@/features/gacha/shared/gachaTypes';

interface UseGachaPageStateOptions {
 pullErrorMessage: string;
}

export function useGachaPageState({ pullErrorMessage }: UseGachaPageStateOptions) {
 const [selectedPoolCode, setSelectedPoolCode] = useState('');
 const [resultModalData, setResultModalData] = useState<PullGachaResult | null>(null);
 const [isResultOpen, setIsResultOpen] = useState(false);
 const [pullError, setPullError] = useState<string | null>(null);

 const { poolsQuery, poolOddsQuery, historyPreviewQuery, resolvedPoolCode } = useGacha({
  selectedPoolCode,
  historyPreviewSize: 6,
 });
 const pullMutation = usePullGacha();

 const activePoolCode = selectedPoolCode || resolvedPoolCode;
 const queryErrorMessage = useMemo(() => {
  const candidates = [poolsQuery.error, poolOddsQuery.error, historyPreviewQuery.error];
  return candidates.find((candidate) => candidate instanceof Error)?.message ?? null;
 }, [historyPreviewQuery.error, poolOddsQuery.error, poolsQuery.error]);

 const onPull = useCallback(async (count = 1) => {
  if (!activePoolCode) {
   return;
  }

  setPullError(null);
  try {
   const result = await pullMutation.mutateAsync({ poolCode: activePoolCode, count });
   setResultModalData(result);
   setIsResultOpen(true);
  } catch (error) {
   setPullError(resolveErrorMessage(error, pullErrorMessage));
  }
 }, [activePoolCode, pullErrorMessage, pullMutation]);

 return {
  activePoolCode,
  historyItems: historyPreviewQuery.data?.items ?? [],
  isResultOpen,
  isPulling: pullMutation.isPending,
  onPull,
  pools: poolsQuery.data ?? [],
  pullError,
  queryErrorMessage,
  resultModalData,
  rewards: poolOddsQuery.data?.rewards ?? [],
  selectedPoolCode,
  setIsResultOpen,
  setSelectedPoolCode,
 };
}
