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
    const walletStore = useWalletStore(useShallow((state) => ({ balance: state.balance, isLoading: state.isLoading })));
    const lastFetchedUserId = useRef<string | null>(null);

    useEffect(() => {
        const store = useWalletStore.getState();

        if (!isAuthenticated || !authUserId) {
            lastFetchedUserId.current = null;
            store.resetWallet();
            return;
        }

        if (lastFetchedUserId.current === authUserId) return;
        lastFetchedUserId.current = authUserId;
        if (store.balance) return;

        store.resetWallet();
        void store.fetchBalance();
    }, [authUserId, isAuthenticated]);

    return { t, locale, balance: walletStore.balance, isLoading: walletStore.isLoading };
}

