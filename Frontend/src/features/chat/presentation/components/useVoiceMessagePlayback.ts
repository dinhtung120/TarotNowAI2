'use client';

import { useCallback, useEffect, useRef, useState } from 'react';

interface UseVoiceMessagePlaybackOptions {
 audioUrl: string;
 durationMs?: number | null;
}

export function useVoiceMessagePlayback({ audioUrl, durationMs }: UseVoiceMessagePlaybackOptions) {
 const [playing, setPlaying] = useState(false);
 const [progress, setProgress] = useState(0);
 const [currentTimeMs, setCurrentTimeMs] = useState(0);
 const [resolvedDurationMs, setResolvedDurationMs] = useState(durationMs ?? 0);

 const audioRef = useRef<HTMLAudioElement | null>(null);
 const blobUrlRef = useRef<string | null>(null);
 const animationFrameRef = useRef<number | null>(null);

 const updateProgress = useCallback(function tick() {
  const audio = audioRef.current;
  if (!audio || audio.paused) return;
  if (Number.isFinite(audio.duration) && audio.duration > 0) {
   setProgress(audio.currentTime / audio.duration);
   setCurrentTimeMs(Math.round(audio.currentTime * 1000));
  }
  animationFrameRef.current = requestAnimationFrame(tick);
 }, []);

 const ensureBlobUrl = useCallback(() => {
  if (blobUrlRef.current) return blobUrlRef.current;
  if (!audioUrl.startsWith('data:')) return audioUrl;

  const commaIndex = audioUrl.indexOf(',');
  if (commaIndex < 0) return audioUrl;
  const header = audioUrl.slice(0, commaIndex);
  const base64Data = audioUrl.slice(commaIndex + 1);
  const mimeType = header.match(/data:([^;]+)/)?.[1] || 'audio/webm';

  const binaryString = atob(base64Data);
  const bytes = new Uint8Array(binaryString.length);
  for (let i = 0; i < binaryString.length; i++) bytes[i] = binaryString.charCodeAt(i);

  const blobUrl = URL.createObjectURL(new Blob([bytes], { type: mimeType }));
  blobUrlRef.current = blobUrl;
  return blobUrl;
 }, [audioUrl]);

 const initAudio = useCallback(async () => {
  const audio = new Audio();
  audioRef.current = audio;
  audio.onloadedmetadata = () => {
   if (Number.isFinite(audio.duration)) setResolvedDurationMs(Math.round(audio.duration * 1000));
  };
  audio.onended = () => {
   setPlaying(false);
   setProgress(0);
   setCurrentTimeMs(0);
   if (animationFrameRef.current) cancelAnimationFrame(animationFrameRef.current);
  };
  audio.onerror = () => {
   setPlaying(false);
   if (animationFrameRef.current) cancelAnimationFrame(animationFrameRef.current);
  };

  audio.src = ensureBlobUrl();
  audio.preload = 'auto';

  await new Promise<void>((resolve, reject) => {
   const timeoutId = window.setTimeout(resolve, 10000);
   audio.oncanplaythrough = () => {
    window.clearTimeout(timeoutId);
    resolve();
   };
   audio.onerror = () => {
    window.clearTimeout(timeoutId);
    reject(new Error('Cannot play audio'));
   };
  });
 }, [ensureBlobUrl]);

 const togglePlay = useCallback(async () => {
  if (!audioRef.current) {
   try {
    await initAudio();
   } catch {
    setPlaying(false);
    audioRef.current = null;
    return;
   }
  }

  const audio = audioRef.current;
  if (!audio) return;

  if (audio.paused) {
   try {
    await audio.play();
    setPlaying(true);
    animationFrameRef.current = requestAnimationFrame(updateProgress);
   } catch {
    setPlaying(false);
   }
   return;
  }

  audio.pause();
  setPlaying(false);
  if (animationFrameRef.current) cancelAnimationFrame(animationFrameRef.current);
 }, [initAudio, updateProgress]);

 useEffect(() => {
  return () => {
   if (audioRef.current) audioRef.current.pause();
   if (animationFrameRef.current) cancelAnimationFrame(animationFrameRef.current);
   if (blobUrlRef.current) URL.revokeObjectURL(blobUrlRef.current);
  };
 }, []);

 return {
  playing,
  progress,
  displayDuration: playing ? currentTimeMs : (resolvedDurationMs || 0),
  togglePlay,
 };
}
