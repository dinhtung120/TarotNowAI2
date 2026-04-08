'use client';

import { useEffect, useRef } from 'react';
import { useTranslations } from 'next-intl';
import { useCallStore } from '@/features/chat/application/call/useCallStore';
import { useCallContext } from '@/features/chat/presentation/call/CallProvider';
import { logger } from '@/shared/infrastructure/logging/logger';

const IncomingState = 'incoming';
const RequestedState = 'requested';
const AcceptedState = 'accepted';
const JoiningState = 'joining';

export function useIncomingCallOverlayState() {
  const { uiState, session, isCaller, localStream, setEnded: setEndedLocalState } = useCallStore();
  const { respondCall, endCall } = useCallContext();
  const t = useTranslations('Chat.call');
  const localVideoRef = useRef<HTMLVideoElement>(null);
  const isOpen =
    uiState === IncomingState ||
    uiState === RequestedState ||
    uiState === AcceptedState ||
    uiState === JoiningState;

  useEffect(() => {
    if (!localVideoRef.current || !localStream) return;
    if (localVideoRef.current.srcObject === localStream) return;
    localVideoRef.current.srcObject = localStream;
  }, [localStream, isOpen]);

  const declineCall = () => {
    if (!session?.id) return;
    void respondCall(session.id, false).catch((error) => {
      logger.warn('Call.UI', 'Unable to decline call.', { error });
      useCallStore.getState().setEnded();
    });
  };

  const acceptCall = () => {
    if (!session?.id) return;
    void respondCall(session.id, true).catch((error) => {
      logger.warn('Call.UI', 'Unable to accept call.', { error });
      useCallStore.getState().setEnded();
    });
  };

  const cancelCall = () => {
    if (session?.id) {
      void endCall(session.id, 'cancelled').catch((error) => {
        logger.warn('Call.UI', 'Unable to cancel call.', { error });
      });
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
