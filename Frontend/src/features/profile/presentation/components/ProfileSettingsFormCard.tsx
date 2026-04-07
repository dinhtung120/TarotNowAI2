"use client";
import { Fragment as o, jsx as a, jsxs as t } from "react/jsx-runtime";
import {
  Calendar as f,
  Loader2 as g,
  Save as u,
  ShieldCheck as m,
  Sparkles as x,
  User as b,
} from "lucide-react";
import { Button as v, GlassCard as N } from "@/shared/components/ui";
import { cn as e } from "@/lib/utils";
function S({
  t: s,
  successMsg: r,
  errorMsg: l,
  register: n,
  handleSubmit: c,
  errors: i,
  isSubmitting: d,
  onSubmit: p,
}) {
  return t(N, {
    className: e("!p-6 sm:!p-8"),
    children: [
      t("h3", {
        className: e(
          "text-lg font-black tn-text-primary italic tracking-tight mb-8 flex items-center gap-2.5",
        ),
        children: [
          a(x, { className: e("w-4 h-4 text-[var(--warning)]") }),
          s("settings_title"),
        ],
      }),
      t("div", {
        className: e("space-y-3 mb-8"),
        children: [
          r
            ? t("div", {
                className: e(
                  "animate-in slide-in-from-top-2 duration-500 bg-[var(--success-bg)] border border-[var(--success)]/30 p-4 rounded-xl flex items-center gap-3 text-[var(--success)] text-xs font-bold uppercase tracking-widest",
                ),
                children: [a(m, { className: e("w-4 h-4") }), r],
              })
            : null,
          l
            ? t("div", {
                className: e(
                  "animate-in slide-in-from-top-2 duration-500 bg-[var(--danger-bg)] border border-[var(--danger)]/30 p-4 rounded-xl flex items-center gap-3 text-[var(--danger)] text-xs font-bold uppercase tracking-widest",
                ),
                children: [a(m, { className: e("w-4 h-4") }), l],
              })
            : null,
        ],
      }),
      t("form", {
        onSubmit: c(p),
        className: e("space-y-6"),
        children: [
          t("div", {
            className: e("grid grid-cols-1 md:grid-cols-2 gap-6"),
            children: [
              t("div", {
                className: e("space-y-2"),
                children: [
                  t("label", {
                    className: e(
                      "flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] ml-1",
                    ),
                    children: [
                      a(b, { className: e("w-3.5 h-3.5") }),
                      s("displayName"),
                    ],
                  }),
                  a("input", {
                    type: "text",
                    ...n("displayName"),
                    className: e(
                      "w-full tn-field rounded-xl px-4 py-3.5 text-sm tn-text-primary font-medium tn-field-accent transition-all shadow-inner",
                    ),
                    placeholder: s("display_name_placeholder"),
                  }),
                  i.displayName
                    ? a("p", {
                        className: e(
                          "text-[var(--danger)] text-[10px] font-bold ml-1 italic",
                        ),
                        children: i.displayName.message,
                      })
                    : null,
                ],
              }),
              t("div", {
                className: e("space-y-2"),
                children: [
                  t("label", {
                    className: e(
                      "flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] ml-1",
                    ),
                    children: [
                      a(f, { className: e("w-3.5 h-3.5") }),
                      s("dateOfBirth"),
                    ],
                  }),
                  a("input", {
                    type: "date",
                    ...n("dateOfBirth"),
                    className: e(
                      "w-full tn-field rounded-xl px-4 py-3.5 text-sm tn-text-primary font-medium tn-field-accent transition-all shadow-inner",
                    ),
                  }),
                  i.dateOfBirth
                    ? a("p", {
                        className: e(
                          "text-[var(--danger)] text-[10px] font-bold ml-1 italic",
                        ),
                        children: i.dateOfBirth.message,
                      })
                    : null,
                ],
              }),
            ],
          }),
          a("div", {
            className: e("pt-4 mt-8 border-t tn-border"),
            children: a(v, {
              variant: "primary",
              type: "submit",
              disabled: d,
              className: e("w-full h-12"),
              children: d
                ? t(o, {
                    children: [
                      a(g, { className: e("w-4 h-4 animate-spin mr-2") }),
                      s("saving"),
                    ],
                  })
                : t(o, {
                    children: [
                      a(u, { className: e("w-4 h-4 mr-2") }),
                      s("save"),
                    ],
                  }),
            }),
          }),
        ],
      }),
    ],
  });
}
export { S as ProfileSettingsFormCard };
