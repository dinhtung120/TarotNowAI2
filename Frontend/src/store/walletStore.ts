
import { create } from 'zustand';
import { WalletBalance } from '@/features/wallet/domain/types';
import type { ActionResult } from '@/shared/domain/actionResult';

type WalletBalanceFetcher = () => Promise<ActionResult<WalletBalance>>;

let walletBalanceFetcher: WalletBalanceFetcher | null = null;

export function setWalletBalanceFetcher(fetcher?: WalletBalanceFetcher) {
	walletBalanceFetcher = fetcher ?? null;
}

interface WalletState {
	balance: WalletBalance | null;
	isLoading: boolean;
	error: string | null;
	resetWallet: () => void;
	fetchBalance: () => Promise<void>;
	setBalance: (balance: WalletBalance) => void;
}

export const useWalletStore = create<WalletState>((set, get) => ({
	balance: null,
	isLoading: false,
	error: null,
	resetWallet: () => set({ balance: null, isLoading: false, error: null }),
	fetchBalance: async () => {
		if (get().isLoading) return;
		set({ isLoading: true, error: null });
		try {
			const fetcher = walletBalanceFetcher;
			if (!fetcher) {
				set({ error: 'Wallet balance fetcher is not configured', isLoading: false });
				return;
			}

			const result = await fetcher();
			if (result.success && result.data) {
				set({ balance: result.data, isLoading: false });
			} else {
				set({ balance: null, error: result.error, isLoading: false });
			}
		} catch (error: unknown) {
			const errorMessage = error instanceof Error ? error.message : 'Unknown error';
			set({ balance: null, error: errorMessage, isLoading: false });
		}
	},
	setBalance: (balance) => set({ balance, isLoading: false, error: null }),
}));
