import type { QueryClient } from '@tanstack/react-query';
import { getLedger } from '@/features/wallet/shared/actions';
import { listDepositPackages, listMyDepositOrders } from '@/features/wallet/deposit/actions';
import { listMyWithdrawals } from '@/features/wallet/withdraw/actions';
import { userStateQueryKeys } from '@/shared/query/userStateQueryKeys';

export async function prefetchWalletOverviewPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.wallet.ledger(1),
  queryFn: async () => {
   const result = await getLedger(1, 10);
   return result.success && result.data ? result.data : null;
  },
 });
}

export async function prefetchDepositPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.wallet.depositPackages(),
  queryFn: async () => {
   const result = await listDepositPackages();
   return result.success && result.data ? result.data : [];
  },
 });
}

export async function prefetchDepositHistoryPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.wallet.depositOrderHistory(1, 10, 'all'),
  queryFn: async () => {
   const result = await listMyDepositOrders(1, 10, null);
   return result.success && result.data
    ? result.data
    : {
      items: [],
      totalCount: 0,
      page: 1,
      pageSize: 10,
      totalPages: 1,
     };
  },
 });
}

export async function prefetchWithdrawPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.wallet.withdrawalsMine(),
  queryFn: async () => {
   const result = await listMyWithdrawals();
   return result.success && result.data ? result.data : [];
  },
 });
}
