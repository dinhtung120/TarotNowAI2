import { create } from 'zustand';
import { WalletBalance } from '@/types/wallet';
import { getWalletBalance } from '@/actions/walletActions';

interface WalletState {
 balance: WalletBalance | null;
 isLoading: boolean;
 error: string | null;
 fetchBalance: () => Promise<void>;
}

export const useWalletStore = create<WalletState>((set) => ({
 balance: null,
 isLoading: false,
 error: null,
 fetchBalance: async () => {
 set({ isLoading: true, error: null });
 try {
 const data = await getWalletBalance();
 if (data) {
 set({ balance: data, isLoading: false });
 } else {
 set({ error: 'Failed to fetch balance', isLoading: false });
 }
 } catch (error: unknown) {
 const errorMessage = error instanceof Error ? error.message : 'Unknown error';
 set({ error: errorMessage, isLoading: false });
 }
 },
}));
