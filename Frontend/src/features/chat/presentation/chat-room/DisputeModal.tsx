import type { DisputeModalProps } from "@/features/chat/presentation/chat-room/chatRoomUi.types";
import DisputeModalActions from "@/features/chat/presentation/chat-room/modals/DisputeModalActions";
import Modal from "@/shared/components/ui/Modal";
import { cn } from "@/lib/utils";

export default function DisputeModal({
  disputeReason,
  isOpen,
  processingAction,
  setDisputeReason,
  onClose,
  onSubmit,
}: DisputeModalProps) {
  return (
    <Modal
      isOpen={isOpen}
      size="sm"
      title="Mở tố cáo cuộc trò chuyện"
      onClose={onClose}
    >
      <div className={cn("space-y-4")}>
        <textarea
          className={cn(
            "w-full rounded-xl border border-white/10 bg-white/5 px-3 py-2 text-sm text-white",
          )}
          placeholder="Nhập lý do tố cáo"
          rows={4}
          value={disputeReason}
          onChange={(event) => setDisputeReason(event.target.value)}
        />
        <DisputeModalActions
          canSubmit={Boolean(disputeReason.trim())}
          isSubmitting={processingAction === "open-dispute"}
          onClose={onClose}
          onSubmit={onSubmit}
        />
      </div>
    </Modal>
  );
}
