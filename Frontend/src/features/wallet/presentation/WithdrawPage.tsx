"use client";

import {
  WithdrawFormCard,
  WithdrawHistorySection,
  WithdrawPageHeader,
} from "@/features/wallet/presentation/components/withdraw";
import { useWithdrawPageViewModel } from "@/features/wallet/presentation/useWithdrawPageViewModel";
import { cn } from "@/lib/utils";

export default function WithdrawPage() {
  const vm = useWithdrawPageViewModel();

  return (
    <div
      className={cn(
        "mx-auto w-full max-w-3xl space-y-12 px-4 pt-8 pb-32 sm:px-6",
      )}
    >
      <WithdrawPageHeader {...vm.headerLabels} />
      <WithdrawFormCard
        locale={vm.locale}
        amount={vm.amount}
        amountNum={vm.amountNum}
        grossVnd={vm.grossVnd}
        feeVnd={vm.feeVnd}
        netVnd={vm.netVnd}
        bankName={vm.bankName}
        accountName={vm.accountName}
        accountNumber={vm.accountNumber}
        userNote={vm.userNote}
        submitting={vm.submitting}
        success={vm.success}
        error={vm.error}
        labels={vm.formLabels}
        onAmountChange={vm.setAmount}
        onBankNameChange={vm.setBankName}
        onAccountNameChange={vm.setAccountName}
        onAccountNumberChange={vm.setAccountNumber}
        onUserNoteChange={vm.setUserNote}
        onSubmit={vm.handleSubmit}
      />
      <WithdrawHistorySection
        locale={vm.locale}
        history={vm.history}
        loadingHistory={vm.loadingHistory}
        getStatusBadge={vm.getStatusBadge}
        {...vm.historyLabels}
      />
    </div>
  );
}
