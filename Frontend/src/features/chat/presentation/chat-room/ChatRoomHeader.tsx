import { ArrowLeft } from "lucide-react";
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
          "flex items-center justify-between gap-3 border-b border-white/10 tn-chat-header-shell px-4 py-3 backdrop-blur-md",
        )}
      >
        <div className={cn("flex min-w-0 items-center gap-3")}>
          <button
            className={cn("rounded-lg p-2 tn-chat-load-more-btn tn-hide-md")}
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
          <ChatHeaderEscrowBadge
            escrowTotalFrozen={conversation?.escrowTotalFrozen}
          />
        </div>
      </div>

      {headerWarning && (
        <div
          className={cn(
            "border-b tn-chat-warning-banner px-4 py-2 text-xs",
          )}
        >
          {warningText}
        </div>
      )}
    </>
  );
}
