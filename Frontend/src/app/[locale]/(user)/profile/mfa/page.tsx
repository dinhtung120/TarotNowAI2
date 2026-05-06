import { ProfileMfaPage } from '@/features/profile/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchProfileMfaPage } from '@/app/_shared/server/prefetch/runners';

export default async function ProfileMfaRoutePage() {
 const state = await dehydrateAppQueries(prefetchProfileMfaPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ProfileMfaPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
