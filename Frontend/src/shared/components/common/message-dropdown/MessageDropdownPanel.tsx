import type { useTranslations } from 'next-intl';
import type { ConversationDto } from '@/features/chat/application/actions';
import MessageDropdownContent from '@/shared/components/common/message-dropdown/MessageDropdownContent';
import { cn } from '@/lib/utils';

interface MessageDropdownPanelProps {
 conversations: ConversationDto[];
 currentUserId: string;
 dropdownRef: React.RefObject<HTMLDivElement | null>;
 hasLoadError: boolean;
 isLoading: boolean;
 loadErrorMessage: string;
 title: string;
 viewAllLabel: string;
 t: ReturnType<typeof useTranslations>;
 onOpenConversation: (conversationId: string) => void;
 onViewAll: () => void;
}

export default function MessageDropdownPanel({
 conversations,
 currentUserId,
 dropdownRef,
 hasLoadError,
 isLoading,
 loadErrorMessage,
 title,
 viewAllLabel,
 t,
 onOpenConversation,
 onViewAll,
}: MessageDropdownPanelProps) {
 return (
  <div ref={dropdownRef} className={cn('tn-notification-panel animate-in slide-in-from-top-2 fade-in duration-200')}>
   <div className={cn('flex items-center justify-between border-b tn-border-soft tn-bg-glass px-4 py-3')}>
    <h3 className={cn('text-sm font-black tracking-tight tn-text-ink')}>{title}</h3>
   </div>
   <div className={cn('tn-notification-panel-content')}>
    <MessageDropdownContent
     conversations={conversations}
     currentUserId={currentUserId}
     hasLoadError={hasLoadError}
     isLoading={isLoading}
     loadErrorMessage={loadErrorMessage}
     t={t}
     onOpenConversation={onOpenConversation}
    />
   </div>
   <button
    type="button"
    onClick={onViewAll}
    className={cn('flex w-full items-center justify-center border-t tn-border-soft px-4 py-2.5 text-xs font-bold tn-text-accent-soft transition-colors tn-hover-text-accent tn-hover-surface-soft')}
   >
    {viewAllLabel}
   </button>
  </div>
 );
}
