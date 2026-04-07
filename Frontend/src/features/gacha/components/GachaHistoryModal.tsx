import { Award, Clock, Coins, Diamond, Sparkles } from "lucide-react";
import { useLocale, useTranslations } from "next-intl";
import type { GachaHistoryItemDto } from "@/features/gacha/gacha.types";
import { useGachaHistory } from "@/features/gacha/hooks/useGacha";
import Badge from "@/shared/components/ui/Badge";
import Modal from "@/shared/components/ui/Modal";
import { formatDateTime } from "@/shared/utils/format/formatDateTime";

interface GachaHistoryModalProps {
 isOpen: boolean;
 onClose: () => void;
}

function HistoryTypeIcon({ type }: { type: string }) {
 if (type === "diamond") return <Diamond className="w-4 h-4 text-cyan-400" />;
 if (type === "gold") return <Coins className="w-4 h-4 text-yellow-400" />;
 if (type === "title") return <Award className="w-4 h-4 text-rose-400" />;
 return <Sparkles className="w-4 h-4" />;
}

export function GachaHistoryModal({ isOpen, onClose }: GachaHistoryModalProps) {
 const t = useTranslations("gacha");
 const locale = useLocale();
 const { data: history, isLoading, isError, error } = useGachaHistory(50, isOpen);
 const rarityVariant = (rarity: string) => (rarity.toLowerCase() === "legendary" ? "amber" : rarity.toLowerCase() === "epic" ? "purple" : rarity.toLowerCase() === "rare" ? "info" : "default");

 return (
  <Modal isOpen={isOpen} onClose={onClose} title={t("historyTitle")} size="lg">
   <div className="mt-2">{isLoading ? <div className="space-y-3">{[1, 2, 3, 4, 5].map((index) => <div key={index} className="h-16 bg-stone-800/50 rounded-2xl animate-pulse" />)}</div> : isError ? <div className="text-center py-12 text-red-400 border border-dashed border-red-900/30 bg-red-950/10 rounded-3xl"><p className="font-bold mb-1 italic">{error instanceof Error ? error.message : t("errorLoadingHistory")}</p></div> : !history?.length ? <div className="text-center py-12 text-stone-500 border border-dashed border-stone-800 rounded-3xl">{t("emptyHistory")}</div> : <div className="h-[450px] overflow-y-auto pr-2 custom-scrollbar space-y-3">{history.map((log: GachaHistoryItemDto, index: number) => <div key={`${log.createdAt}-${index}`} className="flex flex-col sm:flex-row items-start sm:items-center justify-between p-4 rounded-2xl bg-stone-900/50 border border-stone-800 hover:border-stone-700 transition-colors"><div className="flex items-center gap-4"><Badge variant={rarityVariant(log.rarity)}>{log.rarity}</Badge><div><div className="flex items-center gap-2"><HistoryTypeIcon type={log.rewardType} /><span className="font-black text-[11px] uppercase tracking-widest text-stone-200">{log.rewardValue}</span></div>{log.wasPityTriggered ? <span className="text-[9px] font-black uppercase text-amber-500 bg-amber-500/10 px-2 py-0.5 rounded-full mt-1 inline-block border border-amber-500/20">Pity Triggered</span> : null}</div></div><div className="flex sm:flex-col items-center sm:items-end gap-2 sm:gap-1 mt-3 sm:mt-0"><span className="flex items-center gap-1 text-amber-400/80 text-[10px] font-bold">-{log.spentDiamond} <Diamond className="w-3 h-3" /></span><span className="flex items-center gap-1 text-[10px] text-stone-500 font-medium"><Clock className="w-3 h-3" />{formatDateTime(new Date(log.createdAt), locale)}</span></div></div>)}</div>}</div>
  </Modal>
 );
}
