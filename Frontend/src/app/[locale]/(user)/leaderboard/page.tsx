import { getTranslations } from "next-intl/server";
import LeaderboardTable from "@/features/gamification/components/LeaderboardTable";
import { BarChart3 } from "lucide-react";
import { cn } from "@/lib/utils";
import { generateLeaderboardMetadata } from "./metadata";

// Export generateMetadata để Next.js sử dụng cho SEO
export { generateLeaderboardMetadata as generateMetadata };

// Định nghĩa các class tiện ích tập trung, tối ưu hiệu suất và dễ quản lý
const pageClasses = {
  root: cn("relative", "min-h-screen", "overflow-hidden", "bg-slate-950", "px-6", "py-8"),
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
    "h-80",
    "w-80",
    "rounded-full",
    "bg-emerald-600/10",
    "blur-3xl",
  ),
  glowRight: cn(
    "pointer-events-none",
    "absolute",
    "-right-12",
    "top-20",
    "h-96",
    "w-72",
    "rounded-full",
    "bg-indigo-600/10",
    "blur-3xl",
  ),
  main: cn("relative", "z-10", "mx-auto", "max-w-4xl"),
  header: cn("mb-12", "flex", "flex-row", "items-center", "gap-6", "text-left"),
  iconWrap: cn(
    "rounded-2xl",
    "bg-gradient-to-br",
    "from-emerald-500",
    "to-indigo-600",
    "p-4",
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
    "text-5xl",
    "font-black",
    "tracking-tight",
    "text-transparent",
    "text-white",
  ),
  desc: cn("mt-3", "max-w-2xl", "text-lg", "text-slate-400"),
};

// Component chính của trang Leaderboard (Bảng Xếp Hạng)
// Hiển thị vị trí xếp hạng của người dùng dựa trên cấu trúc đã có
export default async function LeaderboardPage() {
  const t = await getTranslations("Gamification");

  return (
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

        {/* Sử dụng component LeaderboardTable đã có, thiết kế full-width cho layout mới */}
        <section>
          <LeaderboardTable />
        </section>
      </main>
    </div>
  );
}
