import { ArrowLeft } from "lucide-react";
import { CallButton } from "@/features/chat/presentation/call";
import type { ChatRoomHeaderProps } from "@/features/chat/presentation/chat-room/chatRoomUi.types";
import ChatHeaderAvatar from "@/features/chat/presentation/chat-room/header/ChatHeaderAvatar";
import ChatHeaderEscrowBadge from "@/features/chat/presentation/chat-room/header/ChatHeaderEscrowBadge";
import ChatHeaderIdentity from "@/features/chat/presentation/chat-room/header/ChatHeaderIdentity";
import { cn } from "@/lib/utils";

export default function ChatRoomHeader({
  conversation,
  headerWarning,
  loading,
  otherAvatar,
  otherName,
  readerStatus,
  title,
  warningText,
  onBack,
}: ChatRoomHeaderProps) {
  return (
    <>
      <div
        className={cn(
          "flex items-center justify-between gap-3 border-b border-white/10 bg-[#0a0a0a]/40 px-4 py-3 backdrop-blur-md",
        )}
      >
        <div className={cn("flex min-w-0 items-center gap-3")}>
          <button
            className={cn("rounded-lg p-2 hover:bg-white/10 md:hidden")}
            type="button"
            onClick={onBack}
          >
            <ArrowLeft className={cn("h-4 w-4")} />
          </button>
          <ChatHeaderAvatar otherAvatar={otherAvatar} otherName={otherName} />
          <ChatHeaderIdentity
            hasConversation={Boolean(conversation)}
            loading={loading}
            otherName={otherName}
            readerStatus={readerStatus}
            title={title}
          />
        </div>

        <div className={cn("flex items-center gap-2")}>
          {conversation?.status === "ongoing" && (
            <CallButton conversationId={conversation.id} />
          )}
          <ChatHeaderEscrowBadge
            escrowTotalFrozen={conversation?.escrowTotalFrozen}
          />
        </div>
      </div>

      {headerWarning && (
        <div
          className={cn(
            "border-b border-[var(--warning)]/20 bg-[var(--warning)]/10 px-4 py-2 text-xs text-[var(--warning)]",
          )}
        >
          {warningText}
        </div>
      )}
    </>
  );
}
