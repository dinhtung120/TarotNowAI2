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

const ICE_SERVERS: RTCConfiguration = {
  iceServers: [
    { urls: 'stun:stun.l.google.com:19302' },
    { urls: 'stun:stun1.l.google.com:19302' },
    { urls: 'stun:stun2.l.google.com:19302' },
    { urls: 'stun:stun3.l.google.com:19302' },
    { urls: 'stun:stun4.l.google.com:19302' },
    { urls: 'stun:global.stun.twilio.com:3478?transport=udp' }
    // TODO: Tương lai thêm TURN servers trả về từ API backend để vượt firewall
  ]
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
      /* FIX #21: Tối ưu audio constraints cho gọi VoIP.
       * - echoCancellation: Chống tiếng vọng (echo) khi dùng loa ngoài.
       * - noiseSuppression: Giảm tiếng ồn nền (quạt, xe cộ, ...).
       * - autoGainControl: Tự động điều chỉnh âm lượng micro khi nói to/nhỏ.
       * - channelCount: 1 (mono) vì VoIP chỉ cần 1 kênh, giảm bandwidth 50%.
       * - sampleRate: 48000Hz là chuẩn WebRTC Opus codec, cho chất lượng tốt nhất.
       * Trước đây chỉ dùng `audio: true` (mặc định) → trình duyệt tự chọn,
       * dẫn đến stereo không cần thiết và thiếu noise suppression → âm thanh giật. */
      const stream = await navigator.mediaDevices.getUserMedia({
        audio: {
          echoCancellation: true,
          noiseSuppression: true,
          autoGainControl: true,
          channelCount: 1,
          sampleRate: 48000,
        },
        video: type === 'video'
      });
      
      useCallStore.getState().setStreams(stream, null);

      const pc = new RTCPeerConnection(ICE_SERVERS);
      peerConnectionRef.current = pc;

      // Thêm local tracks vào RTCPeerConnection
      stream.getTracks().forEach(track => pc.addTrack(track, stream));

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
