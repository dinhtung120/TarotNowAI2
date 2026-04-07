'use client';

import { CallOverlayContent } from '@/features/chat/presentation/call/CallOverlayContent';
import { useAttachCallVideoStreams } from '@/features/chat/presentation/call/hooks/useAttachCallVideoStreams';
import { useCallDuration } from '@/features/chat/presentation/call/hooks/useCallDuration';
import { useActiveCallOverlayState } from '@/features/chat/presentation/call/hooks/useActiveCallOverlayState';
import { useRemoteAudioPlayback } from '@/features/chat/presentation/call/hooks/useRemoteAudioPlayback';
import { cn } from '@/lib/utils';

export const ActiveCallOverlay = () => {
  const vm = useActiveCallOverlayState();
  const durationLabel = useCallDuration({ isOpen: vm.isOpen, sessionId: vm.session?.id });
  useAttachCallVideoStreams({
    localStream: vm.localStream,
    localVideoRef: vm.localVideoRef,
    refreshKey: vm.isMinimized,
    remoteStream: vm.remoteStream,
    remoteVideoRef: vm.remoteVideoRef,
  });
  useRemoteAudioPlayback({ remoteAudioRef: vm.remoteAudioRef, remoteStream: vm.remoteStream });

  if (!vm.isOpen) return null;

  return (
    <>
      <audio ref={vm.remoteAudioRef} autoPlay playsInline className={cn('sr-only')} />
      <CallOverlayContent
        activeTitle={vm.t('active_title')}
        durationLabel={durationLabel}
        endCallLabel={vm.t('end_call')}
        isMinimized={vm.isMinimized}
        isVideo={vm.isVideo}
        liveLabel={vm.t('live')}
        localVideoRef={vm.localVideoRef}
        maximizeLabel={vm.t('maximize')}
        minimizeLabel={vm.t('minimize')}
        remoteVideoRef={vm.remoteVideoRef}
        onEndCall={vm.handleEndCall}
        onMaximize={vm.maximize}
        onMinimize={vm.minimize}
      />
    </>
  );
};
