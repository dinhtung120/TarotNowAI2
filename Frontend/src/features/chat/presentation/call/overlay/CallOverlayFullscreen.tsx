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
    <button type="button" onClick={onMinimize} className={cn("tn-mr-140 pointer-events-auto w-10 h-10 bg-white/10 rounded-full flex items-center justify-center backdrop-blur-sm cursor-pointer")} title={minimizeLabel}>
     <svg xmlns="http://www.w3.org/2000/svg" className={cn("h-6 w-6 text-white")} fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 8V4m0 0h4M4 4l5 5m11-1V4m0 0h-4m4 0l-5 5M4 16v4m0 0h4m-4 0l5-5m11 5l-5-5m5 5v-4m0 4h-4" />
     </svg>
    </button>
   </div>
   <div className={cn("flex-1 relative flex items-center justify-center overflow-hidden")}>
    {isVideo ? <video ref={remoteVideoRef} autoPlay playsInline muted className={cn("w-full h-full object-cover")} /> : <div className={cn("flex flex-col items-center")}><div className={cn("w-32 h-32 rounded-full bg-indigo-500/30 flex items-center justify-center animate-pulse shadow-xl mb-6")}><span className={cn("text-white text-5xl")}>🔮</span></div></div>}
   </div>
   {isVideo ? <div className={cn("absolute top-2.5 right-2.5 w-32 h-44 bg-gray-800 rounded-xl overflow-hidden shadow-2xl border border-white/20 z-50")}><video ref={localVideoRef} autoPlay playsInline muted className={cn("w-full h-full object-cover shadow-inner")} /></div> : null}
   <div className={cn("absolute bottom-6 left-0 right-0 flex items-center justify-center z-50")}>
    <button type="button" onClick={onEndCall} className={cn("w-16 h-16 rounded-full bg-red-600 flex items-center justify-center shadow-xl cursor-pointer")} title={endCallLabel}>
     <svg className={cn("w-8 h-8 text-white")} fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
     </svg>
    </button>
   </div>
   <div className={cn("absolute top-5 right-5 px-3 py-1 tn-bg-accent-10 border tn-border-accent-20 rounded-full")}>
    <span className={cn("tn-text-10 tn-text-accent font-bold uppercase tracking-widest")}>{liveLabel}</span>
   </div>
  </div>
 );
}
