"use client";

import { Sparkles } from "lucide-react";
import ProfileUpgradeApplyState from "@/features/profile/overview/components/profile-upgrade/ProfileUpgradeApplyState";
import ProfileUpgradeHeader from "@/features/profile/overview/components/profile-upgrade/ProfileUpgradeHeader";
import ProfileUpgradePendingState from "@/features/profile/overview/components/profile-upgrade/ProfileUpgradePendingState";
import ProfileUpgradeRejectedState from "@/features/profile/overview/components/profile-upgrade/ProfileUpgradeRejectedState";
import type { ProfileReaderUpgradeCardProps } from "@/features/profile/overview/components/profile-upgrade/types";
import { useOptimizedNavigation } from "@/shared/navigation/useOptimizedNavigation";
import { cn } from "@/lib/utils";
import { GlassCard } from "@/shared/ui";

export function ProfileReaderUpgradeCard({
  t,
  readerRequest,
  readerRequestLoading,
  isAdmin,
  isTarotReader,
}: ProfileReaderUpgradeCardProps) {
  const navigation = useOptimizedNavigation();

  if (isAdmin || isTarotReader) return null;

  if (readerRequestLoading) {
    return <GlassCard className={cn("!tn-pad-6-8-sm min-h-48")} aria-hidden="true"> </GlassCard>;
  }

  const openApply = () => navigation.push("/reader/apply");

  return (
    <GlassCard className={cn("group relative overflow-hidden !tn-pad-6-8-sm")}>
      <div
        className={cn(
          "pointer-events-none absolute top-0 right-0 p-6 opacity-5 transition-transform duration-700 group-hover:scale-110 group-hover:rotate-12",
        )}
      >
        <Sparkles className={cn("h-32 w-32 tn-text-accent")} />
      </div>
      <div className={cn("relative z-10 space-y-5")}>
        <ProfileUpgradeHeader
          title={t("upgrade.title")}
          subtitle={t("upgrade.subtitle")}
        />
        {!readerRequest?.hasRequest ? (
          <ProfileUpgradeApplyState
            cta={t("upgrade.cta")}
            description={t("upgrade.desc")}
            onApply={openApply}
          />
        ) : null}
        {readerRequest?.hasRequest && readerRequest.status === "pending" ? (
          <ProfileUpgradePendingState
            title={t("upgrade.pending_title")}
            description={t("upgrade.pending_desc")}
          />
        ) : null}
        {readerRequest?.hasRequest && readerRequest.status === "rejected" ? (
          <ProfileUpgradeRejectedState
            title={t("upgrade.rejected_title")}
            reasonText={
              readerRequest.adminNote
                ? t("upgrade.rejected_reason", {
                    note: readerRequest.adminNote,
                  })
                : undefined
            }
            cta={t("upgrade.reapply_cta")}
            onApply={openApply}
          />
        ) : null}
      </div>
    </GlassCard>
  );
}
