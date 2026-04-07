'use client';

import { useCallback, useEffect, useRef } from 'react';
import { useTranslations } from 'next-intl';
import { useCallStore } from '@/features/chat/application/call/useCallStore';
import { useCallContext } from '@/features/chat/presentation/call/CallProvider';

const IncomingState = 'incoming';
const RingingState = 'ringing';

export function useIncomingCallOverlayState() {
  const { uiState, session, isCaller, localStream, setEnded: setEndedLocalState } = useCallStore();
  const { respondCall, endCall } = useCallContext();
  const t = useTranslations('Chat.call');
  const localVideoRef = useRef<HTMLVideoElement>(null);
  const isOpen = uiState === IncomingState || uiState === RingingState;

  useEffect(() => {
    if (!localVideoRef.current || !localStream) return;
    if (localVideoRef.current.srcObject === localStream) return;
    localVideoRef.current.srcObject = localStream;
  }, [localStream, isOpen]);

  const declineCall = useCallback(() => {
    if (!session?.id) return;
    void respondCall(session.id, false);
  }, [respondCall, session?.id]);

  const acceptCall = useCallback(() => {
    if (!session?.id) return;
    void respondCall(session.id, true);
  }, [respondCall, session?.id]);

  const cancelCall = useCallback(() => {
    if (session?.id)
    {
      void endCall(session.id, 'cancelled');
      return;
    }

    setEndedLocalState();
  }, [endCall, session?.id, setEndedLocalState]);

  return {
    t,
    session,
    isCaller,
    localStream,
    localVideoRef,
    isOpen,
    declineCall,
    acceptCall,
    cancelCall,
  };
}
