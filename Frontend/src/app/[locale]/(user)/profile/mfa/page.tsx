import { ProfileMfaPage } from '@/features/profile/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchProfileMfaPage } from '@/shared/server/prefetch/runners';

export default async function ProfileMfaRoutePage() {
 const state = await dehydrateAppQueries(prefetchProfileMfaPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ProfileMfaPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
