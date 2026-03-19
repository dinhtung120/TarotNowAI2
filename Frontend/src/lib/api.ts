/*
 * ===================================================================
 * COMPONENT/FILE: Khởi tạo Cấu hình API (api.ts)
 * BỐI CẢNH (CONTEXT):
 *   Đóng vai trò là trung tâm phân giải (Resolver) URL cho các cuộc gọi API từ Frontend 
 *   lên Backend.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Lấy URL gốc (Base URL) từ biến môi trường `NEXT_PUBLIC_API_URL` hoặc 
 *     dùng giá trị mặc định là `http://localhost:5037/api/v1`.
 *   - Cung cấp hàm `apiUrl` để nối chuỗi (append) Path con vào URL gốc một cách an toàn, 
 *     tránh lỗi dư hoặc thiếu dấu gạch chéo `/`.
 *   - Cung cấp hàm `apiOriginUrl` để trích xuất `Origin` (phục vụ kết nối SignalR).
 * ===================================================================
 */
const DEFAULT_API_ORIGIN = "http://localhost:5037";
const API_VERSION_PATH = "/api/v1";

function normalizeApiBaseUrl(value: string): string {
  const trimmed = value.trim().replace(/\/+$/, "");
  if (!trimmed) return `${DEFAULT_API_ORIGIN}${API_VERSION_PATH}`;

  if (trimmed.endsWith(API_VERSION_PATH)) return trimmed;
  return `${trimmed}${API_VERSION_PATH}`;
}

function apiOriginFromBase(baseUrl: string): string {
  const trimmed = baseUrl.trim().replace(/\/+$/, "");
  if (trimmed.endsWith(API_VERSION_PATH)) return trimmed.slice(0, -API_VERSION_PATH.length);
  return trimmed;
}

const rawApiBaseUrl =
  process.env.NEXT_PUBLIC_API_URL ?? `${DEFAULT_API_ORIGIN}${API_VERSION_PATH}`;

export const API_BASE_URL = normalizeApiBaseUrl(rawApiBaseUrl);
export const API_ORIGIN = apiOriginFromBase(API_BASE_URL);

export function apiUrl(path: string): string {
  const base = API_BASE_URL.replace(/\/+$/, "");
  if (!path) return base;
  if (/^https?:\/\//i.test(path)) return path;
  return `${base}${path.startsWith("/") ? path : `/${path}`}`;
}

export function apiOriginUrl(path: string): string {
  const base = API_ORIGIN.replace(/\/+$/, "");
  if (!path) return base;
  if (/^https?:\/\//i.test(path)) return path;
  return `${base}${path.startsWith("/") ? path : `/${path}`}`;
}

