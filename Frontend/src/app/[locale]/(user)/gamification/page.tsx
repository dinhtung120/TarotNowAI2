import { getTranslations } from 'next-intl/server';
import QuestsPanel from '@/features/gamification/components/QuestsPanel';
import AchievementsGrid from '@/features/gamification/components/AchievementsGrid';
import TitleSelector from '@/features/gamification/components/TitleSelector';
import LeaderboardTable from '@/features/gamification/components/LeaderboardTable';
import GamificationStatsBar from '@/features/gamification/components/GamificationStatsBar';
import { Gamepad2 } from 'lucide-react';

export async function generateMetadata({ params }: { params: Promise<{ locale: string }> }) {
  const { locale } = await params;
  const t = await getTranslations({ locale, namespace: 'Gamification' });

  return {
    title: `${t('GamificationMetaTitle')} - TarotNow AI`,
    description: t('GamificationMetaDesc'),
  };
}

export default async function GamificationPage() {
  const t = await getTranslations('Gamification');

  return (
    <div className="min-h-screen bg-slate-950 px-4 py-8 sm:px-6 lg:px-8 relative overflow-hidden">
      {/* Background Effects */}
      <div className="absolute top-0 inset-x-0 h-96 bg-gradient-to-b from-indigo-900/20 to-slate-950 pointer-events-none" />
      <div className="absolute top-[-10%] left-[-10%] w-[40%] h-[40%] bg-purple-600/10 rounded-full blur-[120px] pointer-events-none" />
      <div className="absolute top-[20%] right-[-10%] w-[30%] h-[50%] bg-blue-600/10 rounded-full blur-[100px] pointer-events-none" />

      <main className="max-w-7xl mx-auto relative z-10">
        <header className="mb-12 text-center md:text-left flex flex-col md:flex-row items-center gap-6">
          <div className="p-4 bg-gradient-to-br from-indigo-500 to-purple-600 rounded-2xl shadow-xl shadow-purple-500/20 ring-1 ring-white/10">
            <Gamepad2 className="w-10 h-10 text-white" />
          </div>
          <div>
            <h1 className="text-4xl font-black tracking-tight text-white sm:text-5xl bg-clip-text text-transparent bg-gradient-to-r from-white to-slate-400">
              {t('GamificationMetaTitle')}
            </h1>
            <p className="mt-3 text-lg text-slate-400 max-w-2xl">
              {t('GamificationMetaDesc')}
            </p>
          </div>
        </header>

        <GamificationStatsBar />

        <div className="grid grid-cols-1 xl:grid-cols-3 gap-8">
          {/* Main Content Area (Quests & Achievements) */}
          <div className="xl:col-span-2 space-y-8">
            <section className="bg-slate-900/40 border border-slate-800/60 rounded-3xl p-6 sm:p-8 backdrop-blur-md shadow-2xl">
              <QuestsPanel />
            </section>
            
            <section>
              <TitleSelector />
            </section>

            <section className="bg-slate-900/40 border border-slate-800/60 rounded-3xl p-6 sm:p-8 backdrop-blur-md shadow-2xl">
              <AchievementsGrid />
            </section>
          </div>

          {/* Sidebar Area (Leaderboard) */}
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
