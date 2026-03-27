/**
 * ===================================================================
 * FILE: signalRUrl.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Cung cấp URL gốc (origin) cho các kết nối SignalR (WebSocket).
 *
 * TẠI SAO CẦN FILE NÀY?
 *   Next.js `rewrites()` trong `next.config.ts` CHỈ proxy được các HTTP request
 *   thông thường (GET, POST, ...). Nó KHÔNG HỖ TRỢ nâng cấp kết nối lên
 *   WebSocket (HTTP Upgrade). Điều này khiến SignalR không thể thiết lập
 *   kết nối thời gian thực khi đi qua proxy của Next.js.
 *
 * GIẢI PHÁP:
 *   Các SignalR Hub (ChatHub, PresenceHub) phải kết nối TRỰC TIẾP tới
 *   Backend server (ví dụ: http://localhost:5037) thay vì đi qua proxy
 *   của Next.js (http://localhost:3000).
 *
 * CÁCH SỬ DỤNG:
 *   import { getSignalRHubUrl } from '@/shared/infrastructure/realtime/signalRUrl';
 *   const hubUrl = getSignalRHubUrl('/api/v1/presence');
 *   // => 'http://localhost:5037/api/v1/presence'
 *
 * CẤU HÌNH:
 *   Sử dụng biến môi trường NEXT_PUBLIC_API_URL (có prefix NEXT_PUBLIC_ để
 *   client-side JavaScript có thể truy cập). Nếu không được set, fallback
 *   về 'http://localhost:5037/api/v1'.
 * ===================================================================
 */

/**
 * URL gốc mặc định của Backend server.
 * Dùng khi biến môi trường NEXT_PUBLIC_API_URL chưa được cấu hình.
 */
const DEFAULT_BACKEND_ORIGIN = 'http://localhost:5037';

/**
 * Lấy origin (scheme + host + port) của Backend từ biến môi trường.
 * Ví dụ: NEXT_PUBLIC_API_URL = 'http://localhost:5037/api/v1'
 *   => origin = 'http://localhost:5037'
 *
 * Nếu biến môi trường chưa set => fallback về DEFAULT_BACKEND_ORIGIN.
 */
function resolveBackendOrigin(): string {
  const raw = process.env.NEXT_PUBLIC_API_URL;

  // Nếu biến môi trường chưa được thiết lập, dùng giá trị mặc định
  if (!raw || raw.trim().length === 0) {
    return DEFAULT_BACKEND_ORIGIN;
  }

  // Cắt bỏ phần path (ví dụ: '/api/v1') để lấy origin
  // URL constructor giúp parse chính xác scheme + host + port
  try {
    const url = new URL(raw.trim());
    return url.origin; // Ví dụ: 'http://localhost:5037'
  } catch {
    // Nếu parse thất bại (không phải URL hợp lệ), fallback
    return DEFAULT_BACKEND_ORIGIN;
  }
}

/**
 * Origin của Backend, được resolve 1 lần khi module load.
 * Giá trị này là hằng số trong suốt vòng đời của ứng dụng.
 */
const BACKEND_ORIGIN = resolveBackendOrigin();

/**
 * Tạo URL đầy đủ cho một SignalR Hub endpoint.
 *
 * @param hubPath - Đường dẫn tới Hub trên Backend
 *   Ví dụ: '/api/v1/presence' hoặc '/api/v1/chat'
 * @returns URL đầy đủ trỏ trực tiếp tới Backend
 *   Ví dụ: 'http://localhost:5037/api/v1/presence'
 */
export function getSignalRHubUrl(hubPath: string): string {
  const origin = BACKEND_ORIGIN.replace(/\/+$/, '');
  const path = hubPath.startsWith('/') ? hubPath : `/${hubPath}`;
  return `${origin}${path}`;
}
