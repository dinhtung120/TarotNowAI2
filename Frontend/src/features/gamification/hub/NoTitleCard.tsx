'use client';

import { CheckCircle2, Medal } from 'lucide-react';
import { cn } from '@/lib/utils';

interface NoTitleCardProps {
  isNoTitleActive: boolean;
  noTitleLabel: string;
  hintLabel: string;
  onClick: () => void;
}

export function NoTitleCard({ isNoTitleActive, noTitleLabel, hintLabel, onClick }: NoTitleCardProps) {
  return (
    <div
      onClick={onClick}
      className={cn(
        "group",
        "relative",
        "cursor-pointer",
        "overflow-hidden",
        "rounded-2xl",
        "border",
        "p-5",
        "text-center",
        "transition-all",
        "duration-500",
        "backdrop-blur-xl",
        "flex",
        "flex-col",
        "items-center",
        isNoTitleActive
          ? "border-blue-500/40 bg-gradient-to-b from-slate-800/80 to-blue-900/20 shadow-lg scale-[1.02] ring-1 ring-blue-500/30"
          : "border-slate-800 bg-slate-900/60",
      )}
    >
      <div className={cn("absolute inset-0 tn-radial-overlay-soft tn-group-fade-in-overlay")} />

      <div className={cn("relative mb-4")}>
        <div
          className={cn(
            "flex",
            "h-16",
            "w-16",
            "items-center",
            "justify-center",
            "rounded-full",
            "border-2",
            isNoTitleActive
              ? "border-blue-400/50 bg-blue-400/10 shadow-lg"
              : "border-slate-700 bg-slate-800",
          )}
        >
          {isNoTitleActive ? (
            <CheckCircle2 className={cn("h-8", "w-8", "text-blue-400")} />
          ) : (
            <Medal className={cn("h-6", "w-6", "text-slate-600")} />
          )}
        </div>
      </div>

      <h3 className={cn("mb-1", "text-sm", "font-bold", isNoTitleActive ? "text-blue-200" : "text-slate-400")}>{noTitleLabel}</h3>

      <p className={cn("text-xs text-slate-500 line-clamp-3 leading-relaxed mt-1")}>{hintLabel}</p>
    </div>
  );
}
