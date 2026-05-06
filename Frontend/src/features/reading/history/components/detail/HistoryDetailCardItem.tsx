import Image from 'next/image';
import { cn } from '@/lib/utils';
import { shouldUseUnoptimizedImage } from '@/shared/http/assetUrl';

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
 const unoptimizedCardImage = shouldUseUnoptimizedImage(cardImageUrl);

 return (
  <div className={cn('group flex flex-col items-center gap-6')}>
   <div className={cn('tn-history-card-lift relative w-full tn-aspect-14-22 flex flex-col items-center group cursor-pointer transition-all duration-700')}>
    <div className={cn('tn-history-card-shell w-full h-full tn-surface-strong rounded-xl flex items-center justify-center shadow-xl overflow-hidden relative border tn-border-soft transition-shadow')}>
     <div className={cn('tn-history-card-overlay absolute inset-0 pointer-events-none')} />
     <div className={cn('tn-history-card-inner-border absolute inset-2 rounded-lg pointer-events-none')} />
     {cardImageUrl ? (
      <Image
       src={cardImageUrl}
       alt={cardName}
       fill
       unoptimized={unoptimizedCardImage}
       sizes="(max-width: 1024px) 45vw, 220px"
       className={cn('h-full w-full object-cover')}
      />
     ) : (
      <span className={cn('text-5xl font-serif font-black tn-text-primary/10 drop-shadow-sm')}>{index + 1}</span>
     )}
    </div>
   </div>
   <div className={cn('tn-group-scale-105 mt-4 text-center px-2 transition-all duration-500')}>
    <h3 className={cn('text-sm font-bold tn-text-primary font-serif leading-tight px-2 mb-2')}>{cardName}</h3>
    <p className={cn('tn-text-9 font-black uppercase tn-tracking-025 tn-text-accent mb-2')}>{essenceLabel}</p>
    <p className={cn('tn-text-11 font-medium tn-text-secondary leading-relaxed italic line-clamp-2 transition-all')}>{cardMeaning}</p>
   </div>
  </div>
 );
}
