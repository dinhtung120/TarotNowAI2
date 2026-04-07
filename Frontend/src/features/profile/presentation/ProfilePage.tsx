"use client";

import { Stars } from "lucide-react";
import { useProfilePage } from "@/features/profile/application/useProfilePage";
import { ProfileLoadingState } from "@/features/profile/presentation/components/ProfileLoadingState";
import { ProfileReaderUpgradeCard } from "@/features/profile/presentation/components/ProfileReaderUpgradeCard";
import { ProfileSettingsFormCard } from "@/features/profile/presentation/components/ProfileSettingsFormCard";
import { ProfileSummaryCard } from "@/features/profile/presentation/components/ProfileSummaryCard";
import ProfileReaderSettingsPage from "@/features/profile/reader/presentation/ProfileReaderSettingsPage";
import { SectionHeader } from "@/shared/components/ui";

export default function ProfilePage() {
  const state = useProfilePage();
  const isAdmin = state.user?.role === "admin";
  const isTarotReader = state.user?.role === "tarot_reader";

  if (state.loading) {
    return <ProfileLoadingState label={state.t("loading")} />;
  }

  return (
    <div className="animate-in fade-in slide-in-from-bottom-8 mx-auto w-full max-w-3xl space-y-10 px-4 pt-8 pb-32 duration-1000 sm:px-6">
      <SectionHeader
        tag={state.t("title")}
        title={state.t("title")}
        subtitle={state.t("subtitle")}
        tagIcon={<Stars className="h-3 w-3 text-[var(--purple-accent)]" />}
      />

      <div className="grid grid-cols-1 gap-8">
        <ProfileSummaryCard
          t={state.t}
          tCommon={state.tCommon}
          router={state.router}
          profileData={state.profileData}
          avatarPreview={state.avatarPreview}
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
