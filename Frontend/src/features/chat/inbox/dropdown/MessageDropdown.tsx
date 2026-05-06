'use client';

import { useEffect } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { useTranslations } from 'next-intl';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';
import { useChatUnreadNotifications } from '@/features/chat/inbox/hooks/useChatUnreadNotifications';
import { useMessageDropdown } from '@/features/chat/inbox/hooks/useMessageDropdown';
import MessageBellButton from '@/features/chat/inbox/dropdown/message-dropdown/MessageBellButton';
import MessageDropdownPanel from '@/features/chat/inbox/dropdown/message-dropdown/MessageDropdownPanel';
import { useMessageDropdownState } from '@/features/chat/inbox/dropdown/message-dropdown/useMessageDropdownState';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';
import { cn } from '@/lib/utils';

interface MessageDropdownProps {
 enabled?: boolean;
}

export default function MessageDropdown({ enabled = true }: MessageDropdownProps) {
 const t = useTranslations('Chat');
 const tCommon = useTranslations('Common');
 const navigation = useOptimizedNavigation();
 const queryClient = useQueryClient();
 const { buttonRef, close, dropdownRef, isOpen, toggleOpen } = useMessageDropdownState();
 const { unreadCount } = useChatUnreadNotifications({ enabled });
 const {
  conversations,
  currentUserId,
  hasLoadError,
  isLoading,
  loadErrorMessage,
  refreshPreview,
 } = useMessageDropdown({ enabled, open: isOpen });

 useEffect(() => {
  if (!isOpen) {
   return;
  }

  void refreshPreview();
  void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.unreadBadge() });
 }, [isOpen, queryClient, refreshPreview]);

 return (
  <div className={cn('relative inline-flex items-center')}>
   <MessageBellButton
    ariaLabel={tCommon('messages')}
    buttonRef={buttonRef}
    isOpen={isOpen}
    unreadCount={unreadCount}
    onToggle={toggleOpen}
   />
   {isOpen ? (
    <MessageDropdownPanel
     conversations={conversations}
     currentUserId={currentUserId}
     dropdownRef={dropdownRef}
     hasLoadError={hasLoadError}
     isLoading={isLoading}
     loadErrorMessage={loadErrorMessage || t('dropdown.load_error')}
     title={t('dropdown.title')}
     viewAllLabel={t('dropdown.view_all')}
     t={t}
     onOpenConversation={(conversationId) => {
      close();
      navigation.push(`/chat/${conversationId}`);
     }}
     onViewAll={() => {
      close();
      navigation.push('/chat');
     }}
    />
   ) : null}
  </div>
 );
}
