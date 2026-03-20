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
import { getWalletBalance } from '@/actions/walletActions';

interface WalletState {
 balance: WalletBalance | null;
 isLoading: boolean;
 error: string | null;
 fetchBalance: () => Promise<void>;
}

export const useWalletStore = create<WalletState>((set, get) => ({
 balance: null,
 isLoading: false,
 error: null,
 fetchBalance: async () => {
 if (get().isLoading) return;
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
