import { getTranslations } from "next-intl/server";
import HeroActions from "@/features/home/landing/components/hero/HeroActions";
import HeroIntro from "@/features/home/landing/components/hero/HeroIntro";
import HeroScrollIndicator from "@/features/home/landing/components/hero/HeroScrollIndicator";
import { cn } from "@/lib/utils";

export async function HeroSection() {
  const t = await getTranslations("Index");

  return (
    <section
      className={cn(
        "relative flex min-h-dvh flex-col items-center justify-center px-4 pt-20 pb-28 sm:px-6 md:pb-24",
      )}
    >
      <div
        className={cn(
          "flex w-full max-w-6xl flex-col items-center text-center",
        )}
      >
        <HeroIntro
          tagline={t("tagline")}
          heroTitle1={t("heroTitle1")}
          heroTitle2={t("heroTitle2")}
          heroDesc={t("heroDesc")}
        />
        <HeroActions
          ctaDraw={t("ctaDraw")}
          ctaMeetReaders={t("ctaMeetReaders")}
        />
      </div>
      <HeroScrollIndicator label={t("scroll")} />
    </section>
  );
}
