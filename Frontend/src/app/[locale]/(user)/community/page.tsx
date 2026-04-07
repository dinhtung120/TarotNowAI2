

import React from 'react';
import { FeedPage } from '@/features/community/components/FeedPage';
import { cn } from '@/lib/utils';

export default function CommunityIndexPage() {
  return (
    <main className={cn("min-h-screen bg-[#0f0f16] text-white")}>
      <FeedPage />
    </main>
  );
}
