import { useCallback, useEffect, useMemo, useState, type Dispatch, type SetStateAction } from 'react';
import { useTranslations } from 'next-intl';
import toast from 'react-hot-toast';
import { submitConversationReview, type ConversationDto } from '@/features/chat/shared/actions';

interface UseChatRoomReviewActionsParams {
  conversation: ConversationDto | null;
  conversationId?: string;
  isUserRole: boolean | null;
  setConversation: Dispatch<SetStateAction<ConversationDto | null>>;
}

const DISMISSED_REVIEW_KEY_PREFIX = 'chat:review:dismissed:';

function getDismissedReviewKey(conversationId?: string) {
  if (!conversationId) {
    return null;
  }

  return `${DISMISSED_REVIEW_KEY_PREFIX}${conversationId}`;
}

function isDismissed(dismissedKey: string) {
  if (typeof window === 'undefined') {
    return false;
  }

  return window.localStorage.getItem(dismissedKey) === '1';
}

function setDismissed(dismissedKey: string, dismissed: boolean) {
  if (typeof window === 'undefined') {
    return;
  }

  if (dismissed) {
    window.localStorage.setItem(dismissedKey, '1');
    return;
  }

  window.localStorage.removeItem(dismissedKey);
}

export function useChatRoomReviewActions({
  conversation,
  conversationId,
  isUserRole,
  setConversation,
}: UseChatRoomReviewActionsParams) {
  const t = useTranslations('Chat');
  const [showReviewModal, setShowReviewModal] = useState(false);
  const [submittingReview, setSubmittingReview] = useState(false);
  const [reviewRating, setReviewRating] = useState(5);
  const [reviewComment, setReviewComment] = useState('');
  const canSubmitReview = Boolean(
    conversation?.hasSubmittedReview !== true
    && (
      conversation?.canSubmitReview === true
      || (isUserRole === true && conversation?.status === 'completed')
    ),
  );
  const dismissedReviewKey = useMemo(() => getDismissedReviewKey(conversationId), [conversationId]);

  useEffect(() => {
    if (!canSubmitReview || !dismissedReviewKey) {
      setShowReviewModal(false);
      return;
    }

    if (isDismissed(dismissedReviewKey)) {
      return;
    }

    setShowReviewModal(true);
  }, [canSubmitReview, dismissedReviewKey]);

  const handleCloseReviewModal = useCallback(() => {
    setShowReviewModal(false);
    if (canSubmitReview && dismissedReviewKey) {
      setDismissed(dismissedReviewKey, true);
    }
  }, [canSubmitReview, dismissedReviewKey]);

  const handleOpenReviewModal = useCallback(() => {
    if (!canSubmitReview) {
      return;
    }

    if (dismissedReviewKey) {
      setDismissed(dismissedReviewKey, false);
    }

    setShowReviewModal(true);
  }, [canSubmitReview, dismissedReviewKey]);

  const handleSubmitReview = useCallback(async () => {
    if (!conversationId || !canSubmitReview || submittingReview) {
      return;
    }

    setSubmittingReview(true);
    try {
      const payload = {
        rating: reviewRating,
        comment: reviewComment.trim() || undefined,
      };
      const result = await submitConversationReview(conversationId, payload);
      if (!result.success || !result.data) {
        toast.error(result.error || t('room.review.submit_failed'));
        return;
      }
      const submittedReview = result.data;

      setConversation((previous) => {
        if (!previous) {
          return previous;
        }

        return {
          ...previous,
          canSubmitReview: false,
          hasSubmittedReview: true,
          reviewedAt: submittedReview.createdAt,
          updatedAt: submittedReview.createdAt,
        };
      });

      if (dismissedReviewKey) {
        setDismissed(dismissedReviewKey, false);
      }
      setShowReviewModal(false);
      setReviewComment('');
      setReviewRating(5);
      toast.success(t('room.review.submit_success'));
    } finally {
      setSubmittingReview(false);
    }
  }, [
    canSubmitReview,
    conversationId,
    dismissedReviewKey,
    reviewComment,
    reviewRating,
    setConversation,
    submittingReview,
    t,
  ]);

  return {
    canSubmitReview,
    reviewComment,
    reviewRating,
    showReviewModal,
    submittingReview,
    setReviewComment,
    setReviewRating,
    onCloseReviewModal: handleCloseReviewModal,
    onOpenReviewModal: handleOpenReviewModal,
    onSubmitReview: handleSubmitReview,
  };
}
