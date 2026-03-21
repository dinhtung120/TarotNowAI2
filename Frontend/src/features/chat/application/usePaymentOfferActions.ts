'use client';

import { useCallback, useState } from 'react';
import toast from 'react-hot-toast';
import { acceptOffer } from '@/actions/escrowActions';
import type { ChatMessageDto } from '@/actions/chatActions';
import { logger } from '@/shared/infrastructure/logging/logger';

interface UsePaymentOfferActionsParams {
 conversationId: string;
 sendTypedMessage: (content: string, messageType: string) => Promise<boolean>;
}

export function usePaymentOfferActions({
 conversationId,
 sendTypedMessage,
}: UsePaymentOfferActionsParams) {
 const [processingOfferId, setProcessingOfferId] = useState<string | null>(null);

 const handleSendPaymentOffer = useCallback(
  async (amount: number, note: string) => {
   const payload = { amountDiamond: amount, description: note };
   await sendTypedMessage(JSON.stringify(payload), 'payment_offer');
  },
  [sendTypedMessage]
 );

 const handleAcceptOffer = useCallback(
  async (message: ChatMessageDto) => {
   if (!message.paymentPayload) return;

   setProcessingOfferId(message.id);
   try {
    const response = await acceptOffer({
     readerId: message.senderId,
     conversationRef: conversationId,
     amountDiamond: message.paymentPayload.amountDiamond,
     proposalMessageRef: message.id,
     idempotencyKey: crypto.randomUUID(),
    });

    if (!response.success) {
     toast.error('Không đủ số dư Kim Cương hoặc đã có lỗi xảy ra.');
     return;
    }

    await sendTypedMessage(JSON.stringify({ offerId: message.id }), 'payment_accept');
   } catch (error) {
    logger.error('[Chat] handleAcceptOffer', error, { messageId: message.id });
    toast.error('Không thể thực hiện thanh toán.');
   } finally {
    setProcessingOfferId(null);
   }
  },
  [conversationId, sendTypedMessage]
 );

 const handleRejectOffer = useCallback(
  async (message: ChatMessageDto) => {
   await sendTypedMessage(JSON.stringify({ offerId: message.id }), 'payment_reject');
  },
  [sendTypedMessage]
 );

 return {
  processingOfferId,
  handleSendPaymentOffer,
  handleAcceptOffer,
  handleRejectOffer,
 };
}
