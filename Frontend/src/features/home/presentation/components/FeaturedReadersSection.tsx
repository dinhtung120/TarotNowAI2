import { Link } from "@/i18n/routing";
import { ArrowUpRight } from "lucide-react";
import { getTranslations } from "next-intl/server";
import { Suspense } from "react";
import FeaturedReadersFallback from "@/features/home/presentation/components/featured-readers/FeaturedReadersFallback";
import FeaturedReadersGrid from "@/features/home/presentation/components/featured-readers/FeaturedReadersGrid";
import { cn } from "@/lib/utils";
import { SectionHeader } from "@/shared/components/ui";

export async function FeaturedReadersSection() {
  const t = await getTranslations("Index");

  return (
    <section
      className={cn("relative mx-auto w-full max-w-7xl px-4 py-32 sm:px-6")}
    >
      <SectionHeader
        tag={t("showcase.tag")}
        title={t("showcase.title")}
        titleMuted={t("showcase.titleMuted")}
        className={cn("mb-20")}
        action={
          <Link
            href="/readers"
            className={cn(
              "group inline-flex min-h-11 items-center gap-2 rounded-xl px-2 text-xs font-black tracking-widest text-[var(--text-secondary)] uppercase transition-colors hover:bg-[var(--purple-50)] hover:text-[var(--text-ink)]",
            )}
          >
            {t("showcase.viewAll")}
            <ArrowUpRight
              className={cn(
                "h-4 w-4 transition-transform group-hover:translate-x-1 group-hover:-translate-y-1",
              )}
            />
          </Link>
        }
      />
      <Suspense fallback={<FeaturedReadersFallback />}>
        <FeaturedReadersGrid />
      </Suspense>
    </section>
  );
}
