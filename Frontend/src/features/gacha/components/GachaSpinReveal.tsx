"use client";

import { cn } from "@/lib/utils";
import type { SpinGachaResult } from "@/features/gacha/gacha.types";
import Button from "@/shared/components/ui/Button";
import Modal from "@/shared/components/ui/Modal";
import { GachaResultItemCard } from "./GachaResultItemCard";
import { useGachaSpinRevealState } from "@/features/gacha/components/hooks/useGachaSpinRevealState";

interface GachaSpinRevealProps {
 result: SpinGachaResult | null;
 isOpen: boolean;
 onClose: () => void;
}

export function GachaSpinReveal({ result, isOpen, onClose }: GachaSpinRevealProps) {
 const vm = useGachaSpinRevealState({ isOpen, result });
 const isLargeBatch = (result?.items.length ?? 0) >= 8;
 const multiGridClass = isLargeBatch
  ? "grid grid-cols-3 gap-2 sm:grid-cols-3 md:grid-cols-5 md:gap-4"
  : "grid grid-cols-2 gap-3 sm:grid-cols-3 md:grid-cols-5 md:gap-4";

 if (!result?.items?.length) return null;

 return (
  <Modal isOpen={isOpen} onClose={() => vm.phase === "revealed" && onClose()} title={vm.t("spinResult")} size="lg" showCloseButton={vm.phase === "revealed"}>
   <div className={cn("flex", "flex-col", "items-center", "justify-center", result.items.length > 1 ? "w-full" : "mx-auto max-w-2xl", vm.phase === "firing" ? "min-h-96" : "min-h-0")}>
    {vm.phase === "firing" ? (
     <div className={cn("flex", "flex-col", "items-center", "justify-center", "animate-pulse")}>
      <div className={cn("mb-6", "h-24", "w-24", "animate-spin", "rounded-full", "border-4", "border-indigo-500", "border-t-transparent")} />
      <p className={cn("text-xl", "font-black", "uppercase", "tracking-widest", "text-indigo-400")}>{vm.t("revealing")}...</p>
     </div>
    ) : null}
    {vm.phase === "revealed" ? (
     <div className={cn("flex", "h-full", "w-full", "flex-col", "items-center")}>
      {result.wasPityTriggered ? (
       <p className={cn("pulse", "mt-2", "mb-6", "rounded-full", "border", "border-amber-500/20", "bg-amber-500/10", "px-4", "py-2", "text-xs", "font-black", "uppercase", "tracking-widest", "text-amber-500")}>
        🌟 {vm.t("pityTriggered")}
       </p>
      ) : null}
      <div className={cn("w-full", result.items.length > 1 ? multiGridClass : "flex justify-center", isLargeBatch ? "max-h-[52dvh] overflow-y-auto pr-1 sm:max-h-none sm:overflow-visible sm:pr-0" : null)}>
       {result.items.map((item, index) => (
        <GachaResultItemCard key={`${index}-${item.rewardType}-${item.rewardValue}-${item.displayIcon ?? "no-icon"}`} item={item} isSingle={result.items.length === 1} isVi={vm.isVi} />
       ))}
      </div>
      <Button variant="brand" size="lg" className={cn("mt-6", "w-full", "sm:mt-10", "sm:w-auto", "sm:min-w-xs")} onClick={onClose}>
       {vm.t("awesome")}
      </Button>
     </div>
    ) : null}
   </div>
  </Modal>
 );
}
