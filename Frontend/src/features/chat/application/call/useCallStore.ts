import { create } from 'zustand';
import { CallJoinTicketDto, CallSessionDto, CallTimeoutsDto } from '../../domain/callTypes';

export type CallUIState =
  | 'idle'
  | 'requested'
  | 'incoming'
  | 'accepted'
  | 'joining'
  | 'connected'
  | 'ending'
  | 'ended'
  | 'failed';

interface CallState {
  uiState: CallUIState;
  session: CallSessionDto | null;
  joinTicket: CallJoinTicketDto | null;
  remoteStream: MediaStream | null;
  localStream: MediaStream | null;
  conversationId: string | null;
  isCaller: boolean;
  ringTimeoutSeconds: number | null;
  joinTimeoutSeconds: number | null;
  reconnectGracePeriodSeconds: number | null;
  endReason: string | null;
  errorCode: string | null;

  setIncomingCall: (session: CallSessionDto, timeouts?: CallTimeoutsDto | null) => void;
  setOutgoingCall: (ticket: CallJoinTicketDto) => void;
  setJoinTicket: (ticket: CallJoinTicketDto) => void;
  setAccepted: (session?: CallSessionDto) => void;
  setJoining: (session?: CallSessionDto) => void;
  setConnected: (session: CallSessionDto) => void;
  setEnding: () => void;
  setEnded: (reason?: string | null) => void;
  setFailed: (reason?: string | null, errorCode?: string | null) => void;
  setStreams: (local: MediaStream | null, remote: MediaStream | null) => void;
  reset: () => void;
}

export const useCallStore = create<CallState>((set) => ({
  uiState: 'idle',
  session: null,
  joinTicket: null,
  remoteStream: null,
  localStream: null,
  conversationId: null,
  isCaller: false,
  ringTimeoutSeconds: null,
  joinTimeoutSeconds: null,
  reconnectGracePeriodSeconds: null,
  endReason: null,
  errorCode: null,

  setIncomingCall: (session, timeouts) =>
    set({
      uiState: 'incoming',
      session,
      conversationId: session.conversationId,
      isCaller: false,
      joinTicket: null,
      ringTimeoutSeconds: timeouts?.ringTimeoutSeconds ?? null,
      joinTimeoutSeconds: timeouts?.joinTimeoutSeconds ?? null,
      reconnectGracePeriodSeconds: timeouts?.reconnectGracePeriodSeconds ?? null,
      endReason: null,
      errorCode: null,
    }),

  setOutgoingCall: (ticket) =>
    set({
      uiState: 'requested',
      session: ticket.session,
      joinTicket: ticket,
      conversationId: ticket.session.conversationId,
      isCaller: true,
      ringTimeoutSeconds: ticket.timeouts?.ringTimeoutSeconds ?? null,
      joinTimeoutSeconds: ticket.timeouts?.joinTimeoutSeconds ?? null,
      reconnectGracePeriodSeconds: ticket.timeouts?.reconnectGracePeriodSeconds ?? null,
      endReason: null,
      errorCode: null,
    }),

  setJoinTicket: (ticket) =>
    set((state) => ({
      joinTicket: ticket,
      session: {
        ...(state.session ?? ticket.session),
        ...ticket.session,
      },
      conversationId: ticket.session.conversationId,
      joinTimeoutSeconds: ticket.timeouts?.joinTimeoutSeconds ?? state.joinTimeoutSeconds,
      reconnectGracePeriodSeconds:
        ticket.timeouts?.reconnectGracePeriodSeconds ?? state.reconnectGracePeriodSeconds,
      ringTimeoutSeconds: ticket.timeouts?.ringTimeoutSeconds ?? state.ringTimeoutSeconds,
      uiState: state.uiState === 'connected' ? 'connected' : 'joining',
      endReason: null,
      errorCode: null,
    })),

  setAccepted: (session) =>
    set((state) => ({
      uiState: state.uiState === 'connected' ? 'connected' : 'accepted',
      session: session
        ? {
            ...(state.session ?? session),
            ...session,
          }
        : state.session,
      endReason: null,
      errorCode: null,
    })),

  setJoining: (session) =>
    set((state) => ({
      uiState: state.uiState === 'connected' ? 'connected' : 'joining',
      session: session
        ? {
            ...(state.session ?? session),
            ...session,
          }
        : state.session,
      endReason: null,
      errorCode: null,
    })),

  setConnected: (session) =>
    set({
      uiState: 'connected',
      session,
      endReason: null,
      errorCode: null,
    }),

  setEnding: () =>
    set((state) => ({
      uiState: state.uiState === 'idle' ? 'idle' : 'ending',
      endReason: null,
      errorCode: null,
    })),

  setEnded: (reason) =>
    set((state) => {
      if (state.localStream) {
        state.localStream.getTracks().forEach((track) => track.stop());
      }
      if (state.remoteStream) {
        state.remoteStream.getTracks().forEach((track) => track.stop());
      }

      return {
        uiState: 'ended',
        remoteStream: null,
        localStream: null,
        endReason: reason ?? null,
        errorCode: null,
      };
    }),

  setFailed: (reason, errorCode) =>
    set((state) => {
      if (state.localStream) {
        state.localStream.getTracks().forEach((track) => track.stop());
      }
      if (state.remoteStream) {
        state.remoteStream.getTracks().forEach((track) => track.stop());
      }

      return {
        uiState: 'failed',
        remoteStream: null,
        localStream: null,
        endReason: reason ?? null,
        errorCode: errorCode ?? null,
      };
    }),

  setStreams: (local, remote) =>
    set({
      localStream: local,
      remoteStream: remote,
    }),

  reset: () =>
    set({
      uiState: 'idle',
      session: null,
      joinTicket: null,
      remoteStream: null,
      localStream: null,
      conversationId: null,
      isCaller: false,
      ringTimeoutSeconds: null,
      joinTimeoutSeconds: null,
      reconnectGracePeriodSeconds: null,
      endReason: null,
      errorCode: null,
    }),
}));
