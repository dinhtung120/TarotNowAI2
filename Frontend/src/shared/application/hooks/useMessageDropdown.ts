'use client';

import { useQuery, useQueryClient } from '@tanstack/react-query';
import { fetchJsonOrThrow } from '@/shared/application/gateways/clientFetch';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';
import { useAuthStore } from '@/store/authStore';
import type { ConversationDto, ListConversationsResult } from '@/features/chat/application/actions';

interface UseMessageDropdownOptions {
 enabled?: boolean;
 open?: boolean;
}

interface MessageDropdownResult {
 conversations: ConversationDto[];
 currentUserId: string;
 hasLoadError: boolean;
 isLoading: boolean;
 loadErrorMessage: string;
 totalCount: number;
 refreshPreview: () => Promise<void>;
}

const EMPTY_RESULT: ListConversationsResult = {
 conversations: [],
 currentUserId: '',
 totalCount: 0,
};

export function useMessageDropdown(options: UseMessageDropdownOptions = {}): MessageDropdownResult {
 const enabled = options.enabled ?? true;
 const open = options.open ?? false;
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const queryClient = useQueryClient();
 const { data, isLoading, isError, error, refetch } = useQuery({
  queryKey: userStateQueryKeys.chat.inboxPreview(),
  enabled: isAuthenticated && enabled && open,
  queryFn: () =>
   fetchJsonOrThrow<ListConversationsResult>(
    '/api/chat/inbox-preview',
    {
     method: 'GET',
     credentials: 'include',
     cache: 'no-store',
    },
    'Failed to load chat inbox preview.',
    8_000,
   ),
  initialData: () => {
   const cached = queryClient.getQueryData<ListConversationsResult>(userStateQueryKeys.chat.inboxActive());
   if (!cached) return undefined;
   return {
    ...cached,
    conversations: cached.conversations.slice(0, 8),
   };
  },
  staleTime: 30_000,
  refetchOnWindowFocus: false,
  refetchOnReconnect: false,
  refetchOnMount: false,
 });

 const result = data ?? EMPTY_RESULT;
 const loadErrorMessage = error instanceof Error ? error.message : 'Failed to load chat inbox preview.';

 return {
  conversations: result.conversations,
  currentUserId: result.currentUserId,
  hasLoadError: isError,
  isLoading,
  loadErrorMessage,
  totalCount: result.totalCount,
  refreshPreview: async () => {
   await refetch();
  },
 };
}
