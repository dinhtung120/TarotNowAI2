'use server';

/*
 * ===================================================================
 * FILE: actions.ts (Subscription Server Actions)
 * ===================================================================
 * MỤC ĐÍCH:
 *   Server Actions xử lý communication giữa Frontend (Client Components) 
 *   và Backend API cho module Subscription.
 *
 * THIẾT KẾ:
 *   - Dùng pattern "use server" → chạy trên Next.js server, KHÔNG bao giờ lộ token ra client.
 *   - Tất cả actions trả về ActionResult<T> = { success: boolean, data?: T, error?: string }.
 *   - getSubscriptionPlansAction: KHÔNG cần token (endpoint AllowAnonymous).
 *   - getMyEntitlementsAction, subscribeToPlanAction: CẦN token JWT từ cookie.
 *   - Error handling: catch tất cả exception → trả actionFail, KHÔNG throw ra ngoài.
 *
 * LÝ DO CHỌN SERVER ACTIONS THAY VÌ API CLIENT:
 *   Dự án TarotNowAI sử dụng kiến trúc Server Actions + serverHttpRequest thay vì 
 *   fetch trực tiếp từ client. Lý do:
 *   1. Token JWT chỉ tồn tại trong HttpOnly cookie → client JS không truy cập được.
 *   2. serverHttpRequest tự gắn Authorization header từ cookie session.
 *   3. Tránh expose API URL ra client bundle (bảo mật endpoint).
 * ===================================================================
 */

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { SubscriptionPlan, EntitlementBalance, SubscribeRequest, SubscribeResponse } from '../types';

/**
 * Lấy danh sách gói đăng ký đang mở bán.
 * Endpoint: GET /api/v1/subscriptions/plans (AllowAnonymous — không cần token).
 * 
 * Trả về: SubscriptionPlan[] chứa thông tin tên, giá, thời hạn, và danh sách entitlements.
 * Nếu backend không phản hồi hoặc trả lỗi → trả actionFail với thông báo lỗi.
 */
export async function getSubscriptionPlansAction(): Promise<ActionResult<SubscriptionPlan[]>> {
  try {
    const result = await serverHttpRequest<SubscriptionPlan[]>('/subscriptions/plans', {
      method: 'GET',
    });

    if (!result.ok) {
      return actionFail(result.error || 'Failed to fetch subscription plans');
    }

    return actionOk(result.data);
  } catch (error) {
    logger.error('SubscriptionAction.getSubscriptionPlansAction', error);
    return actionFail('Network error');
  }
}

/**
 * Lấy số dư quyền lợi hiện tại của user đã đăng nhập.
 * Endpoint: GET /api/v1/subscriptions/me/entitlements (cần JWT token).
 * 
 * Trả về: EntitlementBalance[] với mỗi item chứa:
 *   - entitlementKey: loại quyền lợi (VD: "free_spread_3_daily")
 *   - dailyQuotaTotal: tổng hạn ngạch/ngày
 *   - usedToday: đã dùng hôm nay
 *   - remainingToday: còn lại hôm nay
 * 
 * Nếu user chưa mua gói nào → backend trả mảng rỗng.
 */
export async function getMyEntitlementsAction(): Promise<ActionResult<EntitlementBalance[]>> {
  try {
    const token = await getServerAccessToken();
    if (!token) return actionFail('Unauthorized');

    const result = await serverHttpRequest<EntitlementBalance[]>('/subscriptions/me/entitlements', {
      method: 'GET',
      token,
    });

    if (!result.ok) {
      if (result.status === 401) return actionFail('Unauthorized');
      return actionFail(result.error || 'Failed to fetch entitlements');
    }

    return actionOk(result.data);
  } catch (error) {
    logger.error('SubscriptionAction.getMyEntitlementsAction', error);
    return actionFail('Network error');
  }
}

/**
 * Mua gói đăng ký cho user hiện tại.
 * Endpoint: POST /api/v1/subscriptions/subscribe (cần JWT token).
 * 
 * Input: { planId: GUID của gói, idempotencyKey: chuỗi chống trùng lặp }
 * 
 * Backend sẽ thực hiện trong 1 transaction:
 *   1. Check idempotency (chống double-click)
 *   2. Check user đã có gói cùng loại active chưa (SCALE-1)
 *   3. Trừ Diamond từ ví
 *   4. Tạo UserSubscription + Entitlement Buckets
 *   5. Publish domain events
 * 
 * Trả về: { subscriptionId: GUID } nếu thành công.
 */
export async function subscribeToPlanAction(request: SubscribeRequest): Promise<ActionResult<SubscribeResponse>> {
  try {
    const token = await getServerAccessToken();
    if (!token) return actionFail('Unauthorized');

    const result = await serverHttpRequest<SubscribeResponse>('/subscriptions/subscribe', {
      method: 'POST',
      token,
      json: request,
    });

    if (!result.ok) {
      return actionFail(result.error || 'Failed to subscribe to plan');
    }

    return actionOk(result.data);
  } catch (error) {
    /* 
     * Spread request object để phù hợp với LogMeta type.
     * Không truyền request trực tiếp vì SubscribeRequest thiếu index signature [key: string].
     */
    logger.error('SubscriptionAction.subscribeToPlanAction', error, { ...request });
    return actionFail('Network error');
  }
}
