import { getTranslations } from "next-intl/server";
import { LeaderboardTable } from '@/features/gamification/public';
import { BarChart3 } from "lucide-react";
import { cn } from "@/lib/utils";
import { AppQueryHydrationBoundary, dehydrateAppQueries } from "@/app/_shared/server/prefetch/appQueryDehydrate";
import { prefetchLeaderboardPage } from "@/app/_shared/server/prefetch/runners";
import { generateLeaderboardMetadata } from "./metadata";
import { leaderboardPageClasses } from "./pageStyles";

// Export generateMetadata để Next.js sử dụng cho SEO
export { generateLeaderboardMetadata as generateMetadata };

// Component chính của trang Leaderboard (Bảng Xếp Hạng)
// Hiển thị vị trí xếp hạng của người dùng dựa trên cấu trúc đã có
export default async function LeaderboardPage() {
  const t = await getTranslations("Gamification");
  const state = await dehydrateAppQueries(prefetchLeaderboardPage);

  return (
    <AppQueryHydrationBoundary state={state}>
      <div className={cn(leaderboardPageClasses.root)}>
        <div className={cn(leaderboardPageClasses.glowTop)} />
        <div className={cn(leaderboardPageClasses.glowLeft)} />
        <div className={cn(leaderboardPageClasses.glowRight)} />

        <main className={cn(leaderboardPageClasses.main)}>
          <header className={cn(leaderboardPageClasses.header)}>
            <div className={cn(leaderboardPageClasses.iconWrap)}>
              <BarChart3 className={cn("h-10", "w-10", "text-white")} />
            </div>
            <div>
              <h1 className={cn(leaderboardPageClasses.title)}>
                {t("LeaderboardMetaTitle")}
              </h1>
              <p className={cn(leaderboardPageClasses.desc)}>
                {t("LeaderboardMetaDesc")}
              </p>
            </div>
          </header>

          <section>
            <LeaderboardTable />
          </section>
        </main>
      </div>
    </AppQueryHydrationBoundary>
  );
}
