import type { ChatRoomActionBannersProps } from "@/features/chat/presentation/chat-room/chatRoomUi.types";
import AwaitingReaderBanner from "@/features/chat/presentation/chat-room/action-banners/AwaitingReaderBanner";
import CancelPendingBanner from "@/features/chat/presentation/chat-room/action-banners/CancelPendingBanner";
import ReaderDecisionBanner from "@/features/chat/presentation/chat-room/action-banners/ReaderDecisionBanner";

export default function ChatRoomActionBanners({
  canCancelPending,
  canReaderAcceptReject,
  conversation,
  isUserRole,
  processingAction,
  rejectReason,
  setRejectReason,
  onAcceptConversation,
  onCancelPending,
  onRejectConversation,
}: ChatRoomActionBannersProps) {
  return (
    <>
      {conversation?.status === "awaiting_acceptance" &&
        isUserRole === true && <AwaitingReaderBanner />}
      {canCancelPending && (
        <CancelPendingBanner
          processingAction={processingAction}
          onCancelPending={onCancelPending}
        />
      )}
      {canReaderAcceptReject && (
        <ReaderDecisionBanner
          processingAction={processingAction}
          rejectReason={rejectReason}
          setRejectReason={setRejectReason}
          onAcceptConversation={onAcceptConversation}
          onRejectConversation={onRejectConversation}
        />
      )}
    </>
  );
}
