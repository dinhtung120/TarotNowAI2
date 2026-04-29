'use client';

import Image from 'next/image';
import { cn, formatCardStat } from '@/lib/utils';
import type { CardOption } from '@/shared/application/inventory/cardOption';
import { shouldUseUnoptimizedImage } from '@/shared/infrastructure/http/assetUrl';

interface UseItemCardPreviewProps {
  card: CardOption | null;
  label: string;
}

export default function UseItemCardPreview({ card, label }: UseItemCardPreviewProps) {
  if (!card) {
    return null;
  }

  // Đảm bảo đường dẫn ảnh hợp lệ (thêm / nếu là đường dẫn tương đối)
  const imageUrl = card.imageUrl 
    ? (card.imageUrl.startsWith('http') || card.imageUrl.startsWith('/') ? card.imageUrl : `/${card.imageUrl}`)
    : null;
  const unoptimizedCardImage = shouldUseUnoptimizedImage(imageUrl);

  return (
    <section className={cn('relative overflow-hidden rounded-[1.5rem] border tn-border-soft bg-[#0a0a0c]/40 p-4 transition-all duration-500 hover:tn-border-accent/30')}>
      <div className={cn("absolute -right-4 -top-4 h-16 w-16 rounded-full bg-violet-500/5 blur-2xl")} />
      
      <p className={cn('tn-text-muted mb-4 text-[10px] font-black tracking-[0.2em] uppercase opacity-40')}>
        {label}
      </p>
      
      <div className={cn('flex items-center gap-5')}>
        <div className={cn(
          'relative h-24 w-16 shrink-0 overflow-hidden rounded-xl border tn-border-soft',
          'shadow-[0_0_15px_rgba(0,0,0,0.5)] transition-transform duration-500 hover:scale-105'
        )}>
          {imageUrl ? (
            <Image 
              src={imageUrl} 
              alt={card.name} 
              fill 
              unoptimized={unoptimizedCardImage}
              className={cn('object-cover')} 
              sizes="64px" 
            />
          ) : (
            <div className={cn('flex h-full w-full items-center justify-center bg-white/[0.02] text-xl tn-text-muted')}>
              <span className={cn("opacity-20 flex flex-col items-center gap-1")}>
                <span className={cn("text-[10px] font-black uppercase tracking-tighter")}>No</span>
                <span className={cn("text-sm")}>Img</span>
              </span>
            </div>
          )}
          {/* Decorative frame overlay */}
          <div className={cn("pointer-events-none absolute inset-0 rounded-xl border border-white/5")} />
        </div>
        
        <div className={cn('min-w-0 flex-1 space-y-3')}>
          <p className={cn('truncate text-base font-black tracking-tight tn-text-primary group-hover:lunar-metallic-text')}>
            {card.name}
          </p>
          
          {card.stats ? (
            <div className={cn("grid grid-cols-2 gap-x-4 gap-y-2")}>
              <div className={cn("flex flex-col")}>
                <span className={cn("text-[9px] font-black uppercase tracking-widest tn-text-muted opacity-40")}>Level</span>
                <span className={cn("text-xs font-black tn-text-secondary")}>Lv {card.stats.level}</span>
              </div>
              <div className={cn("flex flex-col")}>
                <span className={cn("text-[9px] font-black uppercase tracking-widest tn-text-muted opacity-40")}>Attack</span>
                <span className={cn("text-xs font-black text-rose-400")}>{formatCardStat(card.stats.totalAtk)}</span>
              </div>
              <div className={cn("flex flex-col")}>
                <span className={cn("text-[9px] font-black uppercase tracking-widest tn-text-muted opacity-40")}>Defense</span>
                <span className={cn("text-xs font-black text-sky-400")}>{formatCardStat(card.stats.totalDef)}</span>
              </div>
              <div className={cn("flex flex-col")}>
                <span className={cn("text-[9px] font-black uppercase tracking-widest tn-text-muted opacity-40")}>Experience</span>
                <span className={cn("text-[10px] font-bold tn-text-secondary")}>
                  {formatCardStat(card.stats.currentExp)} <span className={cn("opacity-30")}>/</span> {formatCardStat(card.stats.expToNextLevel)}
                </span>
              </div>
            </div>
          ) : (
            <p className={cn("text-[10px] font-medium tn-text-secondary opacity-40 italic")}>
              No stats available
            </p>
          )}
        </div>
      </div>
    </section>
  );
}
