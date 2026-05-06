import { MoreVertical } from "lucide-react";
import type { ChatActionMenuProps } from "@/features/chat/room/chatRoomUi.types";
import ChatActionMenuDropdown from "@/features/chat/room/components/action-menu/ChatActionMenuDropdown";
import { cn } from "@/lib/utils";

export default function ChatActionMenu({
  actionMenuRef,
  awaitingCompleteResponse,
  canUseActionMenu,
  isUserRole,
  processingAction,
  requestingAddMoney,
  showActionMenu,
  setShowActionMenu,
  onOpenDispute,
  onOpenPaymentOffer,
  onRequestComplete,
}: ChatActionMenuProps) {
  if (!canUseActionMenu) {
    return null;
  }

  return (
    <div ref={actionMenuRef} className={cn("relative")}>
      <button
        className={cn(
          "flex h-11 w-11 items-center justify-center rounded-xl border border-white/10 bg-white/5 text-[var(--text-secondary)] hover:text-white disabled:opacity-50",
        )}
        disabled={processingAction !== null || requestingAddMoney}
        title="Thao tác cuộc trò chuyện"
        type="button"
        onClick={() => setShowActionMenu(!showActionMenu)}
      >
        <MoreVertical className={cn("h-4 w-4")} />
      </button>

      {showActionMenu && (
        <ChatActionMenuDropdown
          awaitingCompleteResponse={awaitingCompleteResponse}
          isUserRole={isUserRole}
          requestingAddMoney={requestingAddMoney}
          onClose={() => setShowActionMenu(false)}
          onOpenDispute={onOpenDispute}
          onOpenPaymentOffer={onOpenPaymentOffer}
          onRequestComplete={onRequestComplete}
        />
      )}
    </div>
  );
}
