/*
 * ===================================================================
 * COMPONENT: AuthSessionManager
 * BỐI CẢNH (CONTEXT):
 *   Component không giao diện (Headless) nằm ở Layer cao nhất để quản lý phiên 
 *   đăng nhập của User (JWT Session).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Đồng bộ trạng thái Auth `useAuthStore` giữa các Tab trình duyệt (StorageEvent).
 *   - Đặt bộ đếm giờ (setTimeout) theo thời gian hết hạn (expiry) của JWT Token.
 *   - **AUTO-REFRESH**: Tự động gọi `refreshAccessTokenAction()` khi Access Token
 *     SẮP hết hạn (trước 60 giây). Nếu refresh thành công → cập nhật token mới 
 *     vào Store/Cookie, user KHÔNG bị gián đoạn. Nếu refresh thất bại → logout.
 *   - Khi tab "wake up" sau sleep → kiểm tra token, thử refresh trước khi logout.
 * 
 * TẠI SAO CẦN AUTO-REFRESH?
 *   Access Token có thời hạn ngắn (~15 phút) vì lý do bảo mật.
 *   Nếu không auto-refresh, user sẽ bị đá ra ngoài mỗi 15 phút → trải nghiệm kém.
 *   Auto-refresh giúp user duy trì phiên liên tục mà không cần đăng nhập lại,
 *   miễn là Refresh Token (7 ngày) vẫn còn hạn.
 * ===================================================================
 */
"use client";

import { useCallback, useEffect, useRef } from "react";
import toast from "react-hot-toast";
import { useTranslations } from "next-intl";
import { usePathname, useRouter } from "@/i18n/routing";
/* Import cả logoutAction lẫn refreshAccessTokenAction:
 * - logoutAction: dùng khi refresh thất bại → buộc đăng xuất
 * - refreshAccessTokenAction: gọi POST /auth/refresh qua Server Action 
 *   để lấy Access Token mới bằng Refresh Token (HttpOnly Cookie)
 */
import { logoutAction, refreshAccessTokenAction } from "@/actions/authActions";
import { useAuthStore } from "@/store/authStore";
import { getJwtExpiryMs, isJwtExpired } from "@/lib/jwt";
import { getAccessToken } from "@/lib/auth-client";

/*
 * EXPIRY_LEEWAY_SECONDS: Số giây "dự phòng" khi kiểm tra token đã hết hạn.
 * Ví dụ: nếu token hết hạn lúc 14:00:00 và leeway = 5 → coi như hết hạn từ 13:59:55.
 * Mục đích: bù trừ độ trễ mạng, tránh gửi request với token vừa hết hạn.
 */
const EXPIRY_LEEWAY_SECONDS = 5;

/*
 * REFRESH_BUFFER_SECONDS: Số giây TRƯỚC KHI token hết hạn mà ta bắt đầu refresh.
 * Ví dụ: nếu token hết hạn lúc 14:15:00 và buffer = 60 → gọi refresh lúc 14:14:00.
 * 
 * TẠI SAO 60 GIÂY?
 * - Đủ sớm để refresh xong trước khi token thực sự hết hạn.
 * - Đủ muộn để không lãng phí request refresh khi token còn dùng được lâu.
 * - Nếu refresh mất 1-3 giây → vẫn còn ~57 giây buffer an toàn.
 */
const REFRESH_BUFFER_SECONDS = 60;

export default function AuthSessionManager() {
  const router = useRouter();
  const pathname = usePathname();
  const tApi = useTranslations("ApiErrors");

  const accessToken = useAuthStore((s) => s.accessToken);
  const isAuthenticated = useAuthStore((s) => s.isAuthenticated);
  const clearAuth = useAuthStore((s) => s.clearAuth);
  const setAuth = useAuthStore((s) => s.setAuth);
  const syncAuth = useAuthStore((s) => s.syncAuth);

  /*
   * Ref flags để ngăn chặn race condition:
   * - logoutInProgressRef: đảm bảo chỉ có 1 logout chạy tại một thời điểm
   * - refreshInProgressRef: đảm bảo chỉ có 1 refresh chạy tại một thời điểm
   *   (tránh trường hợp nhiều timer/event trigger cùng lúc → gọi refresh trùng lặp)
   */
  const logoutInProgressRef = useRef(false);
  const refreshInProgressRef = useRef(false);
  const expiryTimerRef = useRef<number | null>(null);

  /*
   * runLogout: Hàm đăng xuất user — chỉ gọi khi refresh thất bại hoặc không thể refresh.
   * 
   * LUỒNG:
   * 1. Kiểm tra flag tránh gọi trùng (race condition khi nhiều event trigger cùng lúc)
   * 2. Gọi Server Action logoutAction() → BE thu hồi Refresh Token trong DB
   * 3. Clear toàn bộ auth state trong Zustand store (user, token, isAuthenticated)
   * 4. Hiển thị toast lỗi nếu cần (showToast = true)
   * 5. Redirect về trang /login nếu user không ở trang login
   */
  const runLogout = useCallback(async (showToast: boolean) => {
    if (logoutInProgressRef.current) return;
    logoutInProgressRef.current = true;

    try {
      await logoutAction();
    } catch {
      /* Best-effort: dù gọi BE thất bại (mạng lỗi), vẫn clear local state.
       * Lý do: state local đã không hợp lệ, để nguyên sẽ gây UX rối. */
    } finally {
      clearAuth();
      logoutInProgressRef.current = false;
    }

    if (showToast) toast.error(tApi("unauthorized"));
    if (!pathname.includes("/login")) router.push("/login");
  }, [clearAuth, pathname, router, tApi]);

  /*
   * tryRefresh: Hàm CỐ GẮNG refresh Access Token trước khi logout.
   * 
   * LUỒNG:
   * 1. Kiểm tra flag refreshInProgressRef tránh gọi trùng
   * 2. Gọi Server Action refreshAccessTokenAction():
   *    → Server Action đọc refreshToken từ HttpOnly Cookie
   *    → Gọi POST /auth/refresh lên Backend
   *    → Backend: verify refresh token → tạo Access Token mới + Refresh Token mới (rotation)
   *    → Server Action: cập nhật cả 2 cookie (accessToken, refreshToken)
   *    → Trả về { success: true, accessToken: "..." } hoặc { error: "..." }
   * 3. Nếu thành công: cập nhật Zustand store với token mới
   *    → Store persist xuống localStorage → các tab khác tự sync
   *    → Timer mới sẽ được đặt lại bởi useEffect phía dưới (vì accessToken thay đổi)
   * 4. Nếu thất bại: gọi runLogout() → đăng xuất user
   * 
   * TẠI SAO DÙNG SERVER ACTION THAY VÌ FETCH TRỰC TIẾP?
   *   - Refresh Token nằm trong HttpOnly Cookie → JavaScript client KHÔNG đọc được.
   *   - Server Action chạy trên Next.js server → có quyền đọc/ghi cookie.
   *   - Đây là cơ chế bảo mật quan trọng: token không bao giờ lộ ra client-side.
   */
  const tryRefresh = useCallback(async () => {
    if (refreshInProgressRef.current || logoutInProgressRef.current) return;
    refreshInProgressRef.current = true;

    try {
      const result = await refreshAccessTokenAction();

      if (result.success && result.accessToken) {
        /*
         * Refresh thành công! Cập nhật token mới vào Zustand store.
         * 
         * Vì useEffect phía dưới watch `accessToken` → khi store thay đổi,
         * useEffect sẽ tự recalculate timer cho token mới.
         * → Vòng lặp auto-refresh tiếp tục liên tục cho đến khi Refresh Token hết hạn.
         * 
         * Giữ nguyên user profile hiện tại (lấy từ store), chỉ đổi token.
         */
        const currentUser = useAuthStore.getState().user;
        if (currentUser) {
          setAuth(currentUser, result.accessToken);
        }
      } else {
        /*
         * Refresh thất bại — các nguyên nhân có thể:
         * - Refresh Token đã hết hạn (quá 7 ngày)
         * - Refresh Token đã bị revoke (đăng xuất từ thiết bị khác)
         * - Phát hiện token reuse attack → BE hủy tất cả session
         * → Buộc phải logout, yêu cầu user đăng nhập lại.
         */
        await runLogout(true);
      }
    } catch {
      /* Lỗi mạng hoặc server down → không thể refresh → logout an toàn */
      await runLogout(true);
    } finally {
      refreshInProgressRef.current = false;
    }
  }, [runLogout, setAuth]);

  /*
   * useEffect #1: Đồng bộ auth state sau hydration và giữa các tab.
   * 
   * HYDRATION: Khi page load lần đầu, Zustand rehydrate từ localStorage.
   * syncAuth() kiểm tra token trong store có còn hợp lệ không.
   * 
   * MULTI-TAB SYNC: Khi Tab A thay đổi localStorage (login/logout),
   * Tab B nhận được StorageEvent → gọi syncAuth() → cập nhật UI.
   * → User logout ở 1 tab → tất cả tab tự logout theo.
   */
  useEffect(() => {
    syncAuth();

    const handleStorage = (e: StorageEvent) => {
      if (e.key === "tarot-now-auth") syncAuth();
    };

    window.addEventListener("storage", handleStorage);
    return () => window.removeEventListener("storage", handleStorage);
  }, [syncAuth]);

  /*
   * useEffect #2: CORE LOGIC — Đặt timer auto-refresh / auto-logout.
   * 
   * CHIẾN LƯỢC TIMER:
   * Khi có accessToken mới (sau login hoặc sau refresh), tính thời gian hết hạn.
   * - Nếu token còn > 60s → đặt timer gọi tryRefresh() trước 60s khi hết hạn
   * - Nếu token còn ≤ 60s nhưng chưa hết → gọi tryRefresh() ngay lập tức
   * - Nếu token đã hết hạn → gọi tryRefresh() ngay (last attempt)
   * 
   * Khi tryRefresh() thành công → store cập nhật accessToken mới
   * → useEffect này chạy lại (vì dependency accessToken thay đổi)
   * → Đặt timer mới cho token mới → VÒng lặp LIên TỤC.
   */
  useEffect(() => {
    /* Clear timer cũ trước khi set timer mới (tránh memory leak + duplicate timer) */
    if (expiryTimerRef.current) {
      window.clearTimeout(expiryTimerRef.current);
      expiryTimerRef.current = null;
    }

    /* Không có token → user chưa đăng nhập, không cần timer */
    if (!accessToken) return;

    /*
     * Kiểm tra cookie accessToken có tồn tại không.
     * Cookie có thể bị xóa bởi middleware hoặc hết hạn trước store.
     * Nếu cookie mất nhưng store vẫn có token → đồng bộ lại.
     */
    const cookieToken = getAccessToken();
    if (!cookieToken) {
      if (isJwtExpired(accessToken, EXPIRY_LEEWAY_SECONDS)) {
        /* Token đã hết hạn, thử refresh trước khi logout */
        void tryRefresh();
      }
      return;
    }

    /*
     * Token đã hết hạn rồi (có thể do tab bị sleep lâu):
     * Thử refresh ngay lập tức — nếu Refresh Token còn hạn, user được cứu.
     */
    if (isJwtExpired(accessToken, EXPIRY_LEEWAY_SECONDS)) {
      void tryRefresh();
      return;
    }

    /* Lấy thời điểm hết hạn (milliseconds) từ JWT payload */
    const expMs = getJwtExpiryMs(accessToken);
    if (!expMs) {
      /* Token không parse được exp → không hợp lệ → thử refresh */
      void tryRefresh();
      return;
    }

    /*
     * TÍNH TOÁN THỜI GIAN CHO TIMER:
     * 
     * refreshAtMs: thời điểm nên gọi refresh = expiry - buffer (60s)
     * delayMs: số milliseconds từ bây giờ đến refreshAtMs
     * 
     * Ví dụ: Token hết hạn lúc 14:15:00
     *   refreshAtMs = 14:15:00 - 60s = 14:14:00
     *   delayMs = 14:14:00 - now
     *   → Timer sẽ fire lúc 14:14:00, gọi tryRefresh()
     *   → Nếu refresh thành công, nhận token mới hết hạn lúc 14:30:00
     *   → useEffect chạy lại, timer mới fire lúc 14:29:00
     *   → Vòng lặp vô hạn đến khi Refresh Token hết hạn (7 ngày)
     */
    const refreshAtMs = expMs - (REFRESH_BUFFER_SECONDS * 1000);
    const delayMs = Math.max(0, refreshAtMs - Date.now());

    expiryTimerRef.current = window.setTimeout(() => {
      void tryRefresh();
    }, delayMs);

    return () => {
      if (expiryTimerRef.current) window.clearTimeout(expiryTimerRef.current);
      expiryTimerRef.current = null;
    };
  }, [accessToken, isAuthenticated, runLogout, tryRefresh]);

  /*
   * useEffect #3: Xử lý tab "wake up" sau sleep hoặc user quay lại tab.
   * 
   * VẤN ĐỀ: Khi laptop sleep hoặc user mở tab khác lâu, setTimeout bị "đóng băng".
   * Khi quay lại, timer có thể đã trễ → token đã hết hạn từ lâu.
   * 
   * GIẢI PHÁP: Lắng nghe event "focus" (click vào tab) và "visibilitychange" (tab visible).
   * Khi tab active → kiểm tra token ngay lập tức:
   * - Nếu token hết hạn → thử refresh (thay vì logout ngay)
   * - Nếu refresh thành công → user tiếp tục bình thường
   * - Nếu refresh thất bại → logout
   * 
   * TẠI SAO THỬ REFRESH THAY VÌ LOGOUT?
   * Ví dụ: User mở Tab lúc 14:00, token hết hạn lúc 14:15, user quay lại lúc 14:20.
   * Token đã hết hạn 5 phút, nhưng Refresh Token vẫn còn hạn (7 ngày).
   * → Gọi refresh → nhận token mới → user KHÔNG bị gián đoạn!
   */
  useEffect(() => {
    const check = () => {
      const token = useAuthStore.getState().accessToken;
      if (!token) return;
      if (isJwtExpired(token, EXPIRY_LEEWAY_SECONDS)) {
        /* Token hết hạn → thử refresh trước, không vội logout */
        void tryRefresh();
      }
    };

    const onVisibilityChange = () => {
      if (document.visibilityState === "visible") check();
    };

    window.addEventListener("focus", check);
    document.addEventListener("visibilitychange", onVisibilityChange);
    return () => {
      window.removeEventListener("focus", check);
      document.removeEventListener("visibilitychange", onVisibilityChange);
    };
  }, [tryRefresh]);

  /* Component không render gì cả — chỉ chạy logic side-effect */
  return null;
}
