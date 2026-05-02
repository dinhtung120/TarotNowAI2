import ChatComposerEditableFooter from '@/features/chat/presentation/chat-room/ChatComposerEditableFooter';
import ChatReadOnlyFooter from '@/features/chat/presentation/chat-room/ChatReadOnlyFooter';
import type { ChatComposerProps } from '@/features/chat/presentation/chat-room/chatRoomUi.types';

export default function ChatComposer({
  canShowInput,
  canStartNewSession,
  canSubmitReview,
  hasSubmittedReview,
  isReadOnly,
  readOnlyHint,
  startingNewSession,
  onOpenReviewModal,
  onStartNewSession,
  ...editableProps
}: ChatComposerProps) {
  if (!canShowInput) {
    return (
      <ChatReadOnlyFooter
        canStartNewSession={canStartNewSession}
        canSubmitReview={canSubmitReview}
        hasSubmittedReview={hasSubmittedReview}
        hint={readOnlyHint}
        isReadOnly={isReadOnly}
        startingNewSession={startingNewSession}
        onOpenReviewModal={onOpenReviewModal}
        onStartNewSession={onStartNewSession}
      />
    );
  }

  return <ChatComposerEditableFooter {...editableProps} />;
}
