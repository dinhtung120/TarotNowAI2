"use client";
import { jsx as t, jsxs as l } from "react/jsx-runtime";
import { cn as e } from "@/lib/utils";
function p({
  isVideo: o,
  activeTitle: r,
  durationLabel: i,
  minimizeLabel: a,
  endCallLabel: n,
  liveLabel: s,
  remoteVideoRef: d,
  localVideoRef: c,
  onMinimize: m,
  onEndCall: u,
}) {
  return l("div", {
    className: e(
      "fixed inset-x-0 top-[70px] bottom-0 z-40 bg-black flex flex-col",
    ),
    children: [
      l("div", {
        className: e(
          "absolute top-0 left-0 right-0 z-20 flex justify-between items-start p-5 bg-gradient-to-b from-black/70 to-transparent pointer-events-none",
        ),
        children: [
          l("div", {
            className: e("text-white drop-shadow-md pointer-events-auto"),
            children: [
              t("h2", {
                className: e("text-lg font-medium opacity-80 mb-1"),
                children: r,
              }),
              t("div", {
                className: e("font-mono text-xl tracking-wider"),
                children: i,
              }),
            ],
          }),
          t("button", {
            type: "button",
            onClick: m,
            className: e(
              "pointer-events-auto w-10 h-10 bg-white/10 hover:bg-white/20 rounded-full flex items-center justify-center backdrop-blur-sm transition-colors cursor-pointer mr-[140px]",
            ),
            title: a,
            children: t("svg", {
              xmlns: "http://www.w3.org/2000/svg",
              className: e("h-6 w-6 text-white"),
              fill: "none",
              viewBox: "0 0 24 24",
              stroke: "currentColor",
              children: t("path", {
                strokeLinecap: "round",
                strokeLinejoin: "round",
                strokeWidth: 2,
                d: "M4 8V4m0 0h4M4 4l5 5m11-1V4m0 0h-4m4 0l-5 5M4 16v4m0 0h4m-4 0l5-5m11 5l-5-5m5 5v-4m0 4h-4",
              }),
            }),
          }),
        ],
      }),
      t("div", {
        className: e(
          "flex-1 relative flex items-center justify-center overflow-hidden",
        ),
        children: o
          ? t("video", {
              ref: d,
              autoPlay: !0,
              playsInline: !0,
              muted: !0,
              className: e("w-full h-full object-cover"),
            })
          : t("div", {
              className: e("flex flex-col items-center"),
              children: t("div", {
                className: e(
                  "w-32 h-32 rounded-full bg-indigo-500/30 flex items-center justify-center animate-pulse shadow-[0_0_40px_rgba(99,102,241,0.5)] mb-6",
                ),
                children: t("span", {
                  className: e("text-white text-5xl"),
                  children: "\u{1F52E}",
                }),
              }),
            }),
      }),
      o
        ? t("div", {
            className: e(
              "absolute top-2.5 right-2.5 w-32 h-44 bg-gray-800 rounded-xl overflow-hidden shadow-[0_0_30px_rgba(0,0,0,0.8)] border border-white/20 z-50",
            ),
            children: t("video", {
              ref: c,
              autoPlay: !0,
              playsInline: !0,
              muted: !0,
              className: e("w-full h-full object-cover shadow-inner"),
            }),
          })
        : null,
      t("div", {
        className: e(
          "absolute bottom-6 left-0 right-0 flex items-center justify-center z-50",
        ),
        children: t("button", {
          type: "button",
          onClick: u,
          className: e(
            "w-16 h-16 rounded-full bg-red-600 flex items-center justify-center shadow-[0_0_30px_rgba(220,38,38,0.6)] hover:bg-red-500 transition-all hover:scale-105 active:scale-95 cursor-pointer",
          ),
          title: n,
          children: t("svg", {
            className: e("w-8 h-8 text-white"),
            fill: "none",
            viewBox: "0 0 24 24",
            stroke: "currentColor",
            children: t("path", {
              strokeLinecap: "round",
              strokeLinejoin: "round",
              strokeWidth: 2,
              d: "M6 18L18 6M6 6l12 12",
            }),
          }),
        }),
      }),
      t("div", {
        className: e(
          "absolute top-5 right-5 px-3 py-1 bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/20 rounded-full",
        ),
        children: t("span", {
          className: e(
            "text-[10px] text-[var(--purple-accent)] font-bold uppercase tracking-widest",
          ),
          children: s,
        }),
      }),
    ],
  });
}
export { p as CallOverlayFullscreen };
