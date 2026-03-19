/*
 * ===================================================================
 * COMPONENT/FILE: Wallet Types (wallet.ts)
 * BỐI CẢNH (CONTEXT):
 *   Chuẩn hóa dữ liệu về Ví điện tử và Biến động số dư (Ledger).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - `WalletBalance`: Tổng quan 3 loại tài sản (Gold, Diamond, Frozen Diamond).
 *   - `WalletTransaction`: Dữ liệu cho một dòng lịch sử giao dịch (Biến động trước/sau).
 *   - Có thêm Wrapper `PaginatedList` cho việc phân trang lịch sử.
 * ===================================================================
 */
export interface WalletBalance {
 goldBalance: number;
 diamondBalance: number;
 frozenDiamondBalance: number;
}

export interface WalletTransaction {
 id: string;
 currency: string;
 type: string;
 amount: number;
 balanceBefore: number;
 balanceAfter: number;
 description?: string;
 createdAt: string;
}

export interface PaginatedList<T> {
 items: T[];
 pageIndex: number;
 totalPages: number;
 totalCount: number;
 hasPreviousPage: boolean;
 hasNextPage: boolean;
}
