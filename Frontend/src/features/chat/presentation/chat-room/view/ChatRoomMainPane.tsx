import type { ChatRoomViewProps } from "@/features/chat/presentation/chat-room/ChatRoomView.types";
import ChatRoomComposerSection from "@/features/chat/presentation/chat-room/view/ChatRoomComposerSection";
import ChatRoomHeaderSection from "@/features/chat/presentation/chat-room/view/ChatRoomHeaderSection";
import ChatRoomMessagesSection from "@/features/chat/presentation/chat-room/view/ChatRoomMessagesSection";
import { GlassCard } from "@/shared/components/ui";
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
