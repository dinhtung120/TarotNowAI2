'use client';

import { useEffect, type MutableRefObject } from 'react';
import { logger } from '@/shared/infrastructure/logging/logger';
import { useCallStore } from './useCallStore';

interface UseWebRTCSignalEventsOptions {
 peerConnectionRef: MutableRefObject<RTCPeerConnection | null>;
 pendingCandidatesRef: MutableRefObject<RTCIceCandidateInit[]>;
 pendingOfferRef: MutableRefObject<RTCSessionDescriptionInit | null>;
 sendAnswerRef: MutableRefObject<(convoId: string, answer: RTCSessionDescriptionInit) => Promise<void>>;
}

export function useWebRTCSignalEvents({
 peerConnectionRef,
 pendingCandidatesRef,
 pendingOfferRef,
 sendAnswerRef,
}: UseWebRTCSignalEventsOptions) {
 useEffect(() => {
  const handleReceiveOffer = async (event: Event) => {
   const offer = (event as CustomEvent).detail as RTCSessionDescriptionInit;
   const pc = peerConnectionRef.current;
   const convoId = useCallStore.getState().conversationId;
   if (!pc || !convoId) {
    pendingOfferRef.current = offer;
    return;
   }
   try {
    await pc.setRemoteDescription(new RTCSessionDescription(offer));
    const answer = await pc.createAnswer();
    await pc.setLocalDescription(answer);
    await sendAnswerRef.current(convoId, answer);
   } catch (error) {
    logger.error('Error handling offer', error);
   }
  };

  const handleReceiveAnswer = async (event: Event) => {
   const answer = (event as CustomEvent).detail as RTCSessionDescriptionInit;
   const pc = peerConnectionRef.current;
   if (!pc) return;
   try {
    await pc.setRemoteDescription(new RTCSessionDescription(answer));
   } catch (error) {
    logger.error('Error handling answer', error);
   }
  };

  const handleReceiveCandidate = async (event: Event) => {
   const candidate = (event as CustomEvent).detail as RTCIceCandidateInit;
   const pc = peerConnectionRef.current;
   if (!pc) {
    pendingCandidatesRef.current.push(candidate);
    return;
   }
   try {
    await pc.addIceCandidate(new RTCIceCandidate(candidate));
   } catch (error) {
    logger.error('Error adding ICE candidate', error);
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
 }, [peerConnectionRef, pendingCandidatesRef, pendingOfferRef, sendAnswerRef]);
}
