'use client';

import type { GachaBannerDto } from '../gacha.types';
import GlassCard from '@/shared/components/ui/GlassCard';
import { GachaOddsModal } from './GachaOddsModal';
import { GachaBannerHeader } from './banner-card/GachaBannerHeader';
import { GachaPityProgress } from './banner-card/GachaPityProgress';
import { GachaSpinButtons } from './banner-card/GachaSpinButtons';
import { cn } from '@/lib/utils';
import { useGachaBannerCardState } from '@/features/gacha/components/hooks/useGachaBannerCardState';

interface GachaBannerCardProps {
  banner: GachaBannerDto;
  onSpin: (bannerCode: string, count: number) => void;
  isSpinning: boolean;
  currentPity: number;
  hardPityCount: number;
}

export function GachaBannerCard({ banner, onSpin, isSpinning, currentPity, hardPityCount }: GachaBannerCardProps) {
  const vm = useGachaBannerCardState({ banner, currentPity, hardPityCount });

  return (
    <GlassCard variant="interactive" padding="none" className={cn('w-full max-w-md bg-stone-900 border-stone-800 text-stone-100 overflow-hidden relative group flex flex-col')}>
      <div className={cn('absolute inset-0 bg-gradient-to-br from-indigo-900/20 via-purple-900/10 to-transparent tn-group-opacity-50-100 pointer-events-none')} />
      <GachaBannerHeader
        description={vm.description}
        name={vm.name}
        onOpenOdds={vm.openOdds}
        viewOddsLabel={vm.t('viewOdds')}
      />
      <div className={cn('px-6 pb-6 relative z-10 space-y-6 flex-1')}>
        <GachaPityProgress
          pityLabel={vm.t('pityProgress')}
          currentPity={currentPity}
          hardPityCount={hardPityCount}
          pityPercentage={vm.pityPercentage}
          guaranteedLabel={vm.t('guaranteedLegendary')}
        />
      </div>
      <GachaSpinButtons
        bannerCode={banner.code}
        costDiamond={banner.costDiamond}
        isSpinning={isSpinning}
        onSpin={onSpin}
        spinningLabel={vm.t('spinning')}
      />
      {vm.isOddsOpen ? <GachaOddsModal bannerCode={banner.code} isOpen={vm.isOddsOpen} onClose={vm.closeOdds} /> : null}
    </GlassCard>
  );
}
