'use client';

import React, { useEffect, useState } from 'react';
import { listConversations, type ConversationDto } from '@/actions/chatActions';
import {
  MessageCircle, Loader2, Sparkles, ChevronRight, Inbox
} from 'lucide-react';
import { useRouter } from '@/i18n/routing';

/**
 * Trang Inbox — danh sách conversations.
 *
 * Thiết kế:
 * → Hiện danh sách conversations với unread badge.
 * → Tabs cho vai trò: "Người hỏi" và "Reader".
 * → Click → navigate đến chat screen.
 * → Premium astral design.
 */
export default function InboxPage() {
  const router = useRouter();
  const [conversations, setConversations] = useState<ConversationDto[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [role, setRole] = useState<'user' | 'reader'>('user');

  // Fetch conversations khi mount hoặc role thay đổi
  useEffect(() => {
    const fetchConversations = async () => {
      setLoading(true);
      const result = await listConversations(role);
      if (result) {
        setConversations(result.conversations);
        setTotalCount(result.totalCount);
      }
      setLoading(false);
    };
    fetchConversations();
  }, [role]);

  /**
   * Helper: Tính thời gian relative (vd: "5 phút trước").
   * Đơn giản hóa — không dùng thư viện bên ngoài.
   */
  const timeAgo = (dateStr?: string | null) => {
    if (!dateStr) return '';
    const diff = Date.now() - new Date(dateStr).getTime();
    const mins = Math.floor(diff / 60000);
    if (mins < 1) return 'Vừa xong';
    if (mins < 60) return `${mins} phút trước`;
    const hrs = Math.floor(mins / 60);
    if (hrs < 24) return `${hrs} giờ trước`;
    const days = Math.floor(hrs / 24);
    return `${days} ngày trước`;
  };

  /** Status badge */
  const getStatusLabel = (status: string) => {
    switch (status) {
      case 'active': return { text: 'Đang chat', color: 'emerald' };
      case 'pending': return { text: 'Chờ phản hồi', color: 'amber' };
      case 'completed': return { text: 'Hoàn thành', color: 'zinc' };
      default: return { text: status, color: 'zinc' };
    }
  };

  return (
    <div className="max-w-3xl mx-auto px-6 py-16 space-y-10 animate-in fade-in slide-in-from-bottom-8 duration-1000">
      {/* Header */}
      <div className="space-y-4">
        <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-purple-500/5 border border-purple-500/10 text-[9px] uppercase tracking-[0.2em] font-black text-purple-400 shadow-xl backdrop-blur-md">
          <Sparkles className="w-3 h-3" />
          Tin nhắn
        </div>
        <h1 className="text-4xl md:text-5xl font-black tracking-tighter text-white uppercase italic">
          Hộp Thư
        </h1>
        <p className="text-zinc-500 font-medium text-sm">
          Quản lý các cuộc trò chuyện với Reader và khách hàng.
        </p>
      </div>

      {/* Role Tabs */}
      <div className="flex gap-2">
        {(['user', 'reader'] as const).map((r) => (
          <button
            key={r}
            onClick={() => setRole(r)}
            className={`px-5 py-2 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all border ${
              role === r
                ? 'bg-purple-500/10 border-purple-500/30 text-purple-400'
                : 'bg-white/[0.02] border-white/5 text-zinc-600 hover:border-white/10'
            }`}
          >
            {r === 'user' ? '🔮 Người hỏi' : '📖 Reader'}
          </button>
        ))}
      </div>

      {/* Loading */}
      {loading && (
        <div className="h-[30vh] flex flex-col items-center justify-center space-y-4">
          <Loader2 className="w-10 h-10 text-purple-500 animate-spin" />
          <span className="text-[10px] font-black uppercase tracking-[0.3em] text-zinc-600">Đang tải...</span>
        </div>
      )}

      {/* Empty */}
      {!loading && conversations.length === 0 && (
        <div className="h-[30vh] flex flex-col items-center justify-center space-y-4">
          <Inbox className="w-16 h-16 text-zinc-800" />
          <p className="text-zinc-600 text-sm font-medium">Chưa có cuộc trò chuyện nào.</p>
        </div>
      )}

      {/* Conversation List */}
      {!loading && conversations.length > 0 && (
        <div className="space-y-3">
          {conversations.map((conv) => {
            const unread = role === 'user' ? conv.unreadCountUser : conv.unreadCountReader;
            const statusInfo = getStatusLabel(conv.status);
            const otherUserId = role === 'user' ? conv.readerId : conv.userId;

            return (
              <button
                key={conv.id}
                onClick={() => router.push(`/chat/${conv.id}` as any)}
                className="w-full group flex items-center gap-4 p-5 bg-white/[0.02] hover:bg-white/[0.04] backdrop-blur-3xl rounded-2xl border border-white/5 hover:border-purple-500/20 transition-all duration-300 text-left"
              >
                {/* Avatar */}
                <div className="relative flex-shrink-0">
                  <div className="w-12 h-12 rounded-xl bg-gradient-to-br from-purple-500/20 to-purple-600/10 border border-purple-500/20 flex items-center justify-center text-base font-black text-purple-400">
                    {otherUserId.charAt(0).toUpperCase()}
                  </div>
                  {/* Unread Badge */}
                  {unread > 0 && (
                    <div className="absolute -top-1 -right-1 w-5 h-5 bg-red-500 rounded-full flex items-center justify-center text-[9px] font-bold text-white">
                      {unread > 9 ? '9+' : unread}
                    </div>
                  )}
                </div>

                {/* Content */}
                <div className="flex-1 min-w-0 space-y-1">
                  <div className="flex items-center justify-between">
                    <span className="text-sm font-black text-white truncate">
                      {role === 'user' ? 'Reader ' : 'User '}{otherUserId.substring(0, 8)}...
                    </span>
                    <span className="text-[9px] text-zinc-700 flex-shrink-0">
                      {timeAgo(conv.lastMessageAt)}
                    </span>
                  </div>
                  <div className="flex items-center gap-2">
                    <span className={`px-2 py-0.5 rounded-full bg-${statusInfo.color}-500/10 text-${statusInfo.color}-400 text-[9px] font-bold uppercase border border-${statusInfo.color}-500/20`}>
                      {statusInfo.text}
                    </span>
                  </div>
                </div>

                <ChevronRight className="w-4 h-4 text-zinc-800 group-hover:text-purple-400 transition-colors" />
              </button>
            );
          })}
        </div>
      )}
    </div>
  );
}
