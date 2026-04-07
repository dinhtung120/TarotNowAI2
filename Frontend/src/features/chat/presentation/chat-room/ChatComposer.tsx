import ChatComposerEditableFooter from '@/features/chat/presentation/chat-room/ChatComposerEditableFooter';
import ChatReadOnlyFooter from '@/features/chat/presentation/chat-room/ChatReadOnlyFooter';
import type { ChatComposerProps } from '@/features/chat/presentation/chat-room/chatRoomUi.types';

export default function ChatComposer({
  canShowInput,
  canStartNewSession,
  isReadOnly,
  readOnlyHint,
  startingNewSession,
  onStartNewSession,
  ...editableProps
}: ChatComposerProps) {
  if (!canShowInput) {
    return (
      <ChatReadOnlyFooter
        canStartNewSession={canStartNewSession}
        hint={readOnlyHint}
        isReadOnly={isReadOnly}
        startingNewSession={startingNewSession}
        onStartNewSession={onStartNewSession}
      />
    );
  }

  return <ChatComposerEditableFooter {...editableProps} />;
}
