'use client';

import { Diamond, Sparkles } from 'lucide-react';
import Button from '@/shared/components/ui/Button';
import { cn } from '@/lib/utils';

interface GachaSpinButtonsProps {
 bannerCode: string;
 costDiamond: number;
 isSpinning: boolean;
 onSpin: (bannerCode: string, count: number) => void;
 spinningLabel: string;
}

export function GachaSpinButtons({
 bannerCode,
 costDiamond,
 isSpinning,
 onSpin,
 spinningLabel,
}: GachaSpinButtonsProps) {
 return (
  <div className={cn('px-6 pb-6 relative z-10 flex gap-3')}>
   <Button variant="primary" size="md" className={cn('flex-1 flex flex-col items-center tn-minh-60')} onClick={() => onSpin(bannerCode, 1)} disabled={isSpinning} isLoading={isSpinning}>
    {!isSpinning ? <div className={cn('flex flex-col items-center py-1')}><span className={cn('flex items-center gap-1')}><Sparkles className={cn('w-3 h-3')} /> 1x</span><span className={cn('flex items-center text-amber-400 tn-text-overline mt-0.5')}>{costDiamond} <Diamond className={cn('w-2.5 h-2.5 ml-1 fill-amber-400')} /></span></div> : null}
   </Button>
   <Button variant="brand" size="md" className={cn('flex-1 flex flex-col items-center tn-minh-60')} onClick={() => onSpin(bannerCode, 10)} disabled={isSpinning}>
    {isSpinning ? <span className={cn('animate-shimmer')}>{spinningLabel}</span> : <div className={cn('flex flex-col items-center py-1')}><span className={cn('flex items-center gap-1')}><Sparkles className={cn('w-3 h-3')} /> 10x</span><span className={cn('flex items-center text-stone-900 tn-text-overline mt-0.5')}>{costDiamond * 10} <Diamond className={cn('w-2.5 h-2.5 ml-1 fill-stone-900')} /></span></div>}
   </Button>
  </div>
 );
}
