import { jsx as r, jsxs as a } from "react/jsx-runtime";
import { CreditCard as x, Loader2 as h } from "lucide-react";
import {
  GlassCard as v,
  StepPagination as y,
  TableStates as f,
} from "@/shared/components/ui";
import { cn as e } from "@/lib/utils";
import { AdminDepositTableRow as A } from "./AdminDepositTableRow";
function P({
  locale: p,
  orders: n,
  loading: i,
  page: s,
  totalCount: d,
  processingId: c,
  labels: t,
  onApprove: m,
  onReject: g,
  onPrev: l,
  onNext: u,
}) {
  return a(v, {
    className: e("!p-0 !rounded-[2.5rem] overflow-hidden text-left"),
    children: [
      r("div", {
        className: e("overflow-x-auto custom-scrollbar"),
        children: a("table", {
          className: e("w-full text-left"),
          children: [
            r("thead", {
              children: a("tr", {
                className: e("border-b tn-border-soft tn-surface"),
                children: [
                  r("th", {
                    className: e(
                      "px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]",
                    ),
                    children: t.headingUser,
                  }),
                  r("th", {
                    className: e(
                      "px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]",
                    ),
                    children: t.headingAmount,
                  }),
                  r("th", {
                    className: e(
                      "px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]",
                    ),
                    children: t.headingAssets,
                  }),
                  r("th", {
                    className: e(
                      "px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]",
                    ),
                    children: t.headingTime,
                  }),
                  r("th", {
                    className: e(
                      "px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center",
                    ),
                    children: t.headingStatus,
                  }),
                  r("th", {
                    className: e(
                      "px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center",
                    ),
                    children: t.headingActions,
                  }),
                ],
              }),
            }),
            a("tbody", {
              className: e("divide-y divide-white/5"),
              children: [
                r(f, {
                  colSpan: 6,
                  isLoading: i,
                  isEmpty: !i && n.length === 0,
                  loadingLabel: t.loading,
                  emptyLabel: t.empty,
                  loadingIcon: r(h, {
                    className: e(
                      "w-8 h-8 animate-spin text-[var(--purple-accent)]",
                    ),
                  }),
                  emptyIcon: r("div", {
                    className: e(
                      "w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center",
                    ),
                    children: r(x, {
                      className: e(
                        "w-8 h-8 text-[var(--text-tertiary)] opacity-50",
                      ),
                    }),
                  }),
                }),
                i
                  ? null
                  : n.map((o) =>
                      r(
                        A,
                        {
                          locale: p,
                          order: o,
                          processingId: c,
                          labels: {
                            systemUser: t.systemUser,
                            userIdPrefix: t.userIdPrefix,
                            statusSuccess: t.statusSuccess,
                            statusFailed: t.statusFailed,
                            statusPending: t.statusPending,
                            approveTitle: t.approveTitle,
                            rejectTitle: t.rejectTitle,
                            notAvailable: t.notAvailable,
                          },
                          onApprove: m,
                          onReject: g,
                        },
                        o.id,
                      ),
                    ),
              ],
            }),
          ],
        }),
      }),
      r(y, {
        summary: t.summary,
        currentLabel: String(s),
        canPrev: s > 1,
        canNext: s * 10 < d,
        onPrev: l,
        onNext: u,
      }),
    ],
  });
}
export { P as AdminDepositsTable };
