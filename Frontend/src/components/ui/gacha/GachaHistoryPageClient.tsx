'use client';

import { useMemo, useState } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import Button from '@/shared/components/ui/Button';
import GachaHistoryTable from '@/components/ui/gacha/GachaHistoryTable';
import { useGachaHistory } from '@/shared/infrastructure/gacha/useGachaHistory';

const DEFAULT_PAGE_SIZE = 20;

export default function GachaHistoryPageClient() {
  const locale = useLocale();
  const t = useTranslations('gacha.gacha');
  const [page, setPage] = useState(1);
  const historyQuery = useGachaHistory({ page, pageSize: DEFAULT_PAGE_SIZE });

  const totalPages = useMemo(() => {
    const totalCount = historyQuery.data?.totalCount ?? 0;
    return Math.max(1, Math.ceil(totalCount / DEFAULT_PAGE_SIZE));
  }, [historyQuery.data?.totalCount]);

  return (
    <div className={cn('mx-auto w-full max-w-6xl space-y-4 px-4 pb-16 pt-24 sm:px-6')}>
      <header className={cn('space-y-1')}>
        <h1 className={cn('text-2xl font-semibold text-slate-900 dark:text-slate-100')}>{t('historyPageTitle')}</h1>
        <p className={cn('text-sm text-slate-600 dark:text-slate-300')}>{t('historyPageSubtitle')}</p>
      </header>

      {historyQuery.error instanceof Error ? (
        <p className={cn('rounded-xl border border-red-300 bg-red-50 px-4 py-3 text-sm text-red-700 dark:border-red-800 dark:bg-red-950/40 dark:text-red-200')}>
          {historyQuery.error.message}
        </p>
      ) : null}

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

      <div className={cn('flex items-center justify-end gap-2')}>
        <Button type="button" disabled={page <= 1 || historyQuery.isLoading} onClick={() => setPage((currentPage) => currentPage - 1)}>
          {t('historyPrev')}
        </Button>
        <span className={cn('text-sm text-slate-700 dark:text-slate-200')}>
          {t('historyPageIndicator', { page, totalPages })}
        </span>
        <Button type="button" disabled={page >= totalPages || historyQuery.isLoading} onClick={() => setPage((currentPage) => currentPage + 1)}>
          {t('historyNext')}
        </Button>
      </div>
    </div>
  );
}

