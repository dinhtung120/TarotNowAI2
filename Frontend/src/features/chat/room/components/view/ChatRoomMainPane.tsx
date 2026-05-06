import type { ChatRoomViewProps } from "@/features/chat/room/ChatRoomView.types";
import ChatRoomComposerSection from "@/features/chat/room/components/view/ChatRoomComposerSection";
import ChatRoomHeaderSection from "@/features/chat/room/components/view/ChatRoomHeaderSection";
import ChatRoomMessagesSection from "@/features/chat/room/components/view/ChatRoomMessagesSection";
import { GlassCard } from "@/shared/ui";
import { cn } from "@/lib/utils";

export default function ChatRoomMainPane(props: ChatRoomViewProps) {
  return (
    <GlassCard
      className={cn(
        "relative flex h-full flex-col overflow-hidden !rounded-none !border-0 !border-l border-white/5 !bg-transparent !p-0",
      )}
    >
      <ChatRoomHeaderSection {...props} />
      <ChatRoomMessagesSection {...props} />
      <ChatRoomComposerSection {...props} />
    </GlassCard>
  );
}
