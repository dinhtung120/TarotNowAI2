'use client';

import React, { createContext, useContext } from 'react';
import { useCallStore } from '../../application/call/useCallStore';
import { useCallSignaling } from '../../application/call/useCallSignaling';
import { useWebRTC } from '../../application/call/useWebRTC';
import { useCallTimeout } from '../../application/call/useCallTimeout';

const CallContext = createContext<{
  initiateCall: (conversationId: string, type: string) => Promise<void>;
  respondCall: (callSessionId: string, accept: boolean) => Promise<void>;
  endCall: (callSessionId: string, reason?: string) => Promise<void>;
  connected: boolean;
} | null>(null);

export const CallProvider = ({ children }: { children: React.ReactNode }) => {
  const { 
    initiateCall, 
    respondCall, 
    endCall, 
    sendOffer, 
    sendAnswer, 
    sendIceCandidate, 
    connected 
  } = useCallSignaling();

  // Đăng ký WebRTC (Listen Offer/Answer & ICE Candidates)
  const { peerConnectionRef } = useWebRTC({ sendOffer, sendAnswer, sendIceCandidate });

  // Hook đo timeout 60s
  useCallTimeout(endCall);

  // Expose các SignalR action qua context để các nút (CallButton, EndCallButton) gọi
  return (
    <CallContext.Provider value={{ initiateCall, respondCall, endCall, connected }}>
      {children}
    </CallContext.Provider>
  );
};

export const useCallContext = () => {
  const context = useContext(CallContext);
  if (!context) {
    throw new Error('useCallContext must be used within CallProvider');
  }
  return context;
};
