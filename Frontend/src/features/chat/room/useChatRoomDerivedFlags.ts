import { useMemo } from 'react';
import { normalizeReaderStatus } from '@/features/reader/shared/readerStatus';
import type { ChatMessageDto, ConversationDto } from '@/features/chat/shared/actions';
import { isSameParticipantId } from '@/features/chat/shared/participantId';
import { parseOfferResponseMap, parseStatusLabel } from '@/features/chat/room/utils';

interface UseChatRoomDerivedFlagsParams {
  conversation: ConversationDto | null;
  currentUserId: string;
  initializing: boolean;
  isUserRole: boolean | null;
  messages: ChatMessageDto[];
}

const isUserVisibleMessage = (message: ChatMessageDto, currentUserId: string) =>
  isSameParticipantId(message.senderId, currentUserId)
  && message.type !== 'system';

export function useChatRoomDerivedFlags({
  conversation,
  currentUserId,
  initializing,
  isUserRole,
  messages,
}: UseChatRoomDerivedFlagsParams) {
  const offerResponseMap = useMemo(() => parseOfferResponseMap(messages), [messages]);
  const normalizedReaderStatus = normalizeReaderStatus(conversation?.readerStatus);
  const readerStatus = parseStatusLabel(conversation?.readerStatus);

  const canShowInput = useMemo(() => {
    if (initializing) return true;
    if (!conversation) return false;

    if (conversation.status === 'pending') {
      const hasUserMessage = messages.some((message) =>
        isUserVisibleMessage(message, currentUserId),
      );
      return isUserRole === true && !hasUserMessage;
    }

    return conversation.status === 'ongoing';
  }, [conversation, currentUserId, initializing, isUserRole, messages]);

  const canReaderAcceptReject = useMemo(
    () => conversation?.status === 'awaiting_acceptance' && isUserRole === false,
    [conversation?.status, isUserRole],
  );

  const isReadOnly = useMemo(() => {
    if (initializing) return false;
    if (!conversation) return true;

    if (['completed', 'cancelled', 'expired', 'disputed', 'awaiting_acceptance'].includes(conversation.status)) {
      return true;
    }

    if (conversation.status === 'pending' && isUserRole === true) {
      return messages.some((message) => isUserVisibleMessage(message, currentUserId));
    }

    return false;
  }, [conversation, currentUserId, initializing, isUserRole, messages]);

  const canStartNewSession = useMemo(
    () => conversation?.status === 'completed' && isUserRole === true,
    [conversation?.status, isUserRole],
  );

  const canCancelPending = useMemo(
    () => conversation?.status === 'pending' && isUserRole === true,
    [conversation?.status, isUserRole],
  );

  const canUseActionMenu = useMemo(() => conversation?.status === 'ongoing', [conversation?.status]);

  const awaitingCompleteResponse = conversation?.status === 'ongoing'
    && Boolean(conversation.confirm?.requestedBy)
    && !isSameParticipantId(conversation.confirm?.requestedBy, currentUserId);

  const readOnlyHint = useMemo(() => {
    if (!conversation) return 'Cuộc trò chuyện đang ở chế độ chỉ đọc.';
    if (conversation.status === 'completed') return 'Cuộc trò chuyện đã hoàn thành.';
    if (conversation.status === 'cancelled') return 'Cuộc trò chuyện đã bị hủy.';
    if (conversation.status === 'expired') return 'Cuộc trò chuyện đã hết hạn.';
    if (conversation.status === 'disputed') return 'Cuộc trò chuyện đang chờ Admin xử lý tranh chấp.';
    if (conversation.status === 'awaiting_acceptance') return 'Cuộc trò chuyện đang chờ Reader phản hồi.';

    if (conversation.status === 'pending' && isUserRole === true) {
      const hasUserMessage = messages.some((message) => isUserVisibleMessage(message, currentUserId));
      if (hasUserMessage) return 'Cuộc trò chuyện đang chờ Reader phản hồi.';
    }

    return 'Bạn chưa thể gửi tin nhắn ở trạng thái hiện tại.';
  }, [conversation, currentUserId, isUserRole, messages]);

  return {
    awaitingCompleteResponse,
    canCancelPending,
    canReaderAcceptReject,
    canShowInput,
    canStartNewSession,
    canUseActionMenu,
    headerWarning: normalizedReaderStatus === 'busy' || normalizedReaderStatus === 'offline',
    offerResponseMap,
    readOnlyHint,
    readerStatus,
    isReadOnly,
  };
}
