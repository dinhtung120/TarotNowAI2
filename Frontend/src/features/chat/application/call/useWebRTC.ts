'use client';

import { useEffect, useRef, useCallback } from 'react';
import { useCallStore } from './useCallStore';
import { logger } from '@/shared/infrastructure/logging/logger';
import { CallType } from '../../domain/callTypes';

interface UseWebRTCOptions {
  sendOffer: (convoId: string, offer: RTCSessionDescriptionInit) => Promise<void>;
  sendAnswer: (convoId: string, answer: RTCSessionDescriptionInit) => Promise<void>;
  sendIceCandidate: (convoId: string, candidate: RTCIceCandidateInit) => Promise<void>;
}

function parseIceUrls(rawValue: string | undefined): string[] {
  if (!rawValue) {
    return [];
  }

  return rawValue
    .split(',')
    .map(value => value.trim())
    .filter(value => value.length > 0);
}

function buildIceConfiguration(): RTCConfiguration {
  const stunServers: RTCIceServer[] = [
    { urls: 'stun:stun.l.google.com:19302' },
    { urls: 'stun:stun1.l.google.com:19302' },
    { urls: 'stun:stun2.l.google.com:19302' },
    { urls: 'stun:stun3.l.google.com:19302' },
    { urls: 'stun:stun4.l.google.com:19302' },
    { urls: 'stun:global.stun.twilio.com:3478' }
  ];

  const turnUrls = parseIceUrls(process.env.NEXT_PUBLIC_TURN_URLS);
  const turnUsername = process.env.NEXT_PUBLIC_TURN_USERNAME?.trim();
  const turnCredential = process.env.NEXT_PUBLIC_TURN_CREDENTIAL?.trim();

  const hasTurnCredentials = !!turnUsername && !!turnCredential;
  const turnServers: RTCIceServer[] = hasTurnCredentials && turnUrls.length > 0
    ? [{ urls: turnUrls, username: turnUsername, credential: turnCredential }]
    : [];

  if (turnUrls.length > 0 && hasTurnCredentials === false) {
    logger.warn('Call.WebRTC', 'TURN URLs configured but missing TURN credential/username.');
  }

  return {
    iceServers: [...stunServers, ...turnServers],
    iceCandidatePoolSize: 8
  };
}

async function getUserMediaWithFallback(type: CallType): Promise<MediaStream> {
  const withPreferredConstraints: MediaStreamConstraints = {
    audio: {
      echoCancellation: { ideal: true },
      noiseSuppression: { ideal: true },
      autoGainControl: { ideal: true }
    },
    video: type === 'video'
  };

  try {
    return await navigator.mediaDevices.getUserMedia(withPreferredConstraints);
  } catch (firstError) {
    logger.warn('Call.WebRTC', 'Preferred media constraints failed, fallback to browser defaults.', { error: firstError });
    return navigator.mediaDevices.getUserMedia({ audio: true, video: type === 'video' });
  }
}

async function applyAudioSenderParametersAsync(pc: RTCPeerConnection): Promise<void> {
  const audioSenders = pc.getSenders().filter(sender => sender.track?.kind === 'audio');
  for (const sender of audioSenders) {
    try {
      const parameters = sender.getParameters();
      parameters.encodings ??= [{}];
      if (parameters.encodings[0].maxBitrate == null) {
        parameters.encodings[0].maxBitrate = 32_000;
      }
      await sender.setParameters(parameters);
    } catch (error) {
      logger.warn('Call.WebRTC', 'Unable to apply audio sender parameters.', { error });
    }
  }
}

const ICE_SERVERS: RTCConfiguration = {
  ...buildIceConfiguration()
};

/**
 * Hook quản lý WebRTC PeerConnection.
 *
 * FIX #8: Buffer ICE candidates và SDP offer nhận được trước khi
 * RTCPeerConnection ready. Khi PC setup xong, flush buffer.
 * Nếu không buffer, offer/candidate đến trước PC = bị drop = cuộc gọi im lặng.
 */
export function useWebRTC({ sendOffer, sendAnswer, sendIceCandidate }: UseWebRTCOptions) {
  const peerConnectionRef = useRef<RTCPeerConnection | null>(null);

  // FIX #8: Queue buffer cho các tín hiệu đến trước khi PeerConnection ready
  const pendingCandidatesRef = useRef<RTCIceCandidateInit[]>([]);
  const pendingOfferRef = useRef<RTCSessionDescriptionInit | null>(null);

  // Ref stable cho callbacks để tránh dependency cycle
  const sendAnswerRef = useRef(sendAnswer);
  const sendOfferRef = useRef(sendOffer);
  const sendIceCandidateRef = useRef(sendIceCandidate);

  useEffect(() => {
    sendAnswerRef.current = sendAnswer;
    sendOfferRef.current = sendOffer;
    sendIceCandidateRef.current = sendIceCandidate;
  }, [sendAnswer, sendOffer, sendIceCandidate]);

  const cleanup = useCallback(() => {
    if (peerConnectionRef.current) {
      peerConnectionRef.current.close();
      peerConnectionRef.current = null;
    }
    pendingCandidatesRef.current = [];
    pendingOfferRef.current = null;
  }, []);

  /**
   * Setup RTCPeerConnection và getUserMedia.
   * Sau khi setup xong, flush pending offer/candidates nếu có.
   */
  const setupWebRTC = useCallback(async (type: CallType = 'audio') => {
    const conversationId = useCallStore.getState().conversationId;

    try {
      const stream = await getUserMediaWithFallback(type);
      
      useCallStore.getState().setStreams(stream, null);

      const pc = new RTCPeerConnection(ICE_SERVERS);
      peerConnectionRef.current = pc;

      // Thêm local tracks vào RTCPeerConnection
      stream.getTracks().forEach(track => pc.addTrack(track, stream));
      await applyAudioSenderParametersAsync(pc);

      // Bắt remote tracks
      pc.ontrack = (event) => {
        if (event.streams && event.streams[0]) {
          useCallStore.getState().setStreams(stream, event.streams[0]);
        }
      };

      // Gửi ICE candidates qua SignalR
      pc.onicecandidate = (event) => {
        const convoId = useCallStore.getState().conversationId;
        if (event.candidate && convoId) {
          sendIceCandidateRef.current(convoId, event.candidate);
        }
      };

      // FIX #8: Flush pending offer nếu có (receiver nhận offer trước khi PC ready)
      if (pendingOfferRef.current && conversationId) {
        try {
          await pc.setRemoteDescription(new RTCSessionDescription(pendingOfferRef.current));
          const answer = await pc.createAnswer();
          await pc.setLocalDescription(answer);
          await sendAnswerRef.current(conversationId, answer);
          pendingOfferRef.current = null;
        } catch (err) {
          logger.error('Error flushing pending offer', err);
        }
      }

      // FIX #8: Flush pending ICE candidates
      for (const candidate of pendingCandidatesRef.current) {
        try {
          await pc.addIceCandidate(new RTCIceCandidate(candidate));
        } catch (err) {
          logger.error('Error flushing pending ICE candidate', err);
        }
      }
      pendingCandidatesRef.current = [];

      return pc;
    } catch (err) {
      logger.error('Lỗi khi truy cập camera/microphone', err);
      return null;
    }
  }, []);

  // Khởi tạo luồng theo UI State
  useEffect(() => {
    const state = useCallStore.getState();

    const handleRole = async () => {
      if (state.uiState === 'connected' && state.session) {
        if (!peerConnectionRef.current) {
          const pc = await setupWebRTC(state.session.type);
          if (pc && state.isCaller && state.conversationId) {
            // Người gọi (Caller) sẽ tạo SDP Offer
            try {
              const offer = await pc.createOffer();
              await pc.setLocalDescription(offer);
              await sendOfferRef.current(state.conversationId, offer);
            } catch (e) {
              logger.error('Error creating offer', e);
            }
          }
        }
      } else if (state.uiState === 'ended') {
        cleanup();
      }
    };
    void handleRole();

    // Subscribe to store changes
    const unsubscribe = useCallStore.subscribe((newState, prevState) => {
      if (newState.uiState !== prevState.uiState) {
        if (newState.uiState === 'connected' && newState.session && !peerConnectionRef.current) {
          void (async () => {
            const pc = await setupWebRTC(newState.session!.type);
            if (pc && newState.isCaller && newState.conversationId) {
              try {
                const offer = await pc.createOffer();
                await pc.setLocalDescription(offer);
                await sendOfferRef.current(newState.conversationId, offer);
              } catch (e) {
                logger.error('Error creating offer', e);
              }
            }
          })();
        } else if (newState.uiState === 'ended') {
          cleanup();
        }
      }
    });

    return () => {
      unsubscribe();
      cleanup();
    };
  }, [setupWebRTC, cleanup]);

  // Nghe sự kiện WebRTC Relay qua các CustomEvent (được phát ra từ useCallSignaling)
  useEffect(() => {
    const handleReceiveOffer = async (e: Event) => {
      const offer = (e as CustomEvent).detail as RTCSessionDescriptionInit;
      const pc = peerConnectionRef.current;
      const convoId = useCallStore.getState().conversationId;

      if (pc && convoId) {
        // PC đã ready → xử lý offer trực tiếp
        try {
          await pc.setRemoteDescription(new RTCSessionDescription(offer));
          const answer = await pc.createAnswer();
          await pc.setLocalDescription(answer);
          await sendAnswerRef.current(convoId, answer);
        } catch (err) {
          logger.error('Error handling offer', err);
        }
      } else {
        // FIX #8: PC chưa ready → buffer offer để flush sau khi setupWebRTC hoàn tất
        pendingOfferRef.current = offer;
      }
    };

    const handleReceiveAnswer = async (e: Event) => {
      const answer = (e as CustomEvent).detail as RTCSessionDescriptionInit;
      const pc = peerConnectionRef.current;
      if (pc) {
        try {
          await pc.setRemoteDescription(new RTCSessionDescription(answer));
        } catch (err) {
          logger.error('Error handling answer', err);
        }
      }
    };

    const handleReceiveCandidate = async (e: Event) => {
      const candidate = (e as CustomEvent).detail as RTCIceCandidateInit;
      const pc = peerConnectionRef.current;
      if (pc) {
        try {
          await pc.addIceCandidate(new RTCIceCandidate(candidate));
        } catch (err) {
          logger.error('Error adding ICE candidate', err);
        }
      } else {
        // FIX #8: PC chưa ready → buffer candidate để flush sau
        pendingCandidatesRef.current.push(candidate);
      }
    };

    window.addEventListener('webrtc.offer', handleReceiveOffer);
    window.addEventListener('webrtc.answer', handleReceiveAnswer);
    window.addEventListener('webrtc.ice-candidate', handleReceiveCandidate);

    return () => {
      window.removeEventListener('webrtc.offer', handleReceiveOffer);
      window.removeEventListener('webrtc.answer', handleReceiveAnswer);
      window.removeEventListener('webrtc.ice-candidate', handleReceiveCandidate);
    };
  }, []);

  return { peerConnectionRef };
}
