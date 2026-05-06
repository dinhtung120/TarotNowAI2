import type { QueryClient } from '@tanstack/react-query';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';

const INVALIDATE_CHAT_DELAY_MS = 1_000;

export interface PresenceChatInvalidationSchedulers {
 invalidateInbox: () => void;
 invalidateUnreadBadge: () => void;
 dispose: () => void;
}

export function createPresenceChatInvalidationSchedulers(
 queryClient: QueryClient,
): PresenceChatInvalidationSchedulers {
 let inboxInvalidateTimeout: NodeJS.Timeout | null = null;
 let unreadInvalidateTimeout: NodeJS.Timeout | null = null;

 return {
  invalidateInbox: () => {
   if (inboxInvalidateTimeout) {
    clearTimeout(inboxInvalidateTimeout);
   }

   inboxInvalidateTimeout = setTimeout(() => {
    void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.inboxRoot() });
   }, INVALIDATE_CHAT_DELAY_MS);
  },
  invalidateUnreadBadge: () => {
   if (unreadInvalidateTimeout) {
    clearTimeout(unreadInvalidateTimeout);
   }

   unreadInvalidateTimeout = setTimeout(() => {
    void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.unreadBadge() });
   }, INVALIDATE_CHAT_DELAY_MS);
  },
  dispose: () => {
   if (inboxInvalidateTimeout) {
    clearTimeout(inboxInvalidateTimeout);
   }
   if (unreadInvalidateTimeout) {
    clearTimeout(unreadInvalidateTimeout);
   }
  },
 };
}
