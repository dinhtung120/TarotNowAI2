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
  <div className={cn('rounded-2xl p-4 flex flex-col items-center justify-center relative border backdrop-blur-md animate-in zoom-in-75 duration-300 group', glowClass, borderClass, 'w-full tn-aspect-4-5')}>
   <div className={cn('absolute top-2 right-2')}>
    <span className={cn('tn-text-2xs font-black uppercase px-2 py-0.5 rounded-full border bg-black/50', rarityStyle.text, rarityStyle.border)}>{rarityInitial}</span>
   </div>
   {renderRewardIcon('w-16 h-16 mb-4 transition-transform duration-500')}
   <h3 className={cn('text-center tn-text-overline font-bold uppercase tracking-tight line-clamp-1', rarityStyle.text)}>{name}</h3>
  </div>
 );
}
