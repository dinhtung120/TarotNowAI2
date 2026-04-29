import { ProfilePage } from '@/features/profile/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchProfilePage } from '@/shared/server/prefetch/runners';

export default async function ProfileRoutePage() {
 const state = await dehydrateAppQueries(prefetchProfilePage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ProfilePage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
