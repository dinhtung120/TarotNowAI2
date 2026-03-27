/*
 * ===================================================================
 * COMPONENT: UserLayout
 * BỐI CẢNH (CONTEXT):
 *   Khung Layout chính cho toàn bộ khu vực dành cho Người Dùng (Dashboard, Chat, Ví...).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Tích hợp `UserSidebar` (Bên trái) cho Desktop và `BottomTabBar` (Bên dưới) cho Mobile.
 *   - Tách biệt vùng Nội Dung Chính (Main Content Area) để thanh trượt (Scroll) 
 *     hoạt động độc lập, không làm Sidebar hay Navbar bị trôi theo.
 *   - Gắn nền `AstralBackground` (với chế độ Subtle) mặc định cho mọi trang User.
 * ===================================================================
 */

import { type ReactNode } from "react";
import UserSidebar from "./UserSidebar";
import BottomTabBar from "./BottomTabBar";
import AstralBackground from "./AstralBackground";
import RoutePrefetcher from "./RoutePrefetcher";

interface UserLayoutProps {
  children: ReactNode;
}

export default function UserLayout({ children }: UserLayoutProps) {
  return (
    <div className="flex h-dvh flex-col bg-[var(--bg-void)] text-[var(--text-primary)] overflow-hidden relative">
      <RoutePrefetcher />
      <AstralBackground variant="subtle" particleCount={6} />
      {/* Phần lưới bên dưới Navbar (Navbar render ở app/[locale]/layout.tsx).
 Cách top bằng `pt-16` (thay đổi tùy breakpoint) để không bị Navbar đè lên content.
 h-screen - pt-16 = flex-1 (chiều cao còn lại).
 */}
      <div className="flex flex-1 min-h-0 pt-14 md:pt-16 overflow-hidden">
        {/*
 Sidebar bên trái (Desktop only).
 Tự động ẩn trên mobile nhờ w-0/hidden trong component của nó.
 */}
        <UserSidebar />

        {/* Main Content Area.
         flex-1: chiếm toàn bộ không gian còn lại (bên phải sidebar).
         overflow-y-auto: phần nội dung được cuộn độc lập với sidebar.
         pb-20 (Mobile only): Chừa không gian cho BottomTabBar tránh bị che khuất.
         */}
        <main className="flex-1 min-h-0 relative z-10 custom-scrollbar overflow-y-auto pb-[calc(5.5rem+env(safe-area-inset-bottom))] md:pb-0">
          {children}
        </main>
      </div>

      {/* Bottom Navigation Bar (Mobile only).
 Nằm dưới cùng màn hình trên điện thoại.
 */}
      <BottomTabBar />
    </div>
  );
}
