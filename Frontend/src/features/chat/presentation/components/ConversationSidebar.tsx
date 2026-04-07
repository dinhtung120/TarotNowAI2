'use client';

import { useParams } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { Loader2, MessageSquareText } from 'lucide-react';
import { useChatInboxPage } from '@/features/chat/application/useChatInboxPage';
import { cn } from '@/lib/utils';
import { ConversationListItem } from '@/features/chat/presentation/components/conversation-sidebar/ConversationListItem';
import { ConversationSidebarTabs } from '@/features/chat/presentation/components/conversation-sidebar/ConversationSidebarTabs';

export default function ConversationSidebar() {
 const t = useTranslations('Chat');
 const params = useParams();
 const currentId = params.id as string | undefined;
 const { tab, setTab, conversations, loading, currentUserId, nowTs } = useChatInboxPage('active');

 return (
  <div className={cn('flex flex-col h-full overflow-hidden gap-3 tn-p-3-4-md')}>
   <ConversationSidebarTabs tab={tab} setTab={setTab} />
   <div className={cn('flex-1 overflow-y-auto space-y-2 pr-1 custom-scrollbar')}>
    {loading ? (
     <div className={cn('h-32 flex items-center justify-center')}>
      <Loader2 className={cn('w-5 h-5 animate-spin tn-text-secondary')} />
     </div>
    ) : null}
    {!loading && conversations.length === 0 ? (
     <div className={cn('h-40 flex flex-col items-center justify-center gap-2 tn-text-secondary text-center px-4')}>
      <MessageSquareText className={cn('w-6 h-6 opacity-50')} />
      <p className={cn('text-sm')}>{t('inbox.empty_title')}</p>
     </div>
    ) : null}
    {conversations.map((conversation) => (
     <ConversationListItem
      key={conversation.id}
      conversation={conversation}
      currentId={currentId}
      currentUserId={currentUserId}
      nowTs={nowTs}
     />
    ))}
   </div>
  </div>
 );
}
