import { Sparkles } from "lucide-react";
import { GlassCard } from "@/shared/components/ui";
import { cn } from "@/lib/utils";
import { DepositCustomAmountInput } from "./DepositCustomAmountInput";
import { DepositPresetGrid } from "./DepositPresetGrid";
import type { DepositAmountSelectionCardProps } from "./DepositAmountSelectionCard.types";

export function DepositAmountSelectionCard({
  title,
  customLabel,
  customPlaceholder,
  minAmount,
  customAmount,
  locale,
  isCustom,
  selectedAmount,
  presetAmounts,
  exchangeRate,
  formatVnd,
  bonusLabel,
  onCustomFocus,
  onCustomChange,
  onSelectPreset,
  getPromotion,
}: DepositAmountSelectionCardProps) {
  return (
    <GlassCard className={cn("space-y-6")}>
      <h3
        className={cn(
          "flex items-center gap-2 text-sm font-black uppercase tracking-widest tn-text-primary",
        )}
      >
        <Sparkles className={cn("w-4 h-4 tn-text-warning")} />
        {title}
      </h3>
      <DepositPresetGrid
        presetAmounts={presetAmounts}
        isCustom={isCustom}
        selectedAmount={selectedAmount}
        exchangeRate={exchangeRate}
        locale={locale}
        formatVnd={formatVnd}
        bonusLabel={bonusLabel}
        onSelectPreset={onSelectPreset}
        getPromotion={getPromotion}
      />
      <DepositCustomAmountInput
        label={customLabel}
        placeholder={customPlaceholder}
        minAmount={minAmount}
        value={customAmount}
        onFocus={onCustomFocus}
        onChange={onCustomChange}
      />
    </GlassCard>
  );
}
