import ChatComposer from "@/features/chat/presentation/chat-room/ChatComposer";
import type { ChatRoomViewProps } from "@/features/chat/presentation/chat-room/ChatRoomView.types";

export default function ChatRoomComposerSection(props: ChatRoomViewProps) {
  return (
    <ChatComposer
      actionMenuRef={props.actionMenuRef}
      awaitingCompleteResponse={props.awaitingCompleteResponse}
      canShowInput={props.canShowInput}
      canStartNewSession={props.canStartNewSession}
      canSubmitReview={Boolean(props.conversation?.canSubmitReview)}
      canUseActionMenu={props.canUseActionMenu}
      conversationExists={Boolean(props.conversation)}
      hasSubmittedReview={Boolean(props.conversation?.hasSubmittedReview)}
      imageInputRef={props.imageInputRef}
      inputPlaceholder={props.inputPlaceholder}
      inputRef={props.inputRef}
      isReadOnly={props.isReadOnly}
      isUserRole={props.isUserRole}
      newMessage={props.newMessage}
      processingAction={props.processingAction}
      readOnlyHint={props.readOnlyHint}
      requestingAddMoney={props.requestingAddMoney}
      sending={props.sending}
      showActionMenu={props.showActionMenu}
      startingNewSession={props.startingNewSession}
      uploadPercent={props.uploadPercent}
      uploadingMedia={props.uploadingMedia}
      uploadingMediaLabel={props.uploadingMediaLabel}
      VoiceRecorderButton={props.VoiceRecorderButton}
      setShowActionMenu={props.onSetShowActionMenu}
      onImageInputChange={props.onImageInputChange}
      onInputChange={props.onInputChange}
      onInputKeyDown={props.onInputKeyDown}
      onOpenDispute={() => props.onSetShowDisputeModal(true)}
      onOpenPaymentOffer={() => props.onSetShowPaymentOffer(true)}
      onOpenReviewModal={props.onOpenReviewModal}
      onRequestComplete={props.onRequestComplete}
      onSendTextMessage={props.onSendTextMessage}
      onStartNewSession={props.onStartNewSession}
      onVoiceRecordingComplete={props.onVoiceRecordingComplete}
    />
  );
}
