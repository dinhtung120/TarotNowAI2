'use client';

import type { RefObject } from 'react';
import { cn } from '@/lib/utils';

interface CallOverlayMinimizedProps {
 isVideo: boolean;
 durationLabel: string;
 maximizeLabel: string;
 endCallLabel: string;
 remoteVideoRef: RefObject<HTMLVideoElement | null>;
 onMaximize: () => void;
 onEndCall: () => void;
}

export function CallOverlayMinimized({
 isVideo,
 durationLabel,
 maximizeLabel,
 endCallLabel,
 remoteVideoRef,
 onMaximize,
 onEndCall,
}: CallOverlayMinimizedProps) {
 return (
  <div className={cn('fixed bottom-24 right-4 z-50 w-48 h-64 bg-gray-900 rounded-xl overflow-hidden shadow-2xl border border-gray-700 flex flex-col group transition-all duration-300 hover:shadow-[0_0_20px_rgba(0,0,0,0.5)]')}>
   <div className={cn('absolute top-2 right-2 z-10 flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity')}>
    <button
     type="button"
     onClick={onMaximize}
     className={cn('p-1.5 bg-black/50 hover:bg-black/80 rounded-md backdrop-blur-md text-white transition-colors')}
     title={maximizeLabel}
    >
     <svg xmlns="http://www.w3.org/2000/svg" className={cn('h-5 w-5')} viewBox="0 0 20 20" fill="currentColor">
      <path fillRule="evenodd" d="M3 3a1 1 0 011-1h4a1 1 0 110 2H5.414l3.293 3.293a1 1 0 11-1.414 1.414L4 5.414V8a1 1 0 11-2 0V4a1 1 0 011-1zm14 0a1 1 0 00-1-1h-4a1 1 0 100 2h2.586l-3.293 3.293a1 1 0 101.414 1.414L20 14.586V12a1 1 0 102 0v-4a1 1 0 00-1-1zm-1 14a1 1 0 01-1 1h-4a1 1 0 110-2h2.586l-3.293-3.293a1 1 0 111.414-1.414L20 14.586V12a1 1 0 112 0v4a1 1 0 01-1 1zm-14 0a1 1 0 001 1h4a1 1 0 100-2H5.414l3.293-3.293a1 1 0 10-1.414-1.414L4 14.586V12a1 1 0 10-2 0v4a1 1 0 001 1z" clipRule="evenodd" />
     </svg>
    </button>
    <button
     type="button"
     onClick={onEndCall}
     className={cn('p-1.5 bg-red-600/80 hover:bg-red-600 rounded-md backdrop-blur-md text-white transition-colors')}
     title={endCallLabel}
    >
     <svg className={cn('w-5 h-5 text-white')} fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
     </svg>
    </button>
   </div>
   <div className={cn('flex-1 relative bg-black flex items-center justify-center')}>
    {isVideo ? (
     <video ref={remoteVideoRef} autoPlay playsInline muted className={cn('w-full h-full object-cover')} />
    ) : (
     <div className={cn('w-16 h-16 rounded-full bg-indigo-500/30 flex items-center justify-center animate-pulse')}>
      <span className={cn('text-white text-2xl')}>🔮</span>
     </div>
    )}
    <div className={cn('absolute bottom-2 left-2 px-2 py-0.5 bg-black/60 rounded text-xs font-mono text-white/90')}>
     {durationLabel}
    </div>
   </div>
  </div>
 );
}
