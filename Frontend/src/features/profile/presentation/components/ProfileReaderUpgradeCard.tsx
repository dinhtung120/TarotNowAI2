"use client";

import { Sparkles } from "lucide-react";
import ProfileUpgradeApplyState from "@/features/profile/presentation/components/profile-upgrade/ProfileUpgradeApplyState";
import ProfileUpgradeHeader from "@/features/profile/presentation/components/profile-upgrade/ProfileUpgradeHeader";
import ProfileUpgradePendingState from "@/features/profile/presentation/components/profile-upgrade/ProfileUpgradePendingState";
import ProfileUpgradeRejectedState from "@/features/profile/presentation/components/profile-upgrade/ProfileUpgradeRejectedState";
import type { ProfileReaderUpgradeCardProps } from "@/features/profile/presentation/components/profile-upgrade/types";
import { cn } from "@/lib/utils";
import { GlassCard } from "@/shared/components/ui";

export function ProfileReaderUpgradeCard({
  t,
  router,
  readerRequest,
  readerRequestLoading,
  isAdmin,
  isTarotReader,
}: ProfileReaderUpgradeCardProps) {
  if (isAdmin || isTarotReader || readerRequestLoading) return null;

  const openApply = () => router.push("/reader/apply");

  return (
    <GlassCard className={cn("group relative overflow-hidden !p-6 sm:!p-8")}>
      <div
        className={cn(
          "pointer-events-none absolute top-0 right-0 p-6 opacity-5 transition-transform duration-700 group-hover:scale-110 group-hover:rotate-12",
        )}
      >
        <Sparkles className={cn("h-32 w-32 text-[var(--purple-accent)]")} />
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
