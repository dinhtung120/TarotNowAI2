'use client';

import { useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { useTranslations } from 'next-intl';
import { useQuery } from '@tanstack/react-query';
import { initReadingSession } from '@/features/reading/application/actions';
import {
 getReadingSetupSnapshotAction,
 type ReadingSetupSnapshotDto,
} from '@/shared/application/actions/reading-setup-snapshot';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';
import { useWalletStore } from '@/store/walletStore';
import { setSessionStorageItem } from '@/shared/infrastructure/storage/browserStorage';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

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

export function useReadingSetupPage() {
 const navigation = useOptimizedNavigation();
 const t = useTranslations('ReadingSetup');
 const fetchBalance = useWalletStore((state) => state.fetchBalance);
 const readingSetupSnapshotQuery = useQuery({
  queryKey: userStateQueryKeys.reading.setupSnapshot(),
  queryFn: async (): Promise<ReadingSetupSnapshotDto> => {
   const result = await getReadingSetupSnapshotAction();
   if (!result.success || !result.data) {
    throw new Error(result.error || 'Failed to load reading setup snapshot');
   }

   return result.data;
  },
  staleTime: 10_000,
 });
 const freeDrawQuotas = readingSetupSnapshotQuery.data?.freeDrawQuotas ?? {
  spread3: 0,
  spread5: 0,
  spread10: 0,
 };

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
    cost: freeDrawQuotas.spread3 > 0
     ? t('cost_free_ticket', { count: freeDrawQuotas.spread3, spread: 3 })
     : selectedCurrency === 'diamond'
      ? t('cost_diamond', { amount: 5 })
      : t('cost_gold', { amount: 50 }),
    exp: selectedCurrency === 'diamond' ? 2 : 1,
    freeDrawCount: freeDrawQuotas.spread3,
    usesFreeDraw: freeDrawQuotas.spread3 > 0,
    icon: 'flame',
   },
   {
    id: 'spread_5',
    name: t('spread_5_name'),
    desc: t('spread_5_desc'),
    cost: freeDrawQuotas.spread5 > 0
     ? t('cost_free_ticket', { count: freeDrawQuotas.spread5, spread: 5 })
     : selectedCurrency === 'diamond'
      ? t('cost_diamond', { amount: 10 })
      : t('cost_gold', { amount: 100 }),
    exp: selectedCurrency === 'diamond' ? 2 : 1,
    freeDrawCount: freeDrawQuotas.spread5,
    usesFreeDraw: freeDrawQuotas.spread5 > 0,
    icon: 'shield',
   },
   {
    id: 'spread_10',
    name: t('spread_10_name'),
    desc: t('spread_10_desc'),
    cost: freeDrawQuotas.spread10 > 0
     ? t('cost_free_ticket', { count: freeDrawQuotas.spread10, spread: 10 })
     : selectedCurrency === 'gold'
      ? t('cost_gold', { amount: 500 })
      : t('cost_diamond', { amount: 50 }),
    exp: selectedCurrency === 'diamond' ? 2 : 1,
    freeDrawCount: freeDrawQuotas.spread10,
    usesFreeDraw: freeDrawQuotas.spread10 > 0,
    icon: 'moon',
   },
  ],
  [freeDrawQuotas.spread10, freeDrawQuotas.spread3, freeDrawQuotas.spread5, selectedCurrency, t],
 );

 const submitSetup = async (data: ReadingSetupFormData) => {
  setIsInitializing(true);
  setInitError('');

  const response = await initReadingSession({
   spreadType: selectedSpread,
   question: data.question,
   currency: selectedCurrency,
  });

 if (!response.success || !response.data) {
   setInitError(response.error || t('error_init_failed'));
   setIsInitializing(false);
   return;
  }

  void fetchBalance();
  const cardsToDraw = CARDS_TO_DRAW_MAP[selectedSpread] || 1;

  if (data.question) {
   setSessionStorageItem(`question_${response.data.sessionId}`, data.question);
  }

  setSessionStorageItem(`cardsToDraw_${response.data.sessionId}`, cardsToDraw.toString());
  navigation.push(`/reading/session/${response.data.sessionId}`);
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
