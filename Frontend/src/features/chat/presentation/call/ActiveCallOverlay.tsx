'use client';

import { CallOverlayContent } from '@/features/chat/presentation/call/CallOverlayContent';
import { useAttachCallVideoStreams } from '@/features/chat/presentation/call/hooks/useAttachCallVideoStreams';
import { useCallDuration } from '@/features/chat/presentation/call/hooks/useCallDuration';
import { useActiveCallOverlayState } from '@/features/chat/presentation/call/hooks/useActiveCallOverlayState';
import { useRemoteAudioPlayback } from '@/features/chat/presentation/call/hooks/useRemoteAudioPlayback';
import { cn } from '@/lib/utils';

export const ActiveCallOverlay = () => {
  const {
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
  } = useActiveCallOverlayState();

  const durationLabel = useCallDuration({ isOpen, sessionId: session?.id });

  useAttachCallVideoStreams({
    localStream,
    localVideoRef,
    refreshKey: isMinimized,
    remoteStream,
    remoteVideoRef,
  });

  useRemoteAudioPlayback({ remoteAudioRef, remoteStream });

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
        onMaximize={maximize}
        onMinimize={minimize}
      />
    </>
  );
};
