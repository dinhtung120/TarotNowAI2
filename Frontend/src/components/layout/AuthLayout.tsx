"use client";

/**
 * AuthLayout — Layout wrapper cho các trang xác thực.
 *
 * Trang áp dụng: Login, Register, Verify Email, Forgot Password, Reset Password.
 *
 * === VẤN ĐỀ TRƯỚC ĐÂY ===
 * Mỗi trang Auth tự viết layout riêng:
 * - Login: `bg-gradient-to-br from-indigo-900 via-purple-900 to-slate-900`
 * - Register: Copy tương tự → code lặp
 * - Forgot Password: Lại copy → 5 files cùng pattern
 *
 * === GIẢI PHÁP ===
 * Component AuthLayout bọc ngoài form content, cung cấp:
 * 1. Gradient background (Astral purple/indigo)
 * 2. Decorative orbs (blur circles)
 * 3. Logo ở trên
 * 4. Title + subtitle
 * 5. Center-aligned glassmorphism card cho form content
 *
 * Trang Auth KHÔNG hiển thị Navbar hoặc Footer (đã config ở Navbar.tsx).
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
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-indigo-950 via-purple-950 to-slate-950 relative overflow-hidden font-sans">
      {/* === DECORATIVE ORBS ===
          2 blob gradient lớn, blur mạnh, tạo ambient light.
          mix-blend-screen: hòa trộn ánh sáng, không che nội dung. */}
      <div className="absolute top-[-10%] left-[-10%] w-96 h-96 bg-purple-600 rounded-full mix-blend-screen filter blur-[120px] opacity-30 animate-slow-pulse" />
      <div className="absolute bottom-[-10%] right-[-10%] w-96 h-96 bg-fuchsia-600 rounded-full mix-blend-screen filter blur-[120px] opacity-30 animate-slow-pulse" />

      {/* === CONTENT CARD ===
          Glassmorphism card chứa toàn bộ form.
          max-w-md: tối ưu cho form fields, không quá rộng. */}
      <div className="relative z-10 w-full max-w-md p-8 bg-white/[0.06] backdrop-blur-2xl border border-white/15 shadow-2xl rounded-3xl">
        {/* Logo link về Home */}
        <div className="text-center mb-3">
          <Link
            href="/"
            className="inline-block text-2xl font-black italic tracking-tighter bg-gradient-to-b from-white to-zinc-400 gradient-text mb-1"
          >
            TarotNow AI
          </Link>
        </div>

        {/* Title + Subtitle */}
        <div className="text-center mb-8">
          <h1 className="text-3xl font-extrabold text-transparent bg-clip-text bg-gradient-to-r from-purple-200 to-fuchsia-200 tracking-tight mb-2">
            {title}
          </h1>
          {subtitle && (
            <p className="text-purple-200/60 text-sm font-medium">
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
