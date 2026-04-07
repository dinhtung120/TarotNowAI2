import { getTranslations } from "next-intl/server";
import AchievementsGrid from "@/features/gamification/components/AchievementsGrid";
import GamificationStatsBar from "@/features/gamification/components/GamificationStatsBar";
import LeaderboardTable from "@/features/gamification/components/LeaderboardTable";
import QuestsPanel from "@/features/gamification/components/QuestsPanel";
import TitleSelector from "@/features/gamification/components/TitleSelector";
import { Gamepad2 } from "lucide-react";
import { cn } from "@/lib/utils";
import { generateGamificationMetadata } from "./metadata";

export { generateGamificationMetadata as generateMetadata };

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
  "bg-purple-600/10",
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
  "bg-blue-600/10",
  "blur-3xl",
 ),
 main: cn("relative", "z-10", "mx-auto", "max-w-7xl"),
 header: cn("mb-12", "flex", "flex-row", "items-center", "gap-6", "text-left"),
 iconWrap: cn(
  "rounded-2xl",
  "bg-gradient-to-br",
  "from-indigo-500",
  "to-purple-600",
  "p-4",
  "shadow-xl",
  "shadow-purple-500/20",
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
 grid: cn("grid", "grid-cols-3", "gap-8"),
 left: cn("col-span-2", "space-y-8"),
 sectionCard: cn(
  "rounded-3xl",
  "border",
  "border-slate-800/60",
  "bg-slate-900/40",
  "p-8",
  "shadow-2xl",
  "backdrop-blur-md",
 ),
 right: cn("col-span-1"),
 rightSticky: cn("sticky", "top-8"),
};

export default async function GamificationPage() {
  const t = await getTranslations("Gamification");

  return (
    <div className={cn(pageClasses.root)}>
      <div className={cn(pageClasses.glowTop)} />
      <div className={cn(pageClasses.glowLeft)} />
      <div className={cn(pageClasses.glowRight)} />

      <main className={cn(pageClasses.main)}>
        <header className={cn(pageClasses.header)}>
          <div className={cn(pageClasses.iconWrap)}>
            <Gamepad2 className={cn("h-10", "w-10", "text-white")} />
          </div>
          <div>
            <h1 className={cn(pageClasses.title)}>
              {t("GamificationMetaTitle")}
            </h1>
            <p className={cn(pageClasses.desc)}>
              {t("GamificationMetaDesc")}
            </p>
          </div>
        </header>

        <GamificationStatsBar />

        <div className={cn(pageClasses.grid)}>
          <div className={cn(pageClasses.left)}>
            <section className={cn(pageClasses.sectionCard)}>
              <QuestsPanel />
            </section>

            <section>
              <TitleSelector />
            </section>

            <section className={cn(pageClasses.sectionCard)}>
              <AchievementsGrid />
            </section>
          </div>

          <div className={cn(pageClasses.right)}>
            <div className={cn(pageClasses.rightSticky)}>
              <LeaderboardTable />
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}
