'use client';
import { useCallback, useEffect, useRef, useState } from 'react';
import {
 ANALYSER_INTERVAL_MS,
 MAX_DURATION_SECONDS,
 WAVEFORM_BAR_COUNT,
 buildWaveformBars,
 mapVoiceRecorderError,
 resolveRecorderMimeType,
 stopMediaStream,
} from '@/features/chat/application/voiceRecorderHelpers';
export interface VoiceRecordingResult { blob: Blob; durationMs: number; }
type RecordingState = 'idle' | 'requesting' | 'recording' | 'error';
export function useVoiceRecorder() {
 const [recordingState, setRecordingState] = useState<RecordingState>('idle');
 const [elapsedMs, setElapsedMs] = useState(0);
 const [audioLevels, setAudioLevels] = useState<number[]>([]);
 const [errorMessage, setErrorMessage] = useState<string | null>(null);
 const mediaRecorderRef = useRef<MediaRecorder | null>(null);
 const mediaStreamRef = useRef<MediaStream | null>(null);
 const analyserStreamRef = useRef<MediaStream | null>(null);
 const audioContextRef = useRef<AudioContext | null>(null);
 const analyserRef = useRef<AnalyserNode | null>(null);
 const analyserTimerRef = useRef<ReturnType<typeof setInterval> | null>(null);
 const elapsedTimerRef = useRef<ReturnType<typeof setInterval> | null>(null);
 const startTimeRef = useRef(0);
 const chunksRef = useRef<BlobPart[]>([]);
 const cancelledRef = useRef(false);
 const cleanup = useCallback(() => {
  if (analyserTimerRef.current) clearInterval(analyserTimerRef.current);
  if (elapsedTimerRef.current) clearInterval(elapsedTimerRef.current);
  analyserTimerRef.current = null; elapsedTimerRef.current = null;
  if (audioContextRef.current) void audioContextRef.current.close().catch(() => undefined);
  audioContextRef.current = null; analyserRef.current = null;
  stopMediaStream(analyserStreamRef.current); stopMediaStream(mediaStreamRef.current);
  analyserStreamRef.current = null; mediaStreamRef.current = null; mediaRecorderRef.current = null; chunksRef.current = [];
 }, []);
 const startRecording = useCallback(async () => {
  setErrorMessage(null); cancelledRef.current = false; chunksRef.current = [];
  if (typeof navigator === 'undefined' || !navigator.mediaDevices?.getUserMedia) {
   setRecordingState('error'); setErrorMessage('Trình duyệt không hỗ trợ ghi âm.'); return;
  }
  setRecordingState('requesting');
  try {
   const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
   mediaStreamRef.current = stream;
   if (cancelledRef.current) return cleanup();
   const audioContext = new AudioContext();
   audioContextRef.current = audioContext;
   analyserStreamRef.current = stream.clone();
   const source = audioContext.createMediaStreamSource(analyserStreamRef.current);
   const analyser = audioContext.createAnalyser();
   analyser.fftSize = 256; source.connect(analyser); analyserRef.current = analyser;
   const mimeType = resolveRecorderMimeType();
   if (!mimeType) { cleanup(); setRecordingState('error'); setErrorMessage('Trình duyệt không hỗ trợ ghi âm WebM/Opus.'); return; }
   const recorder = new MediaRecorder(stream, { mimeType, audioBitsPerSecond: 16_000 });
   mediaRecorderRef.current = recorder;
   recorder.ondataavailable = (event) => { if (event.data.size > 0) chunksRef.current.push(event.data); };
   recorder.start(250);
   startTimeRef.current = Date.now();
   setRecordingState('recording'); setElapsedMs(0); setAudioLevels([]);
   elapsedTimerRef.current = setInterval(() => {
    const elapsed = Date.now() - startTimeRef.current;
    setElapsedMs(elapsed);
    if (elapsed >= MAX_DURATION_SECONDS * 1000 && mediaRecorderRef.current?.state === 'recording') mediaRecorderRef.current.stop();
   }, 200);
   analyserTimerRef.current = setInterval(() => {
    if (!analyserRef.current) return;
    const dataArray = new Uint8Array(analyserRef.current.frequencyBinCount);
    analyserRef.current.getByteFrequencyData(dataArray);
    setAudioLevels(buildWaveformBars(dataArray, WAVEFORM_BAR_COUNT));
   }, ANALYSER_INTERVAL_MS);
  } catch (error) {
   cleanup(); setRecordingState('error'); setErrorMessage(mapVoiceRecorderError(error));
  }
 }, [cleanup]);
 const stopRecording = useCallback((): Promise<VoiceRecordingResult | null> => new Promise((resolve) => {
  const recorder = mediaRecorderRef.current;
  const finish = (blob: Blob | null, durationMs: number) => {
   cleanup(); setRecordingState('idle'); setElapsedMs(0); setAudioLevels([]);
   resolve(blob && blob.size > 0 && !cancelledRef.current ? { blob, durationMs: Math.max(1, durationMs) } : null);
  };
  if (!recorder || recorder.state === 'inactive') {
   const blob = chunksRef.current.length > 0 ? new Blob(chunksRef.current, { type: recorder?.mimeType || 'audio/webm' }) : null;
   finish(blob, Date.now() - startTimeRef.current); return;
  }
  recorder.onstop = () => finish(new Blob(chunksRef.current, { type: recorder.mimeType || 'audio/webm' }), Date.now() - startTimeRef.current);
  recorder.stop();
 }), [cleanup]);
 const cancelRecording = useCallback(() => {
  cancelledRef.current = true;
  const recorder = mediaRecorderRef.current;
  if (recorder && recorder.state !== 'inactive') recorder.stop();
  cleanup(); setRecordingState('idle'); setElapsedMs(0); setAudioLevels([]); setErrorMessage(null);
 }, [cleanup]);
 const dismissError = useCallback(() => { setRecordingState('idle'); setErrorMessage(null); }, []);

 useEffect(() => () => {
  cleanup();
 }, [cleanup]);

 return { recordingState, isRecording: recordingState === 'recording', isRequesting: recordingState === 'requesting', elapsedMs, audioLevels, errorMessage, startRecording, stopRecording, cancelRecording, dismissError };
}
