import { Loader2 } from 'lucide-react';
import ChatMessageListItem from '@/features/chat/presentation/chat-room/messages/ChatMessageListItem';
import type { ChatMessagesPanelProps } from '@/features/chat/presentation/chat-room/chatRoomUi.types';
import { cn } from '@/lib/utils';

export default function ChatMessagesPanel({
  currentUserId,
  hasMore,
  isUserRole,
  loadMore,
  loading,
  loadingMore,
  locale,
  messages,
  messagesEndRef,
  offerResponseMap,
  processingOfferId,
  remoteTyping,
  scrollRef,
  onAcceptOffer,
  onRejectOffer,
  VoiceMessageBubble,
}: ChatMessagesPanelProps) {
  return (
    <div ref={scrollRef} className={cn('flex-1 space-y-4 overflow-y-auto bg-black/5 px-3 py-3')}>
      {loading ? <div className={cn('flex h-full items-center justify-center')}><Loader2 className={cn('h-5 w-5 animate-spin tn-text-secondary')} /></div> : null}

      {!loading && hasMore ? (
        <div className={cn('flex justify-center py-2')}>
          <button
            type="button"
            onClick={() => void loadMore()}
            disabled={loadingMore}
            className={cn('rounded-full tn-chat-load-more-btn px-3 py-1 text-xs')}
          >
            {loadingMore ? 'Đang tải...' : 'Tải tin nhắn cũ'}
          </button>
        </div>
      ) : null}

      {messages.map((message) => (
        <ChatMessageListItem
          key={message.id}
          currentUserId={currentUserId}
          isUserRole={isUserRole}
          locale={locale}
          message={message}
          offerResponseMap={offerResponseMap}
          processingOfferId={processingOfferId}
          onAcceptOffer={onAcceptOffer}
          onRejectOffer={onRejectOffer}
          VoiceMessageBubble={VoiceMessageBubble}
        />
      ))}

      {remoteTyping ? <div className={cn('px-2 text-xs tn-text-secondary')}>Đối phương đang nhập...</div> : null}
      <div ref={messagesEndRef} />
    </div>
  );
}
