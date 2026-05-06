'use client';

import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import type { WalletPaginatedList, WalletTransaction } from '@/features/wallet/shared/types';
import { fetchJsonOrThrow } from '@/shared/gateways/clientFetch';
import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';
import { useWalletBalanceQuery } from '@/features/wallet/overview/useWalletBalanceQuery';
import { useAuthStore } from '@/features/auth/public';
import { useLocale, useTranslations } from 'next-intl';

export function useWalletOverviewPage() {
 const t = useTranslations('Wallet');
 const locale = useLocale();
 const balanceQuery = useWalletBalanceQuery();
 const role = useAuthStore((state) => state.user?.role);

 const [page, setPage] = useState(1);

 const { data: ledger, isFetching: isLoadingLedger } = useQuery<
  WalletPaginatedList<WalletTransaction> | null
 >({
  queryKey: userStateQueryKeys.wallet.ledger(page),
  queryFn: async () => fetchJsonOrThrow<WalletPaginatedList<WalletTransaction>>(
   `/api/wallet/ledger?page=${page}&limit=10`,
   {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
   },
   'Failed to load wallet ledger.',
   8_000,
  ),
 });

 const formatType = (typeStr: string) => typeStr.replace(/([A-Z])/g, ' $1').trim();

 return {
  t,
  locale,
  balance: balanceQuery.data ?? null,
  canWithdraw: role === 'tarot_reader',
  ledger,
  isLoadingLedger,
  page,
  setPage,
  formatType,
 };
}
