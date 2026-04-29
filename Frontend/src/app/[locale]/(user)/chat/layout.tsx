import { ChatSegmentShell } from '@/features/chat/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchChatInboxShell } from '@/shared/server/prefetch/runners';

export default async function ChatSegmentLayout({ children }: { children: React.ReactNode }) {
 const state = await dehydrateAppQueries(prefetchChatInboxShell);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ChatSegmentShell>{children}</ChatSegmentShell>
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
