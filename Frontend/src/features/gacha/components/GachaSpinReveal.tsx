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

 if (!result?.items?.length) return null;

 return (
  <Modal isOpen={isOpen} onClose={() => vm.phase === "revealed" && onClose()} title={vm.t("spinResult")} size="lg" showCloseButton={vm.phase === "revealed"}>
   <div className={cn("flex flex-col items-center justify-center min-h-[400px]", result.items.length > 1 ? "w-full" : "max-w-2xl mx-auto")}>
    {vm.phase === "firing" ? <div className="flex flex-col items-center justify-center animate-pulse"><div className="w-24 h-24 rounded-full border-4 border-indigo-500 border-t-transparent animate-spin mb-6" /><p className="text-xl font-black text-indigo-400 tracking-widest uppercase">{vm.t("revealing")}...</p></div> : null}
    {vm.phase === "revealed" ? <div className="w-full h-full flex flex-col items-center">{result.wasPityTriggered ? <p className="text-amber-500 text-xs mt-2 mb-6 font-black bg-amber-500/10 px-4 py-2 rounded-full border border-amber-500/20 uppercase tracking-widest pulse">🌟 {vm.t("pityTriggered")}</p> : null}<div className={cn("w-full", result.items.length > 1 ? "grid grid-cols-2 md:grid-cols-5 gap-4" : "flex justify-center")}>{result.items.map((item, index) => <GachaResultItemCard key={`${index}-${item.rewardType}-${item.rewardValue}-${item.displayIcon ?? "no-icon"}`} item={item} isSingle={result.items.length === 1} isVi={vm.isVi} />)}</div><Button variant="brand" size="lg" className="mt-10 min-w-xs" onClick={onClose}>{vm.t("awesome")}</Button></div> : null}
   </div>
  </Modal>
 );
}
