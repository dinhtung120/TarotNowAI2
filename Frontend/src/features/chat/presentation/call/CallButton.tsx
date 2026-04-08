'use client';

import React from 'react';
import { useTranslations } from 'next-intl';
import toast from 'react-hot-toast';
import { useCallStore } from '../../application/call/useCallStore';
import { useCallContext } from './CallProvider';
import { cn } from '@/lib/utils';
import { logger } from '@/shared/infrastructure/logging/logger';
import { CallApiError } from '../../application/call/callApi';

interface CallButtonProps {
  conversationId: string;
}

export const CallButton = ({ conversationId }: CallButtonProps) => {
  const { connected, initiateCall } = useCallContext();
  const { uiState } = useCallStore();
  const t = useTranslations('Chat.call');

  const handleStartAudio = async () => {
    if (!conversationId || typeof conversationId !== 'string') return;
    if (!connected) {
      toast.error('Mất kết nối thời gian thực (SignalR). Vui lòng kiểm tra chứng chỉ SSL.');
      return;
    }
    if (uiState !== 'idle' && uiState !== 'ended' && uiState !== 'failed') return;
    try {
      await initiateCall(conversationId, 'audio');
    } catch (error) {
      logger.warn('Call.UI', 'Unable to start audio call.', { error });
      toast.error(resolveStartCallErrorMessage(error));
      useCallStore.getState().reset();
    }
  };

  const handleStartVideo = async () => {
    if (!conversationId || typeof conversationId !== 'string') return;
    if (!connected) {
      toast.error('Mất kết nối thời gian thực (SignalR). Vui lòng kiểm tra chứng chỉ SSL.');
      return;
    }
    if (uiState !== 'idle' && uiState !== 'ended' && uiState !== 'failed') return;
    try {
      await initiateCall(conversationId, 'video');
    } catch (error) {
      logger.warn('Call.UI', 'Unable to start video call.', { error });
      toast.error(resolveStartCallErrorMessage(error));
      useCallStore.getState().reset();
    }
  };

  return (
    <div className={cn("flex items-center gap-2")}>
      <button 
        type="button"
        onClick={handleStartAudio} 
        className={cn("p-2 rounded-full tn-call-btn", !connected && "opacity-50")}
        title={t('start_audio')}
      >
        <svg className={cn("w-5 h-5")} fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" />
        </svg>
      </button>

      <button 
        type="button"
        onClick={handleStartVideo}
        className={cn("p-2 rounded-full tn-call-btn", !connected && "opacity-50")}
        title={t('start_video')}
      >
        <svg className={cn("w-5 h-5")} fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 10l4.553-2.276A1 1 0 0121 8.618v6.764a1 1 0 01-1.447.894L15 14M5 18h8a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v8a2 2 0 002 2z" />
        </svg>
      </button>
    </div>
  );
};

function resolveStartCallErrorMessage(error: unknown): string {
  if (error instanceof CallApiError) {
    return error.message;
  }

  if (error instanceof Error && error.message) {
    return error.message;
  }

  return 'Không thể bắt đầu cuộc gọi. Vui lòng thử lại.';
}
