import { getTranslations } from "next-intl/server";

export async function generateGamificationMetadata({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  const t = await getTranslations({ locale, namespace: "Gamification" });

  return {
    title: `${t("GamificationMetaTitle")} - TarotNow AI`,
    description: t("GamificationMetaDesc"),
  };
}
