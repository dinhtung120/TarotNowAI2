import type { ChatRoomActionBannersProps } from "@/features/chat/room/chatRoomUi.types";
import AwaitingReaderBanner from "@/features/chat/room/components/action-banners/AwaitingReaderBanner";
import CancelPendingBanner from "@/features/chat/room/components/action-banners/CancelPendingBanner";
import ReaderDecisionBanner from "@/features/chat/room/components/action-banners/ReaderDecisionBanner";

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
