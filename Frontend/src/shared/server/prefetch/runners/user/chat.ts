import type { QueryClient } from '@tanstack/react-query';
import { listConversations } from '@/features/chat/application/actions';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { CHAT_INBOX_PREFETCH_STALE_MS } from '@/shared/server/prefetch/runners/user/shared';

async function chatInboxActiveQueryFn() {
 const result = await listConversations('active', 1, 100);
 if (result.success && result.data) {
  return result.data;
 }
 return {
  conversations: [],
  currentUserId: '',
  totalCount: 0,
 };
}

export async function prefetchChatInboxShell(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.chat.inboxActive(),
  queryFn: chatInboxActiveQueryFn,
  staleTime: CHAT_INBOX_PREFETCH_STALE_MS,
 });
}
