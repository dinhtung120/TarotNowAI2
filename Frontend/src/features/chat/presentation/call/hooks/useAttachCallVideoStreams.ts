import { useEffect, type MutableRefObject } from 'react';

interface UseAttachCallVideoStreamsArgs {
  localStream: MediaStream | null;
  localVideoRef: MutableRefObject<HTMLVideoElement | null>;
  refreshKey: boolean;
  remoteStream: MediaStream | null;
  remoteVideoRef: MutableRefObject<HTMLVideoElement | null>;
}

export function useAttachCallVideoStreams({ localStream, localVideoRef, refreshKey, remoteStream, remoteVideoRef }: UseAttachCallVideoStreamsArgs) {
  useEffect(() => {
    if (!localVideoRef.current || !localStream) return;
    if (localVideoRef.current.srcObject !== localStream) localVideoRef.current.srcObject = localStream;
  }, [localStream, localVideoRef, refreshKey]);

  useEffect(() => {
    if (!remoteVideoRef.current || !remoteStream) return;
    if (remoteVideoRef.current.srcObject === remoteStream) return;
    remoteVideoRef.current.muted = true;
    remoteVideoRef.current.srcObject = remoteStream;
  }, [remoteStream, remoteVideoRef, refreshKey]);
}
