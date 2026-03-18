"use client";

import { useCallback, useEffect, useRef } from "react";
import toast from "react-hot-toast";
import { useTranslations } from "next-intl";
import { usePathname, useRouter } from "@/i18n/routing";
import { logoutAction } from "@/actions/authActions";
import { useAuthStore } from "@/store/authStore";
import { getJwtExpiryMs, isJwtExpired } from "@/lib/jwt";
import { getAccessToken } from "@/lib/auth-client";

const EXPIRY_LEEWAY_SECONDS = 5;

export default function AuthSessionManager() {
  const router = useRouter();
  const pathname = usePathname();
  const tApi = useTranslations("ApiErrors");

  const accessToken = useAuthStore((s) => s.accessToken);
  const isAuthenticated = useAuthStore((s) => s.isAuthenticated);
  const clearAuth = useAuthStore((s) => s.clearAuth);
  const syncAuth = useAuthStore((s) => s.syncAuth);

  const logoutInProgressRef = useRef(false);
  const expiryTimerRef = useRef<number | null>(null);

  const runLogout = useCallback(async (showToast: boolean) => {
    if (logoutInProgressRef.current) return;
    logoutInProgressRef.current = true;

    try {
      await logoutAction();
    } catch {
      // Best-effort: still clear local auth state below.
    } finally {
      clearAuth();
      logoutInProgressRef.current = false;
    }

    if (showToast) toast.error(tApi("unauthorized"));
    if (!pathname.includes("/login")) router.push("/login");
  }, [clearAuth, pathname, router, tApi]);

  // Keep store consistent after hydration + between tabs.
  useEffect(() => {
    syncAuth();

    const handleStorage = (e: StorageEvent) => {
      if (e.key === "tarot-now-auth") syncAuth();
    };

    window.addEventListener("storage", handleStorage);
    return () => window.removeEventListener("storage", handleStorage);
  }, [syncAuth]);

  // Auto-logout exactly when the JWT expires.
  useEffect(() => {
    if (expiryTimerRef.current) {
      window.clearTimeout(expiryTimerRef.current);
      expiryTimerRef.current = null;
    }

    if (!accessToken) return;

    // If cookie has already been cleared (e.g. middleware), reflect it immediately in UI.
    const cookieToken = getAccessToken();
    if (!cookieToken) {
      // Avoid logout race right after login when cookie propagation can be delayed briefly.
      if (isJwtExpired(accessToken, EXPIRY_LEEWAY_SECONDS)) {
        void runLogout(isAuthenticated);
      }
      return;
    }

    if (isJwtExpired(accessToken, EXPIRY_LEEWAY_SECONDS)) {
      void runLogout(isAuthenticated);
      return;
    }

    const expMs = getJwtExpiryMs(accessToken);
    if (!expMs) {
      void runLogout(isAuthenticated);
      return;
    }

    const delayMs = Math.max(0, expMs - Date.now());
    expiryTimerRef.current = window.setTimeout(() => {
      void runLogout(true);
    }, delayMs);

    return () => {
      if (expiryTimerRef.current) window.clearTimeout(expiryTimerRef.current);
      expiryTimerRef.current = null;
    };
  }, [accessToken, isAuthenticated, runLogout]);

  // If the tab wakes up after sleep, re-check expiry immediately.
  useEffect(() => {
    const check = () => {
      const token = useAuthStore.getState().accessToken;
      if (!token) return;
      if (isJwtExpired(token, EXPIRY_LEEWAY_SECONDS)) {
        void runLogout(true);
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
  }, [runLogout]);

  return null;
}
