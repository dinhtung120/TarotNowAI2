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
import MetadataInitialLoader from "@/shared/components/common/MetadataInitialLoader";
import { getInitialMetadata } from "@/shared/application/actions/metadata";

interface UserSegmentLayoutProps {
  children: ReactNode;
}

/**
 * Server Component: UserSegmentLayout.
 * TỐI ƯU HÓA: Thực hiện fetch Metadata Batch ngay trên Server để tránh bão request ở Client.
 */
export default async function UserSegmentLayout({ children }: UserSegmentLayoutProps) {
  // 1. Fetch Metadata ngay trên Server
  const metadataResult = await getInitialMetadata();
  const initialMetadata = metadataResult.success ? metadataResult.data : null;

  return (
    <AppQueryProvider>
      <AppAuthSessionManager />
      {/* 2. Truyền dữ liệu xuống Client Loader để mồi cache tức thì */}
      <MetadataInitialLoader initialMetadata={initialMetadata} />
      <WalletStoreBridge />
      <AppNavbar />
      <UserLayout>{children}</UserLayout>
    </AppQueryProvider>
  );
}
