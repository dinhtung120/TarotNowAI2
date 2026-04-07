import { jsx as e, jsxs as a } from "react/jsx-runtime";
import { Gem as p, User as c } from "lucide-react";
import { formatCurrency as m } from "@/shared/utils/format/formatCurrency";
import {
  formatDate as l,
  formatTime as x,
} from "@/shared/utils/format/formatDateTime";
import { cn as t } from "@/lib/utils";
import { AdminDepositRowActions as v } from "./AdminDepositRowActions";
import { AdminDepositStatusBadge as f } from "./AdminDepositStatusBadge";
function w({
  locale: r,
  order: i,
  processingId: n,
  labels: s,
  onApprove: o,
  onReject: d,
}) {
  return a("tr", {
    className: t("group/row hover:tn-surface transition-colors"),
    children: [
      e("td", {
        className: t("px-8 py-5"),
        children: a("div", {
          className: t("flex items-center gap-3"),
          children: [
            e("div", {
              className: t(
                "w-8 h-8 rounded-lg tn-panel-overlay flex items-center justify-center shadow-inner",
              ),
              children: e(c, {
                className: t("w-4 h-4 text-[var(--text-secondary)]"),
              }),
            }),
            a("div", {
              children: [
                e("div", {
                  className: t(
                    "text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm",
                  ),
                  children: i.username || s.systemUser,
                }),
                e("div", {
                  className: t(
                    "text-[9px] font-bold text-[var(--text-tertiary)] italic tracking-tighter",
                  ),
                  children: s.userIdPrefix(i.userId.split("-")[0]),
                }),
              ],
            }),
          ],
        }),
      }),
      e("td", {
        className: t("px-8 py-5"),
        children: e("div", {
          className: t(
            "text-[11px] font-black tn-text-primary uppercase tracking-tighter",
          ),
          children: m(i.amountVnd, r),
        }),
      }),
      e("td", {
        className: t("px-8 py-5"),
        children: a("div", {
          className: t(
            "flex items-center gap-2 text-[11px] font-black text-[var(--purple-accent)] italic",
          ),
          children: [
            e(p, { className: t("w-3.5 h-3.5") }),
            "+",
            i.diamondAmount.toLocaleString(r),
          ],
        }),
      }),
      e("td", {
        className: t("px-8 py-5"),
        children: a("div", {
          className: t("flex flex-col text-left"),
          children: [
            e("div", {
              className: t(
                "text-[10px] font-black text-[var(--text-secondary)] uppercase tracking-tighter",
              ),
              children: l(i.createdAt, r),
            }),
            e("div", {
              className: t(
                "text-[10px] font-bold text-[var(--text-tertiary)] italic",
              ),
              children: x(i.createdAt, r),
            }),
          ],
        }),
      }),
      e("td", {
        className: t("px-8 py-5 text-center"),
        children: e(f, {
          status: i.status,
          labels: {
            success: s.statusSuccess,
            failed: s.statusFailed,
            pending: s.statusPending,
          },
        }),
      }),
      e("td", {
        className: t("px-8 py-5 text-center"),
        children: e(v, {
          order: i,
          processingId: n,
          labels: {
            approveTitle: s.approveTitle,
            rejectTitle: s.rejectTitle,
            notAvailable: s.notAvailable,
          },
          onApprove: o,
          onReject: d,
        }),
      }),
    ],
  });
}
export { w as AdminDepositTableRow };
