'use client';

import { useRef, useState } from 'react';
import { useTranslations } from 'next-intl';
import { useCallStore } from '../../application/call/useCallStore';
import { useCallContext } from './CallProvider';
import { CallOverlayContent } from '@/features/chat/presentation/call/CallOverlayContent';
import { useAttachCallVideoStreams } from '@/features/chat/presentation/call/hooks/useAttachCallVideoStreams';
import { useCallDuration } from '@/features/chat/presentation/call/hooks/useCallDuration';
import { useRemoteAudioPlayback } from '@/features/chat/presentation/call/hooks/useRemoteAudioPlayback';
import { cn } from '@/lib/utils';

export const ActiveCallOverlay = () => {
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

  useAttachCallVideoStreams({ localStream, localVideoRef, refreshKey: isMinimized, remoteStream, remoteVideoRef });
  useRemoteAudioPlayback({ remoteAudioRef, remoteStream });

  const handleEndCall = () => {
    if (session?.id) endCall(session.id, 'normal');
    useCallStore.getState().setEnded();
    window.setTimeout(() => useCallStore.getState().reset(), 1000);
  };

  if (!isOpen) return null;

  return (
    <>
      <audio ref={remoteAudioRef} autoPlay playsInline className={cn('sr-only')} />
      <CallOverlayContent
        activeTitle={t('active_title')}
        durationLabel={durationLabel}
        endCallLabel={t('end_call')}
        isMinimized={isMinimized}
        isVideo={isVideo}
        liveLabel={t('live')}
        localVideoRef={localVideoRef}
        maximizeLabel={t('maximize')}
        minimizeLabel={t('minimize')}
        remoteVideoRef={remoteVideoRef}
        onEndCall={handleEndCall}
        onMaximize={() => setIsMinimized(false)}
        onMinimize={() => setIsMinimized(true)}
      />
    </>
  );
};
