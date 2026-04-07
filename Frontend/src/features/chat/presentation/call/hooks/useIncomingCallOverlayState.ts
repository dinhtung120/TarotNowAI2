'use client';

import { useEffect, useRef } from 'react';
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

  const declineCall = () => {
    if (!session?.id) return;
    void respondCall(session.id, false);
  };

  const acceptCall = () => {
    if (!session?.id) return;
    void respondCall(session.id, true);
  };

  const cancelCall = () => {
    if (session?.id) {
      void endCall(session.id, 'cancelled');
      return;
    }

    setEndedLocalState();
  };

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
