import { useEffect } from 'react';
import { useParams } from 'next/navigation';
import { useLocale, useTranslations } from 'next-intl';
import { useChatRoomPageState } from '@/features/chat/room/useChatRoomPageState';
import type { ChatRoomViewProps } from '@/features/chat/room/ChatRoomView.types';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';

interface UseChatRoomPageViewModelParams extends Pick<
  ChatRoomViewProps,
  'PaymentOfferModal' | 'VoiceMessageBubble' | 'VoiceRecorderButton'
> {
  externalConversationId?: string;
}

export function useChatRoomPageViewModel({
  externalConversationId,
  PaymentOfferModal,
  VoiceMessageBubble,
  VoiceRecorderButton,
}: UseChatRoomPageViewModelParams) {
  const params = useParams();
  const navigation = useOptimizedNavigation();
  const locale = useLocale();
  const t = useTranslations('Chat');

  const conversationId =
    externalConversationId ?? (params.id as string | undefined);

  const state = useChatRoomPageState({
    conversationId,
    pushRoute: navigation.push,
  });

  const {
    actionMenuRef,
    disputeReason,
    rejectReason,
    setDisputeReason,
    setRejectReason,
    setShowActionMenu,
    setShowDisputeModal,
    setShowPaymentOffer,
    showActionMenu,
    showDisputeModal,
    showPaymentOffer,
  } = state.uiState;

  useEffect(() => {
    setShowActionMenu(false);
    setShowDisputeModal(false);
  }, [conversationId, setShowActionMenu, setShowDisputeModal]);

  const viewProps: ChatRoomViewProps = {
      actionMenuRef,
      awaitingCompleteResponse: state.derived.awaitingCompleteResponse,
      canCancelPending: state.derived.canCancelPending,
      canReaderAcceptReject: state.derived.canReaderAcceptReject,
      canShowInput: state.derived.canShowInput,
      canStartNewSession: state.derived.canStartNewSession,
      canUseActionMenu: state.derived.canUseActionMenu,
      conversation: state.chatConnection.conversation,
      currentUserId: state.chatConnection.currentUserId,
      disputeReason,
      hasMore: state.chatConnection.hasMore,
      headerWarning: state.derived.headerWarning,
      imageInputRef: state.inputMedia.imageInputRef,
      inputPlaceholder: t('room.input_placeholder'),
      inputRef: state.chatConnection.inputRef,
      isReadOnly: state.derived.isReadOnly,
      isUserRole: state.chatConnection.isUserRole,
      loading: state.chatConnection.loading,
      loadingMore: state.chatConnection.loadingMore,
      locale,
      messages: state.chatConnection.messages,
      messagesEndRef: state.chatConnection.messagesEndRef,
      newMessage: state.chatConnection.newMessage,
      offerResponseMap: state.derived.offerResponseMap,
      otherAvatar: state.chatConnection.otherAvatar,
      otherName: state.chatConnection.otherName,
      processingAction: state.conversationActions.processingAction,
      processingOfferId: state.paymentOffers.processingOfferId,
      readOnlyHint: state.derived.readOnlyHint,
      readerStatus: state.derived.readerStatus,
      rejectReason,
      remoteTyping: state.chatConnection.remoteTyping,
      requestingAddMoney: state.paymentOffers.requestingAddMoney,
      reviewComment: state.reviewActions.reviewComment,
      reviewRating: state.reviewActions.reviewRating,
      scrollRef: state.inputMedia.scrollRef,
      sending: state.chatConnection.sending,
      showActionMenu,
      showDisputeModal,
      showPaymentOffer,
      showReviewModal: state.reviewActions.showReviewModal,
      startingNewSession: state.conversationActions.startingNewSession,
      submittingReview: state.reviewActions.submittingReview,
      title: t('room.title'),
      uploadingMedia: state.inputMedia.uploadingMedia,
      uploadPercent: state.inputMedia.uploadPercent,
      uploadingMediaLabel: t('room.uploading_media', { percent: state.inputMedia.uploadPercent }),
      warningText:
        '⚠️ Reader đang không hoạt động. Thời gian phản hồi có thể lâu hơn SLA cam kết.',
      PaymentOfferModal,
      VoiceMessageBubble,
      VoiceRecorderButton,
      onAcceptConversation: state.conversationActions.handleAcceptConversation,
      onAcceptOffer: state.paymentOffers.handleAcceptOffer,
      onBack: () => navigation.push('/chat'),
      onCancelPending: state.conversationActions.handleCancelPending,
      onCloseDisputeModal: () => setShowDisputeModal(false),
      onClosePaymentOffer: () => setShowPaymentOffer(false),
      onImageInputChange: state.inputMedia.onImageInputChange,
      onInputChange: state.inputMedia.onInputChange,
      onInputKeyDown: state.inputMedia.onInputKeyDown,
      onLoadMore: state.chatConnection.loadMore,
      onRejectConversation: state.conversationActions.handleRejectConversation,
      onRejectOffer: state.paymentOffers.handleRejectOffer,
      onRequestComplete: state.conversationActions.handleRequestComplete,
      onRespondComplete: state.conversationActions.handleRespondComplete,
      onSendPaymentOffer: state.paymentOffers.handleSendPaymentOffer,
      onSendTextMessage: state.chatConnection.handleSendTextMessage,
      onSetDisputeReason: setDisputeReason,
      onSetRejectReason: setRejectReason,
      onSetShowActionMenu: setShowActionMenu,
      onSetShowDisputeModal: setShowDisputeModal,
      onSetShowPaymentOffer: setShowPaymentOffer,
      onSetReviewComment: state.reviewActions.setReviewComment,
      onSetReviewRating: state.reviewActions.setReviewRating,
      onCloseReviewModal: state.reviewActions.onCloseReviewModal,
      onOpenReviewModal: state.reviewActions.onOpenReviewModal,
      onSubmitReview: state.reviewActions.onSubmitReview,
      onStartNewSession: state.conversationActions.handleStartNewSession,
      onSubmitDispute: state.conversationActions.handleOpenDispute,
      onVoiceRecordingComplete: state.inputMedia.onVoiceRecordingComplete,
    };

  return {
    conversationId,
    emptyStateText: t('room.empty'),
    viewProps,
  };
}
