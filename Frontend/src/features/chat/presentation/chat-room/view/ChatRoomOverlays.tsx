import DisputeModal from "@/features/chat/presentation/chat-room/DisputeModal";
import type { ChatRoomViewProps } from "@/features/chat/presentation/chat-room/ChatRoomView.types";

export default function ChatRoomOverlays(props: ChatRoomViewProps) {
  return (
    <>
      <DisputeModal
        disputeReason={props.disputeReason}
        isOpen={props.showDisputeModal}
        processingAction={props.processingAction}
        setDisputeReason={props.onSetDisputeReason}
        onClose={props.onCloseDisputeModal}
        onSubmit={props.onSubmitDispute}
      />

      {props.showPaymentOffer && (
        <props.PaymentOfferModal
          onClose={props.onClosePaymentOffer}
          onSubmit={async (amount, note) => {
            const sent = await props.onSendPaymentOffer(amount, note);
            if (sent) {
              props.onClosePaymentOffer();
            }
          }}
        />
      )}
    </>
  );
}
