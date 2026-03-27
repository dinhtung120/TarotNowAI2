'use client';

import { useCallback, useState } from 'react';
import toast from 'react-hot-toast';
import {
 requestConversationAddMoney,
 respondConversationAddMoney,
 type ChatMessageDto,
} from '@/features/chat/application/actions';
import { logger } from '@/shared/infrastructure/logging/logger';

interface UsePaymentOfferActionsParams {
 conversationId: string;
}

export function usePaymentOfferActions({ conversationId }: UsePaymentOfferActionsParams) {
 const [processingOfferId, setProcessingOfferId] = useState<string | null>(null);
 const [requestingAddMoney, setRequestingAddMoney] = useState(false);

 const handleSendPaymentOffer = useCallback(
  async (amount: number, note: string) => {
   setRequestingAddMoney(true);
   try {
    const response = await requestConversationAddMoney(conversationId, {
     amountDiamond: amount,
     description: note,
    });

    if (!response.success) {
     toast.error(response.error || 'Không thể gửi yêu cầu cộng tiền.');
     return false;
    }

    return true;
   } catch (error) {
    logger.error('[Chat] handleSendPaymentOffer', error, { conversationId });
    toast.error('Không thể gửi yêu cầu cộng tiền.');
    return false;
   } finally {
    setRequestingAddMoney(false);
   }
  },
  [conversationId]
 );

 const handleAcceptOffer = useCallback(
  async (message: ChatMessageDto) => {
   setProcessingOfferId(message.id);
   try {
    const response = await respondConversationAddMoney(conversationId, {
      accept: true,
      offerMessageId: message.id,
    });

    if (!response.success) {
     toast.error(response.error || 'Không thể chấp nhận đề xuất cộng tiền.');
     return false;
    }

    return true;
   } catch (error) {
    logger.error('[Chat] handleAcceptOffer', error, { messageId: message.id, conversationId });
    toast.error('Không thể chấp nhận đề xuất cộng tiền.');
    return false;
   } finally {
    setProcessingOfferId(null);
   }
  },
  [conversationId]
 );

 const handleRejectOffer = useCallback(
  async (message: ChatMessageDto, reason?: string) => {
   setProcessingOfferId(message.id);
   try {
    const response = await respondConversationAddMoney(conversationId, {
      accept: false,
      offerMessageId: message.id,
      rejectReason: reason,
    });

    if (!response.success) {
     toast.error(response.error || 'Không thể từ chối đề xuất cộng tiền.');
     return false;
    }

    return true;
   } catch (error) {
    logger.error('[Chat] handleRejectOffer', error, { messageId: message.id, conversationId });
    toast.error('Không thể từ chối đề xuất cộng tiền.');
    return false;
   } finally {
    setProcessingOfferId(null);
   }
  },
  [conversationId]
 );

 return {
  processingOfferId,
  requestingAddMoney,
  handleSendPaymentOffer,
  handleAcceptOffer,
  handleRejectOffer,
 };
}
