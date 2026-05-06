import dynamic from 'next/dynamic';
import { Suspense } from 'react';
import LoadingSpinner from '@/app/_shared/ui/LoadingSpinner';

const ChatRoomPage = dynamic(
 () => import('@/features/chat/public').then((m) => m.ChatRoomPage),
);

export default function ChatRoomRoutePage() {
 return (
  <Suspense fallback={<LoadingSpinner fullPage message="Loading chat room" />}>
   <ChatRoomPage />
  </Suspense>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
