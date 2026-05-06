import ConversationReviewModal from "@/features/chat/room/ConversationReviewModal";
import DisputeModal from "@/features/chat/room/DisputeModal";
import type { ChatRoomViewProps } from "@/features/chat/room/ChatRoomView.types";

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

      <ConversationReviewModal
        comment={props.reviewComment}
        isOpen={props.showReviewModal}
        rating={props.reviewRating}
        submitting={props.submittingReview}
        onClose={props.onCloseReviewModal}
        onCommentChange={props.onSetReviewComment}
        onRatingChange={props.onSetReviewRating}
        onSubmit={props.onSubmitReview}
      />
    </>
  );
}
