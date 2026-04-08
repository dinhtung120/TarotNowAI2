'use client';

import { useRef, useState } from 'react';
import { useTranslations } from 'next-intl';
import { useCallStore } from '@/features/chat/application/call/useCallStore';
import { useCallContext } from '@/features/chat/presentation/call/CallProvider';
import { logger } from '@/shared/infrastructure/logging/logger';

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

  const handleEndCall = () => {
    useCallStore.getState().setEnding();
    if (session?.id) {
      void endCall(session.id, 'normal').catch((error) => {
        logger.warn('Call.UI', 'Unable to end call.', { error });
      });
    }
    useCallStore.getState().setEnded();
    window.setTimeout(() => useCallStore.getState().reset(), RESET_DELAY_MS);
  };

  const minimize = () => setIsMinimized(true);
  const maximize = () => setIsMinimized(false);

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
    handleEndCall,
    minimize,
    maximize,
  };
}
