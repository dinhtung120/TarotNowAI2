"use client";
import { jsx as a, jsxs as n } from "react/jsx-runtime";
import {
  BookOpen as u,
  Calendar as r,
  Search as g,
  User as v,
} from "lucide-react";
import { useTranslations as y } from "next-intl";
import { Button as b } from "@/shared/components/ui";
import { cn as e } from "@/lib/utils";
function k({
  username: l,
  spreadType: i,
  startDate: o,
  endDate: d,
  onUsernameChange: p,
  onSpreadTypeChange: c,
  onStartDateChange: m,
  onEndDateChange: f,
  onSubmit: x,
}) {
  const t = y("Admin");
  return n("form", {
    onSubmit: x,
    className: e(
      "p-6 rounded-[2.5rem] bg-[var(--purple-accent)]/5 border tn-border-soft shadow-inner flex flex-wrap items-end gap-6",
    ),
    children: [
      n("div", {
        className: e("flex-1 min-w-[240px] space-y-3 text-left"),
        children: [
          n("label", {
            className: e(
              "text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] flex items-center gap-2",
            ),
            children: [
              a(v, { className: e("w-3.5 h-3.5") }),
              t("readings.filters.username_label"),
            ],
          }),
          a("input", {
            type: "text",
            value: l,
            onChange: (s) => p(s.target.value),
            placeholder: t("readings.filters.username_placeholder"),
            className: e(
              "w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all placeholder:text-[var(--text-tertiary)] shadow-inner",
            ),
          }),
        ],
      }),
      n("div", {
        className: e("w-56 space-y-3 text-left"),
        children: [
          n("label", {
            className: e(
              "text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] flex items-center gap-2",
            ),
            children: [
              a(u, { className: e("w-3.5 h-3.5") }),
              t("readings.filters.spread_label"),
            ],
          }),
          n("select", {
            value: i,
            onChange: (s) => c(s.target.value),
            className: e(
              "w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all appearance-none cursor-pointer shadow-inner",
            ),
            children: [
              a("option", {
                value: "",
                className: e("tn-surface"),
                children: t("readings.filters.spread_all"),
              }),
              a("option", {
                value: "daily_1",
                className: e("tn-surface"),
                children: t("readings.filters.spread_daily"),
              }),
              a("option", {
                value: "spread_3",
                className: e("tn-surface"),
                children: t("readings.filters.spread_3"),
              }),
              a("option", {
                value: "spread_5",
                className: e("tn-surface"),
                children: t("readings.filters.spread_5"),
              }),
              a("option", {
                value: "spread_10",
                className: e("tn-surface"),
                children: t("readings.filters.spread_10"),
              }),
            ],
          }),
        ],
      }),
      n("div", {
        className: e("w-44 space-y-3 text-left"),
        children: [
          n("label", {
            className: e(
              "text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] flex items-center gap-2",
            ),
            children: [
              a(r, { className: e("w-3.5 h-3.5") }),
              t("readings.filters.start_date_label"),
            ],
          }),
          a("input", {
            type: "date",
            value: o,
            onChange: (s) => m(s.target.value),
            className: e(
              "w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all shadow-inner",
            ),
          }),
        ],
      }),
      n("div", {
        className: e("w-44 space-y-3 text-left"),
        children: [
          n("label", {
            className: e(
              "text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] flex items-center gap-2",
            ),
            children: [
              a(r, { className: e("w-3.5 h-3.5") }),
              t("readings.filters.end_date_label"),
            ],
          }),
          a("input", {
            type: "date",
            value: d,
            onChange: (s) => f(s.target.value),
            className: e(
              "w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all shadow-inner",
            ),
          }),
        ],
      }),
      n(b, {
        type: "submit",
        variant: "primary",
        className: e(
          "px-8 py-4 shrink-0 shadow-md flex items-center justify-center min-w-[140px]",
        ),
        children: [
          a(g, { className: e("w-4 h-4") }),
          t("readings.filters.submit"),
        ],
      }),
    ],
  });
}
export { k as AdminReadingsFilters };
