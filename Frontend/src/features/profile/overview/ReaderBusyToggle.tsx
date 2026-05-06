"use client";

import { Loader2 } from "lucide-react";
import { useReaderBusyToggleState } from "@/features/profile/overview/useReaderBusyToggleState";
import { cn } from "@/lib/utils";

export default function ReaderBusyToggle() {
  const { handleToggle, isBusy, isLoading, statusText, title } =
    useReaderBusyToggleState();

  return (
    <div
      className={cn(
        "ml-auto inline-flex w-fit items-center gap-3 transition-all",
      )}
      title={title}
    >
      <span
        className={cn(
          "text-[10px] font-black tracking-widest text-[var(--text-secondary)] uppercase",
        )}
      >
        Trạng thái:{" "}
        <span
          className={cn(
            "ml-1",
            isBusy ? "text-[var(--warning)]" : "text-[var(--success)]",
          )}
        >
          {statusText}
        </span>
      </span>
      <button
        role="switch"
        type="button"
        aria-checked={isBusy}
        onClick={() => void handleToggle()}
        disabled={isLoading}
        className={cn(
          "focus-visible:ring-opacity-75 relative inline-flex h-6 w-11 shrink-0 cursor-pointer items-center rounded-full border-2 border-transparent transition-colors duration-300 ease-in-out focus:outline-none focus-visible:ring-2 focus-visible:ring-offset-2",
          isBusy
            ? "bg-[var(--warning)] focus-visible:ring-[var(--warning)]"
            : "bg-[var(--success)] focus-visible:ring-[var(--success)]",
          isLoading
            ? "cursor-not-allowed opacity-50"
            : "hover:brightness-110 active:scale-95",
        )}
      >
        <span className={cn("sr-only")}>Toggle Busy Status</span>
        <span
          className={cn(
            "pointer-events-none flex h-5 w-5 transform items-center justify-center rounded-full bg-white shadow-lg ring-0 transition duration-300 ease-in-out",
            isBusy ? "translate-x-5" : "translate-x-0",
          )}
        >
          {isLoading ? (
            <Loader2 className={cn("h-3 w-3 animate-spin text-slate-500")} />
          ) : null}
        </span>
      </button>
    </div>
  );
}
