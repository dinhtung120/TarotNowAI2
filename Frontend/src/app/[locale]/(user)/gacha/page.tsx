"use client";

import { History, Sparkles } from "lucide-react";
import { GachaBannerCard } from "@/features/gacha/components/GachaBannerCard";
import { GachaHistoryModal } from "@/features/gacha/components/GachaHistoryModal";
import { GachaSpinReveal } from "@/features/gacha/components/GachaSpinReveal";
import { useGachaPageState } from "@/features/gacha/presentation/hooks/useGachaPageState";
import { cn } from "@/lib/utils";
import Button from "@/shared/components/ui/Button";

const pageClasses = {
 container: cn("container", "mx-auto", "flex", "max-w-5xl", "flex-col", "items-center", "px-4", "py-8"),
 toolbar: cn("mb-4", "flex", "w-full", "justify-end"),
 hero: cn("mb-12", "space-y-4", "text-center"),
 heroTitle: cn(
  "flex",
  "items-center",
  "justify-center",
  "gap-3",
  "bg-gradient-to-r",
  "from-amber-200",
  "via-yellow-400",
  "to-orange-500",
  "bg-clip-text",
  "text-5xl",
  "font-black",
  "text-transparent",
  "drop-shadow-sm",
 ),
 heroIconLeft: cn("h-8", "w-8", "text-yellow-400"),
 heroIconRight: cn("h-8", "w-8", "text-orange-400"),
 heroSubtitle: cn("mx-auto", "max-w-xl", "text-lg", "text-stone-400"),
 grid: cn("flex", "w-full", "flex-wrap", "justify-center", "gap-8"),
 loadingCard: cn(
  "flex",
  "h-96",
  "w-full",
  "max-w-md",
  "items-center",
  "justify-center",
  "rounded-3xl",
  "border",
  "border-stone-800",
  "bg-stone-900/50",
  "animate-pulse",
 ),
 loadingText: cn("italic", "text-stone-600"),
 errorCard: cn(
  "w-full",
  "max-w-2xl",
  "rounded-2xl",
  "border",
  "border-dashed",
  "border-red-900/30",
  "bg-red-950/20",
  "p-12",
  "text-center",
  "text-red-400",
 ),
 errorTitle: cn("mb-2", "font-bold"),
 errorBody: cn("mb-4", "text-sm", "opacity-80"),
 emptyCard: cn(
  "w-full",
  "max-w-2xl",
  "rounded-2xl",
  "border",
  "border-dashed",
  "border-stone-800",
  "p-12",
  "text-center",
  "text-stone-500",
 ),
};

export default function GachaPage() {
 const vm = useGachaPageState();

 return (
  <div className={cn(pageClasses.container)}>
   <div className={cn(pageClasses.toolbar)}>
    <Button variant="secondary" className={cn("gap-2")} onClick={vm.openHistory}>
     <History className={cn("h-4", "w-4")} />
     {vm.t("historyTitle")}
    </Button>
   </div>
   <div className={cn(pageClasses.hero)}>
    <h1 className={cn(pageClasses.heroTitle)}>
     <Sparkles className={cn(pageClasses.heroIconLeft)} />
     {vm.t("title")}
     <Sparkles className={cn(pageClasses.heroIconRight)} />
    </h1>
    <p className={cn(pageClasses.heroSubtitle)}>{vm.t("subtitle")}</p>
   </div>
   <div className={cn(pageClasses.grid)}>
    {vm.isLoading ? (
     <div className={cn(pageClasses.loadingCard)}>
      <span className={cn(pageClasses.loadingText)}>Đang tải dữ liệu vòng quay...</span>
     </div>
    ) : vm.isError ? (
     <div className={cn(pageClasses.errorCard)}>
      <p className={cn(pageClasses.errorTitle)}>Opps! Đã có lỗi xảy ra:</p>
      <p className={cn(pageClasses.errorBody)}>{vm.error?.message ?? "Có lỗi không xác định"}</p>
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
     <div className={cn(pageClasses.emptyCard)}>
      {vm.t("noActiveBanners")}
     </div>
    )}
   </div>
   <GachaSpinReveal result={vm.spinResult} isOpen={vm.isRevealOpen} onClose={vm.closeReveal} />
   <GachaHistoryModal isOpen={vm.isHistoryOpen} onClose={vm.closeHistory} />
  </div>
 );
}
