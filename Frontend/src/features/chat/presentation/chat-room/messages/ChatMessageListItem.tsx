import ChatImageMessage from '@/features/chat/presentation/chat-room/messages/ChatImageMessage';
import ChatPaymentOfferMessage from '@/features/chat/presentation/chat-room/messages/ChatPaymentOfferMessage';
import ChatSystemMessage from '@/features/chat/presentation/chat-room/messages/ChatSystemMessage';
import ChatTextMessage from '@/features/chat/presentation/chat-room/messages/ChatTextMessage';
import ChatVoiceMessage from '@/features/chat/presentation/chat-room/messages/ChatVoiceMessage';
import type { ChatMessageListItemProps } from '@/features/chat/presentation/chat-room/chatRoomUi.types';
import { isSystemMessage } from '@/features/chat/presentation/chat-room/messages/messageHelpers';

export default function ChatMessageListItem({
  currentUserId,
  isUserRole,
  locale,
  message,
  offerResponseMap,
  processingOfferId,
  onAcceptOffer,
  onRejectOffer,
  VoiceMessageBubble,
}: ChatMessageListItemProps) {
  const isMe = message.senderId === currentUserId;

  if (isSystemMessage(message)) {
    return <ChatSystemMessage content={message.content} />;
  }

  if (message.type === 'payment_offer') {
    return (
      <ChatPaymentOfferMessage
        isMe={isMe}
        isUserRole={isUserRole}
        message={message}
        processingOfferId={processingOfferId}
        response={offerResponseMap[message.id]}
        onAccept={onAcceptOffer}
        onReject={onRejectOffer}
      />
    );
  }

  if (message.type === 'image' && message.mediaPayload?.url) {
    return <ChatImageMessage imageUrl={message.mediaPayload.url} isMe={isMe} />;
  }

  if (message.type === 'voice' && message.mediaPayload?.url) {
    return <ChatVoiceMessage audioUrl={message.mediaPayload.url} durationMs={message.mediaPayload.durationMs} createdAt={message.createdAt} isMe={isMe} isRead={message.isRead} locale={locale} VoiceMessageBubble={VoiceMessageBubble} />;
  }

  return <ChatTextMessage content={message.content} createdAt={message.createdAt} isMe={isMe} isRead={message.isRead} locale={locale} />;
}
