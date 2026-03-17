'use client';

import React, { useEffect, useState } from 'react';
import { getReaderProfile, type ReaderProfile } from '@/actions/readerActions';
import {
  Star, Gem, MessageCircle, Loader2, Sparkles, ArrowLeft, Clock, CheckCircle2, User, Activity
} from 'lucide-react';
import { useRouter } from '@/i18n/routing';
import { useParams } from 'next/navigation';
import { createConversation } from '@/actions/chatActions';
import toast from 'react-hot-toast';
import UserLayout from '@/components/layout/UserLayout';
import { GlassCard, Button } from '@/components/ui';

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
      <UserLayout>
        <div className="h-[60vh] flex flex-col items-center justify-center space-y-6">
           <div className="relative group">
             <div className="absolute inset-x-0 top-0 h-40 w-40 bg-[var(--purple-accent)]/20 blur-[60px] rounded-full animate-pulse" />
             <Loader2 className="w-12 h-12 animate-spin text-[var(--purple-accent)] relative z-10" />
           </div>
           <div className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">Đang tải hồ sơ...</div>
        </div>
      </UserLayout>
    );
  }

  if (!profile) {
    return (
      <UserLayout>
        <div className="h-[60vh] flex flex-col items-center justify-center space-y-6">
          <div className="w-24 h-24 bg-white/[0.02] border border-white/5 rounded-full flex items-center justify-center mb-2 shadow-inner">
            <User className="w-10 h-10 text-[var(--text-tertiary)] opacity-50" />
          </div>
          <p className="text-[var(--text-secondary)] text-sm font-medium">Không tìm thấy hồ sơ Reader.</p>
          <Button
            variant="ghost"
            onClick={() => router.push('/readers' as any)}
            className="text-[var(--text-secondary)] hover:text-white mt-4"
          >
            <ArrowLeft className="w-4 h-4 mr-2" />
            Quay lại danh sách
          </Button>
        </div>
      </UserLayout>
    );
  }

  // Lấy bio theo thứ tự ưu tiên: VI → EN → ZH
  const bio = profile.bioVi || profile.bioEn || profile.bioZh || 'Chưa có mô tả chi tiết.';

  const getStatusBadge = () => {
    switch (profile.status) {
      case 'accepting_questions':
        return (
          <div className="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full bg-[var(--success)]/10 border border-[var(--success)]/20 shadow-[0_0_15px_rgba(16,185,129,0.1)] backdrop-blur-md">
            <div className="w-2 h-2 rounded-full bg-[var(--success)] animate-pulse shadow-[0_0_8px_currentColor]" />
            <span className="text-[10px] font-black uppercase tracking-widest text-[var(--success)]">Đang nhận câu hỏi</span>
          </div>
        );
      case 'online':
        return (
          <div className="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full bg-[var(--warning)]/10 border border-[var(--warning)]/20 shadow-[0_0_15px_rgba(245,158,11,0.1)] backdrop-blur-md">
            <div className="w-2 h-2 rounded-full bg-[var(--warning)] shadow-[0_0_8px_currentColor]" />
            <span className="text-[10px] font-black uppercase tracking-widest text-[var(--warning)]">Đang bận</span>
          </div>
        );
      default:
        return (
          <div className="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full bg-white/[0.05] border border-white/10 backdrop-blur-md">
            <div className="w-2 h-2 rounded-full bg-zinc-500" />
            <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Ngoại tuyến</span>
          </div>
        );
    }
  };

  return (
    <UserLayout>
      <div className="max-w-3xl mx-auto px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000">
        
        {/* Back Button */}
        <button
          onClick={() => router.push('/readers' as any)}
          className="flex items-center gap-2 text-[11px] font-black uppercase tracking-widest text-[var(--text-secondary)] hover:text-white transition-colors group"
        >
          <ArrowLeft className="w-4 h-4 text-[var(--purple-accent)] group-hover:-translate-x-1 transition-transform" />
          <span>Quay lại Directory</span>
        </button>

        {/* Profile Card */}
        <GlassCard className="relative overflow-hidden !p-0 !rounded-[3rem] border-white/10">
          {/* Cover Background */}
          <div className="absolute inset-0 bg-gradient-to-b from-[var(--purple-accent)]/20 via-transparent to-transparent opacity-50" />
          <div className="absolute top-0 right-0 p-10 opacity-[0.03] pointer-events-none">
            <Sparkles size={240} className="text-[var(--purple-accent)]" />
          </div>

          <div className="relative z-10 p-10 md:p-14 space-y-12">
            
            {/* Header: Avatar + Info */}
            <div className="flex flex-col md:flex-row items-center gap-8 text-center md:text-left">
              <div className="relative w-28 h-28 shrink-0 group">
                 <div className="absolute inset-0 bg-gradient-to-br from-[var(--purple-accent)]/50 to-[var(--warning)]/30 rounded-[2rem] blur-xl opacity-60 group-hover:opacity-100 transition-opacity duration-500" />
                 <div className="w-full h-full rounded-[2rem] bg-zinc-900 border-2 border-white/10 flex items-center justify-center text-5xl font-black text-white relative z-10 shadow-2xl overflow-hidden">
                   {profile.displayName?.charAt(0)?.toUpperCase() || '?'}
                 </div>
              </div>
              
              <div className="space-y-4">
                <h1 className="text-4xl md:text-5xl font-black text-white uppercase italic tracking-tighter drop-shadow-lg">
                  {profile.displayName || 'Reader'}
                </h1>
                <div className="flex flex-wrap justify-center md:justify-start gap-3">
                  {getStatusBadge()}
                  <div className="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full bg-white/[0.03] border border-white/10 backdrop-blur-md">
                     <Gem className="w-3.5 h-3.5 text-[var(--purple-accent)]" />
                     <span className="text-[10px] font-black uppercase tracking-widest text-white">{profile.diamondPerQuestion} / Q</span>
                  </div>
                </div>
              </div>
            </div>

            {/* Stats Row */}
            <div className="grid grid-cols-2 gap-4">
              <div className="p-6 rounded-3xl bg-white/[0.02] border border-white/5 text-center space-y-2 hover:bg-white/[0.04] transition-colors shadow-inner">
                <Star className="w-6 h-6 text-[var(--warning)] mx-auto mb-3" fill="currentColor" />
                <div className="text-3xl font-black text-white italic drop-shadow-md">
                  {profile.avgRating > 0 ? profile.avgRating.toFixed(1) : '--'}
                </div>
                <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">Điểm Đánh Giá</div>
              </div>
              <div className="p-6 rounded-3xl bg-white/[0.02] border border-white/5 text-center space-y-2 hover:bg-white/[0.04] transition-colors shadow-inner">
                <MessageCircle className="w-6 h-6 text-[var(--purple-accent)] mx-auto mb-3" />
                <div className="text-3xl font-black text-white italic drop-shadow-md">
                  {profile.totalReviews}
                </div>
                <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">Lượt Nhận Xét</div>
              </div>
            </div>

            {/* Bio & Specialties */}
            <div className="space-y-8 bg-black/20 p-8 rounded-3xl border border-white/5 shadow-inner">
              <div className="space-y-4">
                <div className="flex items-center gap-2 text-[11px] font-black uppercase tracking-widest text-[var(--purple-accent)]">
                  <User className="w-4 h-4" /> Liên kết tâm hồn
                </div>
                <p className="text-[15px] font-medium text-[var(--text-secondary)] leading-relaxed italic border-l-2 border-[var(--purple-accent)]/30 pl-4 py-1">
                  "{bio}"
                </p>
              </div>

              {profile.specialties.length > 0 && (
                <div className="space-y-4 pt-6 border-t border-white/5">
                  <div className="flex items-center gap-2 text-[11px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">
                     <Sparkles className="w-4 h-4" /> Lĩnh vực chuyên môn
                  </div>
                  <div className="flex flex-wrap gap-2.5">
                    {profile.specialties.map((spec) => (
                      <span
                        key={spec}
                        className="px-4 py-2 rounded-xl bg-white/[0.03] text-white text-[11px] font-black uppercase tracking-widest border border-white/10 shadow-sm hover:border-[var(--purple-accent)]/50 transition-colors cursor-default"
                      >
                        {spec}
                      </span>
                    ))}
                  </div>
                </div>
              )}
            </div>

            {/* CTA Button */}
            {profile.status === 'accepting_questions' ? (
              <Button
                variant="primary"
                disabled={startingChat}
                onClick={async () => {
                  setStartingChat(true);
                  const result = await createConversation(profile.userId);
                  if (result && result.id) {
                    router.push(`/chat/${result.id}` as any);
                  } else {
                    toast.error('Không thể tạo cuộc hội thoại', {
                      style: { background: 'var(--danger-bg)', color: 'var(--danger)', border: '1px solid var(--danger)' }
                    });
                    setStartingChat(false);
                  }
                }}
                className="w-full h-16 text-lg tracking-widest shadow-[0_0_30px_rgba(168,85,247,0.3)] hover:shadow-[0_0_50px_rgba(168,85,247,0.5)] !rounded-2xl"
              >
                {startingChat ? (
                   <>
                     <Loader2 className="w-5 h-5 animate-spin mr-3" />
                     Đang kết nối tâm linh...
                   </>
                ) : (
                   <>
                     <MessageCircle className="w-5 h-5 mr-3" />
                     Gửi Câu Hỏi Ngay
                   </>
                )}
              </Button>
            ) : (
              <div className="w-full h-16 rounded-2xl bg-white/[0.02] border border-white/5 flex items-center justify-center gap-3 shadow-inner">
                 <Activity className="w-5 h-5 text-[var(--text-tertiary)]" />
                 <span className="text-xs font-black uppercase tracking-widest text-[var(--text-secondary)]">Hiện không nhận khách</span>
              </div>
            )}
            
          </div>
        </GlassCard>

        {/* Member Since Footer */}
        <div className="text-center pt-8">
          <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-white/[0.02] border border-white/10 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] shadow-inner">
            <Clock className="w-3 h-3 text-[var(--text-tertiary)]" />
            Tham gia Astral từ {new Date(profile.createdAt).toLocaleDateString('vi-VN')}
          </div>
        </div>
        
      </div>
    </UserLayout>
  );
}
