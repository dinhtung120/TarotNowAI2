'use client';

import { MessageCircle } from 'lucide-react';
import type { useTranslations } from 'next-intl';
import { useRelativeTimeNow } from '@/shared/hooks/useRelativeTimeNow';
import type { ConversationDto } from '@/features/chat/shared/actions';
import MessageDropdownItem from '@/features/chat/inbox/dropdown/message-dropdown/MessageDropdownItem';
import { formatMessageRelativeTime } from '@/features/chat/inbox/dropdown/message-dropdown/messageTime';
import { cn } from '@/lib/utils';

interface MessageDropdownContentProps {
 conversations: ConversationDto[];
 currentUserId: string;
 hasLoadError: boolean;
 isLoading: boolean;
 loadErrorMessage: string;
 t: ReturnType<typeof useTranslations>;
 onOpenConversation: (conversationId: string) => void;
}

export default function MessageDropdownContent({
 conversations,
 currentUserId,
 hasLoadError,
 isLoading,
 loadErrorMessage,
 t,
 onOpenConversation,
}: MessageDropdownContentProps) {
 const referenceNowMs = useRelativeTimeNow();

 if (isLoading) {
  return <div className={cn('flex h-24 items-center justify-center tn-text-muted')}><MessageCircle className={cn('h-5 w-5 animate-pulse')} /></div>;
 }

 if (hasLoadError) {
  return <div className={cn('px-4 py-8 text-center text-xs font-semibold text-red-300')}>{loadErrorMessage}</div>;
 }

 if (conversations.length === 0) {
  return (
   <div className={cn('px-4 py-8 text-center')}>
    <div className={cn('mx-auto mb-3 flex h-12 w-12 items-center justify-center rounded-full tn-bg-surface-hover')}>
     <MessageCircle className={cn('h-5 w-5 tn-text-muted')} />
    </div>
    <p className={cn('text-sm font-medium tn-text-secondary')}>{t('dropdown.empty')}</p>
   </div>
  );
 }

 return (
  <div className={cn('flex flex-col')}>
   {conversations.map((conversation) => (
    <MessageDropdownItem
     key={conversation.id}
     awaitingReplyLabel={t('dropdown.preview_awaiting_reply')}
     completedLabel={t('dropdown.preview_completed')}
     conversation={conversation}
     currentUserId={currentUserId}
     newConversationLabel={t('dropdown.preview_new_conversation')}
     timeLabel={formatMessageRelativeTime(conversation.lastMessageAt, t, referenceNowMs)}
     onOpenConversation={onOpenConversation}
    />
   ))}
  </div>
 );
}
