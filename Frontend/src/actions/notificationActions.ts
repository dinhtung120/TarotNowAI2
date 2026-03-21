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

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

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
  const accessToken = await getServerAccessToken();
  if (!accessToken) return null;

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

    const result = await serverHttpRequest<NotificationListResponse>(
      `/Notification?${params.toString()}`,
      {
        method: 'GET',
        token: accessToken,
        fallbackErrorMessage: 'Failed to get notifications',
      }
    );

    if (!result.ok) {
      logger.error('NotificationAction.getNotifications', result.error, {
        status: result.status,
        page,
        pageSize,
        isRead,
      });
      return null;
    }

    return result.data;
  } catch (error) {
    logger.error('NotificationAction.getNotifications', error, { page, pageSize, isRead });
    return null;
  }
}

/// Đếm số thông báo chưa đọc — dùng cho badge count icon chuông
export async function getUnreadNotificationCount(): Promise<number> {
  const accessToken = await getServerAccessToken();
  if (!accessToken) return 0;

  try {
    const result = await serverHttpRequest<{ count?: number }>('/Notification/unread-count', {
      method: 'GET',
      token: accessToken,
      fallbackErrorMessage: 'Failed to get unread notification count',
    });

    if (!result.ok) {
      /* Không throw error — trả 0 để UI không hiển thị badge lỗi */
      logger.error('NotificationAction.getUnreadNotificationCount', result.error, {
        status: result.status,
      });
      return 0;
    }

    /* BE trả { count: 5 } — lấy field count */
    return result.data.count ?? 0;
  } catch (error) {
    /* Lỗi mạng → trả 0, không break UI */
    logger.error('NotificationAction.getUnreadNotificationCount', error);
    return 0;
  }
}

/// Đánh dấu 1 thông báo đã đọc
export async function markNotificationAsRead(
  notificationId: string
): Promise<{ success: boolean; error?: string }> {
  const accessToken = await getServerAccessToken();
  if (!accessToken) return { success: false, error: 'Unauthorized' };

  try {
    const result = await serverHttpRequest<unknown>(`/Notification/${notificationId}/read`, {
      method: 'PATCH',
      token: accessToken,
      fallbackErrorMessage: 'Failed to mark as read',
    });

    if (!result.ok) {
      logger.error('NotificationAction.markNotificationAsRead', result.error, {
        status: result.status,
        notificationId,
      });
      return { success: false, error: result.error };
    }

    return { success: true };
  } catch (error) {
    logger.error('NotificationAction.markNotificationAsRead', error, { notificationId });
    return { success: false, error: 'Network error' };
  }
}
