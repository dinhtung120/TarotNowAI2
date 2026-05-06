'use client';

import { useCallback, useState } from 'react';
import toast from 'react-hot-toast';
import {
 requestConversationAddMoney,
 respondConversationAddMoney,
 type ChatMessageDto,
} from '@/features/chat/shared/actions';
import { logger } from '@/shared/application/gateways/logger';
import type { ActionResult } from '@/shared/domain/actionResult';
import { AUTH_ERROR } from '@/shared/domain/authErrors';

interface UsePaymentOfferActionsParams {
 conversationId: string;
}

const CHAT_FINANCE_ERROR_CODES = {
 REQUEST_PENDING_OFFER_EXISTS: 'chat.add_money.request.pending_offer_exists',
 REQUEST_FORBIDDEN_CONVERSATION: 'chat.add_money.request.forbidden_conversation',
 REQUEST_INVALID_CONVERSATION_STATUS: 'chat.add_money.request.invalid_conversation_status',
 RESPOND_ALREADY_HANDLED: 'chat.add_money.respond.already_handled',
 RESPOND_EXPIRED: 'chat.add_money.respond.expired',
 RESPOND_FORBIDDEN_CONVERSATION: 'chat.add_money.respond.forbidden_conversation',
 RESPOND_INVALID_CONVERSATION_STATUS: 'chat.add_money.respond.invalid_conversation_status',
 RESPOND_INVALID_OFFER: 'chat.add_money.respond.invalid_offer',
} as const;

function resolveAddMoneyFailureMessage<T>(
 response: ActionResult<T>,
 fallbackMessage: string,
): string {
 if (response.success) {
  return fallbackMessage;
 }

 if (response.status === 503 || response.status === 504) {
  return 'Kết nối đang chậm hoặc gián đoạn, vui lòng thử lại.';
 }

 if (response.status === 401 || response.error === AUTH_ERROR.UNAUTHORIZED) {
  return 'Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại.';
 }

 switch (response.error) {
  case CHAT_FINANCE_ERROR_CODES.REQUEST_PENDING_OFFER_EXISTS:
   return 'Đã có một yêu cầu cộng tiền đang chờ phản hồi.';
  case CHAT_FINANCE_ERROR_CODES.REQUEST_FORBIDDEN_CONVERSATION:
  case CHAT_FINANCE_ERROR_CODES.REQUEST_INVALID_CONVERSATION_STATUS:
  case CHAT_FINANCE_ERROR_CODES.RESPOND_FORBIDDEN_CONVERSATION:
  case CHAT_FINANCE_ERROR_CODES.RESPOND_INVALID_CONVERSATION_STATUS:
   return 'Không thể thao tác cộng tiền ở trạng thái cuộc trò chuyện hiện tại.';
  case CHAT_FINANCE_ERROR_CODES.RESPOND_ALREADY_HANDLED:
   return 'Đề xuất cộng tiền này đã được xử lý trước đó.';
  case CHAT_FINANCE_ERROR_CODES.RESPOND_EXPIRED:
   return 'Đề xuất cộng tiền đã hết hạn.';
  case CHAT_FINANCE_ERROR_CODES.RESPOND_INVALID_OFFER:
   return 'Đề xuất cộng tiền không hợp lệ hoặc không còn khả dụng.';
  default:
   return response.error || fallbackMessage;
 }
}

export function usePaymentOfferActions({ conversationId }: UsePaymentOfferActionsParams) {
 const [processingOfferId, setProcessingOfferId] = useState<string | null>(null);
 const [requestingAddMoney, setRequestingAddMoney] = useState(false);
 const defaultRejectReason = 'User từ chối đề xuất cộng tiền.';

 const handleSendPaymentOffer = useCallback(
  async (amount: number, note: string) => {
   setRequestingAddMoney(true);
   try {
    const response = await requestConversationAddMoney(conversationId, {
     amountDiamond: amount,
     description: note,
    });

    if (!response.success) {
     toast.error(resolveAddMoneyFailureMessage(response, 'Không thể gửi yêu cầu cộng tiền.'));
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
     toast.error(resolveAddMoneyFailureMessage(response, 'Không thể chấp nhận đề xuất cộng tiền.'));
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
    const normalizedReason = reason?.trim();
    const response = await respondConversationAddMoney(conversationId, {
      accept: false,
      offerMessageId: message.id,
      rejectReason: normalizedReason && normalizedReason.length >= 3
        ? normalizedReason
        : defaultRejectReason,
    });

    if (!response.success) {
     toast.error(resolveAddMoneyFailureMessage(response, 'Không thể từ chối đề xuất cộng tiền.'));
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
