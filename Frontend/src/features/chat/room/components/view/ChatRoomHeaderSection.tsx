import ChatRoomActionBanners from "@/features/chat/room/ChatRoomActionBanners";
import ChatRoomHeader from "@/features/chat/room/ChatRoomHeader";
import type { ChatRoomViewProps } from "@/features/chat/room/ChatRoomView.types";

export default function ChatRoomHeaderSection(props: ChatRoomViewProps) {
  return (
    <>
      <ChatRoomHeader
        conversation={props.conversation}
        headerWarning={props.headerWarning}
        loading={props.loading}
        otherAvatar={props.otherAvatar}
        otherName={props.otherName}
        readerStatus={props.readerStatus}
        title={props.title}
        warningText={props.warningText}
        onBack={props.onBack}
      />
      <ChatRoomActionBanners
        canCancelPending={props.canCancelPending}
        canReaderAcceptReject={props.canReaderAcceptReject}
        conversation={props.conversation}
        isUserRole={props.isUserRole}
        processingAction={props.processingAction}
        rejectReason={props.rejectReason}
        setRejectReason={props.onSetRejectReason}
        onAcceptConversation={props.onAcceptConversation}
        onCancelPending={props.onCancelPending}
        onRejectConversation={props.onRejectConversation}
      />
    </>
  );
}
