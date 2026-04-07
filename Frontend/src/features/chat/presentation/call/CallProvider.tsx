'use client';

import React, { createContext, useContext } from 'react';
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

  useWebRTC({ sendOffer, sendAnswer, sendIceCandidate });

  useCallTimeout(endCall);

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
