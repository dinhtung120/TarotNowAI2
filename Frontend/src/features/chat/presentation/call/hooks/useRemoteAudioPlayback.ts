import { useCallback, useEffect, useRef, type MutableRefObject } from 'react';

interface UseRemoteAudioPlaybackArgs {
  remoteAudioRef: MutableRefObject<HTMLAudioElement | null>;
  remoteStream: MediaStream | null;
}

export function useRemoteAudioPlayback({ remoteAudioRef, remoteStream }: UseRemoteAudioPlaybackArgs) {
  const audioContextRef = useRef<AudioContext | null>(null);
  const audioSourceRef = useRef<MediaStreamAudioSourceNode | null>(null);
  const audioGainRef = useRef<GainNode | null>(null);

  const resolveRemoteGain = useCallback(() => {
    const parsed = Number(process.env.NEXT_PUBLIC_CALL_REMOTE_GAIN || 2.2);
    return Number.isFinite(parsed) ? Math.min(4, Math.max(1, parsed)) : 2.2;
  }, []);

  const cleanup = useCallback(() => {
    audioSourceRef.current?.disconnect();
    audioGainRef.current?.disconnect();
    audioSourceRef.current = null;
    audioGainRef.current = null;
    if (audioContextRef.current) void audioContextRef.current.close().catch(() => undefined);
    audioContextRef.current = null;
  }, []);

  useEffect(() => {
    const remoteAudio = remoteAudioRef.current;
    if (!remoteAudio) return;

    cleanup();
    if (!remoteStream) return void (remoteAudio.srcObject = null);

    const audioTracks = remoteStream.getAudioTracks();
    const audioOnlyStream = audioTracks.length > 0 ? new MediaStream(audioTracks) : remoteStream;
    const AudioContextCtor = window.AudioContext || ((window as Window & { webkitAudioContext?: typeof AudioContext }).webkitAudioContext ?? null);
    if (!AudioContextCtor) {
      remoteAudio.srcObject = audioOnlyStream;
      remoteAudio.volume = 1;
      void remoteAudio.play().catch(() => undefined);
      return;
    }

    try {
      const context = new AudioContextCtor();
      const source = context.createMediaStreamSource(audioOnlyStream);
      const gainNode = context.createGain();
      const compressor = context.createDynamicsCompressor();
      const destination = context.createMediaStreamDestination();

      gainNode.gain.value = resolveRemoteGain();
      compressor.threshold.value = -18;
      compressor.knee.value = 18;
      compressor.ratio.value = 3;
      compressor.attack.value = 0.003;
      compressor.release.value = 0.25;

      source.connect(gainNode);
      gainNode.connect(compressor);
      compressor.connect(destination);

      audioContextRef.current = context;
      audioSourceRef.current = source;
      audioGainRef.current = gainNode;

      remoteAudio.srcObject = destination.stream;
      remoteAudio.muted = false;
      remoteAudio.volume = 1;

      const mediaWithSinkId = remoteAudio as HTMLAudioElement & { setSinkId?: (sinkId: string) => Promise<void> };
      if (typeof mediaWithSinkId.setSinkId === 'function') void mediaWithSinkId.setSinkId('default').catch(() => undefined);
      if (context.state === 'suspended') void context.resume().catch(() => undefined);
      void remoteAudio.play().catch(() => undefined);
    } catch {
      remoteAudio.srcObject = audioOnlyStream;
      remoteAudio.volume = 1;
      void remoteAudio.play().catch(() => undefined);
    }

    return () => cleanup();
  }, [cleanup, remoteAudioRef, remoteStream, resolveRemoteGain]);

  useEffect(() => () => cleanup(), [cleanup]);
}
