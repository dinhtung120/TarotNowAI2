'use client';

import dynamic from 'next/dynamic';
import type { ChatRoomViewProps } from '@/features/chat/room/ChatRoomView.types';
import ChatRoomView from '@/features/chat/room/ChatRoomView';
import { useChatRoomPageViewModel } from '@/features/chat/room/useChatRoomPageViewModel';
import { cn } from '@/lib/utils';
import { GlassCard } from '@/shared/ui';

const PaymentOfferModal = dynamic(
  () => import('@/features/chat/payment-offers/PaymentOfferModal'),
  { loading: () => null },
) as ChatRoomViewProps['PaymentOfferModal'];

const VoiceRecorderButton = dynamic(
  () => import('@/features/chat/room/components/VoiceRecorderButton'),
  { ssr: false },
) as ChatRoomViewProps['VoiceRecorderButton'];

const VoiceMessageBubble = dynamic(
  () => import('@/features/chat/room/components/VoiceMessageBubble'),
  { ssr: false },
) as ChatRoomViewProps['VoiceMessageBubble'];

interface ChatRoomPageProps {
  conversationId?: string;
}

export default function ChatRoomPage({ conversationId }: ChatRoomPageProps) {
  const { conversationId: resolvedConversationId, emptyStateText, viewProps } =
    useChatRoomPageViewModel({
      externalConversationId: conversationId,
      PaymentOfferModal,
      VoiceMessageBubble,
      VoiceRecorderButton,
    });

  if (!resolvedConversationId) {
    return (
      <GlassCard
        className={cn(
          'flex h-full items-center justify-center text-[var(--text-secondary)]',
        )}
      >
        {emptyStateText}
      </GlassCard>
    );
  }

  return <ChatRoomView {...viewProps} />;
}
