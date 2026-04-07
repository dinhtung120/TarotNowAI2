import { GlassCard } from "@/shared/components/ui";
import { cn } from "@/lib/utils";
import { DepositOrderState } from "./DepositOrderState";
import { DepositSubmitAction } from "./DepositSubmitAction";
import { DepositSummaryBreakdown } from "./DepositSummaryBreakdown";
import type { DepositSummaryCardProps } from "./DepositSummaryCard.types";

export function DepositSummaryCard({
  locale,
  amountVnd,
  baseDiamond,
  bonusGold,
  submitting,
  isValid,
  order,
  error,
  formatVnd,
  onDeposit,
  labels,
}: DepositSummaryCardProps) {
  return (
    <GlassCard className={cn("space-y-6")}>
      <h3 className={cn("text-sm font-black uppercase tracking-widest tn-text-primary")}>
        {labels.title}
      </h3>
      <DepositSummaryBreakdown
        locale={locale}
        amountVnd={amountVnd}
        baseDiamond={baseDiamond}
        bonusGold={bonusGold}
        formatVnd={formatVnd}
        labels={{
          valueLabel: labels.valueLabel,
          diamondReceiveLabel: labels.diamondReceiveLabel,
          promoBonusLabel: labels.promoBonusLabel,
          totalAssetsLabel: labels.totalAssetsLabel,
        }}
      />
      <DepositOrderState
        order={order}
        error={error}
        labels={{ orderReady: labels.orderReady, payNow: labels.payNow }}
      />
      <DepositSubmitAction
        submitting={submitting}
        isValid={isValid}
        submittingLabel={labels.submitting}
        submitLabel={labels.submit}
        securityNote={labels.securityNote}
        onDeposit={onDeposit}
      />
    </GlassCard>
  );
}
