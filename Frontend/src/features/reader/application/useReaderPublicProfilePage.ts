'use client';

import { useMutation, useQuery } from '@tanstack/react-query';
import { useParams } from 'next/navigation';
import { useRouter } from '@/i18n/routing';
import toast from 'react-hot-toast';
import { getReaderProfile, type ReaderProfile } from '@/features/reader/application/actions';
import { createConversation } from '@/features/chat/public';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

export function useReaderPublicProfilePage(t: TranslateFn) {
  const params = useParams();
  const router = useRouter();
  const userId = params.id as string;

 const { data: profile, isLoading, isFetching } = useQuery<ReaderProfile | null>({
  queryKey: ['reader-profile', userId],
  queryFn: async () => {
   const result = await getReaderProfile(userId);
   return result.success ? result.data ?? null : null;
  },
 enabled: Boolean(userId),
 });

 const startChatMutation = useMutation({
  mutationFn: async () => createConversation(userId),
  onSuccess: (result) => {
   if (result.success && result.data?.id) {
    router.push(`/chat/${result.data.id}`);
    return;
   }

   toast.error(result.error || t('profile.toast_create_conversation_fail'));
  },
  onError: () => {
   toast.error(t('profile.toast_create_conversation_fail'));
  },
 });

  return {
    router,
    profile,
  loading: isLoading || isFetching,
  startingChat: startChatMutation.isPending,
  startChat: () => startChatMutation.mutate(),
  };
}
