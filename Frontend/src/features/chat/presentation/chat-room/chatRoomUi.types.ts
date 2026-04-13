import type {
  ChangeEvent,
  ComponentType,
  KeyboardEvent,
  MutableRefObject,
} from 'react';
import type {
  ChatMessageDto,
  ConversationDto,
} from '@/features/chat/application/actions';
import type { VoiceRecordingResult } from '@/features/chat/application/useVoiceRecorder';

export interface ChatRoomHeaderProps {
  conversation: ConversationDto | null;
  headerWarning: boolean;
  loading: boolean;
  otherAvatar: string | null;
  otherName: string;
  readerStatus: { text: string; color: string };
  title: string;
  warningText: string;
  onBack: () => void;
}

export interface ChatRoomActionBannersProps {
  canCancelPending: boolean;
  canReaderAcceptReject: boolean;
  conversation: ConversationDto | null;
  isUserRole: boolean | null;
  processingAction: string | null;
  rejectReason: string;
  setRejectReason: (value: string) => void;
  onAcceptConversation: () => Promise<void>;
  onCancelPending: () => Promise<void>;
  onRejectConversation: () => Promise<void>;
}

export interface ChatActionMenuProps {
  actionMenuRef: MutableRefObject<HTMLDivElement | null>;
  awaitingCompleteResponse: boolean;
  canUseActionMenu: boolean;
  isUserRole: boolean | null;
  processingAction: string | null;
  requestingAddMoney: boolean;
  showActionMenu: boolean;
  setShowActionMenu: (value: boolean) => void;
  onOpenDispute: () => void;
  onOpenPaymentOffer: () => void;
  onRequestComplete: () => Promise<void>;
}

export interface ChatComposerInputRowProps {
  conversationExists: boolean;
  imageInputRef: MutableRefObject<HTMLInputElement | null>;
  inputPlaceholder: string;
  inputRef: MutableRefObject<HTMLInputElement | null>;
  newMessage: string;
  sending: boolean;
  uploadingMedia: boolean;
  uploadPercent: number;
  uploadingMediaLabel: string;
  VoiceRecorderButton: ComponentType<{
    disabled?: boolean;
    onRecordingComplete: (result: VoiceRecordingResult) => void;
  }>;
  onImageInputChange: (event: ChangeEvent<HTMLInputElement>) => Promise<void>;
  onInputChange: (value: string) => void;
  onInputKeyDown: (event: KeyboardEvent<HTMLInputElement>) => void;
  onSendTextMessage: () => Promise<boolean>;
  onVoiceRecordingComplete: (result: VoiceRecordingResult) => Promise<void>;
}

export interface ChatComposerProps extends ChatComposerInputRowProps {
  actionMenuRef: MutableRefObject<HTMLDivElement | null>;
  awaitingCompleteResponse: boolean;
  canUseActionMenu: boolean;
  canShowInput: boolean;
  canStartNewSession: boolean;
  isReadOnly: boolean;
  isUserRole: boolean | null;
  processingAction: string | null;
  readOnlyHint: string;
  requestingAddMoney: boolean;
  showActionMenu: boolean;
  startingNewSession: boolean;
  setShowActionMenu: (value: boolean) => void;
  onOpenDispute: () => void;
  onOpenPaymentOffer: () => void;
  onRequestComplete: () => Promise<void>;
  onStartNewSession: () => Promise<void>;
}

export interface DisputeModalProps {
  disputeReason: string;
  isOpen: boolean;
  processingAction: string | null;
  setDisputeReason: (value: string) => void;
  onClose: () => void;
  onSubmit: () => Promise<void>;
}

export interface ChatMessagesPanelProps {
  currentUserId: string;
  hasMore: boolean;
  isUserRole: boolean | null;
  loadMore: () => Promise<void>;
  loading: boolean;
  loadingMore: boolean;
  locale: string;
  messages: ChatMessageDto[];
  messagesEndRef: MutableRefObject<HTMLDivElement | null>;
  offerResponseMap: Record<string, 'accept' | 'reject'>;
  processingOfferId: string | null;
  remoteTyping: boolean;
  scrollRef: MutableRefObject<HTMLDivElement | null>;
  onAcceptOffer: (message: ChatMessageDto) => Promise<boolean>;
  onRejectOffer: (message: ChatMessageDto) => Promise<boolean>;
  VoiceMessageBubble: ComponentType<{
    audioUrl: string;
    durationMs?: number | null;
    isMe: boolean;
  }>;
}

export interface ChatMessageListItemProps {
  currentUserId: string;
  isUserRole: boolean | null;
  locale: string;
  message: ChatMessageDto;
  offerResponseMap: Record<string, 'accept' | 'reject'>;
  processingOfferId: string | null;
  onAcceptOffer: (message: ChatMessageDto) => Promise<boolean>;
  onRejectOffer: (message: ChatMessageDto) => Promise<boolean>;
  VoiceMessageBubble: ComponentType<{
    audioUrl: string;
    durationMs?: number | null;
    isMe: boolean;
  }>;
}
