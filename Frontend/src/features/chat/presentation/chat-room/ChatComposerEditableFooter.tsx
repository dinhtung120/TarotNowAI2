import ChatActionMenu from '@/features/chat/presentation/chat-room/ChatActionMenu';
import ChatComposerInputRow from '@/features/chat/presentation/chat-room/ChatComposerInputRow';
import type { ChatComposerProps } from '@/features/chat/presentation/chat-room/chatRoomUi.types';
import { cn } from '@/lib/utils';
type ChatComposerEditableFooterProps = Omit<ChatComposerProps, 'canShowInput' | 'canStartNewSession' | 'isReadOnly' | 'readOnlyHint' | 'startingNewSession' | 'onStartNewSession'>;

export default function ChatComposerEditableFooter({
  actionMenuRef,
  awaitingCompleteResponse,
  canUseActionMenu,
  conversationExists,
  imageInputRef,
  inputPlaceholder,
  inputRef,
  isUserRole,
  newMessage,
  processingAction,
  requestingAddMoney,
  sending,
  showActionMenu,
  uploadPercent,
  uploadingMedia,
  uploadingMediaLabel,
  VoiceRecorderButton,
  setShowActionMenu,
  onImageInputChange,
  onInputChange,
  onInputKeyDown,
  onOpenDispute,
  onOpenPaymentOffer,
  onRequestComplete,
  onSendTextMessage,
  onVoiceRecordingComplete,
}: ChatComposerEditableFooterProps) {
  return (
    <div className={cn('border-t border-white/10 bg-black/30 p-3')}>
      <div className={cn('flex gap-2')}>
        <ChatComposerInputRow
          conversationExists={conversationExists}
          imageInputRef={imageInputRef}
          inputPlaceholder={inputPlaceholder}
          inputRef={inputRef}
          newMessage={newMessage}
          sending={sending}
          uploadPercent={uploadPercent}
          uploadingMedia={uploadingMedia}
          uploadingMediaLabel={uploadingMediaLabel}
          VoiceRecorderButton={VoiceRecorderButton}
          onInputChange={onInputChange}
          onInputKeyDown={onInputKeyDown}
          onSendTextMessage={onSendTextMessage}
          onVoiceRecordingComplete={onVoiceRecordingComplete}
          onImageInputChange={onImageInputChange}
        />
        <ChatActionMenu
          actionMenuRef={actionMenuRef}
          awaitingCompleteResponse={awaitingCompleteResponse}
          canUseActionMenu={canUseActionMenu}
          isUserRole={isUserRole}
          processingAction={processingAction}
          requestingAddMoney={requestingAddMoney}
          showActionMenu={showActionMenu}
          setShowActionMenu={setShowActionMenu}
          onOpenDispute={onOpenDispute}
          onOpenPaymentOffer={onOpenPaymentOffer}
          onRequestComplete={onRequestComplete}
        />
      </div>
      <input ref={imageInputRef} type="file" accept="image/avif,image/webp,image/jpeg,image/png,image/gif" className={cn('hidden')} onChange={(event) => void onImageInputChange(event)} />
    </div>
  );
}
