'use client';

import Image from 'next/image';
import { cn } from '@/lib/utils';
import type { ConversationDto } from '@/features/chat/application/actions';
import { isSameParticipantId } from '@/features/chat/domain/participantId';
import { resolveAvatarUrl, shouldUseUnoptimizedImage } from '@/shared/infrastructure/http/assetUrl';

interface MessageDropdownItemProps {
 awaitingReplyLabel: string;
 completedLabel: string;
 conversation: ConversationDto;
 currentUserId: string;
 newConversationLabel: string;
 timeLabel: string;
 onOpenConversation: (conversationId: string) => void;
}

function resolvePreview(
 conversation: ConversationDto,
 awaitingReplyLabel: string,
 completedLabel: string,
 newConversationLabel: string,
): string {
 if (conversation.lastMessagePreview?.trim()) {
  return conversation.lastMessagePreview;
 }

 switch (conversation.status) {
  case 'awaiting_acceptance':
   return awaitingReplyLabel;
  case 'completed':
   return completedLabel;
  default:
   return newConversationLabel;
 }
}

export default function MessageDropdownItem({
 awaitingReplyLabel,
 completedLabel,
 conversation,
 currentUserId,
 newConversationLabel,
 timeLabel,
 onOpenConversation,
}: MessageDropdownItemProps) {
 const isUserRole = isSameParticipantId(conversation.userId, currentUserId);
 const peerName = isUserRole ? conversation.readerName || 'Reader' : conversation.userName || 'User';
 const peerAvatar = isUserRole ? conversation.readerAvatar : conversation.userAvatar;
 const unreadCount = isUserRole ? conversation.unreadCountUser : conversation.unreadCountReader;
 const avatarUrl = resolveAvatarUrl(peerAvatar);
 const useUnoptimized = shouldUseUnoptimizedImage(avatarUrl);

 return (
  <button
   type="button"
   onClick={() => onOpenConversation(conversation.id)}
   className={cn(
    'group flex w-full items-start gap-3 px-4 py-3 text-left transition-colors',
    unreadCount > 0 ? 'tn-notification-item-unread' : 'tn-notification-item-read',
   )}
  >
   <div className={cn('relative h-10 w-10 shrink-0 overflow-hidden rounded-full border tn-border-soft')}>
    {avatarUrl ? (
     <Image
      src={avatarUrl}
      alt={peerName}
      width={40}
      height={40}
      sizes="40px"
      loading="lazy"
      unoptimized={useUnoptimized}
      className={cn('h-10 w-10 object-cover')}
     />
    ) : (
     <div className={cn('flex h-10 w-10 items-center justify-center tn-bg-surface-hover text-sm font-semibold tn-text-ink')}>
      {peerName.charAt(0)}
     </div>
    )}
    {unreadCount > 0 ? (
     <span className={cn('absolute -right-1 -top-1 flex min-h-5 min-w-5 items-center justify-center rounded-full tn-bg-danger px-1 tn-text-9 font-black text-white')}>
      {unreadCount > 9 ? '9+' : unreadCount}
     </span>
    ) : null}
   </div>

   <div className={cn('min-w-0 flex-1')}>
    <div className={cn('flex items-center justify-between gap-2')}>
     <p className={cn('truncate text-sm font-semibold tn-text-ink')}>{peerName}</p>
     <span className={cn('tn-text-10 tn-text-secondary')}>{timeLabel}</span>
    </div>
    <p className={cn('mt-0.5 truncate tn-text-11 tn-text-secondary')}>
     {resolvePreview(conversation, awaitingReplyLabel, completedLabel, newConversationLabel)}
    </p>
   </div>
  </button>
 );
}
