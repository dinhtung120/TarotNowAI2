
/**
 * Step: Định nghĩa Origin mặc định cho Backend khi không có cấu hình environment.
 * Lý do: Đảm bảo ứng dụng vẫn có thể kết nối tới local API (thường là http://127.0.0.1:5037) trong quá trình phát triển.
 */
const DEFAULT_BACKEND_ORIGIN = 'http://127.0.0.1:5037';

/**
 * Step: Hàm phân giải Origin của Backend từ biến môi trường NEXT_PUBLIC_API_URL.
 * Logic:
 * 1. Lấy giá trị từ process.env.NEXT_PUBLIC_API_URL (ví dụ: http://192.168.0.102:5037/api/v1).
 * 2. Sử dụng class URL để trích xuất phần origin (protocol + host + port).
 * 3. Nếu config bị lỗi hoặc trống, fallback về DEFAULT_BACKEND_ORIGIN.
 */
function resolveBackendOrigin(): string {
  const raw = typeof process !== 'undefined' ? process.env.NEXT_PUBLIC_API_URL : null;

  if (!raw || raw.trim().length === 0) {
    return DEFAULT_BACKEND_ORIGIN;
  }

  try {
    const url = new URL(raw.trim());
    let origin = url.origin;

    /**
     * Step: Kiểm tra Mixed Content (HTTPS -> HTTP).
     * Lý do: Nếu trang web đang chạy trên HTTPS, trình duyệt sẽ chặn tất cả yêu cầu fetch tới HTTP SignalR.
     * Logic: Nếu window.location là https mà origin là http, chúng ta cảnh báo hoặc tự động nâng cấp (nếu cần).
     */
    if (typeof window !== 'undefined' && window.location.protocol === 'https:' && url.protocol === 'http:') {
      console.warn(
        `[SignalR] Phát hiện Mixed Content: Trang web đang chạy ở HTTPS nhưng API URL cấu hình là HTTP (${origin}). 
        Điều này sẽ gây lỗi 'Load failed'. Vui lòng cập nhật .env thành HTTPS.`
      );
    }

    return origin; 
  } catch (err) {
    console.error('[SignalR] Lỗi phân giải NEXT_PUBLIC_API_URL:', err);
    return DEFAULT_BACKEND_ORIGIN;
  }
}

const BACKEND_ORIGIN = resolveBackendOrigin();

/**
 * Step: Hàm tạo URL đầy đủ cho SignalR Hub.
 * @param hubPath Đường dẫn của hub (ví dụ: '/chat' hoặc 'api/v1/chat').
 * @returns Trả về URL tuyệt đối bao gồm Origin và Path.
 * Logic: Chuẩn hóa dấu gạch chéo (slash) để tránh việc URL bị dư hoặc thiếu '/', giúp kết nối ổn định.
 */
export function getSignalRHubUrl(hubPath: string): string {
  const origin = BACKEND_ORIGIN.replace(/\/+$/, '');
  const path = hubPath.startsWith('/') ? hubPath : `/${hubPath}`;
  return `${origin}${path}`;
}
