import Lottie from 'lottie-react';
import { cn } from '@/lib/utils';

interface GachaResultModalRevealStageProps {
  animationData: object | null;
  isUltraRare: boolean;
  revealLabel: string;
}

export function GachaResultModalRevealStage({ animationData, isUltraRare, revealLabel }: GachaResultModalRevealStageProps) {
  return (
    <div className={cn('animate-in fade-in duration-500 flex flex-col items-center justify-center space-y-6')}>
      <div className={cn('absolute inset-0 -z-10 bg-gradient-to-b from-transparent to-transparent opacity-20 blur-[120px]', isUltraRare ? 'from-amber-500' : 'from-purple-500')} />

      <div className={cn('relative h-64 w-64')}>
        {animationData ? (
          <Lottie animationData={animationData} loop={false} autoplay className={cn('h-full w-full')} />
        ) : (
          <div className={cn('flex h-64 w-64 items-center justify-center rounded-full bg-gradient-to-tr from-emerald-500/20 via-amber-500/20 to-purple-500/20 animate-pulse')}>
            <span className={cn('lunar-metallic-text text-6xl font-black')}>✦</span>
          </div>
        )}
      </div>

      <p className={cn('lunar-metallic-text animate-pulse text-sm font-black uppercase tracking-[0.3em]')}>
        {revealLabel}
      </p>
    </div>
  );
}
