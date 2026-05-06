'use client';

import { memo, useCallback } from 'react';
import { formatCurrency } from '@/shared/utils/format/formatCurrency';
import { useDepositHistoryPage } from '@/features/wallet/deposit/useDepositHistoryPage';
import {
 DepositHistoryFilters,
 DepositHistoryHeader,
 DepositHistoryPagination,
 DepositHistoryTable,
} from '@/features/wallet/deposit/components/history';
import { cn } from '@/lib/utils';

const DepositHistoryPage = memo(function DepositHistoryPage() {
 const vm = useDepositHistoryPage();
 const formatVnd = useCallback((value: number) => formatCurrency(value, vm.locale), [vm.locale]);

 return (
  <div className={cn('mx-auto w-full max-w-6xl space-y-6 px-4 pb-24 pt-8 sm:px-6')}>
   <DepositHistoryHeader
    locale={vm.locale}
    tag={vm.t('deposit.history.tag')}
    title={vm.t('deposit.history.title')}
    subtitle={vm.t('deposit.history.subtitle')}
    backLabel={vm.t('deposit.history.back_to_deposit')}
   />

   <section className={cn('space-y-4 rounded-3xl border tn-border-soft tn-panel-soft p-5')}>
    <DepositHistoryFilters
      currentFilter={vm.statusFilter}
      onChange={vm.setFilter}
      labels={{
       all: vm.t('deposit.history.filter_all'),
       pending: vm.t('deposit.history.filter_pending'),
       success: vm.t('deposit.history.filter_success'),
       failed: vm.t('deposit.history.filter_failed'),
      }}
    />

    <DepositHistoryTable
      locale={vm.locale}
      items={vm.items}
      isLoading={vm.isLoading || vm.isFetching}
      error={vm.error}
      emptyLabel={vm.t('deposit.history.empty')}
      loadingLabel={vm.t('deposit.history.loading')}
      formatVnd={formatVnd}
      labels={{
       amount: vm.t('deposit.history.amount'),
       diamond: vm.t('deposit.history.diamond'),
       bonusGold: vm.t('deposit.history.bonus_gold'),
       transaction: vm.t('deposit.history.transaction'),
       failureReason: vm.t('deposit.history.failure_reason'),
       reconcile: vm.t('deposit.history.reconcile'),
      }}
      statusText={{
       pending: vm.t('deposit.status.pending'),
       success: vm.t('deposit.status.success'),
       failed: vm.t('deposit.status.failed'),
      }}
      onReconcile={vm.reconcilePendingOrder}
    />

    <DepositHistoryPagination
      page={vm.page}
      totalPages={vm.totalPages}
      isLoading={vm.isLoading || vm.isFetching}
      pageLabel={(page, totalPages) => vm.t('deposit.history.page_indicator', { page, totalPages })}
      prevLabel={vm.t('deposit.history.prev')}
      nextLabel={vm.t('deposit.history.next')}
      onPrev={vm.goPrev}
      onNext={vm.goNext}
    />
   </section>
  </div>
 );
});

export default DepositHistoryPage;
