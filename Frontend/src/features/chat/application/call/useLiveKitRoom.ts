'use client';

import { useEffect, useRef, type MutableRefObject } from 'react';
import { Room, RoomEvent, type Participant, type Track } from 'livekit-client';
import { useCallStore } from './useCallStore';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { CallJoinTicketDto } from '@/features/chat/domain/callTypes';

interface UseLiveKitRoomOptions {
 issueToken: (callSessionId: string) => Promise<CallJoinTicketDto | null>;
}

export function useLiveKitRoom({ issueToken }: UseLiveKitRoomOptions) {
  const roomRef = useRef<Room | null>(null);
  const activeSessionIdRef = useRef<string | null>(null);
  const reconnectAttemptedRef = useRef(false);

  // Hook này theo dõi sự thay đổi của CallStore để quản lý kết nối LiveKit.
  useEffect(() => {
    const unsubscribe = useCallStore.subscribe((state, prevState) => {
      // Nếu trạng thái là kết thúc hoặc lỗi, ngắt kết nối ngay lập tức.
      if ((state.uiState === 'ended' || state.uiState === 'failed') && prevState.uiState !== state.uiState) {
        disconnectRoom(roomRef);
        activeSessionIdRef.current = null;
        reconnectAttemptedRef.current = false;
        return;
      }

      const ticket = state.joinTicket;
      const sessionId = ticket?.session?.id;

      // Guard clauses: bỏ qua nếu không có ticket hoặc trạng thái không phù hợp.
      if (!ticket || !sessionId || state.uiState === 'ended' || state.uiState === 'failed') return;
      if (state.isCaller && state.uiState === 'requested') return;

      /*
       * Nếu session ID không đổi và Room đã tồn tại, không làm gì cả.
       * Điều này cực kỳ quan trọng để tránh việc connectRoom bị gọi lặp lại khi store update.
       */
      if (activeSessionIdRef.current === sessionId && roomRef.current) {
        return;
      }

      // Đánh dấu session hiện tại và bắt đầu kết nối.
      activeSessionIdRef.current = sessionId;
      reconnectAttemptedRef.current = false;

      // Sử dụng setTimeout(0) để đẩy việc kết nối ra ngoài tick hiện tại,
      // tránh các vấn đề về đồng bộ khi Zustand subscribe được gọi.
      setTimeout(() => {
        void connectRoom({ roomRef, ticket, issueToken, reconnectAttemptedRef, activeSessionIdRef });
      }, 0);
    });

    return () => {
      unsubscribe();
      // Chỉ ngắt kết nối thực sự khi component Unmount hoặc Token thay đổi.
      disconnectRoom(roomRef);
    };
  }, [issueToken]);
}

/**
 * Thực hiện kết nối tới LiveKit Room.
 * @param roomRef Reference tới instance Room hiện tại để quản lý vòng đời.
 * @param ticket Thông tin vé tham gia cuộc gọi (token, url).
 * @param issueToken Hàm lấy token mới khi cần.
 * @param reconnectAttemptedRef Cờ đánh dấu đã thử kết nối lại hay chưa.
 * @param activeSessionIdRef Reference lưu trữ ID phiên làm việc hiện tại.
 */
async function connectRoom({ roomRef, ticket, issueToken, reconnectAttemptedRef, activeSessionIdRef }: {
  roomRef: MutableRefObject<Room | null>;
  ticket: CallJoinTicketDto;
  issueToken: (callSessionId: string) => Promise<CallJoinTicketDto | null>;
  reconnectAttemptedRef: MutableRefObject<boolean>;
  activeSessionIdRef: MutableRefObject<string | null>;
}) {
  const sessionId = ticket.session.id;

  /*
   * Kiểm tra nếu Room hiện tại đang hoạt động cho cùng một Session.
   * Nếu đúng, không cần tạo kết nối mới để tránh xung đột 'Client initiated disconnect'.
   */
  if (roomRef.current) {
    // Nếu Room đã kết nối hoặc đang kết nối cho cùng sessionId, bỏ qua.
    const isSameSession = activeSessionIdRef.current === sessionId;
    if (isSameSession && (roomRef.current.state === 'connected' || roomRef.current.state === 'connecting')) {
      return;
    }
    // Nếu khác session hoặc trạng thái không hợp lệ, ngắt kết nối cũ.
    disconnectRoom(roomRef);
  }

  // Khởi tạo instance Room mới với các cấu hình tối ưu.
  const room = new Room({
    adaptiveStream: true, // Tự động điều chỉnh chất lượng stream theo băng thông.
    dynacast: true,        // Tối ưu hóa việc gửi nhiều tầng chất lượng video.
    stopLocalTrackOnUnpublish: true, // Tự động dừng phần cứng khi track không còn dùng (tiết kiệm pin mobile).
    publishDefaults: {
      audioPreset: { maxBitrate: 32000 }, // Giới hạn bitrate audio để tiết kiệm tài nguyên.
      videoCodec: 'h264', // Ép sử dụng H264 để tăng khả năng tương thích trên các thiết bị cũ.
    }
  });

  roomRef.current = room;
  registerRoomEvents(room, issueToken, reconnectAttemptedRef);

  // Cập nhật trạng thái UI sang 'joining'.
  useCallStore.getState().setJoining(ticket.session);

  try {
    /*
     * Thực hiện kết nối tới server LiveKit.
     * Quá trình này có thể mất thời gian, vì vậy các guard clause ở trên cực kỳ quan trọng.
     */
    await room.connect(ticket.liveKitUrl, ticket.accessToken, {
      autoSubscribe: true, // Tự động nhận track từ người tham gia khác.
    });

    // Sau khi kết nối, kích hoạt Media (Microphone/Camera).
    await room.localParticipant.setMicrophoneEnabled(true);
    if (ticket.session.type === 'video') {
      await room.localParticipant.setCameraEnabled(true);
    }

    syncStreams(room);
    updateConnectivityState(room, ticket.session);
  } catch (error) {
    /*
     * Xử lý lỗi kết nối.
     * Nếu lỗi do chủ động ngắt (Client initiated disconnect) thì không báo lỗi Failed.
     */
    const isClientDisconnect = error instanceof Error && error.message.includes('Client initiated disconnect');

    if (!isClientDisconnect) {
      logger.error('Call.LiveKit', error, { message: 'Unable to connect to room', sessionId });
      useCallStore.getState().setFailed('join_failed', 'ROOM_UNAVAILABLE');
      window.setTimeout(() => useCallStore.getState().reset(), 1000);
    }
  }
}

function registerRoomEvents(
 room: Room,
 issueToken: (callSessionId: string) => Promise<CallJoinTicketDto | null>,
 reconnectAttemptedRef: MutableRefObject<boolean>,
) {
 const refresh = () => {
  syncStreams(room);
  updateConnectivityState(room);
 };

 room.on(RoomEvent.TrackSubscribed, refresh);
 room.on(RoomEvent.TrackUnsubscribed, refresh);
 room.on(RoomEvent.LocalTrackPublished, refresh);
 room.on(RoomEvent.LocalTrackUnpublished, refresh);
 room.on(RoomEvent.ParticipantConnected, refresh);
 room.on(RoomEvent.ParticipantDisconnected, refresh);
 room.on(RoomEvent.Reconnected, refresh);
 room.on(RoomEvent.Disconnected, () => {
  syncStreams(room);
  const state = useCallStore.getState();
  if (state.uiState === 'ended' || state.uiState === 'failed') return;
  if (!state.session?.id || reconnectAttemptedRef.current) {
   useCallStore.getState().setFailed('reconnect_failed');
   window.setTimeout(() => useCallStore.getState().reset(), 1000);
   return;
  }

  reconnectAttemptedRef.current = true;
  void reconnectWithFreshToken(room, state.session.id, issueToken);
 });
}

async function reconnectWithFreshToken(
 room: Room,
 callSessionId: string,
 issueToken: (callSessionId: string) => Promise<CallJoinTicketDto | null>,
) {
 try {
  const ticket = await issueToken(callSessionId);
  if (!ticket) throw new Error('Token refresh failed');
  useCallStore.getState().setJoinTicket(ticket);
  await room.connect(ticket.liveKitUrl, ticket.accessToken, { autoSubscribe: true });
  syncStreams(room);
  updateConnectivityState(room, ticket.session);
 } catch {
  useCallStore.getState().setFailed('reconnect_failed', 'TOKEN_EXPIRED');
  window.setTimeout(() => useCallStore.getState().reset(), 1000);
 }
}

function syncStreams(room: Room) {
 const localTracks = room.localParticipant
  .getTrackPublications()
  .map(publication => publication.track)
  .filter((track): track is Track => track != null)
  .map(track => track.mediaStreamTrack);

 const remoteParticipant = room.remoteParticipants.values().next().value as Participant | undefined;
 const remoteTracks = remoteParticipant
  ? remoteParticipant
   .getTrackPublications()
   .map(publication => publication.track)
   .filter((track): track is Track => track != null)
   .map(track => track.mediaStreamTrack)
  : [];

 const localStream = localTracks.length > 0 ? new MediaStream(localTracks) : null;
 const remoteStream = remoteTracks.length > 0 ? new MediaStream(remoteTracks) : null;

 useCallStore.getState().setStreams(localStream, remoteStream);
}

function disconnectRoom(roomRef: MutableRefObject<Room | null>) {
 if (!roomRef.current) return;
 roomRef.current.disconnect();
 roomRef.current = null;
 useCallStore.getState().setStreams(null, null);
}

function updateConnectivityState(room: Room, fallbackSession?: CallJoinTicketDto['session']) {
 const store = useCallStore.getState();
 if (store.uiState === 'ended' || store.uiState === 'failed') return;
 const session = store.session ? { ...store.session } : fallbackSession ? { ...fallbackSession } : null;
 if (!session) return;

 if (room.remoteParticipants.size > 0) {
  session.status = 'connected';
  store.setConnected(session);
  return;
 }

 session.status = 'joining';
 store.setJoining(session);
}
