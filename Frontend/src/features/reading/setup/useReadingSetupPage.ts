'use client';

import { useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { useTranslations } from 'next-intl';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import type { InitReadingResponse } from '@/features/reading/shared/actions/types';
import {
 type ReadingSetupSnapshotDto,
} from '@/features/reading/setup/reading-setup-snapshot';
import { fetchJsonOrThrow } from '@/shared/gateways/clientFetch';
import { useOptimizedNavigation } from '@/shared/gateways/useOptimizedNavigation';
import { setSessionStorageItem } from '@/shared/gateways/browserStorage';
import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';

interface ReadingSetupFormData {
 question?: string;
}

export interface ReadingSpreadOption {
 id: 'daily_1' | 'spread_3' | 'spread_5' | 'spread_10';
 name: string;
 desc: string;
 cost: string;
 exp: number;
 freeDrawCount: number;
 usesFreeDraw: boolean;
 icon: 'star' | 'flame' | 'shield' | 'moon';
}

const CARDS_TO_DRAW_MAP: Record<string, number> = {
 daily_1: 1,
 spread_3: 3,
 spread_5: 5,
 spread_10: 10,
};

const READING_SETUP_TIMEOUT_MS = 8_000;
const READING_INIT_TIMEOUT_MS = 12_000;

export function useReadingSetupPage() {
 const navigation = useOptimizedNavigation();
 const t = useTranslations('ReadingSetup');
 const queryClient = useQueryClient();
 const readingSetupSnapshotQuery = useQuery({
  queryKey: userStateQueryKeys.reading.setupSnapshot(),
  queryFn: ({ signal }) => fetchJsonOrThrow<ReadingSetupSnapshotDto>(
   '/api/reading/setup-snapshot',
   {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
    signal,
   },
   'Failed to load reading setup snapshot',
   READING_SETUP_TIMEOUT_MS,
  ),
  staleTime: 10_000,
 });
 const freeDrawQuotas = readingSetupSnapshotQuery.data?.freeDrawQuotas ?? {
  spread3: 0,
  spread5: 0,
  spread10: 0,
 };
 const pricing = readingSetupSnapshotQuery.data?.pricing;

 const [selectedSpread, setSelectedSpread] = useState<ReadingSpreadOption['id']>('daily_1');
 const [selectedCurrency, setSelectedCurrency] = useState<'gold' | 'diamond'>('gold');
 const [initError, setInitError] = useState('');
 const [isInitializing, setIsInitializing] = useState(false);

 const formSchema = useMemo(
  () =>
   z.object({
    question: z.string().max(300, t('validation.question_too_long')).optional(),
   }),
  [t],
 );

 const {
  register,
  handleSubmit,
  formState: { errors },
 } = useForm<ReadingSetupFormData>({
  resolver: zodResolver(formSchema),
 });

 const spreads = useMemo<ReadingSpreadOption[]>(
  () => [
   {
    id: 'daily_1',
    name: t('daily_1_name'),
    desc: t('daily_1_desc'),
    cost: t('cost_free'),
    exp: 1,
    freeDrawCount: 0,
    usesFreeDraw: true,
    icon: 'star',
   },
   {
    id: 'spread_3',
    name: t('spread_3_name'),
    desc: t('spread_3_desc'),
    cost: selectedCurrency === 'gold' && freeDrawQuotas.spread3 > 0
     ? t('cost_free_ticket', { count: freeDrawQuotas.spread3, spread: 3 })
     : selectedCurrency === 'diamond'
      ? t('cost_diamond', { amount: pricing?.spread3DiamondCost ?? 0 })
      : t('cost_gold', { amount: pricing?.spread3GoldCost ?? 0 }),
    exp: selectedCurrency === 'diamond' ? 2 : 1,
    freeDrawCount: selectedCurrency === 'gold' ? freeDrawQuotas.spread3 : 0,
    usesFreeDraw: selectedCurrency === 'gold' && freeDrawQuotas.spread3 > 0,
    icon: 'flame',
   },
   {
    id: 'spread_5',
    name: t('spread_5_name'),
    desc: t('spread_5_desc'),
    cost: selectedCurrency === 'gold' && freeDrawQuotas.spread5 > 0
     ? t('cost_free_ticket', { count: freeDrawQuotas.spread5, spread: 5 })
     : selectedCurrency === 'diamond'
      ? t('cost_diamond', { amount: pricing?.spread5DiamondCost ?? 0 })
      : t('cost_gold', { amount: pricing?.spread5GoldCost ?? 0 }),
    exp: selectedCurrency === 'diamond' ? 2 : 1,
    freeDrawCount: selectedCurrency === 'gold' ? freeDrawQuotas.spread5 : 0,
    usesFreeDraw: selectedCurrency === 'gold' && freeDrawQuotas.spread5 > 0,
    icon: 'shield',
   },
   {
    id: 'spread_10',
    name: t('spread_10_name'),
    desc: t('spread_10_desc'),
    cost: selectedCurrency === 'gold' && freeDrawQuotas.spread10 > 0
     ? t('cost_free_ticket', { count: freeDrawQuotas.spread10, spread: 10 })
     : selectedCurrency === 'gold'
      ? t('cost_gold', { amount: pricing?.spread10GoldCost ?? 0 })
      : t('cost_diamond', { amount: pricing?.spread10DiamondCost ?? 0 }),
    exp: selectedCurrency === 'diamond' ? 2 : 1,
    freeDrawCount: selectedCurrency === 'gold' ? freeDrawQuotas.spread10 : 0,
    usesFreeDraw: selectedCurrency === 'gold' && freeDrawQuotas.spread10 > 0,
    icon: 'moon',
   },
  ],
  [freeDrawQuotas.spread10, freeDrawQuotas.spread3, freeDrawQuotas.spread5, selectedCurrency, pricing, t],
 );

 const submitSetup = async (data: ReadingSetupFormData) => {
  setIsInitializing(true);
  setInitError('');

  if (!readingSetupSnapshotQuery.data?.pricing) {
   setInitError(t('error_init_failed'));
   setIsInitializing(false);
   return;
  }

  let response: InitReadingResponse;
  try {
   response = await fetchJsonOrThrow<InitReadingResponse>(
    '/api/reading/init',
    {
     method: 'POST',
     credentials: 'include',
     headers: {
      'Content-Type': 'application/json',
     },
     body: JSON.stringify({
      spreadType: selectedSpread,
      question: data.question,
      currency: selectedCurrency,
     }),
    },
    t('error_init_failed'),
    READING_INIT_TIMEOUT_MS,
   );
  } catch (error) {
   setInitError(error instanceof Error ? error.message : t('error_init_failed'));
   setIsInitializing(false);
   return;
  }

  void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.wallet.balance() });
  const cardsToDraw = CARDS_TO_DRAW_MAP[selectedSpread] || 1;

  if (data.question) {
   setSessionStorageItem(`question_${response.sessionId}`, data.question);
  }

  setSessionStorageItem(`cardsToDraw_${response.sessionId}`, cardsToDraw.toString());
  navigation.push(`/reading/session/${response.sessionId}`);
 };

 return {
  t,
  selectedSpread,
  setSelectedSpread,
  selectedCurrency,
  setSelectedCurrency,
  initError,
  isInitializing,
  register,
  handleSubmit,
  errors,
  spreads,
  freeDrawQuotas,
  submitSetup,
 };
}
