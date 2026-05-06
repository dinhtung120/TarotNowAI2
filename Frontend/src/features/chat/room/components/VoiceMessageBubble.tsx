'use client';

import { useMemo } from 'react';
import { Pause, Play } from 'lucide-react';
import { cn } from '@/lib/utils';
import { useVoiceMessagePlayback } from '@/features/chat/room/components/useVoiceMessagePlayback';
import {
 formatDuration,
 generateStaticBars,
 STATIC_BAR_COUNT,
} from '@/features/chat/room/components/voiceMessageUtils';

interface VoiceMessageBubbleProps {
 audioUrl: string;
 durationMs?: number | null;
 isMe: boolean;
}

function resolveWaveHeightClass(level: number): string {
 const height = Math.max(3, Math.round(level * 24));
 if (height >= 25) return 'tn-wave-h-25';
 if (height >= 23) return 'tn-wave-h-23';
 if (height >= 21) return 'tn-wave-h-21';
 if (height >= 19) return 'tn-wave-h-19';
 if (height >= 17) return 'tn-wave-h-17';
 if (height >= 15) return 'tn-wave-h-15';
 if (height >= 13) return 'tn-wave-h-13';
 if (height >= 11) return 'tn-wave-h-11';
 if (height >= 9) return 'tn-wave-h-9';
 if (height >= 7) return 'tn-wave-h-7';
 if (height >= 5) return 'tn-wave-h-5';
 return 'tn-wave-h-3';
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
       className={cn(
        'flex-1 rounded-full transition-opacity duration-150',
        resolveWaveHeightClass(level),
        isPlayed ? barColor : barDimColor,
       )}
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
