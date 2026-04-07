'use client';

import { useTranslations } from 'next-intl';
import type { GachaBannerDto } from '../gacha.types';
import GlassCard from '@/shared/components/ui/GlassCard';
import { useState } from 'react';
import { GachaOddsModal } from './GachaOddsModal';
import { GachaBannerHeader } from './banner-card/GachaBannerHeader';
import { GachaPityProgress } from './banner-card/GachaPityProgress';
import { GachaSpinButtons } from './banner-card/GachaSpinButtons';
import { cn } from '@/lib/utils';

interface GachaBannerCardProps {
  banner: GachaBannerDto;
  onSpin: (bannerCode: string, count: number) => void;
  isSpinning: boolean;
  currentPity: number;
  hardPityCount: number;
}

export function GachaBannerCard({ banner, onSpin, isSpinning, currentPity, hardPityCount }: GachaBannerCardProps) {
  const t = useTranslations('gacha');
  const [isOddsOpen, setIsOddsOpen] = useState(false);

  const isVi = t('lang') === 'vi';
  const name = (isVi ? banner.nameVi : banner.nameEn) || '';
  const description = (isVi ? banner.descriptionVi : banner.descriptionEn) || '';

  const pityPercentage = Math.min(Math.max((currentPity / hardPityCount) * 100, 2), 100);

  return (
    <GlassCard variant="interactive" padding="none" className={cn('w-full max-w-md bg-stone-900 border-stone-800 text-stone-100 overflow-hidden relative group flex flex-col')}>
      <div className={cn('absolute inset-0 bg-gradient-to-br from-indigo-900/20 via-purple-900/10 to-transparent opacity-50 group-hover:opacity-100 transition-opacity duration-500 pointer-events-none')} />
      <GachaBannerHeader
        description={description}
        name={name}
        onOpenOdds={() => setIsOddsOpen(true)}
        viewOddsLabel={t('viewOdds')}
      />
      <div className={cn('px-6 pb-6 relative z-10 space-y-6 flex-1')}>
        <GachaPityProgress
          pityLabel={t('pityProgress')}
          currentPity={currentPity}
          hardPityCount={hardPityCount}
          pityPercentage={pityPercentage}
          guaranteedLabel={t('guaranteedLegendary')}
        />
      </div>
      <GachaSpinButtons
        bannerCode={banner.code}
        costDiamond={banner.costDiamond}
        isSpinning={isSpinning}
        onSpin={onSpin}
        spinningLabel={t('spinning')}
      />
      {isOddsOpen ? <GachaOddsModal bannerCode={banner.code} isOpen={isOddsOpen} onClose={() => setIsOddsOpen(false)} /> : null}
    </GlassCard>
  );
}
