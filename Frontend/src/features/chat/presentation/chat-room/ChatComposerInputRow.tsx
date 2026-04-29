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
  uploadingMediaLabel,
  VoiceRecorderButton,
  onInputChange,
  onInputKeyDown,
  onSendTextMessage,
  onVoiceRecordingComplete,
}: ChatComposerInputRowProps) {
  return (
    <div className={cn('relative flex gap-2')}>
      <button
        type="button"
        onClick={() => imageInputRef.current?.click()}
        disabled={!conversationExists || uploadingMedia}
        className={cn('flex h-11 w-11 shrink-0 items-center justify-center rounded-xl tn-chat-icon-btn')}
        title="Gửi ảnh"
        aria-label="Gửi ảnh"
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
        disabled={!newMessage.trim() || sending || uploadingMedia || !conversationExists}
        className={cn('flex h-11 w-11 shrink-0 items-center justify-center rounded-xl tn-chat-send-btn')}
        aria-label="Gửi tin nhắn"
      >
        {sending ? <Loader2 className={cn('h-4 w-4 animate-spin')} /> : <Send className={cn('h-4 w-4')} />}
      </button>
      {uploadingMedia ? <span className={cn('absolute -bottom-5 left-1 text-[11px] text-slate-400')}>{uploadingMediaLabel}</span> : null}
    </div>
  );
}
