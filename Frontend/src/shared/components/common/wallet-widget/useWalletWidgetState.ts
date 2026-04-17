"use client";

import { useEffect, useRef, useMemo } from "react";
import { useLocale, useTranslations } from "next-intl";
import { usePathname } from "@/i18n/routing";
import { useAuthStore } from "@/store/authStore";
import { useWalletStore } from "@/store/walletStore";
import { shouldEnableRealtimeForPath } from "@/shared/infrastructure/navigation/normalizePathname";

export function useWalletWidgetState() {
   const t = useTranslations("Wallet");
   const locale = useLocale();
   const pathname = usePathname();
   const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
   const authUserId = useAuthStore((state) => state.user?.id ?? null);

   const balance = useWalletStore((state) => state.balance);
   const isLoading = useWalletStore((state) => state.isLoading);

   const lastFetchedUserId = useRef<string | null>(null);
   const shouldFetchOnCurrentRoute = shouldEnableRealtimeForPath(pathname);

   useEffect(() => {
      const store = useWalletStore.getState();

      if (!shouldFetchOnCurrentRoute) {
         return;
      }

      if (!isAuthenticated || !authUserId) {
         lastFetchedUserId.current = null;
         if (store.balance !== null || store.isLoading) {
            store.resetWallet();
         }
         return;
      }

      if (lastFetchedUserId.current === authUserId) return;
      lastFetchedUserId.current = authUserId;
      if (store.isLoading) return;

      if (store.balance !== null) return;

      void store.fetchBalance();
   }, [authUserId, isAuthenticated, shouldFetchOnCurrentRoute]);

   return useMemo(() => ({
      t,
      locale,
      balance,
      isLoading
   }), [t, locale, balance, isLoading]);
}
