import { Award, Clock, Coins, Diamond, Sparkles } from "lucide-react";
import { useLocale, useTranslations } from "next-intl";
import type { GachaHistoryItemDto } from "@/features/gacha/gacha.types";
import { useGachaHistory } from "@/features/gacha/hooks/useGacha";
import { cn } from "@/lib/utils";
import Badge from "@/shared/components/ui/Badge";
import Modal from "@/shared/components/ui/Modal";
import { formatDateTime } from "@/shared/utils/format/formatDateTime";

interface GachaHistoryModalProps {
 isOpen: boolean;
 onClose: () => void;
}

function HistoryTypeIcon({ type }: { type: string }) {
 if (type === "diamond") return <Diamond className={cn("h-4", "w-4", "text-cyan-400")} />;
 if (type === "gold") return <Coins className={cn("h-4", "w-4", "text-yellow-400")} />;
 if (type === "title") return <Award className={cn("h-4", "w-4", "text-rose-400")} />;
 return <Sparkles className={cn("h-4", "w-4")} />;
}

export function GachaHistoryModal({ isOpen, onClose }: GachaHistoryModalProps) {
 const t = useTranslations("gacha");
 const locale = useLocale();
 const { data: history, isLoading, isError, error } = useGachaHistory(50, isOpen);
 const rarityVariant = (rarity: string) => (rarity.toLowerCase() === "legendary" ? "amber" : rarity.toLowerCase() === "epic" ? "purple" : rarity.toLowerCase() === "rare" ? "info" : "default");

 return (
  <Modal isOpen={isOpen} onClose={onClose} title={t("historyTitle")} size="lg">
   <div className={cn("mt-2")}>
    {isLoading ? (
     <div className={cn("space-y-3")}>
      {[1, 2, 3, 4, 5].map((index) => (
       <div key={index} className={cn("h-16", "animate-pulse", "rounded-2xl", "bg-stone-800/50")} />
      ))}
     </div>
    ) : isError ? (
     <div className={cn("rounded-3xl", "border", "border-dashed", "border-red-900/30", "bg-red-950/10", "py-12", "text-center", "text-red-400")}>
      <p className={cn("mb-1", "font-bold", "italic")}>{error instanceof Error ? error.message : t("errorLoadingHistory")}</p>
     </div>
    ) : !history?.length ? (
     <div className={cn("rounded-3xl", "border", "border-dashed", "border-stone-800", "py-12", "text-center", "text-stone-500")}>
      {t("emptyHistory")}
     </div>
    ) : (
     <div className={cn("h-96", "space-y-3", "overflow-y-auto", "pr-2", "custom-scrollbar")}>
      {history.map((log: GachaHistoryItemDto, index: number) => (
       <div
        key={`${log.createdAt}-${index}`}
        className={cn(
         "flex",
         "items-center",
         "justify-between",
         "gap-3",
         "rounded-2xl",
         "border",
         "border-stone-800",
         "bg-stone-900/50",
         "p-4",
         "transition-colors",
         "hover:border-stone-700",
        )}
       >
        <div className={cn("flex", "items-center", "gap-4")}>
         <Badge variant={rarityVariant(log.rarity)}>{log.rarity}</Badge>
         <div>
          <div className={cn("flex", "items-center", "gap-2")}>
           <HistoryTypeIcon type={log.rewardType} />
           <span className={cn("text-xs", "font-black", "uppercase", "tracking-widest", "text-stone-200")}>{log.rewardValue}</span>
          </div>
          {log.wasPityTriggered ? (
           <span className={cn("mt-1", "inline-block", "rounded-full", "border", "border-amber-500/20", "bg-amber-500/10", "px-2", "py-0.5", "text-xs", "font-black", "uppercase", "text-amber-500")}>
            Pity Triggered
           </span>
          ) : null}
         </div>
        </div>
        <div className={cn("mt-0", "flex", "flex-col", "items-end", "gap-1")}>
         <span className={cn("flex", "items-center", "gap-1", "text-xs", "font-bold", "text-amber-400/80")}>
          -{log.spentDiamond} <Diamond className={cn("h-3", "w-3")} />
         </span>
         <span className={cn("flex", "items-center", "gap-1", "text-xs", "font-medium", "text-stone-500")}>
          <Clock className={cn("h-3", "w-3")} />
          {formatDateTime(new Date(log.createdAt), locale)}
         </span>
        </div>
       </div>
      ))}
     </div>
    )}
   </div>
  </Modal>
 );
}
