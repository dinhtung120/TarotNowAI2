'use client';

import { useCallback, useState } from 'react';
import { useParams } from 'next/navigation';
import Image from 'next/image';
import { useRouter } from '@/i18n/routing';
import { useLocale, useTranslations } from 'next-intl';
import type { ChatMessageDto } from '@/features/chat/application/actions';
import {
 Send,
 Loader2,
 ArrowLeft,
 Flag,
 MessageCircle,
 Coins,
 Check,
 X,
} from 'lucide-react';
import ReportModal from '@/features/chat/presentation/components/ReportModal';
import EscrowPanel from '@/features/chat/presentation/components/EscrowPanel';
import PaymentOfferModal from '@/features/chat/presentation/components/PaymentOfferModal';
import { GlassCard } from '@/shared/components/ui';
import { useChatConnection } from '@/features/chat/application/useChatConnection';
import { usePaymentOfferActions } from '@/features/chat/application/usePaymentOfferActions';

export default function ChatRoomPage() {
 const params = useParams();
 const router = useRouter();
 const t = useTranslations('Chat');
 const locale = useLocale();
 const conversationId = params.id as string;

 const {
  messages,
  newMessage,
  setNewMessage,
  loading,
  sending,
  connected,
  currentUserId,
  otherName,
  otherAvatar,
  isUserRole,
  messagesEndRef,
  inputRef,
  sendTypedMessage,
  handleSendTextMessage,
 } = useChatConnection(conversationId);

 const {
  processingOfferId,
  handleSendPaymentOffer,
  handleAcceptOffer,
  handleRejectOffer,
 } = usePaymentOfferActions({
  conversationId,
  sendTypedMessage,
 });

 const [showReport, setShowReport] = useState(false);
 const [showPaymentOffer, setShowPaymentOffer] = useState(false);

 const handleKeyDown = (event: React.KeyboardEvent) => {
  if (event.key === 'Enter' && !event.shiftKey) {
   event.preventDefault();
   void handleSendTextMessage();
  }
 };

 const renderMessage = useCallback(
  (message: ChatMessageDto) => {
   const isMe = message.senderId === currentUserId;
   const isSystem = message.type === 'system' || message.type.startsWith('system_');

   if (isSystem) {
    return (
     <div key={message.id} className="flex justify-center py-3">
      <span className="px-4 py-1.5 rounded-full bg-white/[0.03] text-[10px] text-zinc-500 font-medium tracking-widest uppercase border border-white/5">
       {message.content}
      </span>
     </div>
    );
   }

   if (message.type.startsWith('payment_')) {
    const isAccepted = messages.some(
     (item) => item.type === 'payment_accept' && item.content.includes(message.id)
    );
    const isProcessing = processingOfferId === message.id;

    let displayContent = message.content;
    let displayDiamond = message.paymentPayload?.amountDiamond;

    if (message.type === 'payment_offer') {
      displayContent = message.content;
      if (!displayDiamond && message.paymentPayload?.amountDiamond) {
       displayDiamond = message.paymentPayload.amountDiamond;
      }
    }

    if (message.type === 'payment_accept' || message.type === 'payment_reject') {
     const fallbackText =
      message.type === 'payment_accept'
       ? t('room.payment_accept_fallback')
       : t('room.payment_reject_fallback');

     try {
      const refData = JSON.parse(message.content);
      if (refData.offerId) {
       const originalOffer = messages.find((item) => item.id === refData.offerId);
       if (originalOffer) {
        if (originalOffer.paymentPayload) {
         displayDiamond = originalOffer.paymentPayload.amountDiamond;
        }
        displayContent = originalOffer.content || fallbackText;
       } else {
        displayContent = fallbackText;
       }
      }
     } catch {
      // Ignore malformed legacy payloads.
     }
    }

    const isAcceptMessage = message.type === 'payment_accept';
    const isRejectMessage = message.type === 'payment_reject';
    const containerBg = isAcceptMessage
     ? 'bg-[var(--success)]/10 border-[var(--success)]/30 shadow-[0_4px_20px_rgba(16,185,129,0.05)]'
     : isRejectMessage
      ? 'bg-[var(--danger)]/10 border-[var(--danger)]/30 shadow-[0_4px_20px_rgba(239,68,68,0.05)]'
      : 'bg-[var(--warning-bg)] border-[var(--warning)]/30 shadow-[0_4px_20px_rgba(245,158,11,0.1)]';
    const textColor = isAcceptMessage
     ? 'text-[var(--success)]'
     : isRejectMessage
      ? 'text-[var(--danger)]'
      : 'text-[var(--warning)]';

    return (
     <div key={message.id} className={`flex ${isMe ? 'justify-end' : 'justify-start'} py-2`}>
      <div className={`max-w-[85%] min-w-[200px] p-4 rounded-2xl border space-y-2 ${containerBg}`}>
       <div
        className={`text-[9px] font-black uppercase tracking-widest ${
         isAcceptMessage
          ? 'text-[var(--success)]'
          : message.type === 'payment_offer'
           ? 'text-[var(--warning)]'
           : 'text-[var(--danger)]'
        }`}
       >
        {message.type === 'payment_offer'
         ? t('room.payment_offer')
         : message.type === 'payment_accept'
          ? t('room.payment_accept')
          : t('room.payment_reject')}
       </div>

       {displayContent && displayContent.trim() !== '' && (
        <p className="text-sm text-white font-medium leading-relaxed">{displayContent}</p>
       )}

       {displayDiamond && (
        <div className="flex items-center justify-between mt-2 pt-2 border-t border-white/5">
         <div className={`text-sm font-black tracking-wider ${textColor}`}>{displayDiamond} 💎</div>

         {isUserRole === true && message.type === 'payment_offer' && (
          <div className="flex items-center gap-2">
           <button
            onClick={() => void handleAcceptOffer(message)}
            disabled={isProcessing || isAccepted}
            className="flex items-center justify-center gap-1 bg-[var(--success)]/20 hover:bg-[var(--success)]/30 border border-[var(--success)]/30 text-[var(--success)] font-bold text-[10px] uppercase tracking-widest px-3 py-1.5 rounded-lg transition-all active:scale-95 disabled:opacity-50"
           >
            {isProcessing ? (
             <Loader2 className="w-3 h-3 animate-spin" />
            ) : (
             <Check className="w-3 h-3" />
            )}
            {isAccepted ? t('room.payment_offer_received') : t('room.payment_offer_accept')}
           </button>
           {!isAccepted && (
            <button
             onClick={() => void handleRejectOffer(message)}
             className="flex items-center justify-center gap-1 px-3 py-1.5 bg-[var(--danger)]/10 hover:bg-[var(--danger)]/20 text-[var(--danger)] rounded-lg transition-all active:scale-95 border border-[var(--danger)]/20 text-[10px] font-bold uppercase tracking-widest"
            >
             <X className="w-3 h-3" />
             {t('room.payment_offer_reject')}
            </button>
           )}
          </div>
         )}
        </div>
       )}
      </div>
     </div>
    );
   }

   return (
    <div key={message.id} className={`flex ${isMe ? 'justify-end' : 'justify-start'} py-2 group`}>
     <div
      className={`max-w-[85%] px-5 py-3.5 rounded-[1.5rem] relative ${
       isMe
        ? 'bg-gradient-to-br from-[var(--purple-accent)]/80 to-[var(--purple-accent)] border border-[var(--purple-accent)]/40 text-white rounded-tr-sm shadow-[0_4px_20px_rgba(168,85,247,0.2)]'
        : 'bg-white/[0.04] border border-white/5 text-zinc-200 rounded-tl-sm hover:bg-white/[0.06] transition-colors'
      }`}
     >
      <p className="text-sm leading-relaxed break-words font-sans">{message.content}</p>
      <div
       className={`text-[9px] mt-1.5 font-bold tracking-widest uppercase ${
        isMe ? 'text-white/60' : 'text-zinc-600'
       }`}
      >
       {new Date(message.createdAt).toLocaleTimeString(locale, {
        hour: '2-digit',
        minute: '2-digit',
       })}
      </div>
     </div>
    </div>
   );
  },
  [
   currentUserId,
   handleAcceptOffer,
   handleRejectOffer,
   isUserRole,
   locale,
   messages,
   processingOfferId,
   t,
  ]
 );

 return (
  <div className="max-w-2xl mx-auto px-3 sm:px-4 md:px-0 pt-4 md:pt-6 pb-4 md:pb-6 h-[calc(100vh-6rem)] min-h-[32rem] flex flex-col w-full animate-in fade-in ease-out duration-700">
   <GlassCard className="flex flex-col flex-1 overflow-hidden !p-0 border-white/10 shadow-2xl relative">
    <div className="flex items-center justify-between px-4 sm:px-6 py-3 sm:py-4 border-b border-white/10 bg-black/40 backdrop-blur-md z-30 shrink-0 relative">
     <div className="flex items-center gap-3 sm:gap-4">
      <button
       onClick={() => router.push('/chat')}
       className="p-2 sm:p-3 min-h-10 min-w-10 sm:min-h-11 sm:min-w-11 rounded-xl hover:bg-white/10 transition-colors bg-white/5 group"
      >
       <ArrowLeft className="w-4 h-4 text-[var(--text-secondary)] group-hover:text-white transition-colors" />
      </button>
      <div>
       <div className="flex items-center gap-2 sm:gap-3">
        {otherAvatar ? (
         <Image
          src={otherAvatar}
          alt={otherName}
          width={32}
          height={32}
          unoptimized
          className="w-8 h-8 rounded-full object-cover border border-white/10"
         />
        ) : (
         <div className="w-8 h-8 rounded-full bg-[var(--purple-accent)]/20 border border-[var(--purple-accent)]/30 flex items-center justify-center text-xs font-black text-[var(--purple-accent)]">
          {otherName ? otherName.charAt(0).toUpperCase() : '?'}
         </div>
        )}
        <div className="text-sm sm:text-base font-black text-white italic tracking-tighter">
         {otherName || t('room.title')}
        </div>
       </div>
       <div className="flex items-center gap-1.5 sm:gap-2 text-[9px] sm:text-[10px] font-bold uppercase tracking-widest mt-0.5 ml-10 sm:ml-11">
        <div
         className={`w-1.5 h-1.5 sm:w-2 sm:h-2 rounded-full ${
          connected
           ? 'bg-[var(--success)] animate-pulse shadow-[0_0_10px_rgba(16,185,129,0.5)]'
           : 'bg-zinc-600'
         }`}
        />
        <span className={connected ? 'text-[var(--success)]' : 'text-zinc-600'}>
         {connected ? t('room.signal_ok') : t('room.syncing')}
        </span>
       </div>
      </div>
     </div>

     <div className="flex items-center gap-2 sm:gap-3">
      <EscrowPanel conversationId={conversationId} currentUserId={currentUserId} isUser={isUserRole} />

      <button
       onClick={() => setShowReport(true)}
       className="p-2.5 sm:p-3 min-h-10 min-w-10 sm:min-h-11 sm:min-w-11 flex items-center justify-center rounded-xl hover:bg-[var(--danger-bg)] transition-colors text-[var(--text-tertiary)] hover:text-[var(--danger)] group border border-transparent hover:border-[var(--danger)]/30"
       title={t('room.report_title')}
      >
       <Flag className="w-4 h-4 sm:w-4 sm:h-4" />
      </button>
     </div>
    </div>

    <div className="flex-1 overflow-y-auto px-3 sm:px-6 py-4 sm:py-6 space-y-2 relative scroll-smooth scrollbar-thin scrollbar-thumb-white/10 scrollbar-track-transparent">
     {loading && (
      <div className="absolute inset-0 flex flex-col items-center justify-center space-y-4 bg-black/40 backdrop-blur-sm z-20">
       <Loader2 className="w-10 h-10 text-[var(--purple-accent)] animate-spin" />
       <span className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">
        {t('room.loading')}
       </span>
      </div>
     )}

     {!loading && messages.length === 0 && (
      <div className="h-full flex flex-col items-center justify-center space-y-4 opacity-60">
       <div className="w-20 h-20 rounded-full border border-dashed border-white/20 flex items-center justify-center">
        <MessageCircle className="w-8 h-8 text-[var(--purple-accent)]/60" />
       </div>
       <p className="text-[var(--text-secondary)] text-sm font-medium">{t('room.empty')}</p>
      </div>
     )}

     {messages.map(renderMessage)}
     <div ref={messagesEndRef} className="h-4" />
    </div>

    <div className="p-4 md:p-5 border-t border-white/10 bg-black/60 backdrop-blur-xl shrink-0">
     <div className="flex items-center gap-3">
      {isUserRole === false && (
       <button
        onClick={() => setShowPaymentOffer(true)}
        disabled={!connected}
        title={t('room.payment_offer')}
        className="p-4 min-w-14 bg-[var(--warning)]/20 hover:bg-[var(--warning)]/30 disabled:bg-white/5 rounded-[1.5rem] transition-all disabled:opacity-50 text-[var(--warning)] disabled:text-zinc-600 active:scale-95"
       >
        <Coins className="w-5 h-5 mx-auto" />
       </button>
      )}
      <input
       ref={inputRef}
       id="chat-input"
       type="text"
       value={newMessage}
       onChange={(event) => setNewMessage(event.target.value)}
       onKeyDown={handleKeyDown}
       placeholder={t('room.input_placeholder')}
       disabled={!connected}
       className="flex-1 bg-white/[0.03] border border-white/10 hover:border-white/20 rounded-[1.5rem] px-5 py-4 text-sm text-white placeholder:text-zinc-600 focus:outline-none focus:border-[var(--purple-accent)]/50 focus:bg-white/[0.05] transition-all disabled:opacity-50"
      />
      <button
       id="chat-send-btn"
       onClick={() => void handleSendTextMessage()}
       disabled={!newMessage.trim() || sending || !connected}
       className="p-4 bg-[var(--purple-accent)] hover:bg-[#9333ea] disabled:bg-white/5 rounded-[1.5rem] transition-all disabled:opacity-50 text-white disabled:text-zinc-600 active:scale-95 shadow-[0_0_20px_rgba(168,85,247,0.3)] disabled:shadow-none"
      >
       {sending ? <Loader2 className="w-5 h-5 animate-spin" /> : <Send className="w-5 h-5 ml-0.5" />}
      </button>
     </div>
    </div>
   </GlassCard>

   {showReport && <ReportModal conversationId={conversationId} onClose={() => setShowReport(false)} />}

   {showPaymentOffer && (
   <PaymentOfferModal
    onClose={() => setShowPaymentOffer(false)}
    onSubmit={(amount, note) => handleSendPaymentOffer(amount, note)}
   />
  )}
 </div>
 );
}
