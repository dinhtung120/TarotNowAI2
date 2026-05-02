'use client';

import { useQuery } from '@tanstack/react-query';
import { useTranslations } from 'next-intl';
import type { ReaderProfile } from '@/features/reader/public';
import FeaturedReaderCard from '@/features/home/presentation/components/featured-readers/FeaturedReaderCard';
import FeaturedReadersFallback from '@/features/home/presentation/components/featured-readers/FeaturedReadersFallback';
import { fetchJsonOrThrow } from '@/shared/application/gateways/clientFetch';
import { useRuntimePolicies } from '@/shared/application/hooks/useRuntimePolicies';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';
import { cn } from '@/lib/utils';

interface FeaturedReadersResponse {
  readers: ReaderProfile[];
  totalCount: number;
}

export default function FeaturedReadersGrid() {
  const t = useTranslations('Index');
  const runtimePoliciesQuery = useRuntimePolicies();
  const featuredLimit = runtimePoliciesQuery.data?.ui.readers.featuredLimit
    ?? RUNTIME_POLICY_FALLBACKS.ui.readers.featuredLimit;
  const directoryStaleMs = runtimePoliciesQuery.data?.ui.readers.directoryStaleMs
    ?? RUNTIME_POLICY_FALLBACKS.ui.readers.directoryStaleMs;
  const clientTimeoutMs = runtimePoliciesQuery.data?.http.clientTimeoutMs
    ?? RUNTIME_POLICY_FALLBACKS.http.clientTimeoutMs;

  const featuredReadersQuery = useQuery({
    queryKey: ['readers', 'home-featured', featuredLimit],
    queryFn: ({ signal }) => fetchJsonOrThrow<FeaturedReadersResponse>(
      `/api/readers?page=1&pageSize=${featuredLimit}`,
      {
        method: 'GET',
        credentials: 'include',
        cache: 'no-store',
        signal,
      },
      'Failed to load featured readers.',
      clientTimeoutMs,
    ),
    staleTime: directoryStaleMs,
    refetchOnWindowFocus: false,
    refetchOnReconnect: true,
  });

  const readers = featuredReadersQuery.data?.readers ?? [];
  if (readers.length === 0 && featuredReadersQuery.isPending) {
    return <FeaturedReadersFallback />;
  }

  if (readers.length === 0) {
    return <FeaturedReadersFallback />;
  }

  return (
    <div className={cn('tn-grid-1-2-4-responsive gap-8')}>
      {readers.map((reader) => (
        <FeaturedReaderCard
          key={reader.userId}
          reader={reader}
          profileCta={t('showcase.profileCta')}
          experienceSuffix={t('showcase.experienceSuffix')}
        />
      ))}
    </div>
  );
}
