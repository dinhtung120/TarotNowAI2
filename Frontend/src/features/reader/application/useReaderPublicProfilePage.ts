'use client';

import { useMutation, useQuery } from '@tanstack/react-query';
import { useParams } from 'next/navigation';
import { useRouter } from '@/i18n/routing';
import toast from 'react-hot-toast';
import { createConversation } from '@/features/chat/public';
import { getReaderProfile, type ReaderProfile } from '@/features/reader/application/actions';

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

 const createConversationMutation = useMutation({
  mutationFn: async (readerId: string) => createConversation(readerId),
 });

  const startConversation = async () => {
    if (!profile) return;

    const result = await createConversationMutation.mutateAsync(profile.userId);
    if (result.success && result.data?.id) {
      router.push(`/chat/${result.data.id}`);
      return;
    }

    toast.error(t('profile.toast_create_conversation_fail'), {
      style: {
        background: 'var(--danger-bg)',
        color: 'var(--danger)',
        border: '1px solid var(--danger)',
      },
    });
  };

  return {
    router,
    profile,
  loading: isLoading || isFetching,
  startingChat: createConversationMutation.isPending,
    startConversation,
  };
}
