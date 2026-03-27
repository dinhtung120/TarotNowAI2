/*
 * ===================================================================
 * FILE: readers/[id]/page.tsx (Chi Tiết Reader)
 * BỐI CẢNH (CONTEXT):
 *   Trang xem thông tin chi tiết một Reader (Nhà xem bài).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Hiển thị Avatar, Tên, Trạng thái (Online/Accepting), Đánh giá và Feedback.
 *   - "Bắt đầu Chat": Nút gọi Server Action `createConversation` để tạo phòng 
 *     trò chuyện (nếu Reader đang chấp nhận câu hỏi).
 * ===================================================================
 */
'use client';

import React from 'react';
import {
 Star, Gem, MessageCircle, Loader2, Sparkles, ArrowLeft, Clock, User,
} from 'lucide-react';
import { useLocale, useTranslations } from 'next-intl';
import { GlassCard, Button } from '@/shared/components/ui';
import { useReaderPublicProfilePage } from '@/features/reader/application/useReaderPublicProfilePage';
import { normalizeReaderStatus } from '@/features/reader/domain/readerStatus';

export default function ReaderProfilePage() {
 const t = useTranslations("Readers");
 const locale = useLocale();
 const { router, profile, loading, startChat, startingChat } = useReaderPublicProfilePage(t);

 if (loading) {
 return (
 <div className="h-[60vh] flex flex-col items-center justify-center space-y-6">
 <div className="relative group">
 <div className="absolute inset-x-0 top-0 h-40 w-40 bg-[var(--purple-accent)]/20 blur-[60px] rounded-full animate-pulse" />
 <Loader2 className="w-12 h-12 animate-spin text-[var(--purple-accent)] relative z-10" />
 </div>
 <div className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">{t("profile.loading")}</div>
 </div>
 );
 }

 if (!profile) {
 return (
 <div className="h-[60vh] flex flex-col items-center justify-center space-y-6">
 <div className="w-24 h-24 tn-panel-soft rounded-full flex items-center justify-center mb-2 shadow-inner">
 <User className="w-10 h-10 text-[var(--text-tertiary)] opacity-50" />
 </div>
 <p className="text-[var(--text-secondary)] text-sm font-medium">{t("profile.not_found")}</p>
 <Button
 variant="ghost"
 onClick={() => router.push('/readers')}
 className="text-[var(--text-secondary)] hover:tn-text-primary mt-4"
 >
 <ArrowLeft className="w-4 h-4 mr-2" />
 {t("profile.back_to_list")}
 </Button>
 </div>
 );
 }

 const bio =
  (
   locale === "vi"
    ? (profile.bioVi || profile.bioEn || profile.bioZh)
    : locale === "en"
     ? (profile.bioEn || profile.bioVi || profile.bioZh)
     : (profile.bioZh || profile.bioEn || profile.bioVi)
  ) || t("directory.bio_fallback");

 const getStatusBadge = () => {
 switch (normalizeReaderStatus(profile.status)) {
 case 'accepting_questions':
 return (
 <div className="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full bg-[var(--success)]/10 border border-[var(--success)]/20 shadow-[0_0_15px_var(--c-16-185-129-10)] ">
 <div className="w-2 h-2 rounded-full bg-[var(--success)] animate-pulse shadow-[0_0_8px_currentColor]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--success)]">{t("profile.status.accepting")}</span>
 </div>
 );
 case 'online':
 return (
 <div className="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full bg-[var(--warning)]/10 border border-[var(--warning)]/20 shadow-[0_0_15px_var(--c-245-158-11-10)] ">
 <div className="w-2 h-2 rounded-full bg-[var(--warning)] shadow-[0_0_8px_currentColor]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--warning)]">{t("profile.status.online")}</span>
 </div>
 );
 case 'away':
 return (
 <div className="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full bg-[var(--warning)]/10 border border-[var(--warning)]/20 shadow-[0_0_15px_var(--c-245-158-11-10)] ">
 <div className="w-2 h-2 rounded-full bg-[var(--warning)] shadow-[0_0_8px_currentColor]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--warning)]">{t("profile.status.away")}</span>
 </div>
 );
 default:
 return (
 <div className="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full tn-surface-strong border tn-border ">
 <div className="w-2 h-2 rounded-full bg-[var(--text-muted)]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("profile.status.offline")}</span>
 </div>
 );
 }
 };

 return (
 <div className="max-w-3xl mx-auto px-4 sm:px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000">
 {/* Back Button */}
 <button
 onClick={() => router.push('/readers')}
 className="flex items-center gap-2 text-[11px] font-black uppercase tracking-widest text-[var(--text-secondary)] hover:tn-text-primary transition-colors group"
 >
 <ArrowLeft className="w-4 h-4 text-[var(--purple-accent)] group-hover:-translate-x-1 transition-transform" />
 <span>{t("profile.back_to_list")}</span>
 </button>

 {/* Profile Card */}
 <GlassCard className="relative overflow-hidden !p-0 !rounded-[3rem] tn-border">
 {/* Cover Background */}
 <div className="absolute inset-0 bg-gradient-to-b from-[var(--purple-accent)]/20 via-transparent to-transparent opacity-50" />
 <div className="absolute top-0 right-0 p-10 opacity-[0.03] pointer-events-none">
 <Sparkles size={240} className="text-[var(--purple-accent)]" />
 </div>

 <div className="relative z-10 p-6 sm:p-8 md:p-14 space-y-10 md:space-y-12">
 {/* Header: Avatar + Info */}
 <div className="flex flex-col md:flex-row items-center gap-8 text-center md:text-left">
 <div className="relative w-28 h-28 shrink-0 group">
 <div className="absolute inset-0 bg-gradient-to-br from-[var(--purple-accent)]/50 to-[var(--warning)]/30 rounded-[2rem] blur-xl opacity-60 group-hover:opacity-100 transition-opacity duration-500" />
 <div className="w-full h-full rounded-[2rem] tn-surface-strong border-2 tn-border flex items-center justify-center text-5xl font-black tn-text-primary relative z-10 shadow-2xl overflow-hidden">
 {profile.displayName?.charAt(0)?.toUpperCase() || '?'}
 </div>
 </div>
 <div className="space-y-4">
 <h1 className="text-4xl md:text-5xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-lg">
 {profile.displayName || t("directory.reader_fallback")}
 </h1>
 <div className="flex flex-wrap justify-center md:justify-start gap-3">
 {getStatusBadge()}
 <div className="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full tn-panel ">
 <Gem className="w-3.5 h-3.5 text-[var(--purple-accent)]" />
 <span className="text-[10px] font-black uppercase tracking-widest tn-text-primary">{profile.diamondPerQuestion} {t("profile.diamond_per_question_suffix")}</span>
 </div>
 </div>
 </div>
 </div>

 {/* Stats Row */}
 <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
 <div className="p-6 rounded-3xl tn-panel-soft text-center space-y-2 hover:tn-surface-strong transition-colors shadow-inner">
 <Star className="w-6 h-6 text-[var(--warning)] mx-auto mb-3" fill="currentColor" />
 <div className="text-3xl font-black tn-text-primary italic drop-shadow-md">
 {profile.avgRating > 0 ? profile.avgRating.toFixed(1) : '--'}
 </div>
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">{t("profile.rating_label")}</div>
 </div>
 <div className="p-6 rounded-3xl tn-panel-soft text-center space-y-2 hover:tn-surface-strong transition-colors shadow-inner">
 <MessageCircle className="w-6 h-6 text-[var(--purple-accent)] mx-auto mb-3" />
 <div className="text-3xl font-black tn-text-primary italic drop-shadow-md">
 {profile.totalReviews}
 </div>
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">{t("profile.reviews_label")}</div>
 </div>
 </div>

 {/* Bio & Specialties */}
 <div className="space-y-8 tn-overlay-soft p-8 rounded-3xl border tn-border-soft shadow-inner">
 <div className="space-y-4">
 <div className="flex items-center gap-2 text-[11px] font-black uppercase tracking-widest text-[var(--purple-accent)]">
 <User className="w-4 h-4" /> {t("profile.soul_link")}
 </div>
 <p className="text-[15px] font-medium text-[var(--text-secondary)] leading-relaxed italic border-l-2 border-[var(--purple-accent)]/30 pl-4 py-1">
 &ldquo;{bio}&rdquo;
 </p>
 </div>

 {profile.specialties.length > 0 && (
 <div className="space-y-4 pt-6 border-t tn-border-soft">
 <div className="flex items-center gap-2 text-[11px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">
 <Sparkles className="w-4 h-4" /> {t("profile.specialties_title")}
 </div>
 <div className="flex flex-wrap gap-2.5">
 {profile.specialties.map((spec) => (
 <span
 key={spec}
 className="px-4 py-2 rounded-xl tn-surface tn-text-primary text-[11px] font-black uppercase tracking-widest border tn-border shadow-sm hover:border-[var(--purple-accent)]/50 transition-colors cursor-default"
 >
 {spec}
 </span>
 ))}
 </div>
 </div>
 )}
 </div>
 </div>
 </GlassCard>

 {/* Member Since Footer */}
 <div className="text-center pt-8">
 <Button
 onClick={startChat}
 disabled={startingChat}
 className="mb-4 px-6 py-3"
 >
 {startingChat ? <Loader2 className="w-4 h-4 animate-spin mr-2" /> : <MessageCircle className="w-4 h-4 mr-2" />}
 {t("profile.cta_send_question")}
 </Button>

 <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full tn-panel text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] shadow-inner">
 <Clock className="w-3 h-3 text-[var(--text-tertiary)]" />
 {t("profile.member_since", { date: new Date(profile.createdAt).toLocaleDateString(locale) })}
 </div>
 </div>
 </div>
 );
}
