'use client';

import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { getLedger } from '@/features/wallet/application/actions';
import type { WalletPaginatedList, WalletTransaction } from '@/features/wallet/domain/types';
import { useWalletStore } from '@/store/walletStore';
import { useLocale, useTranslations } from 'next-intl';

export function useWalletOverviewPage() {
 const t = useTranslations('Wallet');
 const locale = useLocale();
 const balance = useWalletStore((state) => state.balance);
 const fetchBalance = useWalletStore((state) => state.fetchBalance);

 const [page, setPage] = useState(1);

 useQuery({
  queryKey: ['wallet', 'balance'],
  queryFn: async () => {
   await fetchBalance();
   return true;
  },
 });

 const { data: ledger, isFetching: isLoadingLedger } = useQuery<
  WalletPaginatedList<WalletTransaction> | null
 >({
  queryKey: ['wallet', 'ledger', page],
  queryFn: async () => {
   const result = await getLedger(page, 10);
   return result.success && result.data ? result.data : null;
  },
 });

 const formatType = (typeStr: string) => typeStr.replace(/([A-Z])/g, ' $1').trim();

 return {
  t,
  locale,
  balance,
  ledger,
  isLoadingLedger,
  page,
  setPage,
  formatType,
 };
}
