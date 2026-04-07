'use client';

import { HelpCircle } from 'lucide-react';
import { cn } from '@/lib/utils';

interface GachaBannerHeaderProps {
 description: string;
 name: string;
 onOpenOdds: () => void;
 viewOddsLabel: string;
}

export function GachaBannerHeader({
 description,
 name,
 onOpenOdds,
 viewOddsLabel,
}: GachaBannerHeaderProps) {
 return (
  <div className={cn('p-6 relative z-10')}>
   <div className={cn('flex justify-between items-start')}>
    <h3 className={cn('text-xl font-black uppercase tracking-widest bg-clip-text text-transparent bg-gradient-to-r from-purple-400 to-indigo-400')}>{name}</h3>
    <button onClick={onOpenOdds} className={cn('p-1.5 rounded-lg bg-stone-800/50 text-stone-400 hover:text-stone-200 transition-colors border border-stone-700/50')} title={viewOddsLabel}>
     <HelpCircle className={cn('w-4 h-4')} />
    </button>
   </div>
   <p className={cn('text-[11px] font-medium text-stone-400 mt-2 uppercase tracking-tight leading-relaxed line-clamp-2')}>{description}</p>
  </div>
 );
}
