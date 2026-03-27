'use client';

import { useMemo } from 'react';
import Image from 'next/image';
import { useParams } from 'next/navigation';
import { useRouter } from '@/i18n/routing';
import { useTranslations } from 'next-intl';
import { Loader2, MessageSquareText } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import { useChatInboxPage } from '@/features/chat/application/useChatInboxPage';
import type { ConversationDto } from '@/features/chat/application/actions';

/*
 * Component ConversationSidebar: Hiển thị danh sách các cuộc trò chuyện bên lề trái.
 * Component này quản lý việc tải dữ liệu, phân loại theo Tab (Đang chat, Pending, Hoàn thành)
 * và điều hướng người dùng đến phòng chat cụ thể.
 */

// Hàm hỗ trợ hiển thị nhãn trạng thái với màu sắc tương ứng
function statusLabel(status: string) {
  switch (status) {
    case 'pending':
      return { text: 'Pending', className: 'bg-white/10 text-[var(--text-secondary)] border-white/15' };
    case 'awaiting_acceptance':
      return { text: 'Awaiting', className: 'bg-[var(--warning)]/15 text-[var(--warning)] border-[var(--warning)]/30' };
    case 'ongoing':
      return { text: 'Ongoing', className: 'bg-[var(--success)]/15 text-[var(--success)] border-[var(--success)]/30' };
    case 'disputed':
      return { text: 'Disputed', className: 'bg-[var(--danger)]/15 text-[var(--danger)] border-[var(--danger)]/30' };
    case 'completed':
      return { text: 'Completed', className: 'bg-[var(--info)]/15 text-[var(--info)] border-[var(--info)]/30' };
    case 'cancelled':
      return { text: 'Cancelled', className: 'bg-white/10 text-[var(--text-secondary)] border-white/15' };
    case 'expired':
      return { text: 'Expired', className: 'bg-[var(--warning)]/15 text-[var(--warning)] border-[var(--warning)]/30' };
    default:
      return { text: status, className: 'bg-white/10 text-[var(--text-secondary)] border-white/15' };
  }
}

// Định dạng thời gian tương đối (vừa xong, 5m, 1h, 2d)
function formatAgo(nowTs: number, iso?: string | null) {
  if (!iso || !nowTs) return '';
  const diff = Math.max(0, nowTs - new Date(iso).getTime());
  const mins = Math.floor(diff / 60000);
  if (mins < 1) return 'vừa xong';
  if (mins < 60) return `${mins}m`;
  const hours = Math.floor(mins / 60);
  if (hours < 24) return `${hours}h`;
  return `${Math.floor(hours / 24)}d`;
}

// Lấy thông tin đối phương (User hoặc Reader) dựa trên ID người dùng hiện tại
function getOther(conversation: ConversationDto, currentUserId: string) {
  const isUser = conversation.userId === currentUserId;
  return {
    isUser,
    id: isUser ? conversation.readerId : conversation.userId,
    name: isUser ? conversation.readerName || 'Reader' : conversation.userName || 'User',
    avatar: isUser ? conversation.readerAvatar : conversation.userAvatar,
    unread: isUser ? conversation.unreadCountUser : conversation.unreadCountReader,
  };
}

// Tạo văn bản xem trước cho tin nhắn cuối cùng hoặc trạng thái hiện tại
function previewText(conversation: ConversationDto) {
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

export default function ConversationSidebar() {
  const t = useTranslations('Chat');
  const router = useRouter();
  const params = useParams();
  const currentId = params.id as string | undefined;
  
  // Sử dụng hook có sẵn để lấy danh sách cuộc trò chuyện
  const { tab, setTab, conversations, loading, currentUserId, nowTs } = useChatInboxPage('active');

  return (
    <div className="flex flex-col h-full overflow-hidden gap-3 p-3 md:p-4">
      {/* Các tab phân loại cuộc trò chuyện */}
      <div className="flex items-center gap-2 overflow-x-auto pb-1 scrollbar-hide">
        <button
          onClick={() => setTab('active')}
          className={`whitespace-nowrap px-3 py-1.5 rounded-full text-xs border transition-all ${
            tab === 'active' 
              ? 'bg-[var(--purple-accent)]/20 border-[var(--purple-accent)]/40 text-white' 
              : 'bg-white/5 border-white/10 text-[var(--text-secondary)] hover:bg-white/10'
          }`}
        >
          Đang chat
        </button>
        <button
          onClick={() => setTab('pending')}
          className={`whitespace-nowrap px-3 py-1.5 rounded-full text-xs border transition-all ${
            tab === 'pending' 
              ? 'bg-[var(--purple-accent)]/20 border-[var(--purple-accent)]/40 text-white' 
              : 'bg-white/5 border-white/10 text-[var(--text-secondary)] hover:bg-white/10'
          }`}
        >
          Pending
        </button>
        <button
          onClick={() => setTab('completed')}
          className={`whitespace-nowrap px-3 py-1.5 rounded-full text-xs border transition-all ${
            tab === 'completed' 
              ? 'bg-[var(--purple-accent)]/20 border-[var(--purple-accent)]/40 text-white' 
              : 'bg-white/5 border-white/10 text-[var(--text-secondary)] hover:bg-white/10'
          }`}
        >
          Đã hoàn thành
        </button>
      </div>

      {/* Danh sách các cuộc trò chuyện */}
      <div className="flex-1 overflow-y-auto space-y-2 pr-1 custom-scrollbar">
        {loading ? (
          <div className="h-32 flex items-center justify-center">
            <Loader2 className="w-5 h-5 animate-spin text-[var(--text-secondary)]" />
          </div>
        ) : null}

        {!loading && conversations.length === 0 ? (
          <div className="h-40 flex flex-col items-center justify-center gap-2 text-[var(--text-secondary)] text-center px-4">
            <MessageSquareText className="w-6 h-6 opacity-50" />
            <p className="text-sm">{t('inbox.empty_title')}</p>
          </div>
        ) : null}

        {conversations.map((conversation) => {
          const other = getOther(conversation, currentUserId);
          const status = statusLabel(conversation.status);
          const active = conversation.id === currentId;

          return (
            <button
              key={conversation.id}
              onClick={() => router.push(`/chat/${conversation.id}`)}
              className={`w-full rounded-xl px-3 py-3 border text-left transition-all duration-200 group ${
                active
                  ? 'bg-[var(--purple-accent)]/15 border-[var(--purple-accent)]/35 shadow-[0_0_15px_rgba(168,85,247,0.1)]'
                  : 'bg-white/5 border-white/10 hover:bg-white/10 hover:border-white/20'
              }`}
            >
              <div className="flex items-center gap-3">
                {/* Avatar đối phương */}
                <div className="relative flex-shrink-0">
                  {other.avatar ? (
                    <Image 
                      src={other.avatar} 
                      alt={other.name} 
                      width={44} 
                      height={44} 
                      unoptimized 
                      className="w-11 h-11 rounded-full object-cover border border-white/10" 
                    />
                  ) : (
                    <div className="w-11 h-11 rounded-full bg-gradient-to-br from-[var(--purple-accent)]/20 to-indigo-500/20 flex items-center justify-center text-sm font-bold border border-white/10">
                      {other.name.charAt(0)}
                    </div>
                  )}
                  {/* Badge số tin nhắn chưa đọc */}
                  {other.unread > 0 ? (
                    <div className="absolute -top-1 -right-1 w-5 h-5 rounded-full bg-[var(--danger)] text-white text-[10px] flex items-center justify-center border-2 border-[#0a0a0a] font-bold">
                      {other.unread > 9 ? '9+' : other.unread}
                    </div>
                  ) : null}
                </div>

                {/* Thông tin nội dung */}
                <div className="min-w-0 flex-1">
                  <div className="flex items-center justify-between gap-2">
                    <p className={`truncate font-semibold text-sm transition-colors ${active ? 'text-white' : 'text-gray-200 group-hover:text-white'}`}>
                      {other.name}
                    </p>
                    <span className="text-[10px] text-[var(--text-secondary)] flex-shrink-0">
                      {formatAgo(nowTs, conversation.lastMessageAt)}
                    </span>
                  </div>
                  
                  <p className="mt-0.5 truncate text-xs text-[var(--text-secondary)]">
                    {previewText(conversation)}
                  </p>
                  
                  <div className="mt-1.5 flex items-center gap-2">
                    <span className={`px-2 py-0.5 rounded-full border text-[9px] font-medium uppercase tracking-wider ${status.className}`}>
                      {status.text}
                    </span>
                    {conversation.escrowTotalFrozen && conversation.escrowTotalFrozen > 0 ? (
                      <span className="text-[10px] text-[var(--warning)] font-medium">
                        {conversation.escrowTotalFrozen} 💎
                      </span>
                    ) : null}
                  </div>
                </div>
              </div>
            </button>
          );
        })}
      </div>
    </div>
  );
}
