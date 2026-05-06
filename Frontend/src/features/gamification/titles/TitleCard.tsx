"use client";

import { CheckCircle2, Lock, Medal } from "lucide-react";
import type { TitleDefinition } from "@/features/gamification/shared/gamification.types";
import { rarityColors } from "@/features/gamification/titles/components/title-card/rarityColors";
import { cn } from "@/lib/utils";

interface TitleCardProps {
 title: TitleDefinition;
 isOwned: boolean;
 isActive: boolean;
 localize: (vi: string, en: string) => string;
 hiddenLabel: string;
 onClick: () => void;
}

export function TitleCard({ title, isOwned, isActive, localize, hiddenLabel, onClick }: TitleCardProps) {
 const style = rarityColors[title.rarity] || rarityColors.Common;
 const ringClass = style.ring.replace("border-", "ring-").replace("/40", "/30");

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
    isActive
     ? cn("bg-gradient-to-b", "from-slate-800/80", style.bg, style.ring, "ring-1", ringClass, "scale-[1.02]", "shadow-lg")
     : isOwned
      ? cn("border-slate-700", "bg-slate-900/60")
      : cn("border-slate-800", "bg-slate-900/40", "grayscale", "opacity-60"),
   )}
  >
   <div className={cn("absolute", "inset-0", "bg-gradient-to-b", "from-white/5", "to-transparent", "opacity-0", "transition-opacity")} />
   <div className={cn("relative", "mb-4")}>
    <div className={cn("flex", "h-16", "w-16", "items-center", "justify-center", "rounded-full", "border-2", "transition-all", "duration-500", isActive ? style.iconBg : isOwned ? "border-slate-700 bg-slate-800" : "border-slate-800 bg-slate-900")}>
     {isActive ? (
      <CheckCircle2 className={cn("h-8", "w-8", style.iconText)} />
     ) : isOwned ? (
      <Medal className={cn("h-6", "w-6", "text-slate-500", "transition-colors")} />
     ) : (
      <Lock className={cn("h-6", "w-6", "text-slate-600")} />
     )}
    </div>
   </div>
   <h3 className={cn("mb-1", "text-sm", "font-bold", isActive ? style.textTitle : isOwned ? "text-slate-300" : "text-slate-500")}>{localize(title.nameVi, title.nameEn)}</h3>
   <p className={cn("mt-1", "line-clamp-3", "text-xs", "leading-relaxed", "opacity-80", isActive ? style.text : "text-slate-400")}>
    {isOwned ? localize(title.descriptionVi, title.descriptionEn) : hiddenLabel}
   </p>
   <div className={cn("mt-auto", "pt-4", "text-xs", "font-semibold", "uppercase", "tracking-wider", "text-slate-500")}>{title.rarity}</div>
  </div>
 );
}
