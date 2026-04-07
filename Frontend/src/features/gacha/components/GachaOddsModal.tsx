"use client";

import { useTranslations } from "next-intl";
import { useGachaOdds } from "@/features/gacha/hooks/useGacha";
import { cn } from "@/lib/utils";
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
   <div className={cn("text-stone-100")}>
    {isLoading ? (
     <div className={cn("space-y-3", "py-4")}>
      <SkeletonLoader type="text" count={3} />
     </div>
    ) : error ? (
     <div className={cn("py-4", "text-center", "text-sm", "text-red-400")}>{tCommon("error")}</div>
    ) : oddsData ? (
     <div className={cn("space-y-4")}>
      <div className={cn("mb-2", "text-xs", "uppercase", "tracking-widest", "text-stone-500")}>
       {t("oddsVersion")}: {oddsData.oddsVersion}
      </div>
      <div className={cn("h-80", "overflow-y-auto", "rounded-2xl", "border", "border-stone-800", "bg-stone-900/50", "p-2", "custom-scrollbar")}>
       <table className={cn("w-full", "border-separate", "border-spacing-y-1", "text-sm")}>
        <thead>
         <tr className={cn("text-xs", "uppercase", "tracking-widest", "text-stone-500")}>
          <th className={cn("px-3", "py-2", "text-left", "font-black")}>{t("item")}</th>
          <th className={cn("px-3", "py-2", "text-center", "font-black")}>{t("rarity")}</th>
          <th className={cn("px-3", "py-2", "text-right", "font-black")}>{t("probability")}</th>
         </tr>
        </thead>
        <tbody>
         {oddsData.items.map((item, index) => (
          <tr key={`${item.rarity}-${index}`} className={cn("group", "transition-colors", "tn-hover-stone-800-40")}>
           <td className={cn("px-3", "py-3", "font-semibold", "text-stone-300")}>{isVi ? item.displayNameVi : item.displayNameEn}</td>
           <td className={cn("px-3", "py-3", "text-center")}>
            <Badge variant={rarityVariant(item.rarity)}>{item.rarity}</Badge>
           </td>
           <td className={cn("px-3", "py-3", "text-right", "font-mono", "text-stone-400")}>{item.probabilityPercent.toFixed(2)}%</td>
          </tr>
         ))}
        </tbody>
       </table>
      </div>
     </div>
    ) : null}
   </div>
  </Modal>
 );
}
