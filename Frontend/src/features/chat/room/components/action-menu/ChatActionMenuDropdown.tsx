import { Coins, Loader2 } from "lucide-react";
import ChatActionMenuItemButton from "@/features/chat/room/components/action-menu/ChatActionMenuItemButton";
import { cn } from "@/lib/utils";

interface ChatActionMenuDropdownProps {
  awaitingCompleteResponse: boolean;
  isUserRole: boolean | null;
  requestingAddMoney: boolean;
  onClose: () => void;
  onOpenDispute: () => void;
  onOpenPaymentOffer: () => void;
  onRequestComplete: () => Promise<void>;
}

export default function ChatActionMenuDropdown({
  awaitingCompleteResponse,
  isUserRole,
  requestingAddMoney,
  onClose,
  onOpenDispute,
  onOpenPaymentOffer,
  onRequestComplete,
}: ChatActionMenuDropdownProps) {
  const handleRequestComplete = () => {
    onClose();
    void onRequestComplete();
  };

  return (
    <div
      className={cn(
        "absolute right-0 bottom-12 z-20 w-52 space-y-1 rounded-xl border border-white/10 tn-chat-action-menu-shell p-1 shadow-xl",
      )}
    >
      {!awaitingCompleteResponse && (
        <ChatActionMenuItemButton onClick={handleRequestComplete}>
          Hoàn thành cuộc trò chuyện
        </ChatActionMenuItemButton>
      )}

      <ChatActionMenuItemButton
        className={cn("tn-text-danger")}
        onClick={() => {
          onClose();
          onOpenDispute();
        }}
      >
        Tố cáo
      </ChatActionMenuItemButton>

      {isUserRole === false && (
        <ChatActionMenuItemButton
          className={cn("flex items-center gap-2 tn-text-warning")}
          onClick={() => {
            onClose();
            onOpenPaymentOffer();
          }}
        >
          {requestingAddMoney ? (
            <Loader2 className={cn("h-3 w-3 animate-spin")} />
          ) : (
            <Coins className={cn("h-3 w-3")} />
          )}
          Yêu cầu cộng tiền
        </ChatActionMenuItemButton>
      )}
    </div>
  );
}
