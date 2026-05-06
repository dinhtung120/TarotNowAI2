import type { ReaderProfile } from "@/features/reader/public";

export interface FeaturedReaderCardProps {
  profileCta: string;
  experienceSuffix: string;
  reader: ReaderProfile;
}

export interface FeaturedReaderMetaProps extends FeaturedReaderCardProps {
  statusClassName: string;
}
