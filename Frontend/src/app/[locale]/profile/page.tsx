"use client";

/**
 * Trang Hồ Sơ Cá Nhân (Profile Page)
 *
 * Phiên bản sửa lỗi:
 * - [TRƯỚC] Gọi trực tiếp fetch("/api/v1/profile") → URL tương đối → Next.js server → 404
 * - [SAU] Gọi qua Server Actions (profileActions.ts) → URL đúng đến Backend .NET → hoạt động
 *
 * Ngoài ra còn sửa:
 * - Thêm auth guard: redirect về /login nếu chưa đăng nhập
 * - Sử dụng i18n keys từ vi.json thay vì hardcode text
 */

import { useTranslations } from "next-intl";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useRouter } from "@/i18n/routing";
import { useAuthStore } from "@/store/authStore";
import { getProfileAction, updateProfileAction } from "@/actions/profileActions";
import { 
    Sparkles, 
    User, 
    Mail, 
    Calendar, 
    AtSign, 
    ShieldCheck, 
    Save, 
    Loader2, 
    ChevronRight,
    Trophy,
    Stars,
    Hash
} from "lucide-react";

/**
 * DTO (Data Transfer Object) — Mô tả cấu trúc dữ liệu trả về từ API Profile.
 * Tại sao cần interface riêng?
 * - Giúp TypeScript kiểm tra kiểu dữ liệu tại compile-time.
 * - Tái sử dụng được nếu cần hiển thị profile ở nhiều nơi.
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
 * Tại sao dùng Zod thay vì validate thủ công?
 * - Zod tích hợp chặt với react-hook-form qua zodResolver.
 * - Cung cấp error messages chi tiết tự động.
 * - Type-safe: TypeScript tự suy ra kiểu từ schema.
 */
const profileSchema = z.object({
    displayName: z.string().min(2, "Tên hiển thị phải có ít nhất 2 ký tự"),
    avatarUrl: z.string().url("URL Avatar không hợp lệ").optional().or(z.literal("")),
    dateOfBirth: z.string().refine((date) => !isNaN(Date.parse(date)), {
        message: "Ngày sinh không hợp lệ",
    }),
});

type ProfileFormValues = z.infer<typeof profileSchema>;

export default function ProfilePage() {
    const t = useTranslations("Profile");
    const router = useRouter();
    const { isAuthenticated, user } = useAuthStore();

    const [profileData, setProfileData] = useState<ProfileResponse | null>(null);
    const [loading, setLoading] = useState(true);
    const [successMsg, setSuccessMsg] = useState("");
    const [errorMsg, setErrorMsg] = useState("");

    const {
        register,
        handleSubmit,
        setValue,
        formState: { errors, isSubmitting },
    } = useForm<ProfileFormValues>({
        resolver: zodResolver(profileSchema),
    });

    /**
     * Auth Guard — Kiểm tra đăng nhập trước khi render trang.
     * Nếu chưa authenticated, redirect về /login ngay lập tức.
     * Tại sao không dùng middleware? Vì Next.js 16 đang trong quá trình
     * chuyển đổi middleware → proxy, nên guard ở component level an toàn hơn.
     */
    useEffect(() => {
        if (!isAuthenticated) {
            router.push("/login");
        }
    }, [isAuthenticated, router]);

    /**
     * Fetch dữ liệu Profile từ Backend qua Server Action.
     * Chạy 1 lần khi component mount (và khi isAuthenticated thay đổi).
     *
     * Flow: ProfilePage → getProfileAction() → fetch(Backend API) → response
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

    /**
     * Xử lý Submit Form — Gọi Server Action updateProfileAction()
     * Sau khi cập nhật thành công, fetch lại profile để hiển thị
     * Zodiac và Numerology mới (được auto-calc từ DOB phía backend).
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
            <div className="min-h-screen bg-[#020108] flex items-center justify-center">
                <div className="relative group">
                    <div className="absolute inset-x-0 top-0 h-40 w-40 bg-purple-600/20 blur-[60px] rounded-full animate-pulse" />
                    <Loader2 className="w-12 h-12 animate-spin text-purple-400 relative z-10" />
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-[#020108] text-zinc-100 selection:bg-purple-500/40 overflow-x-hidden font-sans">
            {/* ##### PREMIUM BACKGROUND SYSTEM ##### */}
            <div className="fixed inset-0 z-0 pointer-events-none">
                <div className="absolute inset-0 bg-[url('https://grainy-gradients.vercel.app/noise.svg')] opacity-[0.03] mix-blend-overlay"></div>
                <div className="absolute top-1/4 -left-1/4 w-[60vw] h-[60vw] bg-purple-900/[0.08] blur-[120px] rounded-full animate-slow-pulse" />
                <div className="absolute bottom-1/4 -right-1/4 w-[50vw] h-[50vw] bg-indigo-900/[0.06] blur-[130px] rounded-full animate-slow-pulse delay-700" />
                
                {/* Spiritual Particles */}
                <div className="absolute inset-0">
                    {Array.from({ length: 25 }).map((_, i) => (
                        <div
                            key={i}
                            className="absolute w-[1.5px] h-[1.5px] bg-white rounded-full animate-float opacity-[0.15]"
                            style={{
                                top: `${Math.random() * 100}%`,
                                left: `${Math.random() * 100}%`,
                                animationDuration: `${20 + Math.random() * 30}s`,
                                animationDelay: `${-Math.random() * 30}s`,
                            }}
                        />
                    ))}
                </div>
            </div>

            <main className="relative z-10 max-w-3xl mx-auto px-6 pt-28 pb-24">
                {/* Header Section - Compact */}
                <div className="flex flex-col items-center mb-10 text-center animate-in fade-in slide-in-from-bottom-4 duration-1000">
                    <div className="inline-flex items-center gap-1.5 px-2.5 py-0.5 rounded-full bg-purple-500/5 border border-purple-500/10 text-[8px] uppercase tracking-[0.2em] font-black text-purple-400 shadow-xl backdrop-blur-md mb-4">
                        <Stars className="w-2.5 h-2.5" />
                        Astral Connection
                    </div>
                    <h1 className="text-3xl md:text-4xl font-black tracking-tighter text-white uppercase italic">
                        {t("title")}
                    </h1>
                    <div className="h-0.5 w-16 bg-gradient-to-r from-transparent via-purple-500/30 to-transparent mt-3" />
                </div>

                <div className="grid grid-cols-1 gap-8 animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-300">
                    {/* Profile Summary Card - Compact */}
                    {profileData && (
                        <div className="relative group overflow-hidden">
                            <div className="absolute inset-0 bg-white/[0.02] backdrop-blur-3xl rounded-[2.5rem] border border-white/10 group-hover:border-purple-500/30 transition-all duration-700 shadow-2xl" />
                            
                            <div className="relative z-10 p-6 md:p-8 flex flex-col md:flex-row items-center md:items-start gap-8">
                                {/* Avatar with Astral Glow - Smaller */}
                                <div className="relative w-24 h-24 md:w-28 md:h-28 shrink-0">
                                    <div className="absolute inset-[-6px] rounded-full bg-gradient-to-tr from-purple-500/20 via-transparent to-amber-500/10 blur-lg group-hover:inset-[-10px] transition-all duration-700" />
                                    <div className="w-full h-full rounded-full border-2 border-white/10 overflow-hidden relative z-10 shadow-2xl bg-zinc-950">
                                        <img
                                            src={profileData.avatarUrl || `https://ui-avatars.com/api/?background=111&color=fff&name=${encodeURIComponent(profileData.displayName)}`}
                                            alt="Avatar"
                                            className="w-full h-full object-cover transition-transform duration-1000 group-hover:scale-110"
                                        />
                                    </div>
                                    <div className="absolute -bottom-1 -right-1 w-8 h-8 bg-zinc-900 border border-white/10 rounded-xl flex items-center justify-center text-amber-500 shadow-xl z-20">
                                        <Trophy className="w-4 h-4" />
                                    </div>
                                </div>

                                <div className="flex-1 text-center md:text-left space-y-4">
                                    <div>
                                        <h2 className="text-2xl md:text-3xl font-black text-white italic tracking-tighter mb-0.5">
                                            {profileData.displayName}
                                        </h2>
                                        <div className="flex items-center justify-center md:justify-start gap-1.5 text-zinc-500 font-medium">
                                            <AtSign className="w-3 h-3" />
                                            <span className="text-xs">@{profileData.username}</span>
                                        </div>
                                    </div>

                                    {/* Aura Chips - Compact Badges */}
                                    <div className="flex flex-wrap gap-2.5 justify-center md:justify-start">
                                        <div className="flex items-center gap-2 px-3.5 py-1.5 rounded-xl bg-white/5 border border-white/10 hover:bg-white/[0.08] transition-all duration-300">
                                            <div className="w-1.5 h-1.5 rounded-full bg-purple-500 shadow-[0_0_6px_rgba(168,85,247,0.5)]" />
                                            <span className="text-[9px] font-black uppercase tracking-widest text-zinc-400">{t("level")}</span>
                                            <span className="text-xs font-black text-white ml-1">{profileData.level}</span>
                                        </div>
                                        <div className="flex items-center gap-2 px-3.5 py-1.5 rounded-xl bg-white/5 border border-white/10 hover:bg-white/[0.08] transition-all duration-300">
                                            <div className="w-1.5 h-1.5 rounded-full bg-blue-500 shadow-[0_0_6px_rgba(59,130,246,0.5)]" />
                                            <span className="text-[9px] font-black uppercase tracking-widest text-zinc-400">{t("zodiac")}</span>
                                            <span className="text-xs font-black text-white ml-1">{profileData.zodiac}</span>
                                        </div>
                                        <div className="flex items-center gap-2 px-3.5 py-1.5 rounded-xl bg-white/5 border border-white/10 hover:bg-white/[0.08] transition-all duration-300">
                                            <div className="w-1.5 h-1.5 rounded-full bg-amber-500 shadow-[0_0_6px_rgba(245,158,11,0.5)]" />
                                            <span className="text-[9px] font-black uppercase tracking-widest text-zinc-400">{t("numerology")}</span>
                                            <span className="text-xs font-black text-white ml-1">{profileData.numerology}</span>
                                        </div>
                                    </div>

                                    {/* Admin Action - Compact */}
                                    {user?.role === "admin" && (
                                        <button
                                            onClick={() => router.push("/admin/users")}
                                            className="w-full group flex justify-center items-center gap-2.5 bg-purple-500/10 hover:bg-purple-500/20 border border-purple-500/20 text-purple-300 px-5 py-2 rounded-xl text-[9px] font-black uppercase tracking-widest transition-all hover:scale-[1.03] active:scale-95 shadow-xl mb-2"
                                        >
                                            <ShieldCheck className="w-3.5 h-3.5 transition-transform group-hover:rotate-12" />
                                            Vào Trang Quản Trị
                                        </button>
                                    )}

                                    {/* Reader Settings Action */}
                                    {(user?.role === "admin" || user?.role === "reader") && (
                                        <button
                                            onClick={() => router.push("/profile/reader" as any)}
                                            className="w-full group flex justify-center items-center gap-2.5 bg-amber-500/10 hover:bg-amber-500/20 border border-amber-500/20 text-amber-400 px-5 py-2 rounded-xl text-[9px] font-black uppercase tracking-widest transition-all hover:scale-[1.03] active:scale-95 shadow-xl"
                                        >
                                            <Sparkles className="w-3.5 h-3.5 transition-transform group-hover:rotate-12" />
                                            Hồ Sơ Reader
                                        </button>
                                    )}
                                </div>
                            </div>
                        </div>
                    )}

                    {/* Settings Form Section - Compact */}
                    <div className="relative">
                        <div className="absolute inset-0 bg-white/[0.01] backdrop-blur-2xl rounded-[2.5rem] border border-white/5 shadow-2xl" />
                        
                        <div className="relative z-10 p-6 md:p-8">
                            <h3 className="text-lg font-black text-white italic tracking-tight mb-8 flex items-center gap-2.5">
                                <Sparkles className="w-4 h-4 text-amber-500" />
                                Hiệu Chỉnh Giao Thức
                            </h3>

                            {/* Status Messages - Compact */}
                            <div className="space-y-3 mb-8">
                                {successMsg && (
                                    <div className="animate-in slide-in-from-top-2 duration-500 bg-emerald-500/10 border border-emerald-500/20 p-4 rounded-2xl flex items-center gap-3 text-emerald-400 text-[11px] font-semibold">
                                        <div className="w-7 h-7 rounded-full bg-emerald-500/10 flex items-center justify-center shrink-0">
                                            <ShieldCheck className="w-3.5 h-3.5" />
                                        </div>
                                        {successMsg}
                                    </div>
                                )}
                                {errorMsg && (
                                    <div className="animate-in slide-in-from-top-2 duration-500 bg-red-500/10 border border-red-500/20 p-4 rounded-2xl flex items-center gap-3 text-red-400 text-[11px] font-semibold">
                                        <div className="w-7 h-7 rounded-full bg-red-500/10 flex items-center justify-center shrink-0">
                                            <ShieldCheck className="w-3.5 h-3.5" />
                                        </div>
                                        {errorMsg}
                                    </div>
                                )}
                            </div>

                            <form onSubmit={handleSubmit(onSubmit)} className="grid grid-cols-1 md:grid-cols-2 gap-6">
                                <div className="space-y-5">
                                    <div className="space-y-2">
                                        <label className="flex items-center gap-2 text-[9px] font-black uppercase tracking-widest text-zinc-500 ml-2">
                                            <User className="w-3 h-3" />
                                            {t("displayName")}
                                        </label>
                                        <input
                                            type="text"
                                            {...register("displayName")}
                                            className="w-full bg-white/[0.03] border border-white/5 rounded-xl px-4 py-3 text-sm text-white font-medium focus:outline-none focus:border-purple-500/50 focus:bg-white/[0.05] transition-all duration-300 shadow-inner"
                                            placeholder="Tên hiển thị của bạn"
                                        />
                                        {errors.displayName && <p className="text-red-400 text-[9px] font-bold ml-2 italic">{errors.displayName.message}</p>}
                                    </div>

                                    <div className="space-y-2">
                                        <label className="flex items-center gap-2 text-[9px] font-black uppercase tracking-widest text-zinc-500 ml-2">
                                            <Calendar className="w-3 h-3" />
                                            {t("dateOfBirth")}
                                        </label>
                                        <input
                                            type="date"
                                            {...register("dateOfBirth")}
                                            className="w-full bg-white/[0.03] border border-white/5 rounded-xl px-4 py-3 text-sm text-white font-medium focus:outline-none focus:border-purple-500/50 focus:bg-white/[0.05] transition-all duration-300 shadow-inner [color-scheme:dark]"
                                        />
                                        {errors.dateOfBirth && <p className="text-red-400 text-[9px] font-bold ml-2 italic">{errors.dateOfBirth.message}</p>}
                                    </div>
                                </div>

                                <div className="space-y-5">
                                    <div className="space-y-2">
                                        <label className="flex items-center gap-2 text-[9px] font-black uppercase tracking-widest text-zinc-500 ml-2">
                                            <AtSign className="w-3 h-3" />
                                            {t("avatarUrl")}
                                        </label>
                                        <input
                                            type="text"
                                            {...register("avatarUrl")}
                                            className="w-full bg-white/[0.03] border border-white/5 rounded-xl px-4 py-3 text-sm text-white font-medium focus:outline-none focus:border-purple-500/50 focus:bg-white/[0.05] transition-all duration-300 shadow-inner"
                                            placeholder="https://example.com/avatar.jpg"
                                        />
                                        {errors.avatarUrl && <p className="text-red-400 text-[9px] font-bold ml-2 italic">{errors.avatarUrl.message}</p>}
                                    </div>

                                    <div className="pt-4">
                                        <button
                                            type="submit"
                                            disabled={isSubmitting}
                                            className="group relative w-full h-12 bg-white text-black rounded-xl overflow-hidden font-black text-[10px] uppercase tracking-widest transition-all hover:scale-[1.02] active:scale-95 disabled:opacity-50 disabled:cursor-not-allowed shadow-2xl"
                                        >
                                            <div className="absolute inset-0 bg-gradient-to-r from-purple-500 via-indigo-500 to-purple-500 opacity-0 group-hover:opacity-10 transition-opacity duration-700" />
                                            <div className="relative z-10 flex items-center justify-center gap-2">
                                                {isSubmitting ? (
                                                    <>
                                                        <Loader2 className="w-3.5 h-3.5 animate-spin" />
                                                        {t("saving")}
                                                    </>
                                                ) : (
                                                    <>
                                                        <Save className="w-3.5 h-3.5" />
                                                        {t("save")}
                                                    </>
                                                )}
                                            </div>
                                        </button>
                                        
                                        <div className="mt-3 flex justify-center">
                                            <div className="flex items-center gap-1.5 text-[7px] text-zinc-600 font-black uppercase tracking-[0.2em]">
                                                <div className="w-1 h-1 bg-zinc-700 rounded-full" />
                                                Powered by TarotNow AI
                                                <div className="w-1 h-1 bg-zinc-700 rounded-full" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </main>

            {/* Custom Animations */}
            <style dangerouslySetInnerHTML={{
                __html: `
                @keyframes float {
                    0% { transform: translateY(0) rotate(0deg); opacity: 0; }
                    15% { opacity: 0.2; }
                    85% { opacity: 0.2; }
                    100% { transform: translateY(-100vh) rotate(360deg); opacity: 0; }
                }
                .animate-float {
                    animation-name: float;
                    animation-timing-function: linear;
                    animation-iteration-count: infinite;
                }
                @keyframes slow-pulse {
                    0%, 100% { opacity: 0.05; transform: scale(1); }
                    50% { opacity: 0.12; transform: scale(1.15); }
                }
                .animate-slow-pulse {
                    animation: slow-pulse 15s ease-in-out infinite;
                }
            `}} />
        </div>
    );
}
