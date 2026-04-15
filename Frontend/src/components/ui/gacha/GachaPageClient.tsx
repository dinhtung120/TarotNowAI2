'use client';

import { useCallback, useMemo, useState } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { Link } from '@/i18n/routing';
import { cn } from '@/lib/utils';
import GachaPoolSelector from '@/components/ui/gacha/GachaPoolSelector';
import GachaRewardPreview from '@/components/ui/gacha/GachaRewardPreview';
import GachaResultModal from '@/components/ui/gacha/GachaResultModal';
import { useGacha } from '@/shared/infrastructure/gacha/useGacha';
import { usePullGacha } from '@/shared/infrastructure/gacha/usePullGacha';
import type { PullGachaResult } from '@/shared/infrastructure/gacha/gachaTypes';

function localizeText(value: { nameVi: string; nameEn: string; nameZh: string }, locale: string): string {
  if (locale === 'en') return value.nameEn;
  if (locale === 'zh') return value.nameZh;
  return value.nameVi;
}

export default function GachaPageClient() {
  const locale = useLocale();
 const t = useTranslations('Gacha');
 const [selectedPoolCode, setSelectedPoolCode] = useState('');
 const [resultModalData, setResultModalData] = useState<PullGachaResult | null>(null);
 const [isResultOpen, setIsResultOpen] = useState(false);

  const { poolsQuery, poolOddsQuery, historyPreviewQuery, resolvedPoolCode } = useGacha({
    selectedPoolCode,
    historyPreviewSize: 6,
  });
  const pullMutation = usePullGacha();
  const activePoolCode = selectedPoolCode || resolvedPoolCode;

  const pools = poolsQuery.data ?? [];
  const rewards = poolOddsQuery.data?.rewards ?? [];
  const historyItems = historyPreviewQuery.data?.items ?? [];

  const onPull = useCallback(async () => {
    if (!activePoolCode) return;
    const result = await pullMutation.mutateAsync({ poolCode: activePoolCode, count: 1 });
    setResultModalData(result);
    setIsResultOpen(true);
  }, [activePoolCode, pullMutation]);

  const queryError = useMemo(() => {
    const candidates = [poolsQuery.error, poolOddsQuery.error, historyPreviewQuery.error];
    return candidates.find((candidate) => candidate instanceof Error) as Error | undefined;
  }, [historyPreviewQuery.error, poolOddsQuery.error, poolsQuery.error]);

  return (
    <div className={cn('mx-auto w-full max-w-6xl space-y-6 px-4 pb-16 pt-24 sm:px-6')}>
      <header className={cn('space-y-1')}>
        <h1 className={cn('text-2xl font-semibold text-slate-900 dark:text-slate-100')}>{t('title')}</h1>
        <p className={cn('text-sm text-slate-600 dark:text-slate-300')}>{t('subtitle')}</p>
      </header>

      {queryError ? <p className={cn('rounded-xl border border-red-300 bg-red-50 px-4 py-3 text-sm text-red-700 dark:border-red-800 dark:bg-red-950/40 dark:text-red-200')}>{queryError.message}</p> : null}

      <GachaPoolSelector
        pools={pools}
        locale={locale}
        selectedPoolCode={activePoolCode}
        isPulling={pullMutation.isPending}
        labels={{ pull: t('pull1x'), pulling: t('pulling'), pity: t('pityProgress'), pityRuleHint: t('pityRuleHint') }}
        onSelectPool={setSelectedPoolCode}
        onPull={onPull}
      />

      <section className={cn('space-y-3')}>
        <h2 className={cn('text-base font-semibold text-slate-900 dark:text-slate-100')}>{t('oddsTitle')}</h2>
        <GachaRewardPreview rewards={rewards} locale={locale} emptyLabel={t('emptyOdds')} />
      </section>

      <section className={cn('space-y-3')}>
        <div className={cn('flex items-center justify-between gap-2')}>
          <h2 className={cn('text-base font-semibold text-slate-900 dark:text-slate-100')}>{t('historyTitle')}</h2>
          <Link href="/gacha/history" className={cn('text-sm font-medium text-emerald-700 hover:underline dark:text-emerald-300')}>
            {t('viewAllHistory')}
          </Link>
        </div>
        <div className={cn('grid grid-cols-1 gap-2 sm:grid-cols-2')}>
          {historyItems.map((item) => {
            const firstReward = item.rewards[0];
            const rewardLabel = firstReward ? localizeText(firstReward, locale) : t('emptyHistory');
            return (
              <article key={item.pullOperationId} className={cn('rounded-xl border border-slate-200 bg-white p-3 text-sm dark:border-slate-700 dark:bg-slate-900')}>
                <p className={cn('font-medium text-slate-900 dark:text-slate-100')}>{rewardLabel}</p>
                <p className={cn('text-xs text-slate-500 dark:text-slate-300')}>
                  {item.poolCode} • {item.pullCount}x • {item.wasPityReset ? t('historyPityResetShort') : t('historyPityStayedShort')}
                </p>
              </article>
            );
          })}
        </div>
      </section>

      <GachaResultModal
        isOpen={isResultOpen}
        locale={locale}
        result={resultModalData}
        labels={{
          title: t('pullResultTitle'),
          pityTriggered: t('pityTriggered'),
          close: t('close'),
          rareDropAnimation: t('rareDropAnimation'),
        }}
        onClose={() => setIsResultOpen(false)}
      />
    </div>
  );
}
