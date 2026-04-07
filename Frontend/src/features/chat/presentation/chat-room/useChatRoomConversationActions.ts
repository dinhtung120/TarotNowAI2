import { useCallback, useState } from 'react';
import toast from 'react-hot-toast';
import {
  acceptConversation,
  cancelPendingConversation,
  createConversation,
  openConversationDispute,
  rejectConversation,
  requestConversationComplete,
  respondConversationComplete,
  type ConversationDto,
} from '@/features/chat/application/actions';

interface UseChatRoomConversationActionsParams {
  conversation: ConversationDto | null;
  conversationId?: string;
  disputeReason: string;
  rejectReason: string;
  setDisputeReason: (value: string) => void;
  setRejectReason: (value: string) => void;
  setShowDisputeModal: (value: boolean) => void;
  pushRoute: (path: string) => void;
}

export function useChatRoomConversationActions({
  conversation,
  conversationId,
  disputeReason,
  rejectReason,
  setDisputeReason,
  setRejectReason,
  setShowDisputeModal,
  pushRoute,
}: UseChatRoomConversationActionsParams) {
  const [processingAction, setProcessingAction] = useState<string | null>(null);
  const [startingNewSession, setStartingNewSession] = useState(false);

  const runAction = useCallback(async (key: string, action: () => Promise<void>) => {
    setProcessingAction(key);
    try {
      await action();
    } finally {
      setProcessingAction(null);
    }
  }, []);

  const handleAcceptConversation = useCallback(
    () => runAction('accept', async () => {
      if (!conversationId) return;
      const result = await acceptConversation(conversationId);
      if (!result.success) {
        toast.error(result.error || 'Không thể chấp nhận cuộc trò chuyện.');
      }
    }),
    [conversationId, runAction],
  );

  const handleRejectConversation = useCallback(
    () => runAction('reject', async () => {
      if (!conversationId) return;
      const result = await rejectConversation(conversationId, rejectReason || 'Không phù hợp');
      if (!result.success) {
        toast.error(result.error || 'Không thể từ chối cuộc trò chuyện.');
        return;
      }
      setRejectReason('');
    }),
    [conversationId, rejectReason, runAction, setRejectReason],
  );

  const handleRequestComplete = useCallback(
    () => runAction('complete-request', async () => {
      if (!conversationId) return;
      const result = await requestConversationComplete(conversationId);
      if (!result.success) {
        toast.error(result.error || 'Không thể gửi yêu cầu hoàn thành.');
      }
    }),
    [conversationId, runAction],
  );

  const handleRespondComplete = useCallback(
    (accept: boolean) => runAction(`complete-respond-${accept ? 'accept' : 'reject'}`, async () => {
      if (!conversationId) return;
      const result = await respondConversationComplete(conversationId, accept);
      if (!result.success) {
        toast.error(result.error || 'Không thể phản hồi yêu cầu hoàn thành.');
      }
    }),
    [conversationId, runAction],
  );

  const handleOpenDispute = useCallback(
    () => runAction('open-dispute', async () => {
      if (!conversationId || !disputeReason.trim()) return;
      const result = await openConversationDispute(conversationId, { reason: disputeReason.trim() });
      if (!result.success) {
        toast.error(result.error || 'Không thể mở tranh chấp.');
        return;
      }
      setDisputeReason('');
      setShowDisputeModal(false);
    }),
    [conversationId, disputeReason, runAction, setDisputeReason, setShowDisputeModal],
  );

  const handleCancelPending = useCallback(
    () => runAction('cancel-pending', async () => {
      if (!conversationId) return;
      const result = await cancelPendingConversation(conversationId);
      if (!result.success) {
        toast.error(result.error || 'Không thể hủy cuộc trò chuyện pending.');
      }
    }),
    [conversationId, runAction],
  );

  const handleStartNewSession = useCallback(async () => {
    if (!conversation?.readerId) return;

    setStartingNewSession(true);
    try {
      const result = await createConversation(conversation.readerId, conversation.slaHours ?? 12);
      if (!result.success || !result.data?.id) {
        toast.error(result.error || 'Không thể bắt đầu phiên tư vấn mới.');
        return;
      }
      pushRoute(`/chat/${result.data.id}`);
    } finally {
      setStartingNewSession(false);
    }
  }, [conversation, pushRoute]);

  return {
    handleAcceptConversation,
    handleCancelPending,
    handleOpenDispute,
    handleRejectConversation,
    handleRequestComplete,
    handleRespondComplete,
    handleStartNewSession,
    processingAction,
    startingNewSession,
  };
}
