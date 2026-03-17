"use client";

/**
 * UserLayout — Layout framework cho khu vực User (Dashboard, Wallet, Chat, etc.)
 *
 * === CẤU TRÚC ===
 * 1. Navbar (top fixed) — Dùng cho tất cả
 * 2. UserSidebar (left fixed) — Chỉ hiện trên Desktop (≥ md)
 * 3. BottomTabBar (bottom fixed) — Chỉ hiện trên Mobile (< md)
 * 4. Main Content Area — Có scroll độc lập.
 *
 * === TẠI SAO CẦN LAYOUT RIÊNG? ===
 * → Trước đây Navbar nằm trong `app/layout.tsx`.
 * → Nhưng mỗi lần cuộn trang, Navbar cũng cuộn theo (trừ khi set fixed).
 * → Nếu có Sidebar, layout cần flex row + flex-1 content area.
 * → Không thể định nghĩa chung ở Root Layout vì Admin, Auth không dùng Sidebar.
 */

import { type ReactNode } from "react";
import Navbar from "../common/Navbar";
import UserSidebar from "./UserSidebar";
import BottomTabBar from "./BottomTabBar";
import AstralBackground from "./AstralBackground";

interface UserLayoutProps {
  children: ReactNode;
}

export default function UserLayout({ children }: UserLayoutProps) {
  return (
    <div className="flex flex-col h-screen bg-[var(--bg-void)] text-[var(--text-primary)] overflow-hidden relative">
      <AstralBackground />
      
      {/* 
        Navbar là fixed absolute (z-50) ở trên cùng.
        Nó cao ~64px (h-16).
        Chúng ta giữ nguyên vị trí fixed của Navbar để nó overlays trên mọi thứ.
      */}
      <Navbar />

      {/* 
        Phần lưới bên dưới Navbar.
        Cách top bằng `pt-16` (thay đổi tùy breakpoint) để không bị Navbar đè lên content.
        h-screen - pt-16 = flex-1 (chiều cao còn lại).
      */}
      <div className="flex flex-1 pt-14 md:pt-16 overflow-hidden">
        {/*
          Sidebar bên trái (Desktop only).
          Tự động ẩn trên mobile nhờ w-0/hidden trong component của nó.
        */}
        <UserSidebar />

        {/* 
          Main Content Area.
          flex-1: chiếm toàn bộ không gian còn lại (bên phải sidebar).
          overflow-y-auto: phần nội dung được cuộn độc lập với sidebar.
          pb-20 (Mobile only): Chừa không gian cho BottomTabBar tránh bị che khuất.
        */}
        <main className="flex-1 relative z-10 overflow-y-auto custom-scrollbar pb-20 md:pb-0">
          <div className="animate-in fade-in duration-500">
            {children}
          </div>
        </main>
      </div>

      {/* 
        Bottom Navigation Bar (Mobile only).
        Nằm dưới cùng màn hình trên điện thoại.
      */}
      <BottomTabBar />
    </div>
  );
}
