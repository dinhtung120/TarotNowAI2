/*
 * ===================================================================
 * COMPONENT/FILE: Bộ công cụ xử lý JWT (jwt.ts)
 * BỐI CẢNH (CONTEXT):
 *   Thư viện nhỏ tự viết để giải mã và kiểm tra hạn sử dụng của JSON Web Token (JWT), 
 *   có thể chạy tốt cả trên Server (Node.js) lẫn Client (Browser).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Hàm `tryDecodeJwtPayload`: Tách phần thân (Payload) của JWT và dùng `atob` hoặc 
 *     `Buffer` để giải mã Base64Url sang chuỗi JSON.
 *   - Hàm `getJwtExpiryMs`: Trích xuất thời điểm hết hạn (exp) từ Payload định dạng Millisecond.
 *   - Hàm `isJwtExpired`: Kiểm tra xem Token đã hết hạn chưa, hỗ trợ thêm tham số `leewaySeconds` 
 *     để bù trừ độ trễ mạng hoặc sai số thời gian giữa Server-Client.
 * ===================================================================
 */
export type JwtPayload = Record<string, unknown> & {
  exp?: number | string;
};

function base64UrlDecodeToUtf8(base64Url: string): string {
  const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
  const padded = base64.padEnd(Math.ceil(base64.length / 4) * 4, "=");

  if (typeof atob === "function") {
    return atob(padded);
  }

  // Node.js fallback (e.g. tests). Avoids relying on atob being present.
  return Buffer.from(padded, "base64").toString("utf8");
}

export function tryDecodeJwtPayload(token: string): JwtPayload | null {
  if (!token) return null;
  const parts = token.split(".");
  if (parts.length < 2) return null;

  try {
    const json = base64UrlDecodeToUtf8(parts[1] ?? "");
    return JSON.parse(json) as JwtPayload;
  } catch {
    return null;
  }
}

export function getJwtExpiryMs(token: string): number | null {
  const payload = tryDecodeJwtPayload(token);
  const exp = payload?.exp;
  if (typeof exp === "number" && Number.isFinite(exp)) return exp * 1000;
  if (typeof exp === "string") {
    const parsed = Number(exp);
    if (Number.isFinite(parsed)) return parsed * 1000;
  }

  return null;
}

export function isJwtExpired(token: string, leewaySeconds = 0): boolean {
  const expMs = getJwtExpiryMs(token);
  if (!expMs) return true;
  const leewayMs = Math.max(0, leewaySeconds) * 1000;
  return Date.now() >= expMs - leewayMs;
}
