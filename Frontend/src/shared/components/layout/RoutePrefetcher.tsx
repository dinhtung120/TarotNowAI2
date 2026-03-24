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
 * 
 * TỐI ƯU:
 *   - Thêm delay 2 giây trước khi bắt đầu prefetch → tránh cạnh tranh bandwidth
 *     với các API call quan trọng (fetchBalance, page data) lúc page load.
 *   - Cleanup timer khi component unmount (StrictMode safe).
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
    /*
     * Delay 2 giây trước khi prefetch các routes.
     * LÝ DO: Khi page vừa load, browser đang bận xử lý:
     *   - Fetch API data cho trang hiện tại (balance, sessions, etc.)
     *   - Hydrate React components
     *   - Render CSS animations
     * Nếu prefetch ngay lập tức → cạnh tranh bandwidth → trang load chậm hơn.
     * Delay 2s cho phép trang hiện tại load xong trước rồi mới prefetch.
     */
    const timer = window.setTimeout(() => {
      PREFETCH_ROUTES.forEach((route) => {
        router.prefetch(route);
      });
    }, 2000);

    /* Cleanup timer khi unmount — tránh memory leak trong StrictMode double-mount */
    return () => window.clearTimeout(timer);
  }, [router]);

  return null;
}
