"use client";
import { jsx as r, jsxs as t } from "react/jsx-runtime";
import d from "react-markdown";
import {
  AlertTriangle as m,
  RefreshCw as c,
  Sparkles as u,
} from "lucide-react";
import { cn as e } from "@/lib/utils";
function g({
  messages: l,
  error: n,
  isStreaming: o,
  bottomRef: i,
  onRetry: p,
  labels: s,
}) {
  return t("div", {
    className: e("flex-1 overflow-y-auto custom-scrollbar space-y-6 pt-4 pb-2"),
    children: [
      n
        ? t("div", {
            className: e(
              "m-6 p-4 bg-[var(--danger)]/20 border border-[var(--danger)]/30 rounded-xl flex items-start text-[var(--danger)]",
            ),
            children: [
              r(m, { className: e("w-5 h-5 mr-3 mt-0.5 shrink-0") }),
              t("div", {
                children: [
                  r("p", {
                    className: e("font-medium"),
                    children: s.errorTitle,
                  }),
                  r("p", { className: e("text-sm opacity-80"), children: n }),
                  t("button", {
                    type: "button",
                    onClick: p,
                    className: e(
                      "mt-3 flex items-center text-sm bg-[var(--danger)]/20 hover:bg-[var(--danger)]/30 px-4 py-2 rounded-lg transition",
                    ),
                    children: [
                      r(c, { className: e("w-4 h-4 mr-2") }),
                      s.errorRetry,
                    ],
                  }),
                ],
              }),
            ],
          })
        : null,
      l.length === 0 && !n && o
        ? r("div", {
            className: e(
              "h-full flex items-center justify-center text-[var(--purple-accent)]/50",
            ),
            children: t("div", {
              className: e("flex flex-col items-center"),
              children: [
                r(u, { className: e("w-10 h-10 animate-pulse mb-4") }),
                r("p", {
                  className: e("font-serif italic animate-pulse"),
                  children: s.streamingPlaceholder,
                }),
              ],
            }),
          })
        : null,
      l.map((a) =>
        r(
          "div",
          {
            className: e(
              "flex w-full",
              a.role === "user" ? "justify-end px-3 md:px-6" : "justify-start",
            ),
            children: r("div", {
              className: e(
                "flex",
                a.role === "user"
                  ? "max-w-[95%] md:max-w-[92%] flex-row-reverse"
                  : "w-full flex-row",
              ),
              children: t("div", {
                className: e(
                  "py-5",
                  a.role === "user"
                    ? "px-6 md:px-8 bg-[var(--warning)]/10 border border-[var(--warning)]/20 rounded-3xl rounded-tr-none"
                    : "w-full px-6 md:px-8 bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/20 rounded-3xl rounded-tl-none prose prose-purple max-w-none prose-p:leading-relaxed prose-p:tn-text-secondary prose-headings:font-serif prose-headings:text-[var(--warning)] prose-strong:text-[var(--purple-accent)] prose-strong:font-bold prose-em:tn-text-secondary prose-em:italic prose-li:tn-text-secondary",
                ),
                children: [
                  a.role === "user"
                    ? r("p", {
                        className: e("text-[var(--warning)]"),
                        children: a.content,
                      })
                    : r(d, { children: a.content }),
                  a.isStreaming
                    ? t("span", {
                        className: e("inline-flex space-x-1 mt-2"),
                        children: [
                          r("span", {
                            className: e(
                              "w-1.5 h-1.5 bg-[var(--purple-accent)] rounded-full animate-bounce [animation-delay:-0.3s]",
                            ),
                          }),
                          r("span", {
                            className: e(
                              "w-1.5 h-1.5 bg-[var(--purple-accent)] rounded-full animate-bounce [animation-delay:-0.15s]",
                            ),
                          }),
                          r("span", {
                            className: e(
                              "w-1.5 h-1.5 bg-[var(--purple-accent)] rounded-full animate-bounce",
                            ),
                          }),
                        ],
                      })
                    : null,
                ],
              }),
            }),
          },
          a.id,
        ),
      ),
      r("div", { ref: i, className: e("h-4") }),
    ],
  });
}
export { g as AiStreamMessages };
