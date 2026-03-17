'use client';

import React, { useEffect, useState } from 'react';
import { listConversations, type ConversationDto } from '@/actions/chatActions';
import {
  MessageCircle, Loader2, Sparkles, ChevronRight, Inbox
} from 'lucide-react';
import { useRouter } from '@/i18n/routing';
import UserLayout from '@/components/layout/UserLayout';
import { SectionHeader, GlassCard } from '@/components/ui';

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
      case 'active': return { text: 'Đang chat', color: 'var(--success)', bg: 'var(--success-bg)' };
      case 'pending': return { text: 'Chờ phản hồi', color: 'var(--warning)', bg: 'var(--warning-bg)' };
      case 'completed': return { text: 'Hoàn thành', color: 'var(--text-secondary)', bg: 'rgba(255,255,255,0.05)' };
      default: return { text: status, color: 'var(--text-secondary)', bg: 'rgba(255,255,255,0.05)' };
    }
  };

  return (
    <UserLayout>
        <div className="max-w-4xl mx-auto px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000">
            {/* Header */}
            <SectionHeader
                tag="Tin nhắn"
                tagIcon={<Sparkles className="w-3 h-3 text-[var(--purple-accent)]" />}
                title="Hộp Thư"
                subtitle="Quản lý các cuộc trò chuyện với Reader và khách hàng."
            />

            {/* Main Content Area */}
            <div className="grid grid-cols-1 gap-6">
                
                {/* Role Tabs */}
                <div className="flex gap-2 mb-2">
                    {(['user', 'reader'] as const).map((r) => (
                    <button
                        key={r}
                        onClick={() => setRole(r)}
                        className={`px-5 py-2.5 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all border ${
                        role === r
                            ? 'bg-[var(--purple-accent)]/10 border-[var(--purple-accent)]/30 text-[var(--purple-accent)]'
                            : 'bg-white/[0.02] border-white/5 text-[var(--text-secondary)] hover:border-white/10 hover:bg-white/[0.04]'
                        }`}
                    >
                        {r === 'user' ? '🔮 Người hỏi' : '📖 Reader'}
                    </button>
                    ))}
                </div>

                {/* Loading State */}
                {loading && (
                    <GlassCard className="h-[40vh] flex flex-col items-center justify-center space-y-4">
                        <Loader2 className="w-10 h-10 text-[var(--purple-accent)] animate-spin" />
                        <span className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">Đang tải...</span>
                    </GlassCard>
                )}

                {/* Empty State */}
                {!loading && conversations.length === 0 && (
                    <GlassCard className="h-[40vh] flex flex-col items-center justify-center space-y-4 border-dashed border-white/10">
                        <div className="w-16 h-16 rounded-full bg-white/5 flex items-center justify-center mb-2">
                            <Inbox className="w-8 h-8 text-[var(--text-tertiary)]" />
                        </div>
                        <p className="text-[var(--text-secondary)] text-sm font-medium">Chưa có cuộc trò chuyện nào.</p>
                        <p className="text-[10px] text-[var(--text-tertiary)] uppercase tracking-widest">Hãy bắt đầu một kết nối mới</p>
                    </GlassCard>
                )}

                {/* Conversation List */}
                {!loading && conversations.length > 0 && (
                    <div className="space-y-4">
                        {conversations.map((conv) => {
                            const unread = role === 'user' ? conv.unreadCountUser : conv.unreadCountReader;
                            const statusInfo = getStatusLabel(conv.status);
                            const otherUserId = role === 'user' ? conv.readerId : conv.userId;

                            return (
                            <button
                                key={conv.id}
                                onClick={() => router.push(`/chat/${conv.id}` as any)}
                                className="w-full group"
                            >
                                <GlassCard className="flex items-center gap-5 !p-5 hover:border-[var(--purple-accent)]/30 hover:bg-white/[0.03] transition-all duration-300 text-left">
                                    {/* Avatar */}
                                    <div className="relative flex-shrink-0">
                                        <div className="w-14 h-14 rounded-2xl bg-gradient-to-br from-[var(--purple-accent)]/20 to-[var(--purple-accent)]/5 border border-[var(--purple-accent)]/20 flex items-center justify-center text-lg font-black text-[var(--purple-accent)] shadow-[0_0_15px_rgba(168,85,247,0.1)] group-hover:scale-105 transition-transform duration-300">
                                            {otherUserId.charAt(0).toUpperCase()}
                                        </div>
                                        {/* Unread Badge */}
                                        {unread > 0 && (
                                            <div className="absolute -top-1.5 -right-1.5 w-6 h-6 bg-[var(--danger)] rounded-full flex items-center justify-center text-[10px] font-black text-white shadow-lg border-2 border-[#020108] animate-bounce-subtle">
                                                {unread > 9 ? '9+' : unread}
                                            </div>
                                        )}
                                    </div>

                                    {/* Content */}
                                    <div className="flex-1 min-w-0 space-y-1.5">
                                        <div className="flex items-center justify-between">
                                            <span className="text-base font-black text-white truncate group-hover:text-[var(--purple-accent)] transition-colors">
                                                {role === 'user' ? 'Reader ' : 'User '}{otherUserId.substring(0, 8)}...
                                            </span>
                                            <span className="text-[10px] font-bold text-[var(--text-tertiary)] flex-shrink-0 uppercase tracking-widest">
                                                {timeAgo(conv.lastMessageAt)}
                                            </span>
                                        </div>
                                        <div className="flex items-center gap-2">
                                            <span 
                                                className="px-2.5 py-0.5 rounded-full text-[9px] font-black uppercase tracking-widest border border-white/5"
                                                style={{ color: statusInfo.color, backgroundColor: statusInfo.bg, borderColor: `${statusInfo.color}30` }}
                                            >
                                                {statusInfo.text}
                                            </span>
                                        </div>
                                    </div>

                                    <div className="w-10 h-10 rounded-full bg-white/5 flex items-center justify-center group-hover:bg-[var(--purple-accent)]/10 transition-colors shrink-0">
                                        <ChevronRight className="w-5 h-5 text-[var(--text-tertiary)] group-hover:text-[var(--purple-accent)] transition-colors" />
                                    </div>
                                </GlassCard>
                            </button>
                            );
                        })}
                    </div>
                )}
            </div>
        </div>
    </UserLayout>
  );
}
