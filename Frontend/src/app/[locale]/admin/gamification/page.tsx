import dynamic from 'next/dynamic';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminGamificationPage } from '@/app/_shared/server/prefetch/runners';

const AdminGamificationClient = dynamic(() =>
 import('@/features/gamification/public').then((m) => m.AdminGamificationClient)
);

export async function generateMetadata() {
  return {
    title: 'Quản Trị Gamification - TarotNow Admin',
  };
}

export default async function AdminGamificationPage() {
  const state = await dehydrateAppQueries(prefetchAdminGamificationPage);

  return (
    <AppQueryHydrationBoundary state={state}>
      <AdminGamificationClient />
    </AppQueryHydrationBoundary>
  );
}
