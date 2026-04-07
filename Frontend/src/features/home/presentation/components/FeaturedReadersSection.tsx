import { jsx as a, jsxs as r } from "react/jsx-runtime";
import { Link as n } from "@/i18n/routing";
import u from "next/image";
import { ArrowUpRight as p, Gem as v, Star as f } from "lucide-react";
import { getTranslations as c } from "next-intl/server";
import { Suspense as g } from "react";
import { listFeaturedReaders as b } from "@/features/reader/public";
import { normalizeReaderStatus as x } from "@/features/reader/domain/readerStatus";
import { Badge as h, SectionHeader as w } from "@/shared/components/ui";
import { cn as e } from "@/lib/utils";
function d() {
  return a("div", {
    className: e("grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8"),
    children: Array.from({ length: 4 }).map((s, i) =>
      a(
        "div",
        {
          className: e(
            "h-96 rounded-[2.5rem] border border-[var(--border-default)] bg-[var(--bg-surface)] animate-pulse",
          ),
        },
        `featured-reader-skeleton-${i}`,
      ),
    ),
  });
}
async function N() {
  const s = await c("Index"),
    i = await b(4),
    o = i.success && i.data ? i.data : [];
  return o.length === 0
    ? a(d, {})
    : a("div", {
        className: e("grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8"),
        children: o.map((t) => {
          const l = x(t.status),
            m =
              l === "online"
                ? "bg-[var(--success)] animate-pulse"
                : l === "busy"
                  ? "bg-[var(--warning)]"
                  : "bg-[var(--text-muted)]";
          return r(
            n,
            {
              href: `/readers/${t.userId}`,
              className: e(
                "group relative h-96 rounded-[2.5rem] overflow-hidden border border-[var(--border-default)] bg-[var(--bg-surface)] hover:border-[var(--border-focus)] transition-all duration-700 hover:-translate-y-4 shadow-[var(--shadow-card)] preserve-3d",
              ),
              children: [
                t.avatarUrl
                  ? a(u, {
                      src: t.avatarUrl,
                      alt: t.displayName,
                      fill: !0,
                      sizes:
                        "(max-width: 640px) 100vw, (max-width: 1024px) 50vw, 25vw",
                      unoptimized: !0,
                      className: e(
                        "absolute inset-0 w-full h-full object-cover z-0 transition-transform duration-700 group-hover:scale-110",
                      ),
                    })
                  : a("div", {
                      className: e(
                        "absolute inset-0 w-full h-full z-0 flex items-center justify-center bg-gradient-to-br from-[var(--purple-accent)]/20 to-[var(--bg-surface)]",
                      ),
                      children: a("span", {
                        className: e(
                          "text-6xl font-black text-[var(--text-muted)]/30 italic uppercase select-none",
                        ),
                        children: t.displayName?.charAt(0) || "?",
                      }),
                    }),
                a("div", {
                  className: e(
                    "absolute inset-0 bg-gradient-to-t from-[var(--bg-void)] via-[var(--bg-void)]/40 to-transparent z-10",
                  ),
                }),
                a("div", {
                  className: e(
                    "absolute inset-0 bg-[var(--purple-glow)] opacity-0 group-hover:opacity-100 transition-opacity duration-700 z-10",
                  ),
                }),
                r("div", {
                  className: e(
                    "absolute inset-x-0 bottom-0 p-8 z-20 space-y-4",
                  ),
                  children: [
                    r("div", {
                      className: e("flex items-center justify-between"),
                      children: [
                        a("div", { className: e("w-3 h-3 rounded-full", m) }),
                        r(h, {
                          variant: "default",
                          size: "sm",
                          className: e(
                            "bg-[var(--bg-glass)] border-[var(--border-default)]",
                          ),
                          children: [
                            a(f, {
                              className: e(
                                "w-3 h-3 text-[var(--amber-accent)] fill-[var(--amber-accent)]",
                              ),
                            }),
                            a("span", {
                              className: e("tn-text-primary"),
                              children: t.avgRating.toFixed(1),
                            }),
                          ],
                        }),
                      ],
                    }),
                    r("div", {
                      children: [
                        a("h3", {
                          className: e(
                            "text-xl font-black text-[var(--text-ink)] tracking-tighter uppercase italic truncate",
                          ),
                          children: t.displayName,
                        }),
                        a("div", {
                          className: e(
                            "text-[9px] font-black uppercase tracking-[0.2em] text-[var(--purple-accent)] mt-1 line-clamp-1",
                          ),
                          children: t.specialties.join(" \u2022 "),
                        }),
                      ],
                    }),
                    r("div", {
                      className: e(
                        "pt-4 border-t border-[var(--border-subtle)] flex items-center justify-between",
                      ),
                      children: [
                        r("div", {
                          className: e("flex items-center gap-1.5"),
                          children: [
                            a(v, {
                              className: e(
                                "w-3.5 h-3.5 text-[var(--amber-accent)]",
                              ),
                            }),
                            r("span", {
                              className: e(
                                "text-xs font-black text-[var(--text-ink)]",
                              ),
                              children: [t.diamondPerQuestion, " \u{1F48E}"],
                            }),
                          ],
                        }),
                        a("div", {
                          className: e(
                            "text-[10px] font-black uppercase tracking-widest text-[var(--purple-accent)] group-hover:translate-x-2 transition-transform",
                          ),
                          children: s("showcase.profileCta"),
                        }),
                      ],
                    }),
                  ],
                }),
              ],
            },
            t.userId,
          );
        }),
      });
}
async function L() {
  const s = await c("Index");
  return r("section", {
    className: e("relative w-full max-w-7xl mx-auto px-4 sm:px-6 py-32"),
    children: [
      a(w, {
        tag: s("showcase.tag"),
        title: s("showcase.title"),
        titleMuted: s("showcase.titleMuted"),
        action: r(n, {
          href: "/readers",
          className: e(
            "text-xs font-black uppercase tracking-widest text-[var(--text-secondary)] hover:text-[var(--text-ink)] transition-colors inline-flex items-center gap-2 group min-h-11 px-2 rounded-xl hover:bg-[var(--purple-50)]",
          ),
          children: [
            s("showcase.viewAll"),
            a(p, {
              className: e(
                "w-4 h-4 transition-transform group-hover:translate-x-1 group-hover:-translate-y-1",
              ),
            }),
          ],
        }),
        className: e("mb-20"),
      }),
      a(g, { fallback: a(d, {}), children: a(N, {}) }),
    ],
  });
}
export { L as FeaturedReadersSection };
