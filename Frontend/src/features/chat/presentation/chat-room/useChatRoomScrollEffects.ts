import { useEffect } from 'react';

interface UseChatRoomScrollEffectsParams {
  hasMore: boolean;
  lastMessageId?: string;
  loadMore: () => Promise<void>;
  loadingMore: boolean;
  messagesEndRef: React.MutableRefObject<HTMLDivElement | null>;
  remoteTyping: boolean;
  scrollRef: React.MutableRefObject<HTMLDivElement | null>;
}

export function useChatRoomScrollEffects({
  hasMore,
  lastMessageId,
  loadMore,
  loadingMore,
  messagesEndRef,
  remoteTyping,
  scrollRef,
}: UseChatRoomScrollEffectsParams) {
  useEffect(() => {
    if (!lastMessageId) return;
    const timerId = window.setTimeout(() => {
      messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
    }, 100);

    return () => window.clearTimeout(timerId);
  }, [lastMessageId, messagesEndRef]);

  useEffect(() => {
    if (!remoteTyping) return;
    const timerId = window.setTimeout(() => {
      messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
    }, 100);

    return () => window.clearTimeout(timerId);
  }, [messagesEndRef, remoteTyping]);

  useEffect(() => {
    const node = scrollRef.current;
    if (!node) return;

    const onScroll = () => {
      if (node.scrollTop < 120 && hasMore && !loadingMore) {
        void loadMore();
      }
    };

    node.addEventListener('scroll', onScroll);
    return () => node.removeEventListener('scroll', onScroll);
  }, [hasMore, loadMore, loadingMore, scrollRef]);
}
