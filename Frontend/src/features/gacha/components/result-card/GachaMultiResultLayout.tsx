'use client';

import type { ReactNode } from 'react';
import { cn } from '@/lib/utils';
import type { GachaRarityStyle } from './rarityConfig';

interface GachaMultiResultLayoutProps {
 borderClass: string;
 glowClass: string;
 name: string;
 rarityInitial: string;
 rarityStyle: GachaRarityStyle;
 renderRewardIcon: (sizeClass: string) => ReactNode;
}

export function GachaMultiResultLayout({
 borderClass,
 glowClass,
 name,
 rarityInitial,
 rarityStyle,
 renderRewardIcon,
}: GachaMultiResultLayoutProps) {
 return (
  <div className={cn('relative flex w-full flex-col items-center justify-center rounded-2xl border p-2.5 backdrop-blur-md animate-in zoom-in-75 duration-300 group sm:p-4', glowClass, borderClass, 'tn-aspect-4-5')}>
   <div className={cn('absolute top-2 right-2')}>
    <span className={cn('rounded-full border bg-black/50 px-1.5 py-0.5 text-[7px] font-black uppercase sm:px-2 sm:text-[8px]', rarityStyle.text, rarityStyle.border)}>{rarityInitial}</span>
   </div>
   {renderRewardIcon('mb-2 h-10 w-10 transition-transform duration-500 sm:mb-4 sm:h-16 sm:w-16')}
   <h3 className={cn('line-clamp-1 text-center text-[9px] font-bold uppercase tracking-tight sm:tn-text-overline', rarityStyle.text)}>{name}</h3>
  </div>
 );
}
