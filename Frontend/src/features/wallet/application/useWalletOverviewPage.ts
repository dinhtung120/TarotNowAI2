'use client';

import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import type { WalletPaginatedList, WalletTransaction } from '@/features/wallet/domain/types';
import { fetchJsonOrThrow } from '@/shared/infrastructure/http/clientFetch';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { useWalletStore } from '@/store/walletStore';
import { useAuthStore } from '@/store/authStore';
import { useLocale, useTranslations } from 'next-intl';

const WALLET_BALANCE_SYNC_QUERY_KEY = ['wallet', 'balance-sync'] as const;

export function useWalletOverviewPage() {
 const t = useTranslations('Wallet');
 const locale = useLocale();
 const balance = useWalletStore((state) => state.balance);
 const fetchBalance = useWalletStore((state) => state.fetchBalance);
 const role = useAuthStore((state) => state.user?.role);

 const [page, setPage] = useState(1);

 useQuery({
  queryKey: WALLET_BALANCE_SYNC_QUERY_KEY,
  queryFn: () => fetchBalance(),
 });

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
  balance,
  canWithdraw: role === 'tarot_reader',
  ledger,
  isLoadingLedger,
  page,
  setPage,
  formatType,
 };
}
