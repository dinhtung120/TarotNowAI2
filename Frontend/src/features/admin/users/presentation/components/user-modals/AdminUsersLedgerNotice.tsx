'use client';

import { cn } from '@/lib/utils';

export function AdminUsersLedgerNotice() {
 return (
  <div className={cn('p-4 rounded-xl tn-surface-soft border border-[var(--info)]/20 bg-[var(--info)]/5')}>
   <p className={cn('text-[10px] leading-relaxed font-medium text-[var(--info)]')}>
    <strong className={cn('uppercase mr-1 block mb-1')}>Cảnh báo Ledger (Sổ cái):</strong>
    Tuỳ tiện sửa đổi số dư ở đây sẽ tự động kích hoạt tính năng phát sinh hóa đơn chênh lệch (Credit / Debit
    Manual Adjustment) tại Backend.
   </p>
  </div>
 );
}
