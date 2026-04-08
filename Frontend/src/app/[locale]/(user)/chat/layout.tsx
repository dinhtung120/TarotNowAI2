'use client';

import { usePathname } from 'next/navigation';
import ConversationSidebar from '@/features/chat/presentation/components/ConversationSidebar';
import { CallProvider, IncomingCallOverlay, ActiveCallOverlay } from '@/features/chat/presentation/call';
import { cn } from '@/lib/utils';

export default function ChatLayout({ children }: { children: React.ReactNode }) {
  const pathname = usePathname();

  const isChatRoom = pathname.split('/').filter(Boolean).length > 2;

  return (
    <CallProvider>

      <div className={cn("flex", "h-full", "w-full", "overflow-hidden", "bg-black/40", "backdrop-blur-sm")}>

        <aside className={cn(
          isChatRoom ? "hidden md:flex" : "flex",
          "w-full md:w-80 lg:w-96",
          "shrink-0",
          "flex-col",
          "border-r",
          "border-white/10"
        )}>
          <ConversationSidebar />
        </aside>


        <main className={cn(
          isChatRoom ? "flex" : "hidden md:flex",
          "relative",
          "min-w-0",
          "flex-1",
          "flex-col"
        )}>
          {children}
        </main>
      </div>

      <IncomingCallOverlay />
      <ActiveCallOverlay />
    </CallProvider>
  );
}
