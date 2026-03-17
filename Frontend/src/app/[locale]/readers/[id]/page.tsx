'use client';

import React, { useEffect, useState } from 'react';
import { getReaderProfile, type ReaderProfile } from '@/actions/readerActions';
import {
  Star, Gem, MessageCircle, Loader2, Sparkles, ArrowLeft, Clock, CheckCircle2, User
} from 'lucide-react';
import { useRouter } from '@/i18n/routing';
import { useParams } from 'next/navigation';
import { createConversation } from '@/actions/chatActions';
import toast from 'react-hot-toast';

/**
 * Trang hồ sơ Reader chi tiết (Public).
 *
 * Hiển thị:
 * → Avatar, tên hiển thị, trạng thái online.
 * → Bio đa ngôn ngữ (ưu tiên VI → EN → ZH).
 * → Danh sách chuyên môn.
 * → Thống kê (rating, reviews).
 * → Giá dịch vụ.
 * → Nút liên hệ (Phase tương lai).
 */
export default function ReaderProfilePage() {
  const params = useParams();
  const router = useRouter();
  const userId = params.id as string;

  const [profile, setProfile] = useState<ReaderProfile | null>(null);
  const [loading, setLoading] = useState(true);
  const [startingChat, setStartingChat] = useState(false);

  useEffect(() => {
    if (!userId) return;
    const fetchProfile = async () => {
      const result = await getReaderProfile(userId);
      setProfile(result);
      setLoading(false);
    };
    fetchProfile();
  }, [userId]);

  if (loading) {
    return (
      <div className="min-h-[60vh] flex flex-col items-center justify-center space-y-4">
        <Loader2 className="w-10 h-10 text-purple-500 animate-spin" />
        <span className="text-[10px] font-black uppercase tracking-[0.3em] text-zinc-600">Đang tải hồ sơ...</span>
      </div>
    );
  }

  if (!profile) {
    return (
      <div className="min-h-[60vh] flex flex-col items-center justify-center space-y-4">
        <User className="w-16 h-16 text-zinc-800" />
        <p className="text-zinc-600 text-sm font-medium">Không tìm thấy hồ sơ Reader.</p>
        <button
          onClick={() => router.push('/readers' as any)}
          className="flex items-center gap-2 text-xs text-purple-400 hover:text-purple-300 transition-colors"
        >
          <ArrowLeft className="w-3 h-3" />
          Quay lại danh sách
        </button>
      </div>
    );
  }

  // Lấy bio theo thứ tự ưu tiên: VI → EN → ZH
  const bio = profile.bioVi || profile.bioEn || profile.bioZh || 'Chưa có mô tả.';

  const getStatusBadge = () => {
    switch (profile.status) {
      case 'accepting_questions':
        return (
          <div className="inline-flex items-center gap-2 px-3 py-1.5 rounded-full bg-emerald-500/10 border border-emerald-500/20">
            <div className="w-2 h-2 rounded-full bg-emerald-400 animate-pulse" />
            <span className="text-[10px] font-black uppercase tracking-widest text-emerald-400">Đang nhận câu hỏi</span>
          </div>
        );
      case 'online':
        return (
          <div className="inline-flex items-center gap-2 px-3 py-1.5 rounded-full bg-amber-500/10 border border-amber-500/20">
            <div className="w-2 h-2 rounded-full bg-amber-400" />
            <span className="text-[10px] font-black uppercase tracking-widest text-amber-400">Trực tuyến</span>
          </div>
        );
      default:
        return (
          <div className="inline-flex items-center gap-2 px-3 py-1.5 rounded-full bg-zinc-500/10 border border-zinc-500/20">
            <div className="w-2 h-2 rounded-full bg-zinc-600" />
            <span className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Ngoại tuyến</span>
          </div>
        );
    }
  };

  return (
    <div className="max-w-3xl mx-auto px-6 py-16 space-y-10 animate-in fade-in slide-in-from-bottom-8 duration-1000">
      {/* Back Button */}
      <button
        onClick={() => router.push('/readers' as any)}
        className="flex items-center gap-2 text-xs text-zinc-500 hover:text-purple-400 transition-colors"
      >
        <ArrowLeft className="w-3 h-3" />
        <span className="font-medium">Quay lại danh sách</span>
      </button>

      {/* Profile Card */}
      <div className="relative overflow-hidden bg-gradient-to-br from-purple-500/10 to-transparent backdrop-blur-3xl rounded-[3rem] border border-purple-500/20 p-12 shadow-2xl">
        <div className="absolute top-0 right-0 p-10 opacity-10 pointer-events-none">
          <Sparkles size={180} className="text-purple-400" />
        </div>

        <div className="relative z-10 space-y-8">
          {/* Avatar + Name + Status */}
          <div className="flex flex-col md:flex-row items-center gap-6">
            <div className="w-20 h-20 rounded-2xl bg-gradient-to-br from-purple-500/30 to-purple-600/20 border border-purple-500/30 flex items-center justify-center text-3xl font-black text-purple-400 shadow-xl">
              {profile.displayName?.charAt(0)?.toUpperCase() || '?'}
            </div>
            <div className="text-center md:text-left space-y-2">
              <h1 className="text-3xl font-black text-white uppercase italic tracking-tighter">
                {profile.displayName || 'Reader'}
              </h1>
              {getStatusBadge()}
            </div>
          </div>

          {/* Stats Row */}
          <div className="grid grid-cols-3 gap-4">
            <div className="p-6 rounded-2xl bg-black/40 border border-white/5 text-center space-y-1">
              <Star className="w-5 h-5 text-amber-400 mx-auto" />
              <div className="text-2xl font-black text-white italic">
                {profile.avgRating > 0 ? profile.avgRating.toFixed(1) : '--'}
              </div>
              <div className="text-[9px] font-black uppercase tracking-widest text-zinc-600">Đánh giá</div>
            </div>
            <div className="p-6 rounded-2xl bg-black/40 border border-white/5 text-center space-y-1">
              <MessageCircle className="w-5 h-5 text-purple-400 mx-auto" />
              <div className="text-2xl font-black text-white italic">
                {profile.totalReviews}
              </div>
              <div className="text-[9px] font-black uppercase tracking-widest text-zinc-600">Lượt đánh</div>
            </div>
            <div className="p-6 rounded-2xl bg-black/40 border border-white/5 text-center space-y-1">
              <Gem className="w-5 h-5 text-purple-400 mx-auto" />
              <div className="text-2xl font-black text-white italic">
                {profile.diamondPerQuestion}
              </div>
              <div className="text-[9px] font-black uppercase tracking-widest text-zinc-600">💎/Câu hỏi</div>
            </div>
          </div>

          {/* Bio */}
          <div className="space-y-3">
            <div className="text-[10px] font-black uppercase tracking-widest text-purple-400">Giới thiệu</div>
            <p className="text-sm text-zinc-400 leading-relaxed">{bio}</p>
          </div>

          {/* Specialties */}
          {profile.specialties.length > 0 && (
            <div className="space-y-3">
              <div className="text-[10px] font-black uppercase tracking-widest text-purple-400">Chuyên môn</div>
              <div className="flex flex-wrap gap-2">
                {profile.specialties.map((spec) => (
                  <span
                    key={spec}
                    className="px-3 py-1.5 rounded-full bg-purple-500/10 text-purple-400 text-[10px] font-bold uppercase tracking-wider border border-purple-500/15"
                  >
                    {spec}
                  </span>
                ))}
              </div>
            </div>
          )}

          {/* CTA Button — Bắt đầu trò chuyện */}
          {profile.status === 'accepting_questions' && (
            <button
              id="reader-contact-btn"
              disabled={startingChat}
              onClick={async () => {
                setStartingChat(true);
                const result = await createConversation(profile.userId);
                if (result && result.id) {
                  router.push(`/chat/${result.id}` as any);
                } else {
                  toast.error('Không thể tạo cuộc hội thoại', {
                    style: { background: '#18181b', color: '#fff', border: '1px solid #27272a' }
                  });
                  setStartingChat(false);
                }
              }}
              className="w-full group overflow-hidden bg-gradient-to-r from-emerald-600 to-emerald-500 hover:from-emerald-500 hover:to-emerald-400 rounded-2xl p-5 text-center font-black uppercase tracking-widest text-sm text-white transition-all duration-300 shadow-xl disabled:opacity-50"
            >
              <span className="flex items-center justify-center gap-3">
                {startingChat ? (
                   <Loader2 className="w-4 h-4 animate-spin" />
                ) : (
                   <MessageCircle className="w-4 h-4" />
                )}
                {startingChat ? 'Đang kết nối...' : 'Gửi Câu Hỏi'}
              </span>
            </button>
          )}
        </div>
      </div>

      {/* Member Since */}
      <div className="text-center">
        <div className="text-[10px] font-black uppercase tracking-[0.2em] text-zinc-700">
          Thành viên từ {new Date(profile.createdAt).toLocaleDateString('vi-VN')}
        </div>
      </div>
    </div>
  );
}
