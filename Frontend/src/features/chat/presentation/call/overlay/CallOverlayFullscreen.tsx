"use client";

import type { RefObject } from "react";
import { cn } from "@/lib/utils";

interface CallOverlayFullscreenProps {
 isVideo: boolean;
 activeTitle: string;
 durationLabel: string;
 minimizeLabel: string;
 endCallLabel: string;
 liveLabel: string;
 remoteVideoRef: RefObject<HTMLVideoElement | null>;
 localVideoRef: RefObject<HTMLVideoElement | null>;
 onMinimize: () => void;
 onEndCall: () => void;
}

export function CallOverlayFullscreen({ isVideo, activeTitle, durationLabel, minimizeLabel, endCallLabel, liveLabel, remoteVideoRef, localVideoRef, onMinimize, onEndCall }: CallOverlayFullscreenProps) {
 return (
  <div className={cn("tn-top-70 fixed inset-x-0 bottom-0 z-40 bg-black flex flex-col")}>
   <div className={cn("absolute top-0 left-0 right-0 z-20 flex justify-between items-start p-5 bg-gradient-to-b from-black/70 to-transparent pointer-events-none")}>
    <div className={cn("text-white drop-shadow-md pointer-events-auto")}>
     <h2 className={cn("text-lg font-medium opacity-80 mb-1")}>{activeTitle}</h2>
     <div className={cn("font-mono text-xl tracking-wider")}>{durationLabel}</div>
    </div>
    <button type="button" onClick={onMinimize} className={cn("pointer-events-auto h-10 w-10 cursor-pointer rounded-full bg-white/10 backdrop-blur-sm flex items-center justify-center mr-0 sm:mr-20 lg:mr-36")} title={minimizeLabel}>
     <svg xmlns="http://www.w3.org/2000/svg" className={cn("h-6 w-6 text-white")} fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 8V4m0 0h4M4 4l5 5m11-1V4m0 0h-4m4 0l-5 5M4 16v4m0 0h4m-4 0l5-5m11 5l-5-5m5 5v-4m0 4h-4" />
     </svg>
    </button>
   </div>
   <div className={cn("flex-1 relative flex items-center justify-center overflow-hidden")}>
    {isVideo ? <video ref={remoteVideoRef} autoPlay playsInline muted className={cn("w-full h-full object-cover")} /> : <div className={cn("flex flex-col items-center")}><div className={cn("w-32 h-32 rounded-full bg-indigo-500/30 flex items-center justify-center animate-pulse shadow-xl mb-6")}><span className={cn("text-white text-5xl")}>🔮</span></div></div>}
   </div>
   {isVideo ? <div className={cn("absolute right-3 top-3 z-50 h-32 w-24 overflow-hidden rounded-xl border border-white/20 bg-gray-800 shadow-2xl sm:h-40 sm:w-28 md:h-44 md:w-32")}><video ref={localVideoRef} autoPlay playsInline muted className={cn("w-full h-full object-cover shadow-inner")} /></div> : null}
   <div className={cn("absolute bottom-6 left-0 right-0 flex items-center justify-center z-50")}>
    <button type="button" onClick={onEndCall} className={cn("w-16 h-16 rounded-full bg-red-600 flex items-center justify-center shadow-xl cursor-pointer")} title={endCallLabel}>
     <svg className={cn("w-8 h-8 text-white")} fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
     </svg>
    </button>
   </div>
   <div className={cn("absolute right-4 top-4 rounded-full border px-3 py-1 tn-bg-accent-10 tn-border-accent-20 sm:right-5 sm:top-5")}>
    <span className={cn("tn-text-10 tn-text-accent font-bold uppercase tracking-widest")}>{liveLabel}</span>
   </div>
  </div>
 );
}
