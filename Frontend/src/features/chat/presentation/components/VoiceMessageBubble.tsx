'use client';

import { useMemo } from 'react';
import { Pause, Play } from 'lucide-react';
import { cn } from '@/lib/utils';
import { useVoiceMessagePlayback } from '@/features/chat/presentation/components/useVoiceMessagePlayback';
import {
 formatDuration,
 generateStaticBars,
 STATIC_BAR_COUNT,
} from '@/features/chat/presentation/components/voiceMessageUtils';

interface VoiceMessageBubbleProps {
 audioUrl: string;
 durationMs?: number | null;
 isMe: boolean;
}

export default function VoiceMessageBubble({
 audioUrl,
 durationMs,
 isMe,
}: VoiceMessageBubbleProps) {
 const { playing, progress, displayDuration, togglePlay } = useVoiceMessagePlayback({
  audioUrl,
  durationMs,
 });

 const bars = useMemo(() => generateStaticBars(audioUrl), [audioUrl]);
 const barColor = isMe ? 'bg-white/80' : 'bg-[var(--purple-accent)]';
 const barDimColor = isMe ? 'bg-white/30' : 'bg-[var(--purple-accent)]/30';
 const playBtnClass = isMe
  ? 'bg-white/20 hover:bg-white/30 text-white'
  : 'bg-[var(--purple-accent)]/20 hover:bg-[var(--purple-accent)]/30 text-[var(--purple-accent)]';

 return (
  <div className={cn('flex items-center gap-2.5 min-w-[180px] max-w-[280px]')}>
   <button
    type="button"
    onClick={() => void togglePlay()}
    className={cn('w-9 h-9 rounded-full flex items-center justify-center shrink-0 transition-colors', playBtnClass)}
    title={playing ? 'Tạm dừng' : 'Phát'}
   >
    {playing ? <Pause className={cn('w-4 h-4')} /> : <Play className={cn('w-4 h-4 ml-0.5')} />}
   </button>

   <div className={cn('flex-1 flex items-center gap-[2px] h-7 min-w-0')}>
    {bars.map((level, index) => {
     const isPlayed = index / STATIC_BAR_COUNT <= progress;
     return (
      <div
       key={index}
       className={cn('flex-1 rounded-full transition-opacity duration-150', isPlayed ? barColor : barDimColor)}
       style={{ height: `${Math.max(3, Math.round(level * 24))}px` }}
      />
     );
    })}
   </div>

   <span className={cn('text-[11px] font-mono tabular-nums shrink-0', isMe ? 'text-white/70' : 'text-[var(--text-secondary)]')}>
    {formatDuration(displayDuration)}
   </span>
  </div>
 );
}
