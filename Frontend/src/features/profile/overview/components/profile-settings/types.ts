import type { useProfilePage } from "@/features/profile/overview/useProfilePage";

type ProfilePageState = ReturnType<typeof useProfilePage>;

export interface ProfileSettingsFormCardProps {
  t: ProfilePageState["t"];
  successMsg: string;
  errorMsg: string;
  payoutBanksError: string;
  payoutBankOptions: Array<{ bankBin: string; bankName: string }>;
  isTarotReader: boolean;
  register: ProfilePageState["register"];
  handleSubmit: ProfilePageState["handleSubmit"];
  errors: ProfilePageState["errors"];
  isSubmitting: boolean;
  onSubmit: ProfilePageState["onSubmit"];
}

export interface ProfileSettingsFieldsGridProps {
  errors: ProfileSettingsFormCardProps["errors"];
  register: ProfileSettingsFormCardProps["register"];
  t: ProfileSettingsFormCardProps["t"];
  isTarotReader: boolean;
  payoutBankOptions: Array<{ bankBin: string; bankName: string }>;
}
