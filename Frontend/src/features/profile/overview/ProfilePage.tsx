"use client";

import { Stars } from "lucide-react";
import { useProfilePage } from "@/features/profile/overview/useProfilePage";
import { ProfileLoadingState } from "@/features/profile/overview/components/ProfileLoadingState";
import { ProfileReaderUpgradeCard } from "@/features/profile/overview/components/ProfileReaderUpgradeCard";
import { ProfileSettingsFormCard } from "@/features/profile/overview/components/ProfileSettingsFormCard";
import { ProfileSummaryCard } from "@/features/profile/overview/components/ProfileSummaryCard";
import ProfileReaderSettingsPage from "@/features/profile/reader-settings/ProfileReaderSettingsPage";
import { SectionHeader } from "@/shared/ui";
import { cn } from "@/lib/utils";

export default function ProfilePage() {
  const state = useProfilePage();
  const isAdmin = state.user?.role === "admin";
  const isTarotReader = state.user?.role === "tarot_reader";

  if (state.loading) {
    return <ProfileLoadingState label={state.t("loading")} />;
  }

  return (
    <div className={cn("mx-auto", "w-full", "max-w-3xl", "space-y-10", "px-6", "pt-8", "pb-32", "animate-in", "fade-in", "slide-in-from-bottom-8", "duration-1000")}>
      <SectionHeader
        tag={state.t("title")}
        title={state.t("title")}
        subtitle={state.t("subtitle")}
        tagIcon={<Stars className={cn("h-3", "w-3", "text-violet-400")} />}
      />

      <div className={cn("grid", "grid-cols-1", "gap-8")}>
        <ProfileSummaryCard
          t={state.t}
          tCommon={state.tCommon}
          router={state.router}
          profileData={state.profileData}
          avatarPreview={state.avatarPreview}
          avatarUploadProgress={state.avatarUploadProgress}
          avatarUploading={state.avatarUploading}
          isSubmitting={state.isSubmitting}
          isAdmin={isAdmin}
          isTarotReader={isTarotReader}
          handleAvatarSelect={state.handleAvatarSelect}
        />
        <ProfileReaderUpgradeCard
          t={state.t}
          router={state.router}
          readerRequest={state.readerRequest}
          readerRequestLoading={state.readerRequestLoading}
          isAdmin={isAdmin}
          isTarotReader={isTarotReader}
        />
        <ProfileSettingsFormCard
          t={state.t}
          successMsg={state.successMsg}
          errorMsg={state.errorMsg}
          payoutBanksError={state.payoutBanksError}
          payoutBankOptions={state.payoutBankOptions}
          isTarotReader={isTarotReader}
          register={state.register}
          handleSubmit={state.handleSubmit}
          errors={state.errors}
          isSubmitting={state.isSubmitting}
          onSubmit={state.onSubmit}
        />
        {isTarotReader ? <ProfileReaderSettingsPage embedded /> : null}
      </div>
    </div>
  );
}
