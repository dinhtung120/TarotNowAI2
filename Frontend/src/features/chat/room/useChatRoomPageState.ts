import { useMemo } from 'react';
import { useChatConnection } from '@/features/chat/room/hooks/useChatConnection';
import { usePaymentOfferActions } from '@/features/chat/payment-offers/usePaymentOfferActions';
import { useChatRoomConversationActions } from '@/features/chat/room/useChatRoomConversationActions';
import { useChatRoomDerivedFlags } from '@/features/chat/room/useChatRoomDerivedFlags';
import { useChatRoomInputMediaActions } from '@/features/chat/room/useChatRoomInputMediaActions';
import { useChatRoomReviewActions } from '@/features/chat/room/useChatRoomReviewActions';
import { useChatRoomScrollEffects } from '@/features/chat/room/useChatRoomScrollEffects';
import { useChatRoomUiState } from '@/features/chat/room/useChatRoomUiState';

interface UseChatRoomPageStateParams {
  conversationId?: string;
  pushRoute: (path: string) => void;
}

export function useChatRoomPageState({
  conversationId,
  pushRoute,
}: UseChatRoomPageStateParams) {
  const chatConnection = useChatConnection({ conversationId });
  const paymentOffers = usePaymentOfferActions({ conversationId: conversationId ?? '' });

  const uiState = useChatRoomUiState();

  const derived = useChatRoomDerivedFlags({
    conversation: chatConnection.conversation,
    currentUserId: chatConnection.currentUserId,
    initializing: chatConnection.initializing,
    isUserRole: chatConnection.isUserRole,
    messages: chatConnection.messages,
  });

  const inputMedia = useChatRoomInputMediaActions({
    conversation: chatConnection.conversation,
    notifyTyping: chatConnection.notifyTyping,
    sendMediaMessage: chatConnection.sendMediaMessage,
    setNewMessage: chatConnection.setNewMessage,
    handleSendTextMessage: chatConnection.handleSendTextMessage,
  });

  const conversationActions = useChatRoomConversationActions({
    conversation: chatConnection.conversation,
    conversationId,
    disputeReason: uiState.disputeReason,
    rejectReason: uiState.rejectReason,
    setDisputeReason: uiState.setDisputeReason,
    setRejectReason: uiState.setRejectReason,
    setShowDisputeModal: uiState.setShowDisputeModal,
    pushRoute,
  });
  const reviewActions = useChatRoomReviewActions({
    conversation: chatConnection.conversation,
    conversationId,
    isUserRole: chatConnection.isUserRole,
    setConversation: chatConnection.setConversation,
  });

  const lastMessageId = chatConnection.messages[chatConnection.messages.length - 1]?.id;

  useChatRoomScrollEffects({
    hasMore: chatConnection.hasMore,
    lastMessageId,
    loadMore: chatConnection.loadMore,
    loadingMore: chatConnection.loadingMore,
    messagesEndRef: chatConnection.messagesEndRef,
    remoteTyping: chatConnection.remoteTyping,
    scrollRef: inputMedia.scrollRef,
  });

  return useMemo(
    () => ({
      chatConnection,
      conversationActions,
      derived,
      inputMedia,
      paymentOffers,
      reviewActions,
      uiState,
    }),
    [chatConnection, conversationActions, derived, inputMedia, paymentOffers, reviewActions, uiState],
  );
}
