/*
 * ===================================================================
 * COMPONENT/FILE: Cửa hàng Trạng thái Ví (walletStore.ts)
 * BỐI CẢNH (CONTEXT):
 *   Store quản lý trạng thái số dư Ví điện tử (Gold, Diamond) của User bằng Zustand.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Là nguồn dữ liệu duy nhất (Single Source of Truth) để các Component (như WalletWidget) 
 *     lấy số dư ra hiển thị tức thì.
 *   - Cung cấp hàm `fetchBalance` để tự gọi Server Action `getWalletBalance` và 
 *     cập nhật số dư vào State, kèm theo Loading flag.
 * ===================================================================
 */
import { create } from 'zustand';
import { WalletBalance } from '@/types/wallet';
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
}));
