"use client";

/*
 * ===================================================================
 * COMPONENT: AuthLayout
 * BỐI CẢNH (CONTEXT):
 *   Layout bọc ngoài (Wrapper) dành riêng cho nhóm trang Xác thực (Authentication)
 *   như: Login, Register, Forgot Password, Verify Email.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Cung cấp giao diện nền tối (Void Background) kết hợp với các đốm sáng mờ 
 *     (Decorative Orbs) tạo cảm giác tĩnh lặng, tập trung.
 *   - Canh giữa một khối Glassmorphism Card để chứa Form đăng nhập/đăng ký nhập vào từ `children`.
 *   - Không render Navbar hay Footer để người dùng tập trung hoàn thành flow Xác thực.
 * ===================================================================
 */

import { type ReactNode } from "react";
import { Link } from "@/i18n/routing";

interface AuthLayoutProps {
 /** Tiêu đề chính (ví dụ: "Welcome Back", "Create Account") */
 title: string;

 /** Mô tả phụ dưới title */
 subtitle?: string;

 /** Form content */
 children: ReactNode;
}

export default function AuthLayout({
 title,
 subtitle,
 children,
}: AuthLayoutProps) {
return (
 <div className="min-h-dvh flex items-center justify-center bg-[var(--bg-void)] relative overflow-hidden font-sans px-4 py-10">
 {/* === DECORATIVE ORBS ===
 2 blob gradient lớn, blur mạnh, tạo ambient light.
 : hòa trộn ánh sáng, không che nội dung. */}
 <div className="absolute top-[-12%] left-[-12%] w-96 h-96 bg-[color:var(--c-215-189-226-62)] rounded-full filter blur-[120px] opacity-70 animate-slow-pulse" />
 <div className="absolute bottom-[-12%] right-[-12%] w-96 h-96 bg-[color:var(--c-178-232-214-58)] rounded-full filter blur-[120px] opacity-70 animate-slow-pulse" />

 {/* === CONTENT CARD ===
 Glassmorphism card chứa toàn bộ form.
 max-w-md: tối ưu cho form fields, không quá rộng. */}
 <div className="relative z-10 w-full max-w-md p-6 sm:p-8 bg-[var(--bg-glass)] border border-[var(--border-default)] shadow-[var(--shadow-elevated)] rounded-3xl ">
 {/* Logo link về Home */}
 <div className="text-center mb-3">
 <Link
 href="/"
 className="inline-flex items-center justify-center min-h-11 px-1 text-2xl font-black italic tracking-tighter lunar-metallic-text mb-1"
 >
 TarotNow AI
 </Link>
 </div>

 {/* Title + Subtitle */}
 <div className="text-center mb-8">
 <h1 className="text-3xl font-extrabold text-[var(--text-ink)] tracking-tight mb-2">
 {title}
 </h1>
 {subtitle && (
 <p className="text-[var(--text-secondary)] text-sm font-medium">
 {subtitle}
 </p>
 )}
 </div>

 {/* Form Content — do page truyền vào */}
 {children}
 </div>
 </div>
 );
}
