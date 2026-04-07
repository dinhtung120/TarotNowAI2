import { Calendar, User } from "lucide-react";
import type { ProfileSettingsFieldsGridProps } from "@/features/profile/presentation/components/profile-settings/types";
import { cn } from "@/lib/utils";

export default function ProfileSettingsFieldsGrid({
  errors,
  register,
  t,
}: ProfileSettingsFieldsGridProps) {
  return (
    <div className={cn("tn-grid-cols-1-2-md gap-6")}>
      <div className={cn("space-y-2")}>
        <label
          className={cn(
            "ml-1 flex items-center gap-2 tn-text-10 font-black tracking-widest tn-text-secondary uppercase",
          )}
        >
          <User className={cn("h-3.5 w-3.5")} />
          {t("displayName")}
        </label>
        <input
          type="text"
          {...register("displayName")}
          className={cn(
            "tn-field tn-field-accent tn-text-primary w-full rounded-xl px-4 py-3.5 text-sm font-medium shadow-inner transition-all",
          )}
          placeholder={t("display_name_placeholder")}
        />
        {errors.displayName ? (
          <p
            className={cn(
              "ml-1 tn-text-10 font-bold tn-text-danger italic",
            )}
          >
            {errors.displayName.message}
          </p>
        ) : null}
      </div>
      <div className={cn("space-y-2")}>
        <label
          className={cn(
            "ml-1 flex items-center gap-2 tn-text-10 font-black tracking-widest tn-text-secondary uppercase",
          )}
        >
          <Calendar className={cn("h-3.5 w-3.5")} />
          {t("dateOfBirth")}
        </label>
        <input
          type="date"
          {...register("dateOfBirth")}
          className={cn(
            "tn-field tn-field-accent tn-text-primary w-full rounded-xl px-4 py-3.5 text-sm font-medium shadow-inner transition-all",
          )}
        />
        {errors.dateOfBirth ? (
          <p
            className={cn(
              "ml-1 tn-text-10 font-bold tn-text-danger italic",
            )}
          >
            {errors.dateOfBirth.message}
          </p>
        ) : null}
      </div>
    </div>
  );
}
