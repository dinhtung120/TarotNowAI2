import { getTranslations } from "next-intl/server";
import { getHomeSnapshotAction } from "@/shared/application/actions/home-snapshot";
import FeaturedReaderCard from "@/features/home/presentation/components/featured-readers/FeaturedReaderCard";
import FeaturedReadersFallback from "@/features/home/presentation/components/featured-readers/FeaturedReadersFallback";
import { cn } from "@/lib/utils";

export default async function FeaturedReadersGrid() {
  const t = await getTranslations("Index");
  const featuredReadersResult = await getHomeSnapshotAction();
  const readers =
    featuredReadersResult.success && featuredReadersResult.data
      ? featuredReadersResult.data.featuredReaders
      : [];

  if (readers.length === 0) {
    return <FeaturedReadersFallback />;
  }

  return (
    <div className={cn("tn-grid-1-2-4-responsive gap-8")}>
      {readers.map((reader) => (
        <FeaturedReaderCard
          key={reader.userId}
          reader={reader}
          profileCta={t("showcase.profileCta")}
          experienceSuffix={t("showcase.experienceSuffix")}
        />
      ))}
    </div>
  );
}
