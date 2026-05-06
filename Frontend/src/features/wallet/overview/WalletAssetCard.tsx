import type { LucideIcon } from 'lucide-react';
import { Activity } from 'lucide-react';
import { GlassCard } from '@/shared/ui';
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
  <GlassCard className={cn('relative group overflow-hidden tn-pad-6-10-sm flex flex-col justify-between tn-minh-13_5-16-sm')}>
   <div className={cn('absolute inset-0 bg-gradient-to-br tn-group-fade-in-overlay transition-opacity duration-700 pointer-events-none', panelGlowClass)} />
   <div className={cn('absolute top-0 right-0 p-8 tn-group-watermark transition-transform duration-700 pointer-events-none')}>
    <Icon className={cn('w-40 h-40', watermarkIconColorClass)} />
   </div>
   <div className={cn('flex items-center gap-4 relative z-10')}>
    <div className={cn('w-14 h-14 rounded-2xl flex items-center justify-center border shadow-xl', iconGlowClass)}>
     <Icon className={cn('w-7 h-7', iconColorClass)} />
    </div>
    <div>
     <div className={cn('tn-text-overline tn-text-secondary')}>{label}</div>
     <div className={cn('text-xs font-bold tn-text-primary')}>{description}</div>
    </div>
   </div>
   <div className={cn('space-y-1 relative z-10 mt-8')}>
    <div className={cn('tn-text-display-4-5-6 font-black tn-text-primary italic tracking-tighter transition-transform duration-700 tn-group-shift-x-2')}>
     {value}
    </div>
    <div className={cn('flex items-center gap-2 tn-text-overline tn-text-tertiary tn-group-text-secondary transition-colors')}>
     {isDiamond ? <Activity className={cn('w-3.5 h-3.5')} /> : null}
     {footer}
    </div>
   </div>
  </GlassCard>
 );
}
