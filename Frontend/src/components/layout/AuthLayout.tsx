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
 * 1. Background tối huyền bí (không tông sáng)
 * 2. Decorative orbs mờ nhẹ để giữ chiều sâu
 * 3. Logo ở trên
 * 4. Title + subtitle
 * 5. Center-aligned surface card cho form content (không glass)
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
    <div className="min-h-screen flex items-center justify-center bg-[var(--bg-void)] relative overflow-hidden font-sans">
      {/* === DECORATIVE ORBS ===
          2 blob gradient lớn, blur mạnh, tạo ambient light.
          mix-blend-screen: hòa trộn ánh sáng, không che nội dung. */}
      <div className="absolute top-[-12%] left-[-12%] w-96 h-96 bg-purple-900/40 rounded-full filter blur-[120px] opacity-45 animate-slow-pulse" />
      <div className="absolute bottom-[-12%] right-[-12%] w-96 h-96 bg-indigo-900/40 rounded-full filter blur-[120px] opacity-45 animate-slow-pulse" />

      {/* === CONTENT CARD ===
          Glassmorphism card chứa toàn bộ form.
          max-w-md: tối ưu cho form fields, không quá rộng. */}
      <div className="relative z-10 w-full max-w-md p-8 bg-[var(--bg-surface)] border border-[var(--border-default)] shadow-[var(--shadow-elevated)] rounded-3xl">
        {/* Logo link về Home */}
        <div className="text-center mb-3">
          <Link
            href="/"
            className="inline-block text-2xl font-black italic tracking-tighter bg-gradient-to-r from-[var(--purple-accent)] to-[var(--amber-accent)] gradient-text mb-1"
          >
            TarotNow AI
          </Link>
        </div>

        {/* Title + Subtitle */}
        <div className="text-center mb-8">
          <h1 className="text-3xl font-extrabold text-[var(--text-primary)] tracking-tight mb-2">
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
