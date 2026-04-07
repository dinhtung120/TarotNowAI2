import { getTranslations } from "next-intl/server";
import { listFeaturedReaders } from "@/features/reader/public";
import FeaturedReaderCard from "@/features/home/presentation/components/featured-readers/FeaturedReaderCard";
import FeaturedReadersFallback from "@/features/home/presentation/components/featured-readers/FeaturedReadersFallback";
import { cn } from "@/lib/utils";

export default async function FeaturedReadersGrid() {
  const t = await getTranslations("Index");
  const featuredReadersResult = await listFeaturedReaders(4);
  const readers =
    featuredReadersResult.success && featuredReadersResult.data
      ? featuredReadersResult.data
      : [];

  if (readers.length === 0) {
    return <FeaturedReadersFallback />;
  }

  return (
    <div className={cn("grid grid-cols-1 gap-8 sm:grid-cols-2 lg:grid-cols-4")}>
      {readers.map((reader) => (
        <FeaturedReaderCard
          key={reader.userId}
          reader={reader}
          profileCta={t("showcase.profileCta")}
        />
      ))}
    </div>
  );
}
