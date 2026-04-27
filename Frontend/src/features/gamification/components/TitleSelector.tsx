"use client";

import { useState } from "react";
import { toast } from "react-hot-toast";
import { useTranslations } from "next-intl";
import type { TitleDefinition, UserTitle } from "@/features/gamification/gamification.types";
import { useSetActiveTitle, useTitles } from "@/features/gamification/useGamification";
import { useLocalizedField } from "@/features/gamification/useLocalizedField";
import GamificationDetailModal from "@/features/gamification/components/GamificationDetailModal";
import { NoTitleCard } from "@/features/gamification/components/NoTitleCard";
import { TitleCard } from "@/features/gamification/components/TitleCard";
import { TitleSelectorHeader } from "@/features/gamification/components/title-selector/TitleSelectorHeader";
import { cn } from "@/lib/utils";

export default function TitleSelector() {
 const t = useTranslations("Gamification");
 const { data, isLoading } = useTitles();
 const setMutation = useSetActiveTitle();
 const { localize } = useLocalizedField();
 const [selectedTitle, setSelectedTitle] = useState<{ definition: TitleDefinition; isOwned: boolean; isActive: boolean; grantedAt?: string } | null>(null);
 if (isLoading) return <div className={cn("flex", "items-center", "justify-center", "py-6")}><div className={cn("h-8", "w-8", "animate-spin", "rounded-full", "border-4", "border-blue-500", "border-t-transparent")} /></div>;
 if (!data) return null;
 if (data.definitions.length === 0) return <div className={cn("flex", "items-center", "justify-center", "py-6", "text-sm", "text-slate-500")}>Không có dữ liệu danh hiệu.</div>;

 const handleEquipTitle = (code: string) => {
  if (code === data.activeTitleCode) return;
  setMutation.mutate(code, { onSuccess: () => toast.success(t("TitleUpdated", { defaultMessage: "Cập nhật danh hiệu thành công" })) });
 };

 return (
  <div className={cn("space-y-6")}>
   <TitleSelectorHeader title={t("YourTitles", { defaultMessage: "Danh hiệu" })} subtitle={t("UnlockedAmount", { count: data.unlockedList.length, total: data.definitions.length })} />
   <div className={cn("grid", "grid-cols-1", "gap-4", "pb-6", "sm:grid-cols-2", "lg:grid-cols-3", "xl:grid-cols-4")}>
    <NoTitleCard isNoTitleActive={!data.activeTitleCode} noTitleLabel={t("NoTitle")} hintLabel={t("EarnTitleHint")} onClick={() => handleEquipTitle("")} />
    {data.definitions.map((title: TitleDefinition) => { const unlockedInfo = data.unlockedList.find((item: UserTitle) => item.titleCode === title.code); const isOwned = Boolean(unlockedInfo); const isActive = isOwned && title.code === data.activeTitleCode; return <TitleCard key={title.code} title={title} isOwned={isOwned} isActive={isActive} localize={localize} hiddenLabel={t("HiddenAchievement", { defaultMessage: "Chưa mở khoá" })} onClick={() => setSelectedTitle({ definition: title, isOwned, isActive, grantedAt: unlockedInfo?.grantedAt })} />; })}
   </div>
   <GamificationDetailModal isOpen={Boolean(selectedTitle)} onClose={() => setSelectedTitle(null)} type="title" titleData={selectedTitle || undefined} onEquipTitle={handleEquipTitle} />
  </div>
 );
}
