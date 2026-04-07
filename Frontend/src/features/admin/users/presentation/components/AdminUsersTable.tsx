import { jsx as t, jsxs as s } from "react/jsx-runtime";
import { Loader2 as d, Users as u } from "lucide-react";
import {
  GlassCard as b,
  StepPagination as h,
  TableStates as y,
} from "@/shared/components/ui";
import { cn as e } from "@/lib/utils";
import { AdminUserTableRow as f } from "./AdminUserTableRow";
function w({
  loading: l,
  locale: o,
  onEdit: p,
  onNextPage: m,
  onPrevPage: x,
  page: r,
  t: a,
  totalCount: n,
  users: i,
}) {
  return s(b, {
    className: e("!p-0 !rounded-[2.5rem] overflow-hidden"),
    children: [
      t("div", {
        className: e("overflow-x-auto custom-scrollbar"),
        children: s("table", {
          className: e("w-full text-left"),
          children: [
            t("thead", {
              children: s("tr", {
                className: e("border-b tn-border-soft tn-surface"),
                children: [
                  t("th", {
                    className: e(
                      "px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]",
                    ),
                    children: a("users.table.heading_account"),
                  }),
                  t("th", {
                    className: e(
                      "px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]",
                    ),
                    children: a("users.table.heading_rank"),
                  }),
                  t("th", {
                    className: e(
                      "px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]",
                    ),
                    children: a("users.table.heading_assets"),
                  }),
                  t("th", {
                    className: e(
                      "px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]",
                    ),
                    children: a("users.table.heading_role"),
                  }),
                  t("th", {
                    className: e(
                      "px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center",
                    ),
                    children: a("users.table.heading_status"),
                  }),
                  t("th", {
                    className: e(
                      "px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-right",
                    ),
                    children: a("users.table.heading_actions"),
                  }),
                ],
              }),
            }),
            s("tbody", {
              className: e("divide-y divide-white/5"),
              children: [
                t(y, {
                  colSpan: 6,
                  isLoading: l,
                  isEmpty: !l && i.length === 0,
                  loadingLabel: a("users.states.loading"),
                  emptyLabel: a("users.states.empty"),
                  loadingIcon: t(d, {
                    className: e(
                      "w-8 h-8 animate-spin text-[var(--purple-accent)]",
                    ),
                  }),
                  emptyIcon: t("div", {
                    className: e(
                      "w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center",
                    ),
                    children: t(u, {
                      className: e(
                        "w-8 h-8 text-[var(--text-tertiary)] opacity-50",
                      ),
                    }),
                  }),
                }),
                l
                  ? null
                  : i.map((c) =>
                      t(f, { user: c, locale: o, onEdit: p, t: a }, c.id),
                    ),
              ],
            }),
          ],
        }),
      }),
      t(h, {
        summary: a("users.pagination.summary", { page: r, total: n }),
        currentLabel: String(r),
        canPrev: r > 1,
        canNext: r * 10 < n,
        onPrev: x,
        onNext: m,
      }),
    ],
  });
}
export { w as AdminUsersTable };
