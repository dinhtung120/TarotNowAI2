'use client';

import { useMemo, useState } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import GlassCard from '@/shared/components/ui/GlassCard';
import GachaHistoryTable from '@/components/ui/gacha/GachaHistoryTable';
import { GachaHistoryPagination } from '@/components/ui/gacha/GachaHistoryPagination';
import { useGachaHistory } from '@/shared/infrastructure/gacha/useGachaHistory';

const DEFAULT_PAGE_SIZE = 20;

export default function GachaHistoryPageClient() {
 const locale = useLocale();
 const t = useTranslations('Gacha');
 const [page, setPage] = useState(1);
 const historyQuery = useGachaHistory({ page, pageSize: DEFAULT_PAGE_SIZE });

 const totalPages = useMemo(() => {
  const totalCount = historyQuery.data?.totalCount ?? 0;
  return Math.max(1, Math.ceil(totalCount / DEFAULT_PAGE_SIZE));
 }, [historyQuery.data?.totalCount]);

 const movePage = (nextPage: number) => {
  setPage(nextPage);
  window.scrollTo({ top: 0, behavior: 'smooth' });
 };

 return (
  <div className={cn('mx-auto w-full max-w-6xl space-y-8 px-4 pb-24 pt-28 sm:px-6')}>
   <GlassCard variant="default" padding="lg" className={cn('relative overflow-hidden')}>
    <div className={cn('absolute -right-24 -top-24 h-80 w-80 rounded-full bg-purple-500/5 blur-[120px]')} />

    <header className={cn('relative z-10 mb-12 space-y-3')}>
     <h1 className={cn('lunar-metallic-text text-3xl font-black uppercase tracking-[0.2em] sm:text-4xl')}>{t('historyPageTitle')}</h1>
     <p className={cn('tn-text-secondary max-w-2xl text-sm font-medium leading-relaxed sm:text-base')}>{t('historyPageSubtitle')}</p>
    </header>

    {historyQuery.error instanceof Error ? (
      <div className={cn('mb-8 rounded-2xl border border-red-500/20 bg-red-500/5 px-6 py-4 text-sm tn-text-danger')}>
       <p className={cn('mb-1 font-bold uppercase tracking-wider')}>Cảnh báo hệ thống:</p>
       {historyQuery.error.message}
      </div>
    ) : null}

    <div className={cn('relative z-10 min-h-[400px]')}>
     <GachaHistoryTable
      entries={historyQuery.data?.items ?? []}
      locale={locale}
      labels={{
       empty: t('emptyHistory'),
       pityReset: t('historyPityReset'),
       pityStayed: t('historyPityStayed'),
       pullCount: t('historyPullCount'),
      }}
     />
    </div>

    <div className={cn('mt-12 flex flex-col items-center justify-between gap-4 border-t tn-border-soft pt-10 sm:flex-row')}>
     <div className={cn('tn-text-secondary text-xs font-black uppercase tracking-[0.25em] opacity-70')}>
      {t('historyPageIndicator', { page, totalPages })}
     </div>
     <GachaHistoryPagination
      page={page}
      totalPages={totalPages}
      isLoading={historyQuery.isLoading}
      previousLabel={t('historyPrev')}
      nextLabel={t('historyNext')}
      onPrevious={() => movePage(page - 1)}
      onNext={() => movePage(page + 1)}
     />
    </div>
   </GlassCard>
  </div>
 );
}
