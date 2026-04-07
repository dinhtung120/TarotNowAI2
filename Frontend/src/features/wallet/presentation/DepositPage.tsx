"use client";

import {
  DepositHeader,
  DepositPageLeftColumn,
  DepositPageRightColumn,
} from "@/features/wallet/presentation/components/deposit";
import { useDepositPageViewModel } from "@/features/wallet/presentation/useDepositPageViewModel";
import { cn } from "@/lib/utils";

export default function DepositPage() {
  const vm = useDepositPageViewModel();

  return (
    <div
      className={cn(
        "mx-auto w-full max-w-5xl space-y-10 px-4 pt-8 pb-32 sm:px-6",
      )}
    >
      <DepositHeader {...vm.headerLabels} />
      <div className={cn("tn-grid-cols-1-12-lg gap-10")}>
        <DepositPageLeftColumn
          balance={vm.balance}
          locale={vm.locale}
          exchangeRate={vm.exchangeRate}
          customAmount={vm.customAmount}
          isCustom={vm.isCustom}
          selectedAmount={vm.selectedAmount}
          presetAmounts={vm.presetAmounts}
          minAmount={vm.minAmount}
          formatVnd={vm.formatVnd}
          getPromotion={vm.promoForPreset}
          onSelectPreset={vm.handleSelectPreset}
          onCustomFocus={vm.onCustomFocus}
          onCustomChange={vm.onCustomChange}
          {...vm.leftColumnLabels}
        />
        <DepositPageRightColumn
          locale={vm.locale}
          amountVnd={vm.amountVnd}
          baseDiamond={vm.baseDiamond}
          bonusGold={vm.bonusGold}
          submitting={vm.submitting}
          isValid={vm.isValid}
          order={vm.order}
          error={vm.error}
          formatVnd={vm.formatVnd}
          onDeposit={vm.handleDeposit}
          summaryLabels={vm.rightColumnLabels.summary}
          notesTitle={vm.rightColumnLabels.notesTitle}
          notesItems={vm.rightColumnLabels.notesItems}
        />
      </div>
    </div>
  );
}
