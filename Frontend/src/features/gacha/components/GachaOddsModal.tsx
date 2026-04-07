"use client";

import { useTranslations } from "next-intl";
import { useGachaOdds } from "@/features/gacha/hooks/useGacha";
import Badge from "@/shared/components/ui/Badge";
import Modal from "@/shared/components/ui/Modal";
import SkeletonLoader from "@/shared/components/ui/SkeletonLoader";

interface GachaOddsModalProps {
 bannerCode: string;
 isOpen: boolean;
 onClose: () => void;
}

export function GachaOddsModal({ bannerCode, isOpen, onClose }: GachaOddsModalProps) {
 const t = useTranslations("gacha");
 const tCommon = useTranslations("common");
 const { data: oddsData, isLoading, error } = useGachaOdds(bannerCode, isOpen);
 const isVi = t("lang") === "vi";
 const rarityVariant = (rarity: string) => (rarity.toLowerCase() === "legendary" ? "amber" : rarity.toLowerCase() === "epic" ? "purple" : rarity.toLowerCase() === "rare" ? "info" : "default");

 return (
  <Modal isOpen={isOpen} onClose={onClose} title={t("oddsTitle")} size="md">
   <div className="text-stone-100">{isLoading ? <div className="space-y-3 py-4"><SkeletonLoader type="text" count={3} /></div> : error ? <div className="text-red-400 py-4 text-sm text-center">{tCommon("error")}</div> : oddsData ? <div className="space-y-4"><div className="text-[10px] uppercase tracking-widest text-stone-500 mb-2">{t("oddsVersion")}: {oddsData.oddsVersion}</div><div className="h-[350px] overflow-y-auto custom-scrollbar rounded-2xl border border-stone-800 p-2 bg-stone-900/50"><table className="w-full text-sm border-separate border-spacing-y-1"><thead><tr className="text-[10px] uppercase tracking-widest text-stone-500"><th className="text-left font-black py-2 px-3">{t("item")}</th><th className="text-center font-black py-2 px-3">{t("rarity")}</th><th className="text-right font-black py-2 px-3">{t("probability")}</th></tr></thead><tbody>{oddsData.items.map((item, index) => <tr key={`${item.rarity}-${index}`} className="group hover:bg-stone-800/40 transition-colors"><td className="py-3 px-3 font-semibold text-stone-300">{isVi ? item.displayNameVi : item.displayNameEn}</td><td className="py-3 px-3 text-center"><Badge variant={rarityVariant(item.rarity)}>{item.rarity}</Badge></td><td className="py-3 px-3 text-right text-stone-400 font-mono">{item.probabilityPercent.toFixed(2)}%</td></tr>)}</tbody></table></div></div> : null}</div>
  </Modal>
 );
}
