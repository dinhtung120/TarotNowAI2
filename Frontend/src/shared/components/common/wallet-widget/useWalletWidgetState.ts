"use client";

import { useEffect, useRef } from "react";
import { useLocale, useTranslations } from "next-intl";
import { useShallow } from "zustand/react/shallow";
import { useAuthStore } from "@/store/authStore";
import { useWalletStore } from "@/store/walletStore";

export function useWalletWidgetState() {
 const t = useTranslations("Wallet");
 const locale = useLocale();
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const authUserId = useAuthStore((state) => state.user?.id ?? null);
 const { balance, isLoading } = useWalletStore(useShallow((state) => ({ balance: state.balance, isLoading: state.isLoading })));
 const lastFetchedUserId = useRef<string | null>(null);

 useEffect(() => {
  const walletStore = useWalletStore.getState();
  if (!isAuthenticated || !authUserId) {
   lastFetchedUserId.current = null;
   walletStore.resetWallet();
   return;
  }
  if (lastFetchedUserId.current === authUserId) return;
  lastFetchedUserId.current = authUserId;
  walletStore.resetWallet();
  void walletStore.fetchBalance();
 }, [authUserId, isAuthenticated]);

 return { t, locale, balance, isLoading };
}
