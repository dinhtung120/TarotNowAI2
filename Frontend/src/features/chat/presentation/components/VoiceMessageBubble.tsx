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
 const barColor = isMe ? 'bg-white/80' : 'tn-bg-accent';
 const barDimColor = isMe ? 'bg-white/30' : 'tn-bg-accent-20';
 const playBtnClass = isMe
  ? 'bg-white/20 text-white tn-voice-play-me-hover'
  : 'tn-bg-accent-20 tn-text-accent tn-voice-play-other-hover';

 return (
  <div className={cn('flex items-center gap-2.5 tn-minw-180 tn-maxw-280')}>
   <button
    type="button"
    onClick={() => void togglePlay()}
    className={cn('w-9 h-9 rounded-full flex items-center justify-center shrink-0 transition-colors', playBtnClass)}
    title={playing ? 'Tạm dừng' : 'Phát'}
   >
    {playing ? <Pause className={cn('w-4 h-4')} /> : <Play className={cn('w-4 h-4 ml-0.5')} />}
   </button>

   <div className={cn('tn-gap-0_5 flex-1 flex items-center h-7 min-w-0')}>
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

   <span className={cn('tn-text-11 font-mono tabular-nums shrink-0', isMe ? 'text-white/70' : 'tn-text-secondary')}>
    {formatDuration(displayDuration)}
   </span>
  </div>
 );
}
