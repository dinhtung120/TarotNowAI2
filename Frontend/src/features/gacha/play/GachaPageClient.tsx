'use client';

import { useLocale, useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import InlineErrorAlert from '@/shared/ui/InlineErrorAlert';
import GlassCard from '@/shared/ui/GlassCard';
import SectionHeader from '@/shared/ui/SectionHeader';
import GachaPoolSelector from '@/features/gacha/play/GachaPoolSelector';
import GachaRewardPreview from '@/features/gacha/play/GachaRewardPreview';
import GachaResultModal from '@/features/gacha/result/GachaResultModal';
import GachaHistoryPreview from '@/features/gacha/history/GachaHistoryPreview';
import { useGachaPageState } from '@/features/gacha/play/useGachaPageState';

export default function GachaPageClient() {
 const locale = useLocale();
 const t = useTranslations('Gacha');
 const pull10xLabel = t.has('pull10x') ? t('pull10x') : 'Rút 10x';
 const {
  activePoolCode,
  historyItems,
  isPulling,
  isResultOpen,
  onPull,
  pools,
  pullError,
  queryErrorMessage,
  resultModalData,
  rewards,
  setIsResultOpen,
  setSelectedPoolCode,
 } = useGachaPageState({
  pullErrorMessage: t('pullError'),
 });

 return (
  <div className={cn('mx-auto w-full max-w-6xl space-y-10 px-4 pb-24 pt-28 sm:px-6')}>
   <header className={cn('space-y-3')}>
    <h1 className={cn('lunar-metallic-text text-3xl font-black uppercase tracking-[0.2em] sm:text-5xl')}>{t('title')}</h1>
    <p className={cn('max-w-2xl text-base font-medium leading-relaxed tn-text-secondary')}>{t('subtitle')}</p>
   </header>

   <InlineErrorAlert message={queryErrorMessage} title="Hệ thống ghi nhận lỗi:" />
   <InlineErrorAlert message={pullError} />

   <section className={cn('relative')}>
    <div className={cn('absolute -left-20 -top-20 h-64 w-64 rounded-full bg-emerald-500/5 blur-[100px]')} />
    <GachaPoolSelector
     pools={pools}
     locale={locale}
     selectedPoolCode={activePoolCode}
     isPulling={isPulling}
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
