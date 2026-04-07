import Image from 'next/image';
import { cn } from '@/lib/utils';

interface HistoryDetailCardItemProps {
 index: number;
 cardName: string;
 cardMeaning: string;
 cardImageUrl?: string;
 essenceLabel: string;
}

export function HistoryDetailCardItem({
 index,
 cardName,
 cardMeaning,
 cardImageUrl,
 essenceLabel,
}: HistoryDetailCardItemProps) {
 return (
  <div className={cn('group flex flex-col items-center gap-6')}>
   <div className={cn('relative w-full aspect-[14/22] flex flex-col items-center group cursor-pointer transition-all duration-700 hover:-translate-y-2')}>
    <div className={cn('w-full h-full tn-surface-strong rounded-xl flex items-center justify-center shadow-xl overflow-hidden relative border tn-border-soft group-hover:shadow-[0_10px_40px_var(--c-168-85-247-20)] transition-shadow')}>
     <div className={cn('absolute inset-0 bg-gradient-to-tr from-[var(--purple-accent)]/20 to-transparent pointer-events-none')} />
     <div className={cn('absolute inset-2 border border-[var(--purple-accent)]/10 rounded-lg pointer-events-none')} />
     {cardImageUrl ? (
      <Image
       src={cardImageUrl}
       alt={cardName}
       fill
       unoptimized
       sizes="(max-width: 1024px) 45vw, 220px"
       className={cn('h-full w-full object-cover')}
      />
     ) : (
      <span className={cn('text-5xl font-serif font-black tn-text-primary/10 drop-shadow-sm')}>{index + 1}</span>
     )}
    </div>
   </div>
   <div className={cn('mt-4 text-center px-2 transition-all duration-500 group-hover:scale-105')}>
    <h3 className={cn('text-sm font-bold tn-text-primary font-serif leading-tight px-2 mb-2')}>{cardName}</h3>
    <p className={cn('text-[9px] font-black uppercase tracking-[0.25em] text-[var(--purple-accent)] mb-2')}>{essenceLabel}</p>
    <p className={cn('text-[11px] font-medium text-[var(--text-secondary)] leading-relaxed italic line-clamp-2 hover:line-clamp-none transition-all')}>{cardMeaning}</p>
   </div>
  </div>
 );
}
