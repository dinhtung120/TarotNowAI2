import ChatImageMessage from '@/features/chat/room/components/messages/ChatImageMessage';
import ChatPaymentOfferMessage from '@/features/chat/room/components/messages/ChatPaymentOfferMessage';
import ChatPaymentResponseMessage from '@/features/chat/room/components/messages/ChatPaymentResponseMessage';
import ChatSystemMessage from '@/features/chat/room/components/messages/ChatSystemMessage';
import ChatTextMessage from '@/features/chat/room/components/messages/ChatTextMessage';
import ChatVoiceMessage from '@/features/chat/room/components/messages/ChatVoiceMessage';
import type { ChatMessageListItemProps } from '@/features/chat/room/chatRoomUi.types';
import { isSameParticipantId } from '@/features/chat/shared/participantId';
import { isSystemMessage } from '@/features/chat/room/components/messages/messageHelpers';

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
  const isMe = isSameParticipantId(message.senderId, currentUserId);

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

  if (message.type === 'payment_accept' || message.type === 'payment_reject') {
    return (
      <ChatPaymentResponseMessage
        content={message.content}
        createdAt={message.createdAt}
        isMe={isMe}
        isRead={message.isRead}
        locale={locale}
        type={message.type}
      />
    );
  }

  return <ChatTextMessage content={message.content} createdAt={message.createdAt} isMe={isMe} isRead={message.isRead} locale={locale} />;
}
