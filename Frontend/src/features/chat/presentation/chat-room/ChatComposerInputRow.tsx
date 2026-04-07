import { Image as ImageIcon, Loader2, Send } from 'lucide-react';
import type { ChatComposerInputRowProps } from '@/features/chat/presentation/chat-room/chatRoomUi.types';
import { cn } from '@/lib/utils';

export default function ChatComposerInputRow({
  conversationExists,
  imageInputRef,
  inputPlaceholder,
  inputRef,
  newMessage,
  sending,
  uploadingMedia,
  VoiceRecorderButton,
  onInputChange,
  onInputKeyDown,
  onSendTextMessage,
  onVoiceRecordingComplete,
}: ChatComposerInputRowProps) {
  return (
    <div className={cn('flex gap-2')}>
      <button
        type="button"
        onClick={() => imageInputRef.current?.click()}
        disabled={!conversationExists || uploadingMedia}
        className={cn('flex h-11 w-11 shrink-0 items-center justify-center rounded-xl tn-chat-icon-btn')}
        title="Gửi ảnh"
      >
        {uploadingMedia ? <Loader2 className={cn('h-4 w-4 animate-spin')} /> : <ImageIcon className={cn('h-4 w-4')} />}
      </button>

      <input
        ref={inputRef}
        value={newMessage}
        onChange={(event) => onInputChange(event.target.value)}
        onKeyDown={onInputKeyDown}
        disabled={sending || !conversationExists}
        placeholder={inputPlaceholder}
        className={cn('h-11 flex-1 rounded-xl tn-chat-input px-3 text-sm')}
      />

      <VoiceRecorderButton
        onRecordingComplete={(result) => void onVoiceRecordingComplete(result)}
        disabled={!conversationExists || uploadingMedia || sending}
      />

      <button
        type="button"
        onClick={() => void onSendTextMessage()}
        disabled={!newMessage.trim() || sending || !conversationExists}
        className={cn('flex h-11 w-11 shrink-0 items-center justify-center rounded-xl tn-chat-send-btn')}
      >
        {sending ? <Loader2 className={cn('h-4 w-4 animate-spin')} /> : <Send className={cn('h-4 w-4')} />}
      </button>
    </div>
  );
}
