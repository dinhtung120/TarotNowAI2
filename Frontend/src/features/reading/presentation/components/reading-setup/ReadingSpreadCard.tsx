import { Flame, Moon, ShieldCheck, Star } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import type { ReadingSpreadOption } from '@/features/reading/application/useReadingSetupPage';
import { cn } from '@/lib/utils';

interface ReadingSpreadCardProps {
 spread: ReadingSpreadOption;
 selectedSpreadId: ReadingSpreadOption['id'];
 selectedCurrency: 'gold' | 'diamond';
 expBonusLabel: (amount: number) => string;
 onSelectSpread: (spreadId: ReadingSpreadOption['id']) => void;
}

function renderSpreadIcon(icon: ReadingSpreadOption['icon']) {
 if (icon === 'flame') return <Flame className={cn('w-5 h-5')} />;
 if (icon === 'shield') return <ShieldCheck className={cn('w-5 h-5')} />;
 if (icon === 'moon') return <Moon className={cn('w-5 h-5')} />;
 return <Star className={cn('w-5 h-5')} />;
}

export function ReadingSpreadCard({
 spread,
 selectedSpreadId,
 selectedCurrency,
 expBonusLabel,
 onSelectSpread,
}: ReadingSpreadCardProps) {
 const isSelected = selectedSpreadId === spread.id;
 const isFree = spread.id === 'daily_1';
 const showExpBonus = spread.exp > 1;

  return (
  <GlassCard
   variant={isSelected ? 'elevated' : 'interactive'}
   className={cn('group relative cursor-pointer transition-all duration-500 overflow-hidden', isSelected ? 'tn-border-accent-50 tn-shadow-glow-accent-15 ring-1 ring-purple-400/50' : 'tn-hover-border-accent-30')}
   onClick={() => onSelectSpread(spread.id)}
   padding="lg"
  >
   <div className={cn('flex justify-between items-start mb-4 relative z-10')}>
    <div className={cn('w-12 h-12 rounded-2xl flex items-center justify-center border transition-all duration-500', isSelected ? 'tn-bg-accent tn-text-ink border-transparent scale-110 tn-shadow-glow-accent' : 'tn-surface tn-text-secondary tn-border-soft')}>
     {renderSpreadIcon(spread.icon)}
    </div>
    <div className={cn('flex flex-col items-end gap-2')}>
     <span className={cn('px-3 py-1 rounded-full tn-text-overline border transition-colors', isFree ? 'tn-bg-success-soft tn-text-success tn-border-success' : selectedCurrency === 'diamond' ? 'tn-bg-info-soft tn-text-info tn-border-info tn-shadow-glow-info-soft' : 'tn-bg-warning-soft tn-text-warning tn-border-warning')}>{spread.cost}</span>
     <span className={cn('px-2 py-0.5 rounded-lg tn-text-2xs font-black uppercase tracking-tighter border animate-pulse', showExpBonus ? 'tn-bg-accent tn-text-ink border-transparent tn-shadow-glow-accent-soft' : 'tn-surface tn-text-tertiary tn-border-soft')}>{expBonusLabel(spread.exp)}</span>
    </div>
   </div>
   <div className={cn('relative z-10')}>
    <h3 className={cn('text-xl font-bold mb-2 tracking-tight transition-colors', isSelected ? 'tn-text-primary' : 'tn-text-secondary')}>{spread.name}</h3>
    <p className={cn('tn-text-tertiary text-sm leading-relaxed font-medium')}>{spread.desc}</p>
   </div>
   {isSelected ? <div className={cn('absolute inset-0 tn-grad-accent-soft pointer-events-none')} /> : null}
  </GlassCard>
 );
}
