'use client';
import { useEffect, useRef, useCallback } from 'react';
import { useCallStore } from './useCallStore';
import { logger } from '@/shared/infrastructure/logging/logger';
import { CallType } from '../../domain/callTypes';
import {
 ICE_SERVERS,
 applyAudioSenderParametersAsync,
 getUserMediaWithFallback,
} from '@/features/chat/application/call/webrtcHelpers';
import { useWebRTCSignalEvents } from '@/features/chat/application/call/useWebRTCSignalEvents';
interface UseWebRTCOptions {
  sendOffer: (convoId: string, offer: RTCSessionDescriptionInit) => Promise<void>;
  sendAnswer: (convoId: string, answer: RTCSessionDescriptionInit) => Promise<void>;
  sendIceCandidate: (convoId: string, candidate: RTCIceCandidateInit) => Promise<void>;
}
export function useWebRTC({ sendOffer, sendAnswer, sendIceCandidate }: UseWebRTCOptions) {
  const peerConnectionRef = useRef<RTCPeerConnection | null>(null);
  const pendingCandidatesRef = useRef<RTCIceCandidateInit[]>([]);
  const pendingOfferRef = useRef<RTCSessionDescriptionInit | null>(null);
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
  const setupWebRTC = useCallback(async (type: CallType = 'audio') => {
    const conversationId = useCallStore.getState().conversationId;
    try {
      const stream = await getUserMediaWithFallback(type);
      useCallStore.getState().setStreams(stream, null);
      const pc = new RTCPeerConnection(ICE_SERVERS);
      peerConnectionRef.current = pc;
      stream.getTracks().forEach(track => {
        if (track.kind === 'audio') {
          track.enabled = true;
          track.contentHint = 'speech';
        }
        pc.addTrack(track, stream);
      });
      await applyAudioSenderParametersAsync(pc);
      pc.ontrack = (event) => {
        if (event.streams && event.streams[0]) {
          const incoming = event.streams[0];
          const state = useCallStore.getState();
          const hasSameLocal = state.localStream?.id === stream.id;
          const hasSameRemote = state.remoteStream?.id === incoming.id;
          if (!hasSameLocal || !hasSameRemote) {
            useCallStore.getState().setStreams(stream, incoming);
          }
        }
      };
      pc.onicecandidate = (event) => {
        const convoId = useCallStore.getState().conversationId;
        if (event.candidate && convoId) {
          sendIceCandidateRef.current(convoId, event.candidate);
        }
      };
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
  useEffect(() => {
    const state = useCallStore.getState();
    const handleRole = async () => {
      if (state.uiState === 'connected' && state.session) {
        if (!peerConnectionRef.current) {
          const pc = await setupWebRTC(state.session.type);
          if (pc && state.isCaller && state.conversationId) {
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
  useWebRTCSignalEvents({
    peerConnectionRef,
    pendingCandidatesRef,
    pendingOfferRef,
    sendAnswerRef,
  });
  return { peerConnectionRef };
}
