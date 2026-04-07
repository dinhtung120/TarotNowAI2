'use client';

import type { UserMetadataDto } from '@/shared/application/actions/metadata';
import { useMetadataInitialLoader } from '@/shared/components/common/hooks/useMetadataInitialLoader';

interface MetadataInitialLoaderProps {
  initialMetadata?: UserMetadataDto | null;
}

export default function MetadataInitialLoader({ initialMetadata }: MetadataInitialLoaderProps) {
  useMetadataInitialLoader({ initialMetadata });

  return null;
}
