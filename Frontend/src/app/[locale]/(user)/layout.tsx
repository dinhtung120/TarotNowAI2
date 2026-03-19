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
import UserLayout from "@/components/layout/UserLayout";

interface UserSegmentLayoutProps {
  children: ReactNode;
}

export default function UserSegmentLayout({ children }: UserSegmentLayoutProps) {
  return <UserLayout>{children}</UserLayout>;
}
