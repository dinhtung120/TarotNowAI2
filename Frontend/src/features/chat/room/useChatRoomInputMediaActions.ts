import { useCallback, useEffect, useRef, useState } from 'react';
import toast from 'react-hot-toast';
import type { ConversationDto, MediaPayloadDto } from '@/features/chat/shared/actions';
import type { VoiceRecordingResult } from '@/features/chat/room/components/useVoiceRecorder';
import {
  buildImageMediaPayload,
  buildVoiceMediaPayloadFromBlob,
  validateRawImageFile,
} from '@/features/chat/room/mediaPayload';

interface UseChatRoomInputMediaActionsParams {
  conversation: ConversationDto | null;
  notifyTyping: (isTyping: boolean) => void;
  sendMediaMessage: (type: 'image' | 'voice', payload: MediaPayloadDto) => Promise<boolean>;
  setNewMessage: (value: string) => void;
  handleSendTextMessage: () => Promise<boolean>;
}

export function useChatRoomInputMediaActions({
  conversation,
  notifyTyping,
  sendMediaMessage,
  setNewMessage,
  handleSendTextMessage,
}: UseChatRoomInputMediaActionsParams) {
  const [uploadingMedia, setUploadingMedia] = useState(false);
  const [uploadPercent, setUploadPercent] = useState(0);
  const scrollRef = useRef<HTMLDivElement | null>(null);
  const imageInputRef = useRef<HTMLInputElement | null>(null);
  const typingStopTimer = useRef<ReturnType<typeof setTimeout> | null>(null);

  useEffect(
    () => () => {
      if (typingStopTimer.current) {
        clearTimeout(typingStopTimer.current);
      }
    },
    [],
  );

  const onInputKeyDown = useCallback(
    (event: React.KeyboardEvent<HTMLInputElement>) => {
      if (event.key === 'Enter' && !event.shiftKey) {
        event.preventDefault();
        void handleSendTextMessage();
      }
    },
    [handleSendTextMessage],
  );

  const onInputChange = useCallback(
    (value: string) => {
      setNewMessage(value);
      notifyTyping(true);

      if (typingStopTimer.current) {
        clearTimeout(typingStopTimer.current);
      }

      typingStopTimer.current = setTimeout(() => notifyTyping(false), 1200);
    },
    [notifyTyping, setNewMessage],
  );

  const sendMedia = useCallback(
    async (type: 'image' | 'voice', payload: MediaPayloadDto, errorMessage: string) => {
      const success = await sendMediaMessage(type, payload);
      if (!success) toast.error(errorMessage);
    },
    [sendMediaMessage],
  );

  const onImageInputChange = useCallback(
    async (event: React.ChangeEvent<HTMLInputElement>) => {
      const file = event.target.files?.[0];
      event.target.value = '';

      if (!file || !conversation) return;

      try {
        validateRawImageFile(file);
        setUploadingMedia(true);
        setUploadPercent(0);
        const payload = await buildImageMediaPayload({
          conversationId: conversation.id,
          file,
          onProgress: setUploadPercent,
        });
        await sendMedia('image', payload, 'Không thể gửi ảnh.');
      } catch (error) {
        toast.error(error instanceof Error ? error.message : 'Không thể gửi ảnh.');
      } finally {
        setUploadingMedia(false);
        setUploadPercent(0);
      }
    },
    [conversation, sendMedia],
  );

  const onVoiceRecordingComplete = useCallback(
    async (result: VoiceRecordingResult) => {
      if (!conversation) return;

      try {
        setUploadingMedia(true);
        setUploadPercent(0);
        const payload = await buildVoiceMediaPayloadFromBlob({
          blob: result.blob,
          conversationId: conversation.id,
          durationMs: result.durationMs,
          onProgress: setUploadPercent,
        });
        await sendMedia('voice', payload, 'Không thể gửi tin nhắn thoại.');
      } catch (error) {
        toast.error(error instanceof Error ? error.message : 'Không thể gửi tin nhắn thoại.');
      } finally {
        setUploadingMedia(false);
        setUploadPercent(0);
      }
    },
    [conversation, sendMedia],
  );

  return {
    imageInputRef,
    onImageInputChange,
    onInputChange,
    onInputKeyDown,
    onVoiceRecordingComplete,
    scrollRef,
    uploadPercent,
    uploadingMedia,
  };
}
