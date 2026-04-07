import type { ChatRoomViewProps } from "@/features/chat/presentation/chat-room/ChatRoomView.types";
import ChatRoomMainPane from "@/features/chat/presentation/chat-room/view/ChatRoomMainPane";
import ChatRoomOverlays from "@/features/chat/presentation/chat-room/view/ChatRoomOverlays";
import { cn } from "@/lib/utils";

export default function ChatRoomView(props: ChatRoomViewProps) {
  return (
    <div className={cn("flex h-full min-h-0 flex-1 flex-col")}>
      <ChatRoomMainPane {...props} />
      <ChatRoomOverlays {...props} />
    </div>
  );
}
