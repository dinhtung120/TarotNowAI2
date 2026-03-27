'use client';

import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { useParams } from 'next/navigation';
import Image from 'next/image';
import dynamic from 'next/dynamic';
import toast from 'react-hot-toast';
import {
 ArrowLeft,
 Check,
 CheckCheck,
 Coins,
 Image as ImageIcon,
 Loader2,
 MoreVertical,
 Music2,
 Paperclip,
 Send,
 Trash2,
 X,
 } from 'lucide-react';
import { useLocale, useTranslations } from 'next-intl';
import { useRouter } from '@/i18n/routing';
import { GlassCard } from '@/shared/components/ui';
import { useChatConnection } from '@/features/chat/application/useChatConnection';
import { usePaymentOfferActions } from '@/features/chat/application/usePaymentOfferActions';
import {
 createConversation,
 acceptConversation,
 rejectConversation,
 requestConversationComplete,
 respondConversationComplete,
 openConversationDispute,
 cancelPendingConversation,
 type ChatMessageDto,
 type MediaPayloadDto,
} from '@/features/chat/application/actions';
import { normalizeReaderStatus } from '@/features/reader/domain/readerStatus';

const PaymentOfferModal = dynamic(
 () => import('@/features/chat/presentation/components/PaymentOfferModal'),
 { loading: () => null }
);

interface ChatRoomPageProps {
 conversationId?: string;
 embedded?: boolean;
 onBack?: () => void;
}

type OfferResponseMap = Record<string, 'accept' | 'reject'>;

function parseOfferResponseMap(messages: ChatMessageDto[]): OfferResponseMap {
 const map: OfferResponseMap = {};
 for (const message of messages) {
  if (message.type !== 'payment_accept' && message.type !== 'payment_reject') continue;
  try {
   const payload = JSON.parse(message.content) as { offerMessageId?: string };
   if (!payload.offerMessageId) continue;
   map[payload.offerMessageId] = message.type === 'payment_accept' ? 'accept' : 'reject';
  } catch {
   // ignore legacy payload
  }
 }
 return map;
}

function parseStatusLabel(status?: string | null) {
 switch (normalizeReaderStatus(status)) {
  case 'accepting_questions':
   return { text: 'Online', color: 'text-[var(--success)]' };
  case 'online':
   return { text: 'Online', color: 'text-[var(--success)]' };
  case 'away':
   return { text: 'Away', color: 'text-[var(--warning)]' };
  default:
   return { text: 'Offline', color: 'text-[var(--danger)]' };
 }
}

const MAX_MEDIA_PAYLOAD_BYTES = 5 * 1024 * 1024;
const MAX_RAW_IMAGE_BYTES = 20 * 1024 * 1024;
const MAX_RAW_AUDIO_BYTES = 12 * 1024 * 1024;

function readBlobAsDataUrl(blob: Blob): Promise<string> {
 return new Promise((resolve, reject) => {
  const reader = new FileReader();
  reader.onerror = () => reject(new Error('Không thể đọc file media.'));
  reader.onload = () => {
   const value = typeof reader.result === 'string' ? reader.result : '';
   if (!value.startsWith('data:')) {
    reject(new Error('Định dạng media không hợp lệ.'));
    return;
   }
   resolve(value);
  };
  reader.readAsDataURL(blob);
 });
}

function canvasToBlob(
 canvas: HTMLCanvasElement,
 mimeType: string,
 quality?: number
): Promise<Blob | null> {
 return new Promise((resolve) => {
  canvas.toBlob((blob) => resolve(blob), mimeType, quality);
 });
}

async function buildImageMediaPayload(file: File): Promise<MediaPayloadDto> {
 const objectUrl = URL.createObjectURL(file);
 const image = new window.Image();

 try {
  await new Promise<void>((resolve, reject) => {
   image.onload = () => resolve();
   image.onerror = () => reject(new Error('Không thể xử lý ảnh đã chọn.'));
   image.src = objectUrl;
  });

  const maxDimension = 2048;
  const ratio = Math.min(1, maxDimension / Math.max(image.width, image.height));
  const targetWidth = Math.max(1, Math.round(image.width * ratio));
  const targetHeight = Math.max(1, Math.round(image.height * ratio));

  const canvas = document.createElement('canvas');
  canvas.width = targetWidth;
  canvas.height = targetHeight;
  const context = canvas.getContext('2d');
  if (!context) {
   throw new Error('Không khởi tạo được bộ xử lý ảnh.');
  }

  context.drawImage(image, 0, 0, targetWidth, targetHeight);

  let blob = await canvasToBlob(canvas, 'image/avif', 0.7);
  let mimeType = 'image/avif';
  let processingStatus = 'client_compressed_avif';

  if (!blob) {
   blob = await canvasToBlob(canvas, 'image/webp', 0.85);
   mimeType = 'image/webp';
   processingStatus = 'client_compressed_webp';
  }

  if (!blob) {
   blob = await canvasToBlob(canvas, 'image/jpeg', 0.85);
   mimeType = 'image/jpeg';
   processingStatus = 'client_compressed_jpeg_fallback';
  }

  if (!blob) {
   throw new Error('Không thể nén ảnh đã chọn.');
  }

  if (blob.size > MAX_MEDIA_PAYLOAD_BYTES) {
   throw new Error('Ảnh sau nén vượt quá 5MB, vui lòng chọn ảnh nhỏ hơn.');
  }

  const url = await readBlobAsDataUrl(blob);
  return {
   url,
   mimeType,
   sizeBytes: blob.size,
   width: targetWidth,
   height: targetHeight,
   description: file.name,
   processingStatus,
  };
 } finally {
  URL.revokeObjectURL(objectUrl);
 }
}

function getAudioDurationMs(file: Blob): Promise<number | null> {
 const objectUrl = URL.createObjectURL(file);
 const audio = document.createElement('audio');
 audio.preload = 'metadata';
 audio.src = objectUrl;

 return new Promise((resolve) => {
  const finalize = (value: number | null) => {
   URL.revokeObjectURL(objectUrl);
   resolve(value);
  };

  audio.onloadedmetadata = () => {
   if (!Number.isFinite(audio.duration) || audio.duration <= 0) {
    finalize(null);
    return;
   }

   finalize(Math.max(1, Math.round(audio.duration * 1000)));
  };

  audio.onerror = () => finalize(null);
 });
}

async function transcodeAudioToOpus(file: File): Promise<{
 blob: Blob;
 mimeType: string;
 processingStatus: string;
 durationMs: number | null;
}> {
 const durationMs = await getAudioDurationMs(file);
 const supportsMediaRecorder = typeof window !== 'undefined'
  && typeof window.MediaRecorder !== 'undefined'
  && window.MediaRecorder.isTypeSupported('audio/webm;codecs=opus');

 if (!supportsMediaRecorder) {
  return {
   blob: file,
   mimeType: file.type || 'audio/webm',
   processingStatus: 'passthrough_requires_server_opus_transcode',
   durationMs,
  };
 }

 const audio = document.createElement('audio');
 audio.preload = 'auto';
 audio.src = URL.createObjectURL(file);
 audio.volume = 0;

 try {
  await new Promise<void>((resolve, reject) => {
   audio.onloadedmetadata = () => resolve();
   audio.onerror = () => reject(new Error('Không thể đọc file âm thanh.'));
  });

  type CaptureAudioElement = HTMLAudioElement & {
   captureStream?: () => MediaStream;
   mozCaptureStream?: () => MediaStream;
  };

  const audioElement = audio as CaptureAudioElement;
  const mediaStream = audioElement.captureStream?.() ?? audioElement.mozCaptureStream?.();
  if (!mediaStream) {
   return {
    blob: file,
    mimeType: file.type || 'audio/webm',
    processingStatus: 'passthrough_requires_server_opus_transcode',
    durationMs,
   };
  }

  const chunks: BlobPart[] = [];
  const recorder = new window.MediaRecorder(mediaStream, {
   mimeType: 'audio/webm;codecs=opus',
   audioBitsPerSecond: 16_000,
  });

  const recordingCompleted = new Promise<Blob>((resolve, reject) => {
   recorder.ondataavailable = (event) => {
    if (event.data && event.data.size > 0) {
     chunks.push(event.data);
    }
   };

   recorder.onerror = () => reject(new Error('Không thể mã hóa âm thanh sang Opus.'));
   recorder.onstop = () => resolve(new Blob(chunks, { type: 'audio/webm' }));
  });

  recorder.start();
  await audio.play();
  await new Promise<void>((resolve) => {
   audio.onended = () => resolve();
  });

  recorder.stop();
  const blob = await recordingCompleted;
  if (blob.size === 0) {
   throw new Error('Âm thanh đã mã hóa không hợp lệ.');
  }

  return {
   blob,
   mimeType: 'audio/webm',
   processingStatus: 'client_transcoded_opus',
   durationMs,
  };
 } catch {
  return {
   blob: file,
   mimeType: file.type || 'audio/webm',
   processingStatus: 'passthrough_requires_server_opus_transcode',
   durationMs,
  };
 } finally {
  URL.revokeObjectURL(audio.src);
 }
}

async function buildVoiceMediaPayload(file: File): Promise<MediaPayloadDto> {
 const transcoded = await transcodeAudioToOpus(file);
 if (transcoded.blob.size > MAX_MEDIA_PAYLOAD_BYTES) {
  throw new Error('File âm thanh sau nén vượt quá 5MB.');
 }

 const url = await readBlobAsDataUrl(transcoded.blob);
 return {
  url,
  mimeType: transcoded.mimeType,
  sizeBytes: transcoded.blob.size,
  durationMs: transcoded.durationMs,
  description: file.name,
  processingStatus: transcoded.processingStatus,
 };
}

export default function ChatRoomPage({ conversationId: externalConversationId, embedded = false, onBack }: ChatRoomPageProps) {
 const params = useParams();
 const router = useRouter();
 const locale = useLocale();
 const t = useTranslations('Chat');
 const resolvedConversationId = externalConversationId ?? (params.id as string | undefined);

 const {
  messages,
  newMessage,
  setNewMessage,
  loading,
  loadingMore,
  hasMore,
  loadMore,
  sending,
  connected,
  currentUserId,
  conversation,
  otherName,
  otherAvatar,
  isUserRole,
  remoteTyping,
 messagesEndRef,
 inputRef,
 sendMediaMessage,
 handleSendTextMessage,
 notifyTyping,
 } = useChatConnection({ conversationId: resolvedConversationId });

 const {
  processingOfferId,
  requestingAddMoney,
  handleSendPaymentOffer,
  handleAcceptOffer,
  handleRejectOffer,
 } = usePaymentOfferActions({
  conversationId: resolvedConversationId ?? '',
 });

 const [showPaymentOffer, setShowPaymentOffer] = useState(false);
 const [showDisputeModal, setShowDisputeModal] = useState(false);
 const [rejectReason, setRejectReason] = useState('');
 const [processingAction, setProcessingAction] = useState<string | null>(null);
 const [startingNewSession, setStartingNewSession] = useState(false);
 const [disputeReason, setDisputeReason] = useState('');
 const [showMediaMenu, setShowMediaMenu] = useState(false);
 const [showActionMenu, setShowActionMenu] = useState(false);
 const [uploadingMedia, setUploadingMedia] = useState(false);
 const scrollRef = useRef<HTMLDivElement>(null);
 const typingStopTimer = useRef<ReturnType<typeof setTimeout> | null>(null);
 const mediaMenuRef = useRef<HTMLDivElement>(null);
 const actionMenuRef = useRef<HTMLDivElement>(null);
 const imageInputRef = useRef<HTMLInputElement>(null);
 const audioInputRef = useRef<HTMLInputElement>(null);

 const offerResponseMap = useMemo(() => parseOfferResponseMap(messages), [messages]);
 const normalizedReaderStatus = normalizeReaderStatus(conversation?.readerStatus);
 const readerStatus = parseStatusLabel(conversation?.readerStatus);

 const canShowInput = useMemo(() => {
  if (!conversation) return false;
  if (conversation.status === 'pending') return isUserRole === true;
  if (conversation.status === 'ongoing') return true;
  return false;
 }, [conversation, isUserRole]);

 const canReaderAcceptReject = useMemo(
  () => conversation?.status === 'awaiting_acceptance' && isUserRole === false,
  [conversation?.status, isUserRole]
 );

 const isReadOnly = useMemo(() => {
  if (!conversation) return true;
  return ['completed', 'cancelled', 'expired', 'disputed', 'awaiting_acceptance'].includes(conversation.status);
 }, [conversation]);

 const canStartNewSession = useMemo(
  () => conversation?.status === 'completed' && isUserRole === true,
  [conversation?.status, isUserRole]
 );

 const canCancelPending = useMemo(
  () => conversation?.status === 'pending' && isUserRole === true,
  [conversation?.status, isUserRole]
 );

 const canUseActionMenu = useMemo(
  () => conversation?.status === 'ongoing',
  [conversation?.status]
 );

 const awaitingCompleteResponse = useMemo(
  () =>
   conversation?.status === 'ongoing'
   && Boolean(conversation.confirm?.requestedBy)
   && conversation.confirm?.requestedBy !== currentUserId,
  [conversation?.confirm?.requestedBy, conversation?.status, currentUserId]
 );

 const readOnlyHint = useMemo(() => {
  if (!conversation) return 'Cuộc trò chuyện đang ở chế độ chỉ đọc.';
  if (conversation.status === 'completed') return 'Cuộc trò chuyện đã hoàn thành.';
  if (conversation.status === 'cancelled') return 'Cuộc trò chuyện đã bị hủy.';
  if (conversation.status === 'expired') return 'Cuộc trò chuyện đã hết hạn.';
  if (conversation.status === 'disputed') return 'Cuộc trò chuyện đang chờ Admin xử lý tranh chấp.';
  if (conversation.status === 'awaiting_acceptance') return 'Cuộc trò chuyện đang chờ Reader phản hồi.';
  return 'Bạn chưa thể gửi tin nhắn ở trạng thái hiện tại.';
 }, [conversation]);

 useEffect(() => {
  if (!remoteTyping) return;
  setTimeout(() => {
   messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, 0);
 }, [messagesEndRef, remoteTyping]);

 useEffect(() => {
  if (!scrollRef.current) return;
  const node = scrollRef.current;
  const onScroll = () => {
   if (node.scrollTop < 120 && hasMore && !loadingMore) {
    void loadMore();
   }
  };
  node.addEventListener('scroll', onScroll);
  return () => node.removeEventListener('scroll', onScroll);
 }, [hasMore, loadMore, loadingMore]);

 useEffect(() => {
  return () => {
   if (typingStopTimer.current) {
    clearTimeout(typingStopTimer.current);
   }
  };
 }, []);

 useEffect(() => {
  const onMouseDown = (event: MouseEvent) => {
   const target = event.target as Node;
   if (mediaMenuRef.current && !mediaMenuRef.current.contains(target)) {
    setShowMediaMenu(false);
   }
   if (actionMenuRef.current && !actionMenuRef.current.contains(target)) {
    setShowActionMenu(false);
   }
  };

  document.addEventListener('mousedown', onMouseDown);
  return () => document.removeEventListener('mousedown', onMouseDown);
 }, []);

 useEffect(() => {
  setShowMediaMenu(false);
  setShowActionMenu(false);
  setShowDisputeModal(false);
 }, [resolvedConversationId]);

 const handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
  if (event.key === 'Enter' && !event.shiftKey) {
   event.preventDefault();
   void handleSendTextMessage();
  }
 };

 const onInputChange = (value: string) => {
  setNewMessage(value);
  notifyTyping(true);
  if (typingStopTimer.current) clearTimeout(typingStopTimer.current);
  typingStopTimer.current = setTimeout(() => notifyTyping(false), 1200);
 };

 const handleMediaFileSelected = useCallback(
  async (type: 'image' | 'voice', event: React.ChangeEvent<HTMLInputElement>) => {
   const file = event.target.files?.[0];
   event.target.value = '';
   if (!file || !connected) return;

   const maxRawSize = type === 'image' ? MAX_RAW_IMAGE_BYTES : MAX_RAW_AUDIO_BYTES;
   if (file.size > maxRawSize) {
    toast.error(
     type === 'image'
      ? 'Ảnh gốc vượt quá 20MB.'
      : 'Âm thanh gốc vượt quá 12MB.'
    );
    return;
   }

   setUploadingMedia(true);
   try {
    const payload = type === 'image'
     ? await buildImageMediaPayload(file)
     : await buildVoiceMediaPayload(file);
    const success = await sendMediaMessage(type, payload);
    if (!success) {
     toast.error('Không thể gửi media.');
    }
   } catch (error) {
    toast.error(error instanceof Error ? error.message : 'Không thể gửi media.');
   } finally {
    setUploadingMedia(false);
   }
  },
  [connected, sendMediaMessage]
 );

 const runAction = useCallback(async (key: string, fn: () => Promise<void>) => {
  setProcessingAction(key);
  try {
   await fn();
  } finally {
   setProcessingAction(null);
  }
 }, []);

 const handleAcceptConversation = () =>
  runAction('accept', async () => {
   if (!resolvedConversationId) return;
   const result = await acceptConversation(resolvedConversationId);
   if (!result.success) {
    toast.error(result.error || 'Không thể chấp nhận cuộc trò chuyện.');
   }
  });

 const handleRejectConversation = () =>
  runAction('reject', async () => {
   if (!resolvedConversationId) return;
   const result = await rejectConversation(resolvedConversationId, rejectReason || 'Không phù hợp');
   if (!result.success) {
    toast.error(result.error || 'Không thể từ chối cuộc trò chuyện.');
    return;
   }
   setRejectReason('');
  });

 const handleRequestComplete = () =>
  runAction('complete-request', async () => {
   if (!resolvedConversationId) return;
   const result = await requestConversationComplete(resolvedConversationId);
   if (!result.success) {
    toast.error(result.error || 'Không thể gửi yêu cầu hoàn thành.');
   }
  });

 const handleRespondComplete = (accept: boolean) =>
  runAction(`complete-respond-${accept ? 'accept' : 'reject'}`, async () => {
   if (!resolvedConversationId) return;
   const result = await respondConversationComplete(resolvedConversationId, accept);
   if (!result.success) {
    toast.error(result.error || 'Không thể phản hồi yêu cầu hoàn thành.');
   }
  });

 const handleOpenDispute = () =>
  runAction('open-dispute', async () => {
   if (!resolvedConversationId || !disputeReason.trim()) return;
   const result = await openConversationDispute(resolvedConversationId, {
    reason: disputeReason.trim(),
   });
   if (!result.success) {
    toast.error(result.error || 'Không thể mở tranh chấp.');
    return;
   }
   setDisputeReason('');
   setShowDisputeModal(false);
  });

 const handleCancelPending = () =>
  runAction('cancel-pending', async () => {
   if (!resolvedConversationId) return;
   const result = await cancelPendingConversation(resolvedConversationId);
   if (!result.success) {
    toast.error(result.error || 'Không thể hủy cuộc trò chuyện pending.');
   }
  });

 const handleStartNewSession = useCallback(async () => {
  if (!conversation || !conversation.readerId) return;

  setStartingNewSession(true);
  try {
   const result = await createConversation(conversation.readerId, conversation.slaHours ?? 12);
   if (!result.success || !result.data?.id) {
    toast.error(result.error || 'Không thể bắt đầu phiên tư vấn mới.');
    return;
   }

   router.push(`/chat/${result.data.id}`);
  } finally {
   setStartingNewSession(false);
  }
 }, [conversation, router]);

 const headerWarning = normalizedReaderStatus === 'away' || normalizedReaderStatus === 'offline';

 if (!resolvedConversationId) {
  return (
   <GlassCard className="h-full flex items-center justify-center text-[var(--text-secondary)]">
    {t('room.empty')}
   </GlassCard>
  );
 }

 return (
   <div className="flex-1 flex flex-col min-h-0 h-full">
    <GlassCard className="flex flex-col h-full !p-0 overflow-hidden relative rounded-none border-0 border-l border-white/5 bg-transparent">
     <div className="px-4 py-3 border-b border-white/10 flex items-center justify-between gap-3 bg-[#0a0a0a]/40 backdrop-blur-md">
     <div className="flex items-center gap-3 min-w-0">
      {/* Nút quay lại: Chỉ hiện trên mobile (md:hidden) vì trên desktop đã có Sidebar luôn hiển thị */}
      <button
       onClick={() => router.push('/chat')}
       className="p-2 rounded-lg hover:bg-white/10 md:hidden"
      >
       <ArrowLeft className="w-4 h-4" />
      </button>

      {otherAvatar ? (
       <Image
        src={otherAvatar}
        alt={otherName || 'user'}
        width={36}
        height={36}
        unoptimized
        className="w-9 h-9 rounded-full object-cover border border-white/10"
       />
      ) : (
       <div className="w-9 h-9 rounded-full bg-[var(--purple-accent)]/20 border border-[var(--purple-accent)]/30 flex items-center justify-center text-sm font-black text-[var(--purple-accent)]">
        {(otherName || '?').charAt(0).toUpperCase()}
       </div>
      )}

      <div className="min-w-0">
       <p className="font-semibold truncate text-white">{otherName || t('room.title')}</p>
       <p className={`text-[11px] ${readerStatus.color}`}>{readerStatus.text}</p>
      </div>
     </div>

     <div className="flex items-center gap-2">
      {conversation?.escrowTotalFrozen && conversation.escrowTotalFrozen > 0 ? (
       <div className="px-2 py-1 rounded-lg text-xs bg-[var(--warning)]/10 border border-[var(--warning)]/25 text-[var(--warning)]">
        Đang đóng băng: {conversation.escrowTotalFrozen} 💎
       </div>
      ) : null}
     </div>
    </div>

    {headerWarning ? (
      <div className="px-4 py-2 text-xs bg-[var(--warning)]/10 text-[var(--warning)] border-b border-[var(--warning)]/20">
       ⚠️ Reader đang không hoạt động. Thời gian phản hồi có thể lâu hơn SLA cam kết.
      </div>
    ) : null}

    {conversation?.status === 'awaiting_acceptance' && isUserRole === true ? (
     <div className="px-4 py-2 text-xs bg-white/5 border-b border-white/10 text-[var(--text-secondary)]">
      Đang chờ Reader chấp nhận hoặc từ chối câu hỏi.
     </div>
    ) : null}

    {canCancelPending ? (
     <div className="px-4 py-2 border-b border-white/10 bg-white/5 flex items-center justify-between gap-3">
      <p className="text-xs text-[var(--text-secondary)]">
       Bạn có thể hủy cuộc trò chuyện pending nếu chưa muốn tiếp tục.
      </p>
      <button
       onClick={() => void handleCancelPending()}
       disabled={processingAction !== null}
       className="px-3 py-1.5 rounded-lg bg-[var(--danger)]/20 border border-[var(--danger)]/30 text-[var(--danger)] text-xs font-semibold disabled:opacity-60 flex items-center gap-1.5"
      >
       {processingAction === 'cancel-pending' ? <Loader2 className="w-3 h-3 animate-spin" /> : <Trash2 className="w-3 h-3" />}
       Xóa pending
      </button>
     </div>
    ) : null}

    {canReaderAcceptReject ? (
     <div className="px-4 py-3 border-b border-white/10 bg-white/5 space-y-2">
      <p className="text-xs text-[var(--text-secondary)]">Yêu cầu đang chờ phản hồi. Reader cần chọn Accept hoặc Reject.</p>
      <div className="flex gap-2">
       <button
        onClick={() => void handleAcceptConversation()}
        disabled={processingAction !== null}
        className="px-3 py-2 rounded-lg bg-[var(--success)]/20 border border-[var(--success)]/30 text-[var(--success)] text-xs font-bold"
       >
        {processingAction === 'accept' ? <Loader2 className="w-3 h-3 animate-spin" /> : 'Accept'}
       </button>
       <input
        value={rejectReason}
        onChange={(e) => setRejectReason(e.target.value)}
        placeholder="Lý do từ chối"
        className="flex-1 bg-white/5 border border-white/10 rounded-lg px-3 py-2 text-xs text-white"
       />
       <button
        onClick={() => void handleRejectConversation()}
        disabled={processingAction !== null}
        className="px-3 py-2 rounded-lg bg-[var(--danger)]/20 border border-[var(--danger)]/30 text-[var(--danger)] text-xs font-bold"
       >
        {processingAction === 'reject' ? <Loader2 className="w-3 h-3 animate-spin" /> : 'Reject'}
       </button>
      </div>
     </div>
    ) : null}

    <div
     ref={scrollRef}
     className="flex-1 overflow-y-auto px-3 py-3 space-y-4 bg-black/5"
    >
     {loading ? (
      <div className="h-full flex items-center justify-center">
       <Loader2 className="w-5 h-5 animate-spin text-[var(--text-secondary)]" />
      </div>
     ) : null}

     {!loading && hasMore ? (
      <div className="flex justify-center py-2">
       <button
        onClick={() => void loadMore()}
        disabled={loadingMore}
        className="text-xs px-3 py-1 rounded-full bg-white/5 hover:bg-white/10"
       >
        {loadingMore ? 'Đang tải...' : 'Tải tin nhắn cũ'}
       </button>
      </div>
     ) : null}

     {messages.map((message) => {
      const isMe = message.senderId === currentUserId;
      const isSystem = message.type === 'system'
       || message.type === 'system_refund'
       || message.type === 'system_release'
       || message.type === 'system_dispute';

      if (isSystem) {
       return (
        <div key={message.id} className="flex justify-center py-1">
         <div className="px-3 py-1 rounded-full bg-white/5 text-[10px] text-[var(--text-secondary)]">
          {message.content}
         </div>
        </div>
       );
      }

      if (message.type === 'payment_offer') {
       const response = offerResponseMap[message.id];
       return (
        <div key={message.id} className={`flex ${isMe ? 'justify-end' : 'justify-start'}`}>
         <div className="max-w-[80%] rounded-xl border border-[var(--warning)]/30 bg-[var(--warning)]/10 p-3 space-y-2">
          <p className="text-xs font-bold text-[var(--warning)]">Yêu cầu cộng tiền</p>
          <p className="text-sm text-white">{message.content}</p>
          <p className="text-sm font-bold text-[var(--warning)]">{message.paymentPayload?.amountDiamond ?? 0} 💎</p>

          {isUserRole === true ? (
           <div className="flex gap-2">
            <button
             disabled={processingOfferId === message.id || Boolean(response)}
             onClick={() => void handleAcceptOffer(message)}
             className="px-2 py-1 text-xs rounded bg-[var(--success)]/20 border border-[var(--success)]/25 text-[var(--success)] disabled:opacity-50"
            >
             Đồng ý
            </button>
            <button
             disabled={processingOfferId === message.id || Boolean(response)}
             onClick={() => void handleRejectOffer(message)}
             className="px-2 py-1 text-xs rounded bg-[var(--danger)]/20 border border-[var(--danger)]/25 text-[var(--danger)] disabled:opacity-50"
            >
             Từ chối
            </button>
           </div>
          ) : null}

          {response ? (
           <p className="text-[11px] text-[var(--text-secondary)]">
            {response === 'accept' ? 'Đã được chấp nhận' : 'Đã bị từ chối'}
           </p>
          ) : null}
         </div>
        </div>
       );
      }

      if (message.type === 'image' && message.mediaPayload?.url) {
       return (
        <div key={message.id} className={`flex ${isMe ? 'justify-end' : 'justify-start'}`}>
         <div className="max-w-[75%] rounded-2xl overflow-hidden border border-white/10">
          <Image
           src={message.mediaPayload.url}
           alt="media"
           width={320}
           height={240}
           unoptimized
           className="w-full h-auto"
          />
         </div>
        </div>
       );
      }

      if (message.type === 'voice' && message.mediaPayload?.url) {
       return (
        <div key={message.id} className={`flex ${isMe ? 'justify-end' : 'justify-start'}`}>
         <div className="max-w-[75%] rounded-xl border border-white/10 bg-white/5 p-3">
          <audio controls src={message.mediaPayload.url} className="w-full h-8" />
         </div>
        </div>
       );
      }

      return (
       <div key={message.id} className={`flex ${isMe ? 'justify-end' : 'justify-start'}`}>
        <div
         className={`max-w-[78%] px-3 py-2 rounded-2xl ${
          isMe
           ? 'bg-[var(--purple-accent)] text-white rounded-br-md'
           : 'bg-white/6 text-white rounded-bl-md border border-white/10'
         }`}
        >
         <p className="text-sm whitespace-pre-wrap break-words">{message.content}</p>
         <div className={`mt-1 text-[10px] flex items-center gap-1 ${isMe ? 'justify-end text-white/75' : 'text-[var(--text-secondary)]'}`}>
          <span>
           {new Date(message.createdAt).toLocaleTimeString(locale, {
            hour: '2-digit',
            minute: '2-digit',
           })}
          </span>
          {isMe ? (
           message.isRead ? <CheckCheck className="w-3 h-3 text-sky-300" /> : <Check className="w-3 h-3" />
          ) : null}
         </div>
        </div>
       </div>
      );
     })}

     {remoteTyping ? (
      <div className="text-xs text-[var(--text-secondary)] px-2">Đối phương đang nhập...</div>
     ) : null}

     <div ref={messagesEndRef} />
    </div>

    {awaitingCompleteResponse ? (
     <div className="px-3 py-2 border-t border-white/10 bg-black/20">
      <div className="flex gap-2 mb-2">
       <button
        onClick={() => void handleRespondComplete(true)}
        disabled={processingAction !== null}
        className="px-3 py-1.5 rounded-lg text-xs bg-[var(--success)]/20 border border-[var(--success)]/25 text-[var(--success)]"
       >
        Đồng ý hoàn thành
       </button>
       <button
        onClick={() => void handleRespondComplete(false)}
        disabled={processingAction !== null}
        className="px-3 py-1.5 rounded-lg text-xs bg-[var(--danger)]/20 border border-[var(--danger)]/25 text-[var(--danger)]"
       >
        Từ chối
       </button>
      </div>
     </div>
    ) : null}

    {canShowInput ? (
     <div className="p-3 border-t border-white/10 bg-black/30">
      <div className="flex gap-2">
       <div className="relative" ref={mediaMenuRef}>
        <button
         onClick={() => setShowMediaMenu((prev) => !prev)}
         disabled={!connected || uploadingMedia}
         className="h-11 w-11 rounded-xl bg-white/5 border border-white/10 text-[var(--text-secondary)] hover:text-white disabled:opacity-50 flex items-center justify-center"
         title="Gửi ảnh hoặc âm thanh"
        >
         {uploadingMedia ? <Loader2 className="w-4 h-4 animate-spin" /> : <Paperclip className="w-4 h-4" />}
        </button>
        {showMediaMenu ? (
         <div className="absolute bottom-12 left-0 w-44 rounded-xl border border-white/10 bg-[#121212] shadow-xl p-1 space-y-1 z-20">
          <button
           type="button"
           onClick={() => {
            setShowMediaMenu(false);
            imageInputRef.current?.click();
           }}
           className="w-full flex items-center gap-2 px-3 py-2 text-xs rounded-lg hover:bg-white/10"
          >
           <ImageIcon className="w-4 h-4 text-[var(--info)]" />
           Gửi ảnh
          </button>
          <button
           type="button"
           onClick={() => {
            setShowMediaMenu(false);
            audioInputRef.current?.click();
           }}
           className="w-full flex items-center gap-2 px-3 py-2 text-xs rounded-lg hover:bg-white/10"
          >
           <Music2 className="w-4 h-4 text-[var(--warning)]" />
           Gửi âm thanh
          </button>
         </div>
        ) : null}
       </div>

       <input
        ref={inputRef}
        value={newMessage}
        onChange={(event) => onInputChange(event.target.value)}
        onKeyDown={handleKeyDown}
        disabled={sending || !connected}
        placeholder={t('room.input_placeholder')}
        className="flex-1 h-11 rounded-xl bg-white/5 border border-white/10 px-3 text-sm text-white"
       />
       <button
        onClick={() => void handleSendTextMessage()}
        disabled={!newMessage.trim() || sending || !connected}
        className="h-11 w-11 rounded-xl bg-[var(--purple-accent)] disabled:bg-white/10 text-white flex items-center justify-center"
       >
        {sending ? <Loader2 className="w-4 h-4 animate-spin" /> : <Send className="w-4 h-4" />}
       </button>

       {canUseActionMenu ? (
        <div className="relative" ref={actionMenuRef}>
         <button
          onClick={() => setShowActionMenu((prev) => !prev)}
          disabled={processingAction !== null || requestingAddMoney}
          className="h-11 w-11 rounded-xl bg-white/5 border border-white/10 text-[var(--text-secondary)] hover:text-white disabled:opacity-50 flex items-center justify-center"
          title="Thao tác cuộc trò chuyện"
         >
          <MoreVertical className="w-4 h-4" />
         </button>

         {showActionMenu ? (
          <div className="absolute bottom-12 right-0 w-52 rounded-xl border border-white/10 bg-[#121212] shadow-xl p-1 space-y-1 z-20">
           {!awaitingCompleteResponse ? (
            <button
             type="button"
             onClick={() => {
              setShowActionMenu(false);
              void handleRequestComplete();
             }}
             className="w-full text-left px-3 py-2 text-xs rounded-lg hover:bg-white/10"
            >
             Hoàn thành cuộc trò chuyện
            </button>
           ) : null}
           <button
            type="button"
            onClick={() => {
             setShowActionMenu(false);
             setShowDisputeModal(true);
            }}
            className="w-full text-left px-3 py-2 text-xs rounded-lg hover:bg-white/10 text-[var(--danger)]"
           >
            Tố cáo
           </button>
           {isUserRole === false ? (
            <button
             type="button"
             onClick={() => {
              setShowActionMenu(false);
              setShowPaymentOffer(true);
             }}
             className="w-full text-left px-3 py-2 text-xs rounded-lg hover:bg-white/10 text-[var(--warning)] flex items-center gap-2"
            >
             {requestingAddMoney ? <Loader2 className="w-3 h-3 animate-spin" /> : <Coins className="w-3 h-3" />}
             Yêu cầu cộng tiền
            </button>
           ) : null}
          </div>
         ) : null}
        </div>
       ) : null}
      </div>
      <input
       ref={imageInputRef}
       type="file"
       accept="image/avif,image/webp,image/jpeg,image/png"
       className="hidden"
       onChange={(event) => void handleMediaFileSelected('image', event)}
      />
      <input
       ref={audioInputRef}
       type="file"
       accept="audio/webm,audio/ogg,audio/mp4,audio/mpeg,audio/wav,.m4a,.ogg,.webm"
       className="hidden"
       onChange={(event) => void handleMediaFileSelected('voice', event)}
      />
     </div>
    ) : (
     <div className="p-3 border-t border-white/10 text-xs text-[var(--text-secondary)]">
      <div>{isReadOnly ? readOnlyHint : 'Bạn chưa thể gửi tin nhắn ở trạng thái hiện tại.'}</div>
      {canStartNewSession ? (
       <button
        onClick={() => void handleStartNewSession()}
        disabled={startingNewSession}
        className="mt-2 px-3 py-2 rounded-lg text-xs bg-[var(--purple-accent)]/20 border border-[var(--purple-accent)]/30 text-[var(--purple-accent)] font-semibold disabled:opacity-60"
       >
        {startingNewSession ? 'Đang tạo phiên...' : 'Bắt đầu phiên tư vấn mới'}
       </button>
      ) : null}
     </div>
   )}
   </GlassCard>

   {showDisputeModal ? (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
     <button
      type="button"
      className="absolute inset-0 bg-black/70 backdrop-blur-sm"
      onClick={() => setShowDisputeModal(false)}
      aria-label="Đóng tố cáo"
     />
     <div className="relative w-full max-w-md rounded-2xl border border-white/15 bg-[#111111] p-5 space-y-4">
      <div className="flex items-center justify-between">
       <h3 className="text-sm font-semibold text-white">Mở tố cáo cuộc trò chuyện</h3>
       <button
        type="button"
        onClick={() => setShowDisputeModal(false)}
        className="w-8 h-8 rounded-lg hover:bg-white/10 flex items-center justify-center text-[var(--text-secondary)]"
       >
        <X className="w-4 h-4" />
       </button>
      </div>
      <textarea
       value={disputeReason}
       onChange={(event) => setDisputeReason(event.target.value)}
       placeholder="Nhập lý do tố cáo"
       rows={4}
       className="w-full rounded-xl border border-white/10 bg-white/5 px-3 py-2 text-sm text-white"
      />
      <div className="flex justify-end gap-2">
       <button
        type="button"
        onClick={() => setShowDisputeModal(false)}
        className="px-3 py-2 rounded-lg text-xs border border-white/15 text-[var(--text-secondary)] hover:bg-white/10"
       >
        Hủy
       </button>
       <button
        type="button"
        onClick={() => void handleOpenDispute()}
        disabled={processingAction !== null || !disputeReason.trim()}
        className="px-3 py-2 rounded-lg text-xs bg-[var(--danger)]/20 border border-[var(--danger)]/30 text-[var(--danger)] disabled:opacity-60"
       >
        {processingAction === 'open-dispute' ? <Loader2 className="w-3 h-3 animate-spin" /> : 'Gửi tố cáo'}
       </button>
      </div>
     </div>
    </div>
   ) : null}

   {showPaymentOffer && (
    <PaymentOfferModal
     onClose={() => setShowPaymentOffer(false)}
     onSubmit={async (amount, note) => {
      const success = await handleSendPaymentOffer(amount, note);
      if (success) setShowPaymentOffer(false);
     }}
    />
   )}
  </div>
 );
}
