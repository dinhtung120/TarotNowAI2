"use client";

/*
 * ===================================================================
 * FILE: (user)/profile/page.tsx (Hồ Sơ Cá Nhân)
 * BỐI CẢNH (CONTEXT):
 *   Trang xem và cập nhật thông tin cá nhân của User (Hiển thị Avatar, Tên, Cung Hoàng Đạo, Thần Số Học, EXP/Level).
 * 
 * DATA FLOW & BẢO MẬT:
 *   - Auth Guard: Redirect về `/login` nếu Zustand báo chưa đăng nhập.
 *   - Tích hợp `react-hook-form` + `zod` để validate form chỉnh sửa thông tin.
 *   - Gọi trực tiếp `getProfileAction` / `updateProfileAction` từ Server Actions đảm bảo che giấu Endpoint.
 * ===================================================================
 */

import { useTranslations } from "next-intl";
import { useEffect, useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useRouter } from "@/i18n/routing";
import { useAuthStore } from "@/store/authStore";
import { getProfileAction, updateProfileAction } from "@/actions/profileActions";
import Image from "next/image";
import { Sparkles, User, Calendar, AtSign, ShieldCheck, Save, Loader2, Trophy,
 Stars, ArrowUpRight, Clock, XCircle
} from "lucide-react";
import { SectionHeader, GlassCard, Button } from "@/components/ui";
import { getMyReaderRequest, type MyReaderRequest } from "@/actions/readerActions";

/**
 * DTO (Data Transfer Object) — Mô tả cấu trúc dữ liệu trả về từ API Profile.
 */
interface ProfileResponse {
 id: string;
 email: string;
 username: string;
 displayName: string;
 avatarUrl: string | null;
 dateOfBirth: string; // ISO String
 zodiac: string;
 numerology: number;
 level: number;
 exp: number;
 hasConsented: boolean;
}

/**
 * Zod Schema — Validate dữ liệu form trước khi gửi lên server.
 */
interface ProfileFormValues {
 displayName: string;
 avatarUrl?: string;
 dateOfBirth: string;
}

export default function ProfilePage() {
 const t = useTranslations("Profile");
 const tCommon = useTranslations("Common");
 const router = useRouter();
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const user = useAuthStore((state) => state.user);

 const [profileData, setProfileData] = useState<ProfileResponse | null>(null);
 const [loading, setLoading] = useState(true);
 const [successMsg, setSuccessMsg] = useState("");
 const [errorMsg, setErrorMsg] = useState("");

 /*
  * State cho Reader Upgrade Card:
  * - readerRequest: trạng thái đơn xin Reader hiện tại (pending/approved/rejected/null)
  * - readerRequestLoading: cờ loading riêng để không block toàn bộ profile page
  */
 const [readerRequest, setReaderRequest] = useState<MyReaderRequest | null>(null);
 const [readerRequestLoading, setReaderRequestLoading] = useState(false);

 const profileSchema = useMemo(() => z.object({
  displayName: z.string().min(2, t("validation.display_name_min")),
  avatarUrl: z.string().url(t("validation.avatar_url_invalid")).optional().or(z.literal("")),
  dateOfBirth: z.string().refine((date) => !isNaN(Date.parse(date)), {
   message: t("validation.date_of_birth_invalid"),
  }),
 }), [t]);

 const {
 register,
 handleSubmit,
 setValue,
 formState: { errors, isSubmitting },
 } = useForm<ProfileFormValues>({
 resolver: zodResolver(profileSchema),
 });

 /**
 * Auth Guard
 */
 useEffect(() => {
 if (!isAuthenticated) {
 router.push("/login");
 }
 }, [isAuthenticated, router]);

 /**
 * Fetch dữ liệu Profile
 */
 useEffect(() => {
 if (!isAuthenticated) return;

 async function fetchProfile() {
 try {
 const result = await getProfileAction();

 if (result.error) {
 setErrorMsg(result.error);
 return;
 }

 if (result.success && result.data) {
 const data = result.data as ProfileResponse;
 setProfileData(data);

 // Điền dữ liệu vào React Hook Form
 setValue("displayName", data.displayName);
 setValue("avatarUrl", data.avatarUrl || "");

 // Chuyển đổi ISO String → yyyy-MM-dd (format cần thiết cho input type="date")
 if (data.dateOfBirth) {
 const dateObj = new Date(data.dateOfBirth);
 const yyyy = dateObj.getFullYear();
 const mm = String(dateObj.getMonth() + 1).padStart(2, "0");
 const dd = String(dateObj.getDate()).padStart(2, "0");
 setValue("dateOfBirth", `${yyyy}-${mm}-${dd}`);
 }
 }
 } catch {
 setErrorMsg(t("errorLoad"));
 } finally {
 setLoading(false);
 }
 }
 fetchProfile();
 }, [isAuthenticated, setValue, t]);

 /*
  * useEffect #3: Fetch trạng thái đơn xin Reader (chỉ cho user thường).
  * LÝ DO TÁCH RIÊNG:
  *   - Không block profile loading (chạy song song)
  *   - Chỉ gọi khi user chưa phải reader/admin
  *   - getMyReaderRequest() trả về null nếu chưa nộp đơn
  */
 useEffect(() => {
 if (!isAuthenticated || !user) return;
 /* Chỉ fetch khi user là "user" thường — reader/admin không cần */
 if (user.role === 'reader' || user.role === 'admin') return;

 setReaderRequestLoading(true);
 getMyReaderRequest().then((result) => {
  setReaderRequest(result);
  setReaderRequestLoading(false);
 });
  // eslint-disable-next-line react-hooks/exhaustive-deps
 }, [isAuthenticated, user?.role]);

 /**
 * Xử lý Submit Form
 */
 const onSubmit = async (data: ProfileFormValues) => {
 setSuccessMsg("");
 setErrorMsg("");
 try {
 const result = await updateProfileAction({
 displayName: data.displayName,
 avatarUrl: data.avatarUrl || null,
 dateOfBirth: new Date(data.dateOfBirth).toISOString(),
 });

 if (result.error) {
 setErrorMsg(result.error);
 return;
 }

 setSuccessMsg(t("successMsg"));

 // Reload lại dữ liệu để cập nhật Zodiac/Numerology
 const profileResult = await getProfileAction();
 if (profileResult.success && profileResult.data) {
 setProfileData(profileResult.data as ProfileResponse);
 }
 } catch {
 setErrorMsg(t("errorSave"));
 }
 };

 // Loading spinner khi đang fetch dữ liệu
 if (loading) {
 return (
 <div className="h-[60vh] flex flex-col items-center justify-center space-y-6">
 <div className="relative group">
 <div className="absolute inset-x-0 top-0 h-40 w-40 bg-[var(--purple-accent)]/20 blur-[60px] rounded-full animate-pulse" />
 <Loader2 className="w-12 h-12 animate-spin text-[var(--purple-accent)] relative z-10" />
 </div>
 <div className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">{t("loading")}</div>
 </div>
 );
 }

 return (
 <div className="max-w-3xl mx-auto px-4 sm:px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000">
 {/* Header */}
 <SectionHeader
 tag={t("title")}
 tagIcon={<Stars className="w-3 h-3 text-[var(--purple-accent)]" />}
 title={t("title")}
 subtitle={t("subtitle")}
 />

 <div className="grid grid-cols-1 gap-8">
 {/* Profile Summary Card */}
 {profileData && (
 <GlassCard className="!p-6 sm:!p-8 overflow-hidden relative group">
 <div className="relative z-10 flex flex-col md:flex-row items-center md:items-start gap-8">
 {/* Avatar */}
 <div className="relative w-28 h-28 shrink-0">
 <div className="absolute inset-[-6px] rounded-full bg-gradient-to-tr from-[var(--purple-accent)]/30 via-transparent to-[var(--warning)]/20 blur-xl group-hover:inset-[-12px] transition-all duration-700 opacity-70" />
 <div className="w-full h-full rounded-full border-2 tn-border overflow-hidden relative z-10 shadow-2xl tn-surface">
 <Image
 src={profileData.avatarUrl || `https://ui-avatars.com/api/?background=111&color=fff&name=${encodeURIComponent(profileData.displayName)}`}
 alt={tCommon("avatar_alt")}
 fill
 sizes="112px"
 unoptimized
 className="object-cover transition-transform duration-1000 group-hover:scale-110"
 />
 </div>
 <div className="absolute -bottom-1 -right-1 w-8 h-8 tn-panel rounded-xl flex items-center justify-center text-[var(--warning)] shadow-xl z-20">
 <Trophy className="w-4 h-4" />
 </div>
 </div>

 <div className="flex-1 text-center md:text-left space-y-5">
 <div>
 <h2 className="text-3xl font-black tn-text-primary italic tracking-tighter mb-1">
 {profileData.displayName}
 </h2>
 <div className="flex items-center justify-center md:justify-start gap-1.5 text-[var(--text-secondary)] font-medium">
 <AtSign className="w-3 h-3" />
 <span className="text-sm">@{profileData.username}</span>
 </div>
 </div>

 {/* Attributes Badges */}
 <div className="flex flex-wrap gap-3 justify-center md:justify-start">
 <div className="flex items-center gap-2 px-3 py-1.5 rounded-xl tn-panel hover:tn-surface-strong transition-all duration-300">
 <div className="w-2 h-2 rounded-full bg-[var(--purple-accent)] shadow-[0_0_8px_var(--c-168-85-247-60)]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">{t("level")}</span>
 <span className="text-xs font-black tn-text-primary ml-0.5">{profileData.level}</span>
 </div>
 <div className="flex items-center gap-2 px-3 py-1.5 rounded-xl tn-panel hover:tn-surface-strong transition-all duration-300">
 <div className="w-2 h-2 rounded-full bg-[var(--info)] shadow-[0_0_8px_var(--c-59-130-246-60)]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">{t("zodiac")}</span>
 <span className="text-xs font-black tn-text-primary ml-0.5">{profileData.zodiac}</span>
 </div>
 <div className="flex items-center gap-2 px-3 py-1.5 rounded-xl tn-panel hover:tn-surface-strong transition-all duration-300">
 <div className="w-2 h-2 rounded-full bg-[var(--warning)] shadow-[0_0_8px_var(--c-245-158-11-60)]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">{t("numerology")}</span>
 <span className="text-xs font-black tn-text-primary ml-0.5">{profileData.numerology}</span>
 </div>
 </div>

 {/* Action Buttons */}
 <div className="flex flex-col sm:flex-row gap-3 pt-2">
 {user?.role === "admin" && (
 <button
 onClick={() => router.push("/admin/users")}
 className="flex-1 group flex justify-center items-center gap-2.5 bg-[var(--purple-accent)]/10 hover:bg-[var(--purple-accent)]/20 border border-[var(--purple-accent)]/30 text-[var(--purple-accent)] px-5 py-2.5 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all hover:scale-[1.02] active:scale-95"
 >
 <ShieldCheck className="w-3.5 h-3.5 transition-transform group-hover:rotate-12" />
 {t("admin_portal")}
 </button>
 )}
 {(user?.role === "admin" || user?.role === "reader") && (
 <button
 onClick={() => router.push("/profile/reader")}
 className="flex-1 group flex justify-center items-center gap-2.5 bg-[var(--warning)]/10 hover:bg-[var(--warning)]/20 border border-[var(--warning)]/30 text-[var(--warning)] px-5 py-2.5 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all hover:scale-[1.02] active:scale-95"
 >
 <Sparkles className="w-3.5 h-3.5 transition-transform group-hover:rotate-12" />
 {t("reader_profile")}
 </button>
 )}
 </div>
 </div>
 </div>
 </GlassCard>
 )}

 {/* ===== READER UPGRADE CARD =====
  Hiển thị CHỈ KHI user là người dùng thường (role !== reader/admin).
  3 trạng thái:
  1. Chưa nộp đơn → CTA button đi đến /reader/apply
  2. Đơn đang chờ (pending) → Badge vàng + thông báo
  3. Đơn bị từ chối (rejected) → Badge đỏ + nút nộp lại
 */}
 {user?.role !== "admin" && user?.role !== "reader" && !readerRequestLoading && (
  <GlassCard className="!p-6 sm:!p-8 overflow-hidden relative group">
  <div className="absolute top-0 right-0 p-6 opacity-5 pointer-events-none transition-transform duration-700 group-hover:scale-110 group-hover:rotate-12">
   <Sparkles className="w-32 h-32 text-[var(--purple-accent)]" />
  </div>
  <div className="relative z-10 space-y-5">
   {/* Header */}
   <div className="flex items-center gap-3">
   <div className="w-12 h-12 rounded-2xl bg-gradient-to-br from-[var(--purple-accent)]/20 to-[var(--warning)]/10 flex items-center justify-center border border-[var(--purple-accent)]/20 shadow-xl">
    <Sparkles className="w-6 h-6 text-[var(--purple-accent)]" />
   </div>
   <div>
    <h3 className="text-lg font-black tn-text-primary italic tracking-tight">
    {t("upgrade.title")}
    </h3>
    <p className="text-[10px] font-bold uppercase tracking-widest text-[var(--text-secondary)]">
    {t("upgrade.subtitle")}
    </p>
   </div>
   </div>

   {/* STATE 1: Chưa nộp đơn hoặc không có request */}
   {(!readerRequest || !readerRequest.hasRequest) && (
   <>
    <p className="text-sm text-[var(--text-secondary)] leading-relaxed">
    {t("upgrade.desc")}
    </p>
    <div className="flex flex-wrap gap-3 pt-1">
    <button
     onClick={() => router.push("/reader/apply")}
     className="group/btn flex items-center gap-2.5 bg-gradient-to-r from-[var(--purple-accent)] to-[var(--purple-accent)] hover:brightness-110 px-6 py-3 rounded-xl text-[10px] font-black uppercase tracking-widest tn-text-primary transition-all hover:scale-[1.02] active:scale-95 shadow-xl cursor-pointer"
    >
     <ArrowUpRight className="w-3.5 h-3.5 transition-transform group-hover/btn:translate-x-0.5 group-hover/btn:-translate-y-0.5" />
     {t("upgrade.cta")}
    </button>
    </div>
   </>
   )}

   {/* STATE 2: Đơn đang chờ duyệt (pending) */}
   {readerRequest?.hasRequest && readerRequest.status === 'pending' && (
   <div className="p-4 rounded-xl bg-[var(--warning)]/5 border border-[var(--warning)]/20 space-y-2">
    <div className="flex items-center gap-2">
    <Clock className="w-4 h-4 text-[var(--warning)]" />
    <span className="text-[10px] font-black uppercase tracking-widest text-[var(--warning)]">
     {t("upgrade.pending_title")}
    </span>
    </div>
    <p className="text-xs text-[var(--text-secondary)] leading-relaxed">
    {t("upgrade.pending_desc")}
    </p>
   </div>
   )}

   {/* STATE 3: Đơn bị từ chối (rejected) → cho phép nộp lại */}
   {readerRequest?.hasRequest && readerRequest.status === 'rejected' && (
   <div className="space-y-3">
    <div className="p-4 rounded-xl bg-[var(--danger)]/5 border border-[var(--danger)]/20 space-y-2">
    <div className="flex items-center gap-2">
     <XCircle className="w-4 h-4 text-[var(--danger)]" />
     <span className="text-[10px] font-black uppercase tracking-widest text-[var(--danger)]">
     {t("upgrade.rejected_title")}
     </span>
    </div>
    {readerRequest.adminNote && (
     <p className="text-xs text-[var(--text-secondary)] leading-relaxed">
     {t("upgrade.rejected_reason", { note: readerRequest.adminNote })}
     </p>
    )}
    </div>
    <button
    onClick={() => router.push("/reader/apply")}
    className="group/btn flex items-center gap-2.5 bg-[var(--purple-accent)]/10 hover:bg-[var(--purple-accent)]/20 border border-[var(--purple-accent)]/30 text-[var(--purple-accent)] px-5 py-2.5 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all hover:scale-[1.02] active:scale-95 cursor-pointer"
    >
    <ArrowUpRight className="w-3.5 h-3.5 transition-transform group-hover/btn:translate-x-0.5 group-hover/btn:-translate-y-0.5" />
    {t("upgrade.reapply_cta")}
    </button>
   </div>
   )}
  </div>
  </GlassCard>
 )}

 {/* Settings Form Section */}
 <GlassCard className="!p-6 sm:!p-8">
 <h3 className="text-lg font-black tn-text-primary italic tracking-tight mb-8 flex items-center gap-2.5">
 <Sparkles className="w-4 h-4 text-[var(--warning)]" />
 {t("settings_title")}
 </h3>

 {/* Status Messages */}
 <div className="space-y-3 mb-8">
 {successMsg && (
 <div className="animate-in slide-in-from-top-2 duration-500 bg-[var(--success-bg)] border border-[var(--success)]/30 p-4 rounded-xl flex items-center gap-3 text-[var(--success)] text-xs font-bold uppercase tracking-widest">
 <ShieldCheck className="w-4 h-4" />
 {successMsg}
 </div>
 )}
 {errorMsg && (
 <div className="animate-in slide-in-from-top-2 duration-500 bg-[var(--danger-bg)] border border-[var(--danger)]/30 p-4 rounded-xl flex items-center gap-3 text-[var(--danger)] text-xs font-bold uppercase tracking-widest">
 <ShieldCheck className="w-4 h-4" />
 {errorMsg}
 </div>
 )}
 </div>

 <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
 <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
 <div className="space-y-2">
 <label className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] ml-1">
 <User className="w-3.5 h-3.5" />
 {t("displayName")}
 </label>
 <input
 type="text"
 {...register("displayName")}
 className="w-full tn-field rounded-xl px-4 py-3.5 text-sm tn-text-primary font-medium tn-field-accent transition-all shadow-inner"
 placeholder={t("display_name_placeholder")}
 />
 {errors.displayName && <p className="text-[var(--danger)] text-[10px] font-bold ml-1 italic">{errors.displayName.message}</p>}
 </div>

 <div className="space-y-2">
 <label className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] ml-1">
 <Calendar className="w-3.5 h-3.5" />
 {t("dateOfBirth")}
 </label>
 <input
 type="date"
 {...register("dateOfBirth")}
 className="w-full tn-field rounded-xl px-4 py-3.5 text-sm tn-text-primary font-medium tn-field-accent transition-all shadow-inner "
 />
 {errors.dateOfBirth && <p className="text-[var(--danger)] text-[10px] font-bold ml-1 italic">{errors.dateOfBirth.message}</p>}
 </div>

 <div className="space-y-2 md:col-span-2">
 <label className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] ml-1">
 <AtSign className="w-3.5 h-3.5" />
 {t("avatarUrl")}
 </label>
 <input
 type="text"
 {...register("avatarUrl")}
 className="w-full tn-field rounded-xl px-4 py-3.5 text-sm tn-text-primary font-medium tn-field-accent transition-all shadow-inner"
 placeholder={t("avatar_url_placeholder")}
 />
 {errors.avatarUrl && <p className="text-[var(--danger)] text-[10px] font-bold ml-1 italic">{errors.avatarUrl.message}</p>}
 </div>
 </div>

 <div className="pt-4 mt-8 border-t tn-border">
 <Button
 variant="primary"
 type="submit"
 disabled={isSubmitting}
 className="w-full h-12"
 >
 {isSubmitting ? (
 <>
 <Loader2 className="w-4 h-4 animate-spin mr-2" />
 {t("saving")}
 </>
 ) : (
 <>
 <Save className="w-4 h-4 mr-2" />
 {t("save")}
 </>
 )}
 </Button>
 </div>
 </form>
 </GlassCard>
 </div>
 </div>
 );
}
