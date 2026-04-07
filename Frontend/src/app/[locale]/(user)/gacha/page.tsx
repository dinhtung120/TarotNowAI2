"use client";

import { History, Sparkles } from "lucide-react";
import { useState } from "react";
import toast from "react-hot-toast";
import { GachaBannerCard } from "@/features/gacha/components/GachaBannerCard";
import { GachaHistoryModal } from "@/features/gacha/components/GachaHistoryModal";
import { GachaSpinReveal } from "@/features/gacha/components/GachaSpinReveal";
import { useGachaBanners, useSpinGacha } from "@/features/gacha/hooks/useGacha";
import type { SpinGachaResult } from "@/features/gacha/gacha.types";
import Button from "@/shared/components/ui/Button";
import { useTranslations } from "next-intl";

export default function GachaPage() {
 const t = useTranslations("gacha");
 const tCommon = useTranslations("common");
 const { data: banners, isLoading, isError, error } = useGachaBanners();
 const spinMutation = useSpinGacha();
 const [spinResult, setSpinResult] = useState<SpinGachaResult | null>(null);
 const [isRevealOpen, setIsRevealOpen] = useState(false);
 const [isHistoryOpen, setIsHistoryOpen] = useState(false);
 const [optimisticPity, setOptimisticPity] = useState<Record<string, number>>({});

 const handleSpin = async (bannerCode: string, count: number) => {
  try {
   const result = await spinMutation.mutateAsync({ bannerCode, count });
   if (!result) return;
   setSpinResult(result as SpinGachaResult);
   setIsRevealOpen(true);
   setOptimisticPity((prev) => ({ ...prev, [bannerCode]: result.currentPityCount }));
  } catch (spinError: unknown) {
   toast.error(spinError instanceof Error ? spinError.message : tCommon("error_unknown"));
  }
 };

 return (
  <div className="container max-w-5xl mx-auto py-8 px-4 flex flex-col items-center">
   <div className="w-full flex justify-end mb-4"><Button variant="secondary" className="gap-2" onClick={() => setIsHistoryOpen(true)}><History className="w-4 h-4" />{t("historyTitle")}</Button></div>
   <div className="text-center mb-12 space-y-4"><h1 className="text-4xl md:text-6xl font-black bg-clip-text text-transparent bg-gradient-to-r from-amber-200 via-yellow-400 to-orange-500 drop-shadow-sm flex items-center justify-center gap-3"><Sparkles className="w-8 h-8 text-yellow-400" />{t("title")}<Sparkles className="w-8 h-8 text-orange-400" /></h1><p className="text-lg text-stone-400 max-w-xl mx-auto">{t("subtitle")}</p></div>
   <div className="w-full flex flex-wrap justify-center gap-8">{isLoading ? <div className="w-full max-w-md h-[400px] border border-stone-800 bg-stone-900/50 rounded-3xl animate-pulse flex items-center justify-center"><span className="text-stone-600 italic">Đang tải dữ liệu vòng quay...</span></div> : isError ? <div className="text-center text-red-400 p-12 border border-dashed border-red-900/30 bg-red-950/20 rounded-2xl w-full max-w-2xl"><p className="font-bold mb-2">Opps! Đã có lỗi xảy ra:</p><p className="text-sm opacity-80 mb-4">{error.message}</p><Button variant="secondary" size="sm" onClick={() => window.location.reload()}>Thử lại</Button></div> : banners?.length ? banners.map((banner) => <GachaBannerCard key={banner.code} banner={banner} onSpin={handleSpin} isSpinning={spinMutation.isPending && spinMutation.variables?.bannerCode === banner.code} currentPity={optimisticPity[banner.code] ?? banner.userCurrentPity ?? 0} hardPityCount={90} />) : <div className="text-center text-stone-500 p-12 border border-dashed border-stone-800 rounded-2xl w-full max-w-2xl">{t("noActiveBanners")}</div>}</div>
   <GachaSpinReveal result={spinResult} isOpen={isRevealOpen} onClose={() => setIsRevealOpen(false)} />
   <GachaHistoryModal isOpen={isHistoryOpen} onClose={() => setIsHistoryOpen(false)} />
  </div>
 );
}
