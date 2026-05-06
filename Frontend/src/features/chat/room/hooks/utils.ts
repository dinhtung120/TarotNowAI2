'use client';

import type { QueryClient } from '@tanstack/react-query';
import type { ILogger, LogLevel } from '@microsoft/signalr';
import type { ConversationDto } from '@/features/chat/shared/actions';

export function getCachedConversation(
 queryClient: QueryClient,
 id?: string | null
): ConversationDto | null {
 if (!id) return null;
 const active = queryClient.getQueryData<{ conversations: ConversationDto[] }>([
  'chat',
  'inbox',
  'active',
 ]);
 const activeMatch = active?.conversations?.find((item) => item.id === id);
 if (activeMatch) return activeMatch;

 const pending = queryClient.getQueryData<{ conversations: ConversationDto[] }>([
  'chat',
  'inbox',
  'pending',
 ]);
 return pending?.conversations?.find((item) => item.id === id) ?? null;
}

export function createSignalRLogger(logLevelMap: {
 error: LogLevel;
 critical: LogLevel;
 warning: LogLevel;
 information: LogLevel;
}): ILogger {
 return {
  log: (level: LogLevel, message: string) => {
   if (level === logLevelMap.error || level === logLevelMap.critical) {
    console.warn(`[SignalR Error] ${message}`);
    return;
   }
   if (level === logLevelMap.warning) {
    console.warn(`[SignalR Warn] ${message}`);
    return;
   }
   if (level === logLevelMap.information) {
    console.info(`[SignalR Info] ${message}`);
    return;
   }
   console.debug(`[SignalR Debug] ${message}`);
  },
 };
}
