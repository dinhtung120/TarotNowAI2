"use client";
import { jsx as t, jsxs as a } from "react/jsx-runtime";
import { RefreshCw as b, Send as f, Sparkles as g } from "lucide-react";
import { cn as e } from "@/lib/utils";
function w({
  isComplete: s,
  isHardCapReached: p,
  isStreaming: o,
  isSendingFollowup: n,
  followupText: i,
  freeSlotsRemaining: l,
  onFollowupTextChange: m,
  onSubmit: d,
  freeBadgeText: c,
  paidBadgeText: u,
  labels: r,
}) {
  return s
    ? p
      ? a("div", {
          className: e(
            "mt-4 shrink-0 text-center p-4 tn-surface-strong border tn-border-soft rounded-2xl animate-in slide-in-from-bottom-5 duration-500",
          ),
          children: [
            t("p", {
              className: e("text-sm font-medium text-[var(--warning)]"),
              children: r.hardCapTitle,
            }),
            t("p", {
              className: e("text-xs tn-text-muted mt-1"),
              children: r.hardCapDesc,
            }),
          ],
        })
      : a("form", {
          onSubmit: d,
          className: e(
            "mt-4 shrink-0 relative animate-in slide-in-from-bottom-5 duration-500",
          ),
          children: [
            t("div", {
              className: e("absolute -top-6 left-2 flex items-center gap-2"),
              children:
                l > 0
                  ? t("span", {
                      className: e(
                        "px-2.5 py-1 text-[10px] uppercase font-bold tracking-wider rounded-md bg-[var(--warning)]/20 text-[var(--warning)] border border-[var(--warning)]/30",
                      ),
                      children: c,
                    })
                  : a("span", {
                      className: e(
                        "px-2.5 py-1 text-[10px] uppercase font-bold tracking-wider rounded-md bg-[var(--purple-accent)]/20 text-[var(--purple-accent)] border border-[var(--purple-accent)]/30 flex items-center",
                      ),
                      children: [t(g, { className: e("w-3 h-3 mr-1") }), u],
                    }),
            }),
            a("div", {
              className: e("relative"),
              children: [
                t("input", {
                  type: "text",
                  value: i,
                  onChange: (x) => m(x.target.value),
                  disabled: o || n,
                  placeholder: r.placeholder,
                  className: e(
                    "w-full tn-field tn-field-accent border-[var(--purple-accent)]/30 tn-text-primary rounded-2xl px-6 py-4 pr-16 disabled:opacity-50 font-serif",
                  ),
                }),
                t("button", {
                  type: "submit",
                  disabled: !i.trim() || o || n,
                  className: e(
                    "absolute right-2 top-2 bottom-2 aspect-square bg-[var(--purple-accent)] hover:bg-[var(--purple-accent)] disabled:tn-surface-strong disabled:tn-text-muted tn-text-primary rounded-xl flex items-center justify-center transition-colors",
                  ),
                  children: n
                    ? t(b, { className: e("w-5 h-5 animate-spin") })
                    : t(f, { className: e("w-5 h-5") }),
                }),
              ],
            }),
            t("p", {
              className: e(
                "text-center text-[10px] tn-text-muted mt-2 font-mono uppercase tracking-wider",
              ),
              children: r.hint,
            }),
          ],
        })
    : null;
}
export { w as AiStreamFollowupComposer };
