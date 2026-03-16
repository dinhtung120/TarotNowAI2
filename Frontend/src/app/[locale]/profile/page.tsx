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
            <div className="flex justify-center items-center min-h-screen text-[#DFF2CB]">
                <div className="animate-spin h-8 w-8 border-4 border-current border-t-transparent rounded-full" />
            </div>
        );
    }

    return (
        <div className="container mx-auto px-4 py-12 pt-24 max-w-3xl">
            <h1 className="text-4xl font-extrabold text-[#DFF2CB] mb-8 drop-shadow-lg text-center">
                {t("title")}
            </h1>

            <div className="bg-[#1A1F2B]/80 backdrop-blur-md rounded-2xl p-8 shadow-2xl border border-[#2D3748]">
                {/* Hiển thị avatar tròn và thông tin readonly (level, zodiac, numerology) */}
                {profileData && (
                    <div className="flex flex-col sm:flex-row items-center sm:items-start gap-6 mb-8 pb-8 border-b border-[#2D3748]">
                        <img
                            src={profileData.avatarUrl || "https://ui-avatars.com/api/?background=2D3748&color=DFF2CB&name=" + encodeURIComponent(profileData.displayName)}
                            alt="Avatar"
                            className="w-24 h-24 rounded-full border-2 border-[#DFF2CB] shadow-lg object-cover"
                        />
                        <div className="text-center sm:text-left text-gray-300 space-y-2">
                            <h2 className="text-2xl font-bold text-white">{profileData.displayName}</h2>
                            <p className="text-sm">@{profileData.username}</p>

                            <div className="flex flex-wrap gap-3 mt-4 justify-center sm:justify-start">
                                <div className="bg-[#2D3748] px-3 py-1 rounded-full text-xs font-semibold text-purple-300">
                                    {t("level")}: {profileData.level}
                                </div>
                                <div className="bg-[#2D3748] px-3 py-1 rounded-full text-xs font-semibold text-blue-300">
                                    {t("zodiac")}: {profileData.zodiac}
                                </div>
                                <div className="bg-[#2D3748] px-3 py-1 rounded-full text-xs font-semibold text-orange-300">
                                    {t("numerology")}: {profileData.numerology}
                                </div>
                            </div>
                            
                            {/* Nút dành cho Admin */}
                            {user?.role === "admin" && (
                                <div className="mt-6 pt-4 border-t border-[#2D3748]/50 flex justify-center sm:justify-start">
                                    <button
                                        onClick={() => router.push("/admin/users")}
                                        className="flex items-center gap-2 bg-purple-500/10 hover:bg-purple-500/20 border border-purple-500/30 text-purple-300 px-4 py-2 rounded-xl text-sm font-bold transition-all hover:scale-105 active:scale-95"
                                    >
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/></svg>
                                        Vào Trang Quản Trị
                                    </button>
                                </div>
                            )}
                        </div>
                    </div>
                )}

                {/* Thông báo cập nhật */}
                {successMsg && (
                    <div className="bg-green-500/10 border border-green-500 text-green-400 p-4 rounded mb-6 text-sm">
                        {successMsg}
                    </div>
                )}
                {errorMsg && (
                    <div className="bg-red-500/10 border border-red-500 text-red-400 p-4 rounded mb-6 text-sm">
                        {errorMsg}
                    </div>
                )}

                <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
                    <div>
                        <label className="block text-sm font-medium text-gray-300 mb-2">{t("displayName")}</label>
                        <input
                            type="text"
                            {...register("displayName")}
                            className="w-full bg-[#0F1219] border border-[#2D3748] rounded-lg px-4 py-3 text-white focus:outline-none focus:border-[#DFF2CB] transition-colors"
                            placeholder="VD: Cố Mạn"
                        />
                        {errors.displayName && <p className="text-red-400 text-xs mt-1">{errors.displayName.message}</p>}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-300 mb-2">{t("avatarUrl")}</label>
                        <input
                            type="text"
                            {...register("avatarUrl")}
                            className="w-full bg-[#0F1219] border border-[#2D3748] rounded-lg px-4 py-3 text-white focus:outline-none focus:border-[#DFF2CB] transition-colors"
                            placeholder="https://example.com/avatar.jpg"
                        />
                        {errors.avatarUrl && <p className="text-red-400 text-xs mt-1">{errors.avatarUrl.message}</p>}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-300 mb-2">{t("dateOfBirth")}</label>
                        <input
                            type="date"
                            {...register("dateOfBirth")}
                            className="w-full bg-[#0F1219] border border-[#2D3748] rounded-lg px-4 py-3 text-white focus:outline-none focus:border-[#DFF2CB] transition-colors [color-scheme:dark]"
                        />
                        {errors.dateOfBirth && <p className="text-red-400 text-xs mt-1">{errors.dateOfBirth.message}</p>}
                    </div>

                    <div className="pt-4">
                        <button
                            type="submit"
                            disabled={isSubmitting}
                            className="w-full bg-gradient-to-r from-[#DFF2CB] to-[#B6D996] text-[#1A1F2B] font-bold py-3 px-6 rounded-lg shadow-lg hover:shadow-xl hover:scale-[1.02] transform transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            {isSubmitting ? t("saving") : t("save")}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}
