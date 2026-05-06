import ChatCompleteResponseBar from "@/features/chat/room/ChatCompleteResponseBar";
import type { ChatRoomViewProps } from "@/features/chat/room/ChatRoomView.types";
import ChatMessagesPanel from "@/features/chat/room/components/messages/ChatMessagesPanel";

export default function ChatRoomMessagesSection(props: ChatRoomViewProps) {
  return (
    <>
      <ChatMessagesPanel
        currentUserId={props.currentUserId}
        hasMore={props.hasMore}
        isUserRole={props.isUserRole}
        loadMore={props.onLoadMore}
        loading={props.loading}
        loadingMore={props.loadingMore}
        locale={props.locale}
        messages={props.messages}
        messagesEndRef={props.messagesEndRef}
        offerResponseMap={props.offerResponseMap}
        processingOfferId={props.processingOfferId}
        remoteTyping={props.remoteTyping}
        scrollRef={props.scrollRef}
        VoiceMessageBubble={props.VoiceMessageBubble}
        onAcceptOffer={props.onAcceptOffer}
        onRejectOffer={props.onRejectOffer}
      />
      <ChatCompleteResponseBar
        awaitingCompleteResponse={props.awaitingCompleteResponse}
        processingAction={props.processingAction}
        onRespondComplete={props.onRespondComplete}
      />
    </>
  );
}
