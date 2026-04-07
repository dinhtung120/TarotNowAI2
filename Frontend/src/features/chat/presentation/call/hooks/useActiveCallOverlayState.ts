'use client';

import { useCallback, useRef, useState } from 'react';
import { useTranslations } from 'next-intl';
import { useCallStore } from '@/features/chat/application/call/useCallStore';
import { useCallContext } from '@/features/chat/presentation/call/CallProvider';
import { useCallDuration } from '@/features/chat/presentation/call/hooks/useCallDuration';

const RESET_DELAY_MS = 1000;

export function useActiveCallOverlayState() {
  const { uiState, session, localStream, remoteStream } = useCallStore();
  const { endCall } = useCallContext();
  const t = useTranslations('Chat.call');
  const localVideoRef = useRef<HTMLVideoElement>(null);
  const remoteVideoRef = useRef<HTMLVideoElement>(null);
  const remoteAudioRef = useRef<HTMLAudioElement>(null);
  const [isMinimized, setIsMinimized] = useState(false);

  const isOpen = uiState === 'connected';
  const isVideo = session?.type === 'video';
  const durationLabel = useCallDuration({ isOpen, sessionId: session?.id });

  const handleEndCall = useCallback(() => {
    if (session?.id) endCall(session.id, 'normal');
    useCallStore.getState().setEnded();
    window.setTimeout(() => useCallStore.getState().reset(), RESET_DELAY_MS);
  }, [endCall, session?.id]);

  return {
    t,
    session,
    localStream,
    remoteStream,
    localVideoRef,
    remoteVideoRef,
    remoteAudioRef,
    isOpen,
    isVideo,
    isMinimized,
    durationLabel,
    handleEndCall,
    minimize: () => setIsMinimized(true),
    maximize: () => setIsMinimized(false),
  };
}
