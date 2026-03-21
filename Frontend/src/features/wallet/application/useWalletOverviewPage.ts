'use client';

import { useEffect, useState } from 'react';
import { getLedger } from '@/actions/walletActions';
import type { PaginatedList, WalletTransaction } from '@/types/wallet';
import { useWalletStore } from '@/store/walletStore';
import { useLocale, useTranslations } from 'next-intl';

export function useWalletOverviewPage() {
 const t = useTranslations('Wallet');
 const locale = useLocale();
 const balance = useWalletStore((state) => state.balance);
 const fetchBalance = useWalletStore((state) => state.fetchBalance);

 const [ledger, setLedger] = useState<PaginatedList<WalletTransaction> | null>(null);
 const [isLoadingLedger, setIsLoadingLedger] = useState(true);
 const [page, setPage] = useState(1);

 useEffect(() => {
  void fetchBalance();
 }, [fetchBalance]);

 useEffect(() => {
  let isMounted = true;

  const loadLedger = async () => {
   setIsLoadingLedger(true);
   const data = await getLedger(page, 10);
   if (!isMounted) return;
   setLedger(data);
   setIsLoadingLedger(false);
  };

  void loadLedger();

  return () => {
   isMounted = false;
  };
 }, [page]);

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
