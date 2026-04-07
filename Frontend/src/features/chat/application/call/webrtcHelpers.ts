import { logger } from '@/shared/infrastructure/logging/logger';
import { CallType } from '../../domain/callTypes';

function parseIceUrls(rawValue: string | undefined): string[] {
 if (!rawValue) return [];
 return rawValue.split(',').map((value) => value.trim()).filter((value) => value.length > 0);
}

function buildIceConfiguration(): RTCConfiguration {
 const stunServers: RTCIceServer[] = [
  { urls: 'stun:stun.l.google.com:19302' },
  { urls: 'stun:stun1.l.google.com:19302' },
  { urls: 'stun:stun2.l.google.com:19302' },
  { urls: 'stun:stun3.l.google.com:19302' },
  { urls: 'stun:stun4.l.google.com:19302' },
  { urls: 'stun:global.stun.twilio.com:3478' },
 ];
 const turnUrls = parseIceUrls(process.env.NEXT_PUBLIC_TURN_URLS);
 const turnUsername = process.env.NEXT_PUBLIC_TURN_USERNAME?.trim();
 const turnCredential = process.env.NEXT_PUBLIC_TURN_CREDENTIAL?.trim();
 const hasTurnCredentials = Boolean(turnUsername && turnCredential);
 const turnServers: RTCIceServer[] = hasTurnCredentials && turnUrls.length > 0 ? [{ urls: turnUrls, username: turnUsername, credential: turnCredential }] : [];
 if (turnUrls.length > 0 && !hasTurnCredentials) {
  logger.warn('Call.WebRTC', 'TURN URLs configured but missing TURN credential/username.');
 }
 return { iceServers: [...stunServers, ...turnServers], iceCandidatePoolSize: 8 };
}

export const ICE_SERVERS: RTCConfiguration = { ...buildIceConfiguration() };

export async function getUserMediaWithFallback(type: CallType): Promise<MediaStream> {
 const enhancedConstraints: MediaStreamConstraints = {
  audio: {
   echoCancellation: { ideal: true },
   noiseSuppression: { ideal: true },
   autoGainControl: { ideal: true },
   channelCount: { ideal: 1 },
   sampleRate: { ideal: 48_000 },
   sampleSize: { ideal: 16 },
  },
  video: type === 'video',
 };
 try {
  return await navigator.mediaDevices.getUserMedia(enhancedConstraints);
 } catch (firstError) {
  logger.warn('Call.WebRTC', 'Enhanced media profile failed, fallback to default constraints.', { error: firstError });
  return navigator.mediaDevices.getUserMedia({ audio: true, video: type === 'video' });
 }
}

export async function applyAudioSenderParametersAsync(pc: RTCPeerConnection): Promise<void> {
 const configuredMaxBitrate = Number(process.env.NEXT_PUBLIC_CALL_AUDIO_MAX_BITRATE ?? '');
 const shouldSetMaxBitrate = Number.isFinite(configuredMaxBitrate) && configuredMaxBitrate > 0;
 const audioSenders = pc.getSenders().filter((sender) => sender.track?.kind === 'audio');
 for (const sender of audioSenders) {
  try {
   const parameters = sender.getParameters();
   parameters.encodings ??= [{}];
   if (shouldSetMaxBitrate) {
    parameters.encodings[0].maxBitrate = configuredMaxBitrate;
   } else {
    delete parameters.encodings[0].maxBitrate;
   }
   await sender.setParameters(parameters);
  } catch (error) {
   logger.warn('Call.WebRTC', 'Unable to apply audio sender parameters.', { error });
  }
 }
}
