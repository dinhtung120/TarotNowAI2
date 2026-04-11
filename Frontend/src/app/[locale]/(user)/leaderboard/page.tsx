import { getTranslations } from "next-intl/server";
import LeaderboardTable from "@/features/gamification/components/LeaderboardTable";
import { BarChart3 } from "lucide-react";
import { cn } from "@/lib/utils";
import { AppQueryHydrationBoundary, dehydrateAppQueries } from "@/shared/server/prefetch/appQueryDehydrate";
import { prefetchLeaderboardPage } from "@/shared/server/prefetch/runners";
import { generateLeaderboardMetadata } from "./metadata";

// Export generateMetadata để Next.js sử dụng cho SEO
export { generateLeaderboardMetadata as generateMetadata };

// Định nghĩa các class tiện ích tập trung, tối ưu hiệu suất và dễ quản lý
const pageClasses = {
  root: cn("relative", "min-h-screen", "overflow-hidden", "bg-slate-950", "px-4", "py-6", "sm:px-6", "sm:py-8"),
  glowTop: cn(
    "pointer-events-none",
    "absolute",
    "inset-x-0",
    "top-0",
    "h-96",
    "bg-gradient-to-b",
    "from-indigo-900/20",
    "to-slate-950",
  ),
  glowLeft: cn(
    "pointer-events-none",
    "absolute",
    "-left-12",
    "-top-12",
    "h-64",
    "w-64",
    "sm:h-80",
    "sm:w-80",
    "rounded-full",
    "bg-emerald-600/10",
    "blur-3xl",
  ),
  glowRight: cn(
    "pointer-events-none",
    "absolute",
    "-right-12",
    "top-20",
    "h-72",
    "w-56",
    "sm:h-96",
    "sm:w-72",
    "rounded-full",
    "bg-indigo-600/10",
    "blur-3xl",
  ),
  main: cn("relative", "z-10", "mx-auto", "max-w-4xl"),
  header: cn("mb-10", "flex", "flex-col", "items-start", "gap-4", "text-left", "sm:mb-12", "sm:flex-row", "sm:items-center", "sm:gap-6"),
  iconWrap: cn(
    "rounded-2xl",
    "bg-gradient-to-br",
    "from-emerald-500",
    "to-indigo-600",
    "p-3",
    "sm:p-4",
    "shadow-xl",
    "shadow-emerald-500/20",
    "ring-1",
    "ring-white/10",
  ),
  title: cn(
    "bg-gradient-to-r",
    "from-white",
    "to-slate-400",
    "bg-clip-text",
    "text-3xl",
    "sm:text-5xl",
    "font-black",
    "tracking-tight",
    "text-transparent",
    "text-white",
  ),
  desc: cn("mt-2", "max-w-2xl", "text-base", "text-slate-400", "sm:mt-3", "sm:text-lg"),
};

// Component chính của trang Leaderboard (Bảng Xếp Hạng)
// Hiển thị vị trí xếp hạng của người dùng dựa trên cấu trúc đã có
export default async function LeaderboardPage() {
  const t = await getTranslations("Gamification");
  const state = await dehydrateAppQueries(prefetchLeaderboardPage);

  return (
    <AppQueryHydrationBoundary state={state}>
      <div className={cn(pageClasses.root)}>
        <div className={cn(pageClasses.glowTop)} />
        <div className={cn(pageClasses.glowLeft)} />
        <div className={cn(pageClasses.glowRight)} />

        <main className={cn(pageClasses.main)}>
          <header className={cn(pageClasses.header)}>
            <div className={cn(pageClasses.iconWrap)}>
              <BarChart3 className={cn("h-10", "w-10", "text-white")} />
            </div>
            <div>
              <h1 className={cn(pageClasses.title)}>
                {t("LeaderboardMetaTitle")}
              </h1>
              <p className={cn(pageClasses.desc)}>
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
