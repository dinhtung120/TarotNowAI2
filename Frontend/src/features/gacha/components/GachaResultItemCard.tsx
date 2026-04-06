"use client";

import { useState } from "react";
import Image from "next/image";
import { Award, Coins, Diamond, Sparkles } from "lucide-react";
import { cn } from "@/shared/utils/cn";
import type { SpinGachaItemResult } from "../gacha.types";

interface GachaResultItemCardProps {
  item: SpinGachaItemResult;
  isSingle: boolean;
  isVi: boolean;
}

const RARITY_CONFIG: Record<string, { glow: string; text: string; bg: string; border: string }> = {
  legendary: {
    glow: "shadow-[0_0_80px_rgba(245,158,11,0.5)]",
    text: "text-amber-400",
    bg: "bg-gradient-to-br from-amber-500/20 via-amber-900/40 to-black/60",
    border: "border-amber-400/60",
  },
  epic: {
    glow: "shadow-[0_0_60px_rgba(168,85,247,0.4)]",
    text: "text-purple-400",
    bg: "bg-gradient-to-br from-purple-500/20 via-purple-900/40 to-black/60",
    border: "border-purple-400/60",
  },
  rare: {
    glow: "shadow-[0_0_40px_rgba(59,130,246,0.3)]",
    text: "text-blue-400",
    bg: "bg-gradient-to-br from-blue-500/20 via-blue-900/40 to-black/60",
    border: "border-blue-400/60",
  },
  common: {
    glow: "shadow-[0_0_20px_rgba(168,162,158,0.2)]",
    text: "text-stone-300",
    bg: "bg-stone-800/40",
    border: "border-stone-700",
  },
};

export function GachaResultItemCard({ item, isSingle, isVi }: GachaResultItemCardProps) {
  const [iconLoadFailed, setIconLoadFailed] = useState(false);
  const name = isVi ? item.displayNameVi : item.displayNameEn;
  const config = RARITY_CONFIG[item.rarity.toLowerCase()] || RARITY_CONFIG.common;

  const renderFallbackIcon = (sizeClass: string) => {
    const iconProps = { className: cn(sizeClass, config.text) };
    if (item.rewardType === "diamond") return <Diamond {...iconProps} />;
    if (item.rewardType === "gold") return <Coins {...iconProps} />;
    if (item.rewardType === "title") return <Award {...iconProps} />;
    return <Sparkles {...iconProps} />;
  };

  const renderRewardIcon = (sizeClass: string) => {
    if (item.displayIcon && !iconLoadFailed) {
      return (
        <Image
          src={`/images/gacha/${item.displayIcon}`}
          alt={name}
          width={192}
          height={192}
          unoptimized
          className={cn(sizeClass, "object-contain drop-shadow-2xl")}
          onError={() => setIconLoadFailed(true)}
        />
      );
    }

    return renderFallbackIcon(sizeClass);
  };

  if (isSingle) {
    return (
      <div
        className={cn(
          "w-full max-w-2xl min-h-[450px] md:min-h-0 md:h-72 rounded-[2.5rem] p-1 flex flex-col md:flex-row items-center relative border-2 backdrop-blur-xl transition-all animate-in zoom-in-95 duration-700",
          config.glow,
          config.bg,
          config.border,
        )}
      >
        <div className="w-full md:w-1/2 h-64 md:h-full flex items-center justify-center p-8 select-none pointer-events-none">
          {renderRewardIcon(
            "w-full h-full transform scale-125 md:scale-150 transition-transform duration-1000 rotate-3 hover:rotate-0",
          )}
        </div>

        <div className="w-full md:w-1/2 h-40 md:h-full flex flex-col justify-center px-8 relative">
          <div className="absolute top-4 md:top-8 right-8">
            <span
              className={cn(
                "px-4 py-1 rounded-full border text-[10px] uppercase font-black tracking-[0.2em] shadow-inner",
                config.text,
                config.border,
                "bg-black/40",
              )}
            >
              {item.rarity}
            </span>
          </div>

          <div className="absolute bottom-6 md:bottom-8 right-8 text-right space-y-1">
            <p className="text-[10px] font-black uppercase tracking-[0.3em] opacity-40 text-stone-200">Reward Unlocked</p>
            <h3 className={cn("text-3xl md:text-4xl font-extrabold italic tracking-tighter uppercase drop-shadow-lg", config.text)}>
              {name}
            </h3>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div
      className={cn(
        "rounded-2xl p-4 flex flex-col items-center justify-center relative border backdrop-blur-md animate-in zoom-in-75 duration-300 group",
        config.glow,
        config.bg,
        config.border,
        "w-full aspect-[4/5]",
      )}
    >
      <div className="absolute top-2 right-2">
        <span className={cn("text-[8px] font-black uppercase px-2 py-0.5 rounded-full border bg-black/50", config.text, config.border)}>
          {item.rarity[0]}
        </span>
      </div>

      {renderRewardIcon("w-16 h-16 mb-4 group-hover:scale-110 transition-transform duration-500")}

      <h3 className={cn("text-center text-[10px] font-bold uppercase tracking-tight line-clamp-1", config.text)}>
        {name}
      </h3>
    </div>
  );
}
