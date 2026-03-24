'use client';

import { useEffect, useState } from 'react';
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

  const [profile, setProfile] = useState<ReaderProfile | null>(null);
  const [loading, setLoading] = useState(true);
  const [startingChat, setStartingChat] = useState(false);

  useEffect(() => {
    if (!userId) return;

    const fetchProfile = async () => {
      const result = await getReaderProfile(userId);
      setProfile(result.success ? result.data ?? null : null);
      setLoading(false);
    };

    void fetchProfile();
  }, [userId]);

  const startConversation = async () => {
    if (!profile) return;

    setStartingChat(true);
    const result = await createConversation(profile.userId);
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
    setStartingChat(false);
  };

  return {
    router,
    profile,
    loading,
    startingChat,
    startConversation,
  };
}
