import type { MutableRefObject } from 'react';
import { CallOverlayFullscreen } from '@/features/chat/presentation/call/overlay/CallOverlayFullscreen';
import { CallOverlayMinimized } from '@/features/chat/presentation/call/overlay/CallOverlayMinimized';

interface CallOverlayContentProps {
  activeTitle: string;
  durationLabel: string;
  endCallLabel: string;
  isMinimized: boolean;
  isVideo: boolean;
  liveLabel: string;
  localVideoRef: MutableRefObject<HTMLVideoElement | null>;
  maximizeLabel: string;
  minimizeLabel: string;
  remoteVideoRef: MutableRefObject<HTMLVideoElement | null>;
  onEndCall: () => void;
  onMaximize: () => void;
  onMinimize: () => void;
}

export function CallOverlayContent({ activeTitle, durationLabel, endCallLabel, isMinimized, isVideo, liveLabel, localVideoRef, maximizeLabel, minimizeLabel, remoteVideoRef, onEndCall, onMaximize, onMinimize }: CallOverlayContentProps) {
  if (isMinimized) {
    return (
      <CallOverlayMinimized
        isVideo={isVideo}
        durationLabel={durationLabel}
        maximizeLabel={maximizeLabel}
        endCallLabel={endCallLabel}
        remoteVideoRef={remoteVideoRef}
        onMaximize={onMaximize}
        onEndCall={onEndCall}
      />
    );
  }

  return (
    <CallOverlayFullscreen
      isVideo={isVideo}
      activeTitle={activeTitle}
      durationLabel={durationLabel}
      minimizeLabel={minimizeLabel}
      endCallLabel={endCallLabel}
      liveLabel={liveLabel}
      remoteVideoRef={remoteVideoRef}
      localVideoRef={localVideoRef}
      onMinimize={onMinimize}
      onEndCall={onEndCall}
    />
  );
}
