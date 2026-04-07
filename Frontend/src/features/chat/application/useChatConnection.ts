'use client';

import { useMemo, useRef, useState, useCallback, useEffect } from 'react';
import type { HubConnection } from '@microsoft/signalr';
import { useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/store/authStore';
import type { ConversationDto } from '@/features/chat/application/actions';
import { useChatHistoryState } from '@/features/chat/application/chat-connection/useChatHistoryState';
import { useChatSendActions } from '@/features/chat/application/chat-connection/useChatSendActions';
import { useChatSignalRLifecycle } from '@/features/chat/application/chat-connection/useChatSignalRLifecycle';

interface UseChatConnectionOptions {
 conversationId?: string | null;
}

export function useChatConnection({ conversationId }: UseChatConnectionOptions) {
 const authStore = useAuthStore();
 const queryClient = useQueryClient();
 const connectionRef = useRef<HubConnection | null>(null);
 const typingTimeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);
 const messagesEndRef = useRef<HTMLDivElement>(null);
 const inputRef = useRef<HTMLInputElement>(null);
 const scrollToBottomRef = useRef<(behavior?: ScrollBehavior) => void>(() => {});
 const [connected, setConnected] = useState(false);
 const [typingUserId, setTypingUserId] = useState<string | null>(null);

 const scrollToBottom = useCallback((behavior: ScrollBehavior = 'smooth') => {
  messagesEndRef.current?.scrollIntoView({ behavior });
 }, []);
 useEffect(() => {
  scrollToBottomRef.current = scrollToBottom;
 }, [scrollToBottom]);

 const {
  messages,
  setMessages,
  conversation,
  setConversation,
  hasMore,
  loading,
  loadingMore,
  initializing,
  setLoading,
  loadInitial,
  loadInitialRef,
  loadMore,
  resetForConversation,
  appendMessage,
  lastInitialLoadTimeRef,
 } = useChatHistoryState({ conversationId, scrollToBottomRef });

 const {
  newMessage,
  setNewMessage,
  sending,
  sendTypedMessage,
  sendMediaMessage,
  handleSendTextMessage,
  notifyTyping,
  markRead,
  markReadRef,
 } = useChatSendActions({
  conversationId,
  connected,
  connectionRef,
  inputRef,
  setMessages,
 });

 useChatSignalRLifecycle({
  conversationId,
  token: authStore.token,
  currentUserId: authStore.user?.id ?? '',
  queryClient,
  connectionRef,
  typingTimeoutRef,
  lastInitialLoadTimeRef,
  loadInitialRef,
  markReadRef,
  setConnected,
  setLoading,
  setTypingUserId,
  setMessages,
  setConversation,
  resetForConversation,
  appendMessage,
 });

 const currentUserId = authStore.user?.id ?? '';
 const isUserRole = useMemo(() => {
  if (!conversation || !currentUserId) return null;
  return conversation.userId === currentUserId;
 }, [conversation, currentUserId]);
 const otherName = useMemo(() => {
  if (!conversation || isUserRole === null) return '';
  return isUserRole ? conversation.readerName ?? '' : conversation.userName ?? '';
 }, [conversation, isUserRole]);
 const otherAvatar = useMemo(() => {
  if (!conversation || isUserRole === null) return null;
  return isUserRole ? conversation.readerAvatar ?? null : conversation.userAvatar ?? null;
 }, [conversation, isUserRole]);
 const remoteTyping = typingUserId !== null && typingUserId !== currentUserId;

 return {
  messages,
  setMessages,
  newMessage,
  setNewMessage,
  loading,
  loadingMore,
  hasMore,
  loadMore,
  sending,
  connected,
  currentUserId,
  conversation: conversation as ConversationDto | null,
  otherName,
  otherAvatar,
  isUserRole,
  remoteTyping,
  messagesEndRef,
  inputRef,
  sendTypedMessage,
  sendMediaMessage,
  handleSendTextMessage,
  notifyTyping,
  markRead,
  initializing,
  loadInitial,
 };
}
