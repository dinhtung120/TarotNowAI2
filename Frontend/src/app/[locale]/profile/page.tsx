"use client";

import { useTranslations } from "next-intl";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";

// DTO định dạng trả về từ [GET] /api/v1/profile
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

// Zod Schema cho dữ liệu Form
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

    // Gọi API lấy dữ liệu Profile khi component được mount
    useEffect(() => {
        async function fetchProfile() {
            try {
                const res = await fetch("/api/v1/profile", {
                    headers: {
                        "Content-Type": "application/json",
                        // Giả định hệ thống tự động đính kèm token (HttpOnly Cookie hoặc Auth Header)
                    },
                });

                if (!res.ok) throw new Error("Không thể tải thông tin cá nhân");

                const data: ProfileResponse = await res.json();
                setProfileData(data);

                // Điền dữ liệu vào form
                setValue("displayName", data.displayName);
                setValue("avatarUrl", data.avatarUrl || "");

                // Xử lý DateOfBirth để hiển thị đúng format yyyy-MM-dd
                if (data.dateOfBirth) {
                    const dateObj = new Date(data.dateOfBirth);
                    const yyyy = dateObj.getFullYear();
                    const mm = String(dateObj.getMonth() + 1).padStart(2, "0");
                    const dd = String(dateObj.getDate()).padStart(2, "0");
                    setValue("dateOfBirth", `${yyyy}-${mm}-${dd}`);
                }
            } catch (err: any) {
                setErrorMsg(err.message || "Lỗi tải dữ liệu");
            } finally {
                setLoading(false);
            }
        }
        fetchProfile();
    }, [setValue]);

    // Xử lý khi Submit Form
    const onSubmit = async (data: ProfileFormValues) => {
        setSuccessMsg("");
        setErrorMsg("");
        try {
            const res = await fetch("/api/v1/profile", {
                method: "PATCH",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    displayName: data.displayName,
                    avatarUrl: data.avatarUrl || null,
                    dateOfBirth: new Date(data.dateOfBirth).toISOString(),
                }),
            });

            if (!res.ok) {
                const errData = await res.json().catch(() => ({}));
                throw new Error(errData.msg || "Cập nhật thất bại");
            }

            setSuccessMsg("Cập nhật thông tin thành công!");

            // Reload lại dữ liệu để cập nhật Zodiac và Numerology hiển thị
            const resProfile = await fetch("/api/v1/profile");
            if (resProfile.ok) {
                setProfileData(await resProfile.json());
            }
        } catch (err: any) {
            setErrorMsg(err.message || "Đã xảy ra lỗi hệ thống.");
        }
    };

    if (loading) {
        return (
            <div className="flex justify-center items-center min-h-screen text-[#DFF2CB]">
                <div className="animate-spin h-8 w-8 border-4 border-current border-t-transparent rounded-full" />
            </div>
        );
    }

    return (
        <div className="container mx-auto px-4 py-12 max-w-3xl">
            <h1 className="text-4xl font-extrabold text-[#DFF2CB] mb-8 drop-shadow-lg text-center">
                {t("title") || "Hồ Sơ Cá Nhân"}
            </h1>

            <div className="bg-[#1A1F2B]/80 backdrop-blur-md rounded-2xl p-8 shadow-2xl border border-[#2D3748]">
                {/* Hiển thị avatar tròn và thông tin readonly nếu đã fetch thành công */}
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
                                    Cấp độ: {profileData.level}
                                </div>
                                <div className="bg-[#2D3748] px-3 py-1 rounded-full text-xs font-semibold text-blue-300">
                                    Cung: {profileData.zodiac}
                                </div>
                                <div className="bg-[#2D3748] px-3 py-1 rounded-full text-xs font-semibold text-orange-300">
                                    Thần Số: {profileData.numerology}
                                </div>
                            </div>
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
                        <label className="block text-sm font-medium text-gray-300 mb-2">Tên hiển thị</label>
                        <input
                            type="text"
                            {...register("displayName")}
                            className="w-full bg-[#0F1219] border border-[#2D3748] rounded-lg px-4 py-3 text-white focus:outline-none focus:border-[#DFF2CB] transition-colors"
                            placeholder="VD: Cố Mạn"
                        />
                        {errors.displayName && <p className="text-red-400 text-xs mt-1">{errors.displayName.message}</p>}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-300 mb-2">Đường dẫn ảnh đại diện (Avatar URL)</label>
                        <input
                            type="text"
                            {...register("avatarUrl")}
                            className="w-full bg-[#0F1219] border border-[#2D3748] rounded-lg px-4 py-3 text-white focus:outline-none focus:border-[#DFF2CB] transition-colors"
                            placeholder="https://example.com/avatar.jpg"
                        />
                        {errors.avatarUrl && <p className="text-red-400 text-xs mt-1">{errors.avatarUrl.message}</p>}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-300 mb-2">Ngày sinh</label>
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
                            {isSubmitting ? "Đang xử lý..." : "Lưu thay đổi"}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}
