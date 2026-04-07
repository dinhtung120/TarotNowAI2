"use client";

import { History, Sparkles } from "lucide-react";
import { GachaBannerCard } from "@/features/gacha/components/GachaBannerCard";
import { GachaHistoryModal } from "@/features/gacha/components/GachaHistoryModal";
import { GachaSpinReveal } from "@/features/gacha/components/GachaSpinReveal";
import { useGachaPageState } from "@/features/gacha/presentation/hooks/useGachaPageState";
import Button from "@/shared/components/ui/Button";

export default function GachaPage() {
 const vm = useGachaPageState();

 return (
  <div className="container max-w-5xl mx-auto py-8 px-4 flex flex-col items-center">
   <div className="w-full flex justify-end mb-4">
    <Button variant="secondary" className="gap-2" onClick={vm.openHistory}>
     <History className="w-4 h-4" />
     {vm.t("historyTitle")}
    </Button>
   </div>
   <div className="text-center mb-12 space-y-4">
    <h1 className="text-4xl md:text-6xl font-black bg-clip-text text-transparent bg-gradient-to-r from-amber-200 via-yellow-400 to-orange-500 drop-shadow-sm flex items-center justify-center gap-3">
     <Sparkles className="w-8 h-8 text-yellow-400" />
     {vm.t("title")}
     <Sparkles className="w-8 h-8 text-orange-400" />
    </h1>
    <p className="text-lg text-stone-400 max-w-xl mx-auto">{vm.t("subtitle")}</p>
   </div>
   <div className="w-full flex flex-wrap justify-center gap-8">
    {vm.isLoading ? (
     <div className="w-full max-w-md h-[400px] border border-stone-800 bg-stone-900/50 rounded-3xl animate-pulse flex items-center justify-center">
      <span className="text-stone-600 italic">Đang tải dữ liệu vòng quay...</span>
     </div>
    ) : vm.isError ? (
     <div className="text-center text-red-400 p-12 border border-dashed border-red-900/30 bg-red-950/20 rounded-2xl w-full max-w-2xl">
      <p className="font-bold mb-2">Opps! Đã có lỗi xảy ra:</p>
      <p className="text-sm opacity-80 mb-4">{vm.error?.message ?? "Có lỗi không xác định"}</p>
      <Button variant="secondary" size="sm" onClick={vm.retryLoad}>
       Thử lại
      </Button>
     </div>
    ) : vm.banners?.length ? (
     vm.banners.map((banner) => (
      <GachaBannerCard
       key={banner.code}
       banner={banner}
       onSpin={vm.handleSpin}
       isSpinning={vm.spinMutation.isPending && vm.spinMutation.variables?.bannerCode === banner.code}
       currentPity={vm.optimisticPity[banner.code] ?? banner.userCurrentPity ?? 0}
       hardPityCount={90}
      />
     ))
    ) : (
     <div className="text-center text-stone-500 p-12 border border-dashed border-stone-800 rounded-2xl w-full max-w-2xl">
      {vm.t("noActiveBanners")}
     </div>
    )}
   </div>
   <GachaSpinReveal result={vm.spinResult} isOpen={vm.isRevealOpen} onClose={vm.closeReveal} />
   <GachaHistoryModal isOpen={vm.isHistoryOpen} onClose={vm.closeHistory} />
  </div>
 );
}
