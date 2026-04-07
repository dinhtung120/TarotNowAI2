import { getTranslations } from "next-intl/server";
import AchievementsGrid from "@/features/gamification/components/AchievementsGrid";
import GamificationStatsBar from "@/features/gamification/components/GamificationStatsBar";
import LeaderboardTable from "@/features/gamification/components/LeaderboardTable";
import QuestsPanel from "@/features/gamification/components/QuestsPanel";
import TitleSelector from "@/features/gamification/components/TitleSelector";
import { Gamepad2 } from "lucide-react";
import { generateGamificationMetadata } from "./metadata";

export { generateGamificationMetadata as generateMetadata };

export default async function GamificationPage() {
  const t = await getTranslations("Gamification");

  return (
    <div className="relative min-h-screen overflow-hidden bg-slate-950 px-4 py-8 sm:px-6 lg:px-8">
      <div className="pointer-events-none absolute inset-x-0 top-0 h-96 bg-gradient-to-b from-indigo-900/20 to-slate-950" />
      <div className="pointer-events-none absolute top-[-10%] left-[-10%] h-[40%] w-[40%] rounded-full bg-purple-600/10 blur-[120px]" />
      <div className="pointer-events-none absolute top-[20%] right-[-10%] h-[50%] w-[30%] rounded-full bg-blue-600/10 blur-[100px]" />

      <main className="relative z-10 mx-auto max-w-7xl">
        <header className="mb-12 flex flex-col items-center gap-6 text-center md:flex-row md:text-left">
          <div className="rounded-2xl bg-gradient-to-br from-indigo-500 to-purple-600 p-4 shadow-xl ring-1 shadow-purple-500/20 ring-white/10">
            <Gamepad2 className="h-10 w-10 text-white" />
          </div>
          <div>
            <h1 className="bg-gradient-to-r from-white to-slate-400 bg-clip-text text-4xl font-black tracking-tight text-transparent text-white sm:text-5xl">
              {t("GamificationMetaTitle")}
            </h1>
            <p className="mt-3 max-w-2xl text-lg text-slate-400">
              {t("GamificationMetaDesc")}
            </p>
          </div>
        </header>

        <GamificationStatsBar />

        <div className="grid grid-cols-1 gap-8 xl:grid-cols-3">
          <div className="space-y-8 xl:col-span-2">
            <section className="rounded-3xl border border-slate-800/60 bg-slate-900/40 p-6 shadow-2xl backdrop-blur-md sm:p-8">
              <QuestsPanel />
            </section>

            <section>
              <TitleSelector />
            </section>

            <section className="rounded-3xl border border-slate-800/60 bg-slate-900/40 p-6 shadow-2xl backdrop-blur-md sm:p-8">
              <AchievementsGrid />
            </section>
          </div>

          <div className="xl:col-span-1">
            <div className="sticky top-8">
              <LeaderboardTable />
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}
