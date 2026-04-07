import { jsx as t, jsxs as L } from "react/jsx-runtime";
import { GlassCard as b } from "@/shared/components/ui";
import { cn as o } from "@/lib/utils";
import { DepositOrderState as c } from "./DepositOrderState";
import { DepositSubmitAction as g } from "./DepositSubmitAction";
import { DepositSummaryBreakdown as y } from "./DepositSummaryBreakdown";
function C({
  locale: r,
  amountVnd: i,
  baseDiamond: a,
  bonusGold: s,
  submitting: n,
  isValid: m,
  order: d,
  error: u,
  formatVnd: p,
  onDeposit: l,
  labels: e,
}) {
  return L(b, {
    className: o("space-y-6"),
    children: [
      t("h3", {
        className: o(
          "text-sm font-black tn-text-primary uppercase tracking-widest",
        ),
        children: e.title,
      }),
      t(y, {
        locale: r,
        amountVnd: i,
        baseDiamond: a,
        bonusGold: s,
        formatVnd: p,
        labels: {
          valueLabel: e.valueLabel,
          diamondReceiveLabel: e.diamondReceiveLabel,
          promoBonusLabel: e.promoBonusLabel,
          totalAssetsLabel: e.totalAssetsLabel,
        },
      }),
      t(c, {
        order: d,
        error: u,
        labels: { orderReady: e.orderReady, payNow: e.payNow },
      }),
      t(g, {
        submitting: n,
        isValid: m,
        submittingLabel: e.submitting,
        submitLabel: e.submit,
        securityNote: e.securityNote,
        onDeposit: l,
      }),
    ],
  });
}
export { C as DepositSummaryCard };
