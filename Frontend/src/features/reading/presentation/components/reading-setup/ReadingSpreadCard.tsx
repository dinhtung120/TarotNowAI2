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
   className={cn('group relative cursor-pointer transition-all duration-500 overflow-hidden', isSelected ? 'border-[var(--purple-accent)]/50 shadow-[0_0_30px_var(--c-168-85-247-15)] ring-1 ring-[var(--purple-accent)]/50' : 'hover:tn-border')}
   onClick={() => onSelectSpread(spread.id)}
   padding="lg"
  >
   <div className={cn('flex justify-between items-start mb-4 relative z-10')}>
    <div className={cn('w-12 h-12 rounded-2xl flex items-center justify-center border transition-all duration-500', isSelected ? 'bg-[var(--purple-accent)] tn-text-ink border-transparent scale-110 shadow-[0_0_20px_var(--purple-accent)]' : 'tn-surface tn-text-secondary tn-border-soft group-hover:tn-surface-strong group-hover:scale-105')}>
     {renderSpreadIcon(spread.icon)}
    </div>
    <div className={cn('flex flex-col items-end gap-2')}>
     <span className={cn('px-3 py-1 rounded-full text-[10px] font-black uppercase tracking-widest border transition-colors', isFree ? 'bg-[var(--success-bg)] text-[var(--success)] border-[var(--success)]' : selectedCurrency === 'diamond' ? 'bg-[var(--info-bg)] text-[var(--info)] border-[var(--info)] shadow-[0_0_10px_var(--info-bg)]' : 'bg-[var(--warning-bg)] text-[var(--warning)] border-[var(--warning)]')}>{spread.cost}</span>
     <span className={cn('px-2 py-0.5 rounded-lg text-[8px] font-black uppercase tracking-tighter border animate-pulse', showExpBonus ? 'bg-[var(--purple-accent)] tn-text-ink border-transparent shadow-[0_0_10px_var(--purple-accent)]' : 'tn-surface tn-text-tertiary tn-border-soft')}>{expBonusLabel(spread.exp)}</span>
    </div>
   </div>
   <div className={cn('relative z-10')}>
    <h3 className={cn('text-xl font-bold mb-2 tracking-tight transition-colors', isSelected ? 'text-[var(--text-primary)]' : 'text-[var(--text-secondary)] group-hover:tn-text-primary')}>{spread.name}</h3>
    <p className={cn('text-[var(--text-tertiary)] text-sm leading-relaxed font-medium')}>{spread.desc}</p>
   </div>
   {isSelected ? <div className={cn('absolute inset-0 bg-gradient-to-br from-[var(--purple-accent)]/10 to-transparent pointer-events-none')} /> : null}
  </GlassCard>
 );
}
