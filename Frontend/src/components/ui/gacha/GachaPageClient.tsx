'use client';

import { useCallback, useMemo, useState } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import GlassCard from '@/shared/components/ui/GlassCard';
import SectionHeader from '@/shared/components/ui/SectionHeader';
import GachaPoolSelector from '@/components/ui/gacha/GachaPoolSelector';
import GachaRewardPreview from '@/components/ui/gacha/GachaRewardPreview';
import GachaResultModal from '@/components/ui/gacha/GachaResultModal';
import GachaHistoryPreview from '@/components/ui/gacha/GachaHistoryPreview';
import { useGacha } from '@/shared/infrastructure/gacha/useGacha';
import { usePullGacha } from '@/shared/infrastructure/gacha/usePullGacha';
import type { PullGachaResult } from '@/shared/infrastructure/gacha/gachaTypes';

export default function GachaPageClient() {
 const locale = useLocale();
 const t = useTranslations('Gacha');
 const pull10xLabel = t.has('pull10x') ? t('pull10x') : 'Rút 10x';

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

 const onPull = useCallback(
  async (count: number = 1) => {
   if (!activePoolCode) return;
   try {
    const result = await pullMutation.mutateAsync({ poolCode: activePoolCode, count });
    setResultModalData(result);
    setIsResultOpen(true);
   } catch (error) {
    console.error('Gacha pull failed', error);
   }
  },
  [activePoolCode, pullMutation],
 );

 const queryError = useMemo(() => {
  const candidates = [poolsQuery.error, poolOddsQuery.error, historyPreviewQuery.error];
  return candidates.find((candidate) => candidate instanceof Error) as Error | undefined;
 }, [historyPreviewQuery.error, poolOddsQuery.error, poolsQuery.error]);

 return (
  <div className={cn('mx-auto w-full max-w-6xl space-y-10 px-4 pb-24 pt-28 sm:px-6')}>
   <header className={cn('space-y-3')}>
    <h1 className={cn('lunar-metallic-text text-3xl font-black uppercase tracking-[0.2em] sm:text-5xl')}>{t('title')}</h1>
    <p className={cn('max-w-2xl text-base font-medium leading-relaxed tn-text-secondary')}>{t('subtitle')}</p>
   </header>

   {queryError ? (
    <div className={cn('rounded-2xl border border-red-500/20 bg-red-500/5 px-6 py-4 text-sm tn-text-danger')}>
     <p className={cn('mb-1 font-bold uppercase tracking-widest')}>Hệ thống ghi nhận lỗi:</p>
     {queryError.message}
    </div>
   ) : null}

   <section className={cn('relative')}>
    <div className={cn('absolute -left-20 -top-20 h-64 w-64 rounded-full bg-emerald-500/5 blur-[100px]')} />
    <GachaPoolSelector
     pools={pools}
     locale={locale}
     selectedPoolCode={activePoolCode}
     isPulling={pullMutation.isPending}
     labels={{
      pull: t('pull1x'),
      pull10x: pull10xLabel,
      pulling: t('pulling'),
      pity: t('pityProgress'),
      pityRuleHint: '',
     }}
     onSelectPool={setSelectedPoolCode}
     onPull={onPull}
    />
   </section>

   <GlassCard variant="default" padding="lg" className={cn('space-y-6')}>
    <SectionHeader title={t('oddsTitle')} className={cn('mb-0')} />
    <GachaRewardPreview rewards={rewards} locale={locale} emptyLabel={t('emptyOdds')} />
   </GlassCard>

   <GachaHistoryPreview
    historyItems={historyItems}
    locale={locale}
    title={t('historyTitle')}
    viewAllLabel={t('viewAllHistory')}
    emptyLabel={t('emptyHistory')}
   />

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
