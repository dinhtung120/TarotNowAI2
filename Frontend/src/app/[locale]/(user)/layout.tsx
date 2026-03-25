/*
 * ===================================================================
 * FILE: (user)/layout.tsx (User Segment Layout)
 * BỐI CẢNH (CONTEXT):
 *   Layout dành riêng cho nhóm Route của User (Đã đăng nhập).
 *   Sử dụng chung UserLayout Component để bao bọc các nội dung.
 *
 * RENDERING:
 *   Server Component. Cung cấp cấu trúc HTML tĩnh chung cho flow người dùng.
 * ===================================================================
 */
import type { ReactNode } from "react";
import AppAuthSessionManager from "@/features/auth/presentation/components/AppAuthSessionManager";
import AppNavbar from "@/features/auth/presentation/components/AppNavbar";
import WalletStoreBridge from "@/features/wallet/presentation/components/WalletStoreBridge";
import AppQueryProvider from "@/shared/components/common/AppQueryProvider";
import UserLayout from "@/shared/components/layout/UserLayout";

interface UserSegmentLayoutProps {
  children: ReactNode;
}

export default function UserSegmentLayout({ children }: UserSegmentLayoutProps) {
  return (
    <AppQueryProvider>
      <AppAuthSessionManager />
      <WalletStoreBridge />
      <AppNavbar />
      <UserLayout>{children}</UserLayout>
    </AppQueryProvider>
  );
}
