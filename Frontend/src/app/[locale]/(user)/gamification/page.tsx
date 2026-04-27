import { getTranslations } from 'next-intl/server';
import { Gamepad2 } from 'lucide-react';
import { cn } from '@/lib/utils';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchGamificationHubPage } from '@/shared/server/prefetch/runners';
import { generateGamificationMetadata } from './metadata';
import { AchievementsGrid, GamificationStatsBar, QuestsPanel, TitleSelector } from './GamificationSections';
import { gamificationPageClasses } from './page.styles';

export { generateGamificationMetadata as generateMetadata };

export default async function GamificationPage() {
  const t = await getTranslations('Gamification');
  const state = await dehydrateAppQueries(prefetchGamificationHubPage);
  const c = gamificationPageClasses;

  return (
    <AppQueryHydrationBoundary state={state}>
      <div className={cn(c.root)}>
        <div className={cn(c.glowTop)} />
        <div className={cn(c.glowLeft)} />
        <div className={cn(c.glowRight)} />
        <main className={cn(c.main)}>
          <header className={cn(c.header)}>
            <div className={cn(c.iconWrap)}>
              <Gamepad2 className={cn('h-10 w-10 text-white')} />
            </div>
            <div>
              <h1 className={cn(c.title)}>{t('GamificationMetaTitle')}</h1>
              <p className={cn(c.desc)}>{t('GamificationMetaDesc')}</p>
            </div>
          </header>
          <GamificationStatsBar />
          <div className={cn(c.contentLayout)}>
            <section className={cn(c.sectionCard)}>
              <QuestsPanel />
            </section>
            <section>
              <TitleSelector />
            </section>
            <section className={cn(c.sectionCard)}>
              <AchievementsGrid />
            </section>
          </div>
        </main>
      </div>
    </AppQueryHydrationBoundary>
  );
}

