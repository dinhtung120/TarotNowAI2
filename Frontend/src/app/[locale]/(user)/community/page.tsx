

import { FeedPage } from '@/features/community/components/FeedPage';
import { cn } from '@/lib/utils';

export default function CommunityIndexPage() {
  return (
    <main className={cn("min-h-screen bg-zinc-950 text-white")}>
      <FeedPage />
    </main>
  );
}
