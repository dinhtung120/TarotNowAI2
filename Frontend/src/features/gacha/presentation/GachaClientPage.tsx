"use client";

import { History, Sparkles } from "lucide-react";
import { GachaBannerCard } from "@/features/gacha/components/GachaBannerCard";
import { GachaHistoryModal } from "@/features/gacha/components/GachaHistoryModal";
import { GachaSpinReveal } from "@/features/gacha/components/GachaSpinReveal";
import { useGachaPageState } from "@/features/gacha/presentation/hooks/useGachaPageState";
import { cn } from "@/lib/utils";
import Button from "@/shared/components/ui/Button";
import { gachaClientPageClasses } from "./GachaClientPage.styles";

export default function GachaClientPage() {
 const vm = useGachaPageState();
 const c = gachaClientPageClasses;

 return (
  <div className={cn(c.container)}>
   <div className={cn(c.toolbar)}>
    <Button variant="secondary" className={cn("gap-2")} onClick={vm.openHistory}>
     <History className={cn("h-4", "w-4")} />
     {vm.t("historyTitle")}
    </Button>
   </div>
   <div className={cn(c.hero)}>
    <h1 className={cn(c.heroTitle)}>
     <Sparkles className={cn(c.heroIconLeft)} />
     {vm.t("title")}
     <Sparkles className={cn(c.heroIconRight)} />
    </h1>
    <p className={cn(c.heroSubtitle)}>{vm.t("subtitle")}</p>
   </div>
   <div className={cn(c.grid)}>
    {vm.isLoading ? (
     <div className={cn(c.loadingCard)}>
      <span className={cn(c.loadingText)}>Đang tải dữ liệu vòng quay...</span>
     </div>
    ) : vm.isError ? (
     <div className={cn(c.errorCard)}>
      <p className={cn(c.errorTitle)}>Opps! Đã có lỗi xảy ra:</p>
      <p className={cn(c.errorBody)}>{vm.error?.message ?? "Có lỗi không xác định"}</p>
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
     <div className={cn(c.emptyCard)}>{vm.t("noActiveBanners")}</div>
    )}
   </div>
   <GachaSpinReveal result={vm.spinResult} isOpen={vm.isRevealOpen} onClose={vm.closeReveal} />
   <GachaHistoryModal isOpen={vm.isHistoryOpen} onClose={vm.closeHistory} />
  </div>
 );
}
