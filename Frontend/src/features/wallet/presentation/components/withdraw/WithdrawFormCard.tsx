import { GlassCard } from "@/shared/components/ui";
import { cn } from "@/lib/utils";
import { WithdrawAmountSection } from "./WithdrawAmountSection";
import { WithdrawBankFields } from "./WithdrawBankFields";
import { WithdrawSubmitSection } from "./WithdrawSubmitSection";
import type { WithdrawFormCardProps } from "./WithdrawFormCard.types";

export function WithdrawFormCard(props: WithdrawFormCardProps) {
  return (
    <GlassCard
      className={cn(
        "animate-in fade-in slide-in-from-bottom-8 delay-200 duration-1000",
      )}
    >
      <form className={cn("space-y-6")} onSubmit={props.onSubmit}>
        <WithdrawAmountSection
          amount={props.amount}
          amountNum={props.amountNum}
          feeVnd={props.feeVnd}
          grossVnd={props.grossVnd}
          labels={{
            amountLabel: props.labels.amountLabel,
            amountPlaceholder: props.labels.amountPlaceholder,
            grossLabel: props.labels.grossLabel,
            feeLabel: props.labels.feeLabel,
            netLabel: props.labels.netLabel,
          }}
          locale={props.locale}
          netVnd={props.netVnd}
          onAmountChange={props.onAmountChange}
        />
        <WithdrawBankFields
          accountName={props.accountName}
          accountNumber={props.accountNumber}
          bankName={props.bankName}
          labels={{
            bankLabel: props.labels.bankLabel,
            bankPlaceholder: props.labels.bankPlaceholder,
            accountNameLabel: props.labels.accountNameLabel,
            accountNamePlaceholder: props.labels.accountNamePlaceholder,
            accountNumberLabel: props.labels.accountNumberLabel,
            accountNumberPlaceholder: props.labels.accountNumberPlaceholder,
          }}
          onAccountNameChange={props.onAccountNameChange}
          onAccountNumberChange={props.onAccountNumberChange}
          onBankNameChange={props.onBankNameChange}
        />
        <WithdrawSubmitSection
          amountNum={props.amountNum}
          error={props.error}
          submitLabel={props.labels.submitLabel}
          submitting={props.submitting}
          submittingLabel={props.labels.submittingLabel}
          success={props.success}
          successMessage={props.labels.successMessage}
        />
      </form>
    </GlassCard>
  );
}
