import { create } from 'zustand';
import { CallSessionDto, CallType } from '../../domain/callTypes';

export type CallUIState = 'idle' | 'ringing' | 'incoming' | 'connected' | 'ended';

interface CallState {
  uiState: CallUIState;
  session: CallSessionDto | null;
  remoteStream: MediaStream | null;
  localStream: MediaStream | null;
  conversationId: string | null;
  isCaller: boolean;
  
  // Actions
  setIncomingCall: (session: CallSessionDto) => void;
  setOutgoingCall: (conversationId: string, type: CallType) => void;
  setConnected: (session: CallSessionDto) => void;
  setEnded: () => void;
  setStreams: (local: MediaStream | null, remote: MediaStream | null) => void;
  reset: () => void;
}

export const useCallStore = create<CallState>((set) => ({
  uiState: 'idle',
  session: null,
  remoteStream: null,
  localStream: null,
  conversationId: null,
  isCaller: false,

  setIncomingCall: (session) => set({ 
    uiState: 'incoming', 
    session, 
    conversationId: session.conversationId,
    isCaller: false 
  }),
  
  setOutgoingCall: (conversationId, type) => {
    void type;
    return set({
      uiState: 'ringing',
      // Chúng ta fake tạm session cho đến khi Backend trả về session id qua SignalR
      session: null,
      conversationId,
      isCaller: true
    });
  },

  setConnected: (session) => set({ 
    uiState: 'connected', 
    session 
  }),
  
  setEnded: () => set(state => {
    // Dừng tất cả stream
    if (state.localStream) {
      state.localStream.getTracks().forEach(track => track.stop());
    }
    if (state.remoteStream) {
      state.remoteStream.getTracks().forEach(track => track.stop());
    }
    return {
      uiState: 'ended', 
      remoteStream: null,
      localStream: null
    };
  }),

  setStreams: (local, remote) => set({
    localStream: local,
    remoteStream: remote
  }),

  reset: () => set({
    uiState: 'idle',
    session: null,
    remoteStream: null,
    localStream: null,
    conversationId: null,
    isCaller: false
  })
}));
