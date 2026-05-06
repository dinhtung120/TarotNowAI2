import type { ConversationDto } from '@/features/chat/shared/actions';
import { isSameParticipantId } from '@/features/chat/shared/participantId';

export function statusLabel(status: string) {
 switch (status) {
  case 'pending':
   return { text: 'Pending', className: 'bg-white/10 tn-text-secondary border-white/15' };
  case 'awaiting_acceptance':
   return { text: 'Awaiting', className: 'tn-bg-warning-10 tn-text-warning tn-border-warning-30' };
  case 'ongoing':
   return { text: 'Ongoing', className: 'tn-bg-success-10 tn-text-success tn-border-success-30' };
  case 'disputed':
   return { text: 'Disputed', className: 'tn-bg-danger-soft tn-text-danger tn-border-danger' };
  case 'completed':
   return { text: 'Completed', className: 'tn-bg-info-10 tn-text-info tn-border-info-20' };
  case 'cancelled':
   return { text: 'Cancelled', className: 'bg-white/10 tn-text-secondary border-white/15' };
  case 'expired':
   return { text: 'Expired', className: 'tn-bg-warning-10 tn-text-warning tn-border-warning-30' };
  default:
   return { text: status, className: 'bg-white/10 tn-text-secondary border-white/15' };
 }
}

export function formatAgo(nowTs: number, iso?: string | null) {
 if (!iso || !nowTs) return '';
 const diff = Math.max(0, nowTs - new Date(iso).getTime());
 const mins = Math.floor(diff / 60000);
 if (mins < 1) return 'vừa xong';
 if (mins < 60) return `${mins}m`;
 const hours = Math.floor(mins / 60);
 if (hours < 24) return `${hours}h`;
 return `${Math.floor(hours / 24)}d`;
}

export function getOther(conversation: ConversationDto, currentUserId: string) {
 const isUser = isSameParticipantId(conversation.userId, currentUserId);
 return {
  isUser,
  id: isUser ? conversation.readerId : conversation.userId,
  name: isUser ? conversation.readerName || 'Reader' : conversation.userName || 'User',
  avatar: isUser ? conversation.readerAvatar : conversation.userAvatar,
  unread: isUser ? conversation.unreadCountUser : conversation.unreadCountReader,
 };
}

export function previewText(conversation: ConversationDto) {
 if (conversation.lastMessagePreview && conversation.lastMessagePreview.trim().length > 0) {
  return conversation.lastMessagePreview;
 }
 switch (conversation.status) {
  case 'pending':
   return 'Chưa có tin nhắn';
  case 'awaiting_acceptance':
   return 'Đang chờ Reader phản hồi';
  case 'completed':
   return 'Cuộc trò chuyện đã hoàn thành';
  case 'cancelled':
   return 'Cuộc trò chuyện đã hủy';
  case 'expired':
   return 'Cuộc trò chuyện đã hết hạn';
  case 'disputed':
   return 'Đang chờ Admin xử lý tranh chấp';
  default:
   return '';
 }
}
