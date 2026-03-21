/*
 * ===================================================================
 * FILE: notificationActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Server Actions cho module Thông báo (Notifications).
 *   Gọi 3 API endpoint của NotificationController trên Backend.
 *
 * TẠI SAO DÙNG SERVER ACTIONS?
 *   - Server Action chạy trên Next.js server → có thể đọc cookies chứa accessToken.
 *   - AccessToken được gửi kèm header Authorization Bearer.
 *   - Client-side JavaScript KHÔNG CẦN biết API URL hay token → bảo mật hơn.
 *
 * CACHE POLICY:
 *   Sử dụng `cache: 'no-store'` cho tất cả request vì:
 *   - Notifications thay đổi realtime (mới đến, đánh dấu đọc).
 *   - Dùng cache sẽ hiển thị dữ liệu cũ → UX kém.
 * ===================================================================
 */
'use server';

import { cookies } from 'next/headers';
import { API_BASE_URL } from '@/lib/api';

/*
 * ========== TYPE DEFINITIONS ==========
 * Định nghĩa TypeScript cho dữ liệu thông báo.
 * Tách riêng ở đây thay vì file types/ riêng vì chỉ dùng trong module notification.
 */

/// Thông báo đơn lẻ — tương ứng NotificationDto từ BE
export interface NotificationItem {
  id: string;           // MongoDB ObjectId dạng string
  userId: string;       // UUID user nhận
  titleVi: string;      // Tiêu đề tiếng Việt
  titleEn: string;      // Tiêu đề tiếng Anh
  bodyVi: string;       // Nội dung tiếng Việt
  bodyEn: string;       // Nội dung tiếng Anh
  type: string;         // Loại: "system"/"quest"/"streak"/"escrow"
  isRead: boolean;      // Đã đọc chưa
  createdAt: string;    // ISO datetime
}

/// Response danh sách thông báo — tương ứng NotificationListResponse từ BE
export interface NotificationListResponse {
  items: NotificationItem[];  // Danh sách thông báo trang hiện tại
  totalCount: number;          // Tổng số thông báo (mọi trang)
  page: number;                // Trang hiện tại
  pageSize: number;            // Kích thước trang
}

/*
 * ========== SERVER ACTIONS ==========
 */

/// Lấy danh sách thông báo của user hiện tại (phân trang, filter isRead)
export async function getNotifications(
  page = 1,
  pageSize = 20,
  isRead?: boolean
): Promise<NotificationListResponse | null> {
  const cookieStore = await cookies();
  /* Lấy accessToken từ cookie — đã được loginAction set sẵn */
  const accessToken = cookieStore.get('accessToken')?.value;

  try {
    /*
     * Xây URL với query params.
     * URLSearchParams tự encode đúng các ký tự đặc biệt.
     * isRead chỉ thêm vào nếu có giá trị (null = lấy tất cả, không cần param).
     */
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: pageSize.toString(),
    });
    if (isRead !== undefined) {
      params.set('isRead', isRead.toString());
    }

    const response = await fetch(
      `${API_BASE_URL}/Notification?${params.toString()}`,
      {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${accessToken}`,
          'Content-Type': 'application/json',
        },
        cache: 'no-store', // Thông báo cần realtime, không cache
      }
    );

    if (!response.ok) {
      console.error('getNotifications error', response.status);
      return null;
    }

    return await response.json();
  } catch (error) {
    console.error('Failed to get notifications:', error);
    return null;
  }
}

/// Đếm số thông báo chưa đọc — dùng cho badge count icon chuông
export async function getUnreadNotificationCount(): Promise<number> {
  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  try {
    const response = await fetch(
      `${API_BASE_URL}/Notification/unread-count`,
      {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${accessToken}`,
          'Content-Type': 'application/json',
        },
        cache: 'no-store',
      }
    );

    if (!response.ok) {
      /* Không throw error — trả 0 để UI không hiển thị badge lỗi */
      return 0;
    }

    /* BE trả { count: 5 } — lấy field count */
    const data = await response.json();
    return data.count ?? 0;
  } catch {
    /* Lỗi mạng → trả 0, không break UI */
    return 0;
  }
}

/// Đánh dấu 1 thông báo đã đọc
export async function markNotificationAsRead(
  notificationId: string
): Promise<{ success: boolean; error?: string }> {
  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  try {
    const response = await fetch(
      `${API_BASE_URL}/Notification/${notificationId}/read`,
      {
        method: 'PATCH',
        headers: {
          'Authorization': `Bearer ${accessToken}`,
          'Content-Type': 'application/json',
        },
      }
    );

    if (!response.ok) {
      const result = await response.json().catch(() => ({}));
      return { success: false, error: result.message || 'Failed to mark as read' };
    }

    return { success: true };
  } catch {
    return { success: false, error: 'Network error' };
  }
}
