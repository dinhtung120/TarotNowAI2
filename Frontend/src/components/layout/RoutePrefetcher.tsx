/*
 * ===================================================================
 * COMPONENT: RoutePrefetcher
 * BỐI CẢNH (CONTEXT):
 *   Utility component siêu nhẹ chạy ngầm (Headless) trên Client.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Sử dụng `router.prefetch()` của Next.js để tải trước (pre-load) mã JS và dữ liệu 
 *     của các trang quan trọng ngay trong background.
 *   - Giúp cho lúc User bấm chuyển trang sẽ có cảm giác "Tức thì" (Instant Navigation).
 * ===================================================================
 */
"use client";

import { useEffect } from "react";
import { useRouter } from "@/i18n/routing";

const PREFETCH_ROUTES = [
  "/",
  "/reading",
  "/chat",
  "/wallet",
  "/profile",
  "/readers",
  "/collection",
];

export default function RoutePrefetcher() {
  const router = useRouter();

  useEffect(() => {
    PREFETCH_ROUTES.forEach((route) => {
      router.prefetch(route);
    });
  }, [router]);

  return null;
}
