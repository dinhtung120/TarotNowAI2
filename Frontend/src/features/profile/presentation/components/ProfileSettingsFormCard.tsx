"use client";

import { Sparkles } from "lucide-react";
import ProfileFormStatusMessages from "@/features/profile/presentation/components/profile-settings/ProfileFormStatusMessages";
import ProfileSettingsFieldsGrid from "@/features/profile/presentation/components/profile-settings/ProfileSettingsFieldsGrid";
import ProfileSettingsSubmitButton from "@/features/profile/presentation/components/profile-settings/ProfileSettingsSubmitButton";
import type { ProfileSettingsFormCardProps } from "@/features/profile/presentation/components/profile-settings/types";
import { cn } from "@/lib/utils";
import { GlassCard } from "@/shared/components/ui";

export function ProfileSettingsFormCard({
  t,
  successMsg,
  errorMsg,
  register,
  handleSubmit,
  errors,
  isSubmitting,
  onSubmit,
}: ProfileSettingsFormCardProps) {
  return (
    <GlassCard className={cn("!tn-pad-6-8-sm")}>
      <h3
        className={cn(
          "tn-text-primary mb-8 flex items-center gap-2.5 text-lg font-black tracking-tight italic",
        )}
      >
        <Sparkles className={cn("h-4 w-4 tn-text-warning")} />
        {t("settings_title")}
      </h3>
      <ProfileFormStatusMessages successMsg={successMsg} errorMsg={errorMsg} />
      <form onSubmit={handleSubmit(onSubmit)} className={cn("space-y-6")}>
        <ProfileSettingsFieldsGrid t={t} register={register} errors={errors} />
        <div className={cn("tn-border mt-8 border-t pt-4")}>
          <ProfileSettingsSubmitButton
            isSubmitting={isSubmitting}
            saveLabel={t("save")}
            savingLabel={t("saving")}
          />
        </div>
      </form>
    </GlassCard>
  );
}
