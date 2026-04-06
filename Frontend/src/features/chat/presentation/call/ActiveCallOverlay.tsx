'use client';

import React, { useCallback, useEffect, useRef, useState } from 'react';
import { useTranslations } from 'next-intl';
import { useCallStore } from '../../application/call/useCallStore';
import { useCallContext } from './CallProvider';

export const ActiveCallOverlay = () => {
  const { uiState, session, localStream, remoteStream } = useCallStore();
  const { endCall } = useCallContext();
  const t = useTranslations('Chat.call');
  
  const localVideoRef = useRef<HTMLVideoElement>(null);
  const remoteVideoRef = useRef<HTMLVideoElement>(null);
  const remoteAudioRef = useRef<HTMLAudioElement>(null);
  const remoteAudioContextRef = useRef<AudioContext | null>(null);
  const remoteAudioSourceRef = useRef<MediaStreamAudioSourceNode | null>(null);
  const remoteAudioGainRef = useRef<GainNode | null>(null);
  const remoteAudioDestinationRef = useRef<MediaStreamAudioDestinationNode | null>(null);

  const [duration, setDuration] = useState(0);
  const [isMinimized, setIsMinimized] = useState(false);

  const isOpen = uiState === 'connected';

  const resolveRemoteGain = useCallback(() => {
    const raw = process.env.NEXT_PUBLIC_CALL_REMOTE_GAIN;
    const parsed = raw ? Number(raw) : 2.2;
    if (!Number.isFinite(parsed)) {
      return 2.2;
    }

    return Math.min(4, Math.max(1, parsed));
  }, []);

  const cleanupRemoteAudioPipeline = useCallback(() => {
    remoteAudioSourceRef.current?.disconnect();
    remoteAudioGainRef.current?.disconnect();

    remoteAudioSourceRef.current = null;
    remoteAudioGainRef.current = null;
    remoteAudioDestinationRef.current = null;

    if (remoteAudioContextRef.current) {
      remoteAudioContextRef.current.close().catch(() => undefined);
      remoteAudioContextRef.current = null;
    }
  }, []);

  // Attach streams to <video> tags
  useEffect(() => {
    if (localVideoRef.current && localStream) {
      if (localVideoRef.current.srcObject !== localStream) {
        localVideoRef.current.srcObject = localStream;
      }
    }
  }, [localStream, isMinimized]); // Ref video có thể mount lại khi toggle minimize.

  useEffect(() => {
    if (remoteVideoRef.current && remoteStream) {
      // Luôn mute thẻ video để tránh phát audio kép (video + audio element).
      remoteVideoRef.current.muted = true;
      if (remoteVideoRef.current.srcObject !== remoteStream) {
        remoteVideoRef.current.srcObject = remoteStream;
      }
    }
  }, [remoteStream, isMinimized]);

  /* Cài đặt audio stream: LUÔN TRỰC TIẾP
   * Không phụ thuộc vào isMinimized vì thẻ audio được gắn cố định tại root của component.
   */
  useEffect(() => {
    const remoteAudio = remoteAudioRef.current;
    if (!remoteAudio) {
      return;
    }

    cleanupRemoteAudioPipeline();

    if (!remoteStream) {
      remoteAudio.srcObject = null;
      return;
    }

    const audioTracks = remoteStream.getAudioTracks();
    if (audioTracks.length === 0) {
      remoteAudio.srcObject = remoteStream;
      return;
    }

    const audioOnlyStream = new MediaStream(audioTracks);
    const AudioContextCtor = window.AudioContext
      || ((window as Window & { webkitAudioContext?: typeof AudioContext }).webkitAudioContext ?? null);
    if (!AudioContextCtor) {
      remoteAudio.srcObject = audioOnlyStream;
      remoteAudio.volume = 1;
      remoteAudio.play().catch(() => undefined);
      return;
    }

    try {
      const context = new AudioContextCtor();
      const source = context.createMediaStreamSource(audioOnlyStream);
      const gainNode = context.createGain();
      const compressor = context.createDynamicsCompressor();
      const destination = context.createMediaStreamDestination();

      gainNode.gain.value = resolveRemoteGain();
      compressor.threshold.value = -18;
      compressor.knee.value = 18;
      compressor.ratio.value = 3;
      compressor.attack.value = 0.003;
      compressor.release.value = 0.25;

      source.connect(gainNode);
      gainNode.connect(compressor);
      compressor.connect(destination);

      remoteAudioContextRef.current = context;
      remoteAudioSourceRef.current = source;
      remoteAudioGainRef.current = gainNode;
      remoteAudioDestinationRef.current = destination;

      remoteAudio.srcObject = destination.stream;
      remoteAudio.muted = false;
      remoteAudio.volume = 1;

      const mediaWithSinkId = remoteAudio as HTMLAudioElement & {
        setSinkId?: (sinkId: string) => Promise<void>;
      };

      if (typeof mediaWithSinkId.setSinkId === 'function') {
        mediaWithSinkId.setSinkId('default').catch(() => undefined);
      }

      if (context.state === 'suspended') {
        context.resume().catch(() => undefined);
      }

      remoteAudio.play().catch(() => undefined);
    } catch {
      remoteAudio.srcObject = audioOnlyStream;
      remoteAudio.volume = 1;
      remoteAudio.play().catch(() => undefined);
    }

    return () => {
      cleanupRemoteAudioPipeline();
    };
  }, [cleanupRemoteAudioPipeline, remoteStream, resolveRemoteGain]);

  useEffect(() => {
    return () => {
      cleanupRemoteAudioPipeline();
    };
  }, [cleanupRemoteAudioPipeline]);

  // Bộ đếm thời gian
  useEffect(() => {
    if (!isOpen) {
      const resetTimer = window.setTimeout(() => {
        setDuration(0);
      }, 0);
      return () => window.clearTimeout(resetTimer);
    }

    let elapsedSeconds = 0;
    const resetTimer = window.setTimeout(() => {
      setDuration(0);
    }, 0);
    const timer = window.setInterval(() => {
      elapsedSeconds += 1;
      setDuration(elapsedSeconds);
    }, 1000);
    return () => {
      window.clearTimeout(resetTimer);
      window.clearInterval(timer);
    };
  }, [isOpen, session?.id]);

  const handleEndCall = () => {
    if (session?.id) {
      endCall(session.id, 'normal');
    }
    // Gỡ lỗi 1: Fallback để xoá UI ngay lập tức bất kể lệnh qua mạng kịp chạy hay không
    useCallStore.getState().setEnded();
    setTimeout(() => useCallStore.getState().reset(), 1000);
  };

  const formatDuration = (seconds: number) => {
    const m = Math.floor(seconds / 60).toString().padStart(2, '0');
    const s = (seconds % 60).toString().padStart(2, '0');
    return `${m}:${s}`;
  };

  if (!isOpen) return null;

  const isVideo = session?.type === 'video';

  return (
    <>
      <audio ref={remoteAudioRef} autoPlay playsInline className="sr-only" />

      {isMinimized ? (
        <div className="fixed bottom-24 right-4 z-50 w-48 h-64 bg-gray-900 rounded-xl overflow-hidden shadow-2xl border border-gray-700 flex flex-col group transition-all duration-300 hover:shadow-[0_0_20px_rgba(0,0,0,0.5)]">
          <div className="absolute top-2 right-2 z-10 flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
            <button 
              onClick={() => setIsMinimized(false)}
              className="p-1.5 bg-black/50 hover:bg-black/80 rounded-md backdrop-blur-md text-white transition-colors"
              title={t('maximize')}
            >
              <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                <path fillRule="evenodd" d="M3 3a1 1 0 011-1h4a1 1 0 110 2H5.414l3.293 3.293a1 1 0 11-1.414 1.414L4 5.414V8a1 1 0 11-2 0V4a1 1 0 011-1zm14 0a1 1 0 00-1-1h-4a1 1 0 100 2h2.586l-3.293 3.293a1 1 0 101.414 1.414L20 14.586V12a1 1 0 102 0v-4a1 1 0 00-1-1zm-1 14a1 1 0 01-1 1h-4a1 1 0 110-2h2.586l-3.293-3.293a1 1 0 111.414-1.414L20 14.586V12a1 1 0 112 0v4a1 1 0 01-1 1zm-14 0a1 1 0 001 1h4a1 1 0 100-2H5.414l3.293-3.293a1 1 0 10-1.414-1.414L4 14.586V12a1 1 0 10-2 0v4a1 1 0 001 1z" clipRule="evenodd" />
              </svg>
            </button>
            <button 
              onClick={handleEndCall}
              className="p-1.5 bg-red-600/80 hover:bg-red-600 rounded-md backdrop-blur-md text-white transition-colors"
              title={t('end_call')}
            >
               <svg className="w-5 h-5 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>

          <div className="flex-1 relative bg-black flex items-center justify-center">
            {isVideo ? (
               <video ref={remoteVideoRef} autoPlay playsInline muted className="w-full h-full object-cover" />
            ) : (
                <div className="w-16 h-16 rounded-full bg-indigo-500/30 flex items-center justify-center animate-pulse">
                    <span className="text-white text-2xl">🔮</span>
                </div>
            )}
             <div className="absolute bottom-2 left-2 px-2 py-0.5 bg-black/60 rounded text-xs font-mono text-white/90">
                {formatDuration(duration)}
             </div>
          </div>
        </div>
      ) : (
        /* GỠ LỖI 3: Để inset-x-0 top-[70px] bottom-0 thay vì inset-0 để không đè lên Navbar của phần mềm */
        <div className="fixed inset-x-0 top-[70px] bottom-0 z-40 bg-black flex flex-col">
          <div className="absolute top-0 left-0 right-0 z-20 flex justify-between items-start p-5 bg-gradient-to-b from-black/70 to-transparent pointer-events-none">
             <div className="text-white drop-shadow-md pointer-events-auto">
                <h2 className="text-lg font-medium opacity-80 mb-1">{t('active_title')}</h2>
                <div className="font-mono text-xl tracking-wider">{formatDuration(duration)}</div>
             </div>
             
             <button 
                onClick={() => setIsMinimized(true)}
                className="pointer-events-auto w-10 h-10 bg-white/10 hover:bg-white/20 rounded-full flex items-center justify-center backdrop-blur-sm transition-colors cursor-pointer mr-[140px]"
                title={t('minimize')}
             >
                <svg xmlns="http://www.w3.org/2000/svg" className="h-6 w-6 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 8V4m0 0h4M4 4l5 5m11-1V4m0 0h-4m4 0l-5 5M4 16v4m0 0h4m-4 0l5-5m11 5l-5-5m5 5v-4m0 4h-4" />
                </svg>
             </button>
          </div>

          <div className="flex-1 relative flex items-center justify-center overflow-hidden">
            {isVideo ? (
              <video 
                ref={remoteVideoRef} 
                autoPlay 
                playsInline 
                muted
                className="w-full h-full object-cover" 
              />
            ) : (
              <div className="flex flex-col items-center">
                 <div className="w-32 h-32 rounded-full bg-indigo-500/30 flex items-center justify-center animate-pulse shadow-[0_0_40px_rgba(99,102,241,0.5)] mb-6">
                    <span className="text-white text-5xl">🔮</span>
                </div>
              </div>
            )}
          </div>

          {/* FIX LỖI: Đưa trực tiếp ra Root của thẻ fixed để tránh việc flex layout kẹp nó vào giữa màn hình */}
          {isVideo && (
            <div className="absolute top-2.5 right-2.5 w-32 h-44 bg-gray-800 rounded-xl overflow-hidden shadow-[0_0_30px_rgba(0,0,0,0.8)] border border-white/20 z-50">
              <video 
                ref={localVideoRef} 
                autoPlay 
                playsInline 
                muted 
                className="w-full h-full object-cover shadow-inner"
              />
            </div>
          )}

          {/* CẢI THIỆN LỖI 1: Tách riêng nút End Call ra khỏi Absolute để chống chặn Pointer Event */}
          <div className="absolute bottom-6 left-0 right-0 flex items-center justify-center z-50">
            <button 
              onClick={handleEndCall}
              className="w-16 h-16 rounded-full bg-red-600 flex items-center justify-center shadow-[0_0_30px_rgba(220,38,38,0.6)] hover:bg-red-500 transition-all hover:scale-105 active:scale-95 cursor-pointer"
              title={t('end_call')}
            >
              <svg className="w-8 h-8 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
        </div>
      )}
    </>
  );
};
