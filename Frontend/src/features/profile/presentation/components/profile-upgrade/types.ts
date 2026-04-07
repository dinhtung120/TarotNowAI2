import type { useProfilePage } from "@/features/profile/application/useProfilePage";

type ProfilePageState = ReturnType<typeof useProfilePage>;

export interface ProfileReaderUpgradeCardProps {
  isAdmin: boolean;
  isTarotReader: boolean;
  readerRequest: ProfilePageState["readerRequest"];
  readerRequestLoading: boolean;
  router: ProfilePageState["router"];
  t: ProfilePageState["t"];
}
