'use client';

import { useMutation, useQuery } from '@tanstack/react-query';
import { useParams } from 'next/navigation';
import toast from 'react-hot-toast';
import type { ReaderProfile } from '@/features/reader/shared';
import { createConversation } from '@/features/chat/public';
import { fetchJsonOrThrow } from '@/shared/gateways/clientFetch';
import { useOptimizedNavigation } from '@/shared/gateways/useOptimizedNavigation';
import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

export function useReaderPublicProfilePage(t: TranslateFn) {
  const params = useParams();
  const navigation = useOptimizedNavigation();
  const userId = params.id as string;

 const { data: profile, isLoading, isFetching } = useQuery<ReaderProfile | null>({
  queryKey: userStateQueryKeys.reader.profile(userId),
  queryFn: () => fetchJsonOrThrow<ReaderProfile>(
   `/api/readers/${encodeURIComponent(userId)}`,
   {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
   },
   'Failed to load reader profile.',
   8_000,
  ),
  enabled: Boolean(userId),
 });

 const startChatMutation = useMutation({
  mutationFn: async () => createConversation(userId),
  onSuccess: (result) => {
   if (result.success && result.data?.id) {
    navigation.push(`/chat/${result.data.id}`);
    return;
   }

   toast.error(result.error || t('profile.toast_create_conversation_fail'));
  },
  onError: () => {
   toast.error(t('profile.toast_create_conversation_fail'));
  },
 });

  return {
    navigation,
    profile,
  loading: isLoading || isFetching,
  startingChat: startChatMutation.isPending,
  startChat: () => startChatMutation.mutate(),
  };
}
