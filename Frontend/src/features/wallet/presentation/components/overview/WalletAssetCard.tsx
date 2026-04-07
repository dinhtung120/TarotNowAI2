import type { LucideIcon } from 'lucide-react';
import { Activity } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface WalletAssetCardProps {
 icon: LucideIcon;
 iconColorClass: string;
 iconGlowClass: string;
 panelGlowClass: string;
 watermarkIconColorClass: string;
 label: string;
 description: string;
 value: string;
 footer: string;
 isDiamond?: boolean;
}

export function WalletAssetCard({
 icon: Icon,
 iconColorClass,
 iconGlowClass,
 panelGlowClass,
 watermarkIconColorClass,
 label,
 description,
 value,
 footer,
 isDiamond = false,
}: WalletAssetCardProps) {
 return (
  <GlassCard className={cn('relative group overflow-hidden !p-6 sm:!p-10 flex flex-col justify-between min-h-[13.5rem] sm:min-h-[16rem]')}>
   <div className={cn('absolute inset-0 bg-gradient-to-br opacity-0 group-hover:opacity-100 transition-opacity duration-700 pointer-events-none', panelGlowClass)} />
   <div className={cn('absolute top-0 right-0 p-8 opacity-5 transition-transform duration-700 group-hover:scale-110 group-hover:rotate-12 pointer-events-none')}>
    <Icon className={cn('w-40 h-40', watermarkIconColorClass)} />
   </div>
   <div className={cn('flex items-center gap-4 relative z-10')}>
    <div className={cn('w-14 h-14 rounded-2xl flex items-center justify-center border shadow-xl', iconGlowClass)}>
     <Icon className={cn('w-7 h-7', iconColorClass)} />
    </div>
    <div>
     <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]')}>{label}</div>
     <div className={cn('text-xs font-bold tn-text-primary')}>{description}</div>
    </div>
   </div>
   <div className={cn('space-y-1 relative z-10 mt-8')}>
    <div className={cn('text-4xl sm:text-5xl md:text-6xl font-black tn-text-primary italic tracking-tighter transition-transform duration-700 group-hover:translate-x-2')}>
     {value}
    </div>
    <div className={cn('flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)] group-hover:text-[var(--text-secondary)] transition-colors')}>
     {isDiamond ? <Activity className={cn('w-3.5 h-3.5')} /> : null}
     {footer}
    </div>
   </div>
  </GlassCard>
 );
}
