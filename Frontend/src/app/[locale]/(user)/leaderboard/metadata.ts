import { getTranslations } from "next-intl/server";

// Hàm generateMetadata tạo metadata chuẩn cho trang Leaderboard
// Sử dụng i18n để hỗ trợ đa ngôn ngữ cho SEO tag
export async function generateLeaderboardMetadata({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  const t = await getTranslations({ locale, namespace: "Gamification" });

  return {
    title: `${t("LeaderboardMetaTitle")} - TarotNow AI`,
    description: t("LeaderboardMetaDesc"),
  };
}
