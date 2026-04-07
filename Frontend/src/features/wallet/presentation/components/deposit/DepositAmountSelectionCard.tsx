import { jsx as t, jsxs as o } from "react/jsx-runtime";
import { Sparkles as h } from "lucide-react";
import { GlassCard as v } from "@/shared/components/ui";
import { cn as e } from "@/lib/utils";
import { DepositCustomAmountInput as P } from "./DepositCustomAmountInput";
import { DepositPresetGrid as x } from "./DepositPresetGrid";
function G({
  title: n,
  customLabel: r,
  customPlaceholder: s,
  minAmount: m,
  customAmount: a,
  locale: u,
  isCustom: i,
  selectedAmount: l,
  presetAmounts: c,
  exchangeRate: p,
  formatVnd: b,
  bonusLabel: d,
  onCustomFocus: g,
  onCustomChange: C,
  onSelectPreset: f,
  getPromotion: A,
}) {
  return o(v, {
    className: e("space-y-6"),
    children: [
      o("h3", {
        className: e(
          "text-sm font-black tn-text-primary uppercase tracking-widest flex items-center gap-2",
        ),
        children: [t(h, { className: e("w-4 h-4 text-[var(--warning)]") }), n],
      }),
      t(x, {
        presetAmounts: c,
        isCustom: i,
        selectedAmount: l,
        exchangeRate: p,
        locale: u,
        formatVnd: b,
        bonusLabel: d,
        onSelectPreset: f,
        getPromotion: A,
      }),
      t(P, {
        label: r,
        placeholder: s,
        minAmount: m,
        value: a,
        onFocus: g,
        onChange: C,
      }),
    ],
  });
}
export { G as DepositAmountSelectionCard };
