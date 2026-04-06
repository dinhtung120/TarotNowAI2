/*
 * ===================================================================
 * FILE: useSubscriptions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Tập hợp các React Query hooks dành cho module Subscription.
 *   Cung cấp 3 hooks chính:
 *   1. useSubscriptionPlans: Lấy danh sách gói đăng ký đang mở bán
 *   2. useMyEntitlements: Lấy số dư quyền lợi hiện tại của user
 *   3. useSubscribe: Mutation mua gói đăng ký
 *
 * THIẾT KẾ:
 *   - Tất cả hooks đều gọi Server Actions (không gọi API trực tiếp từ client).
 *   - Server Actions trả về ActionResult<T> → hooks phải unwrap success/error.
 *   - useSubscribe sau khi thành công sẽ:
 *     a) Invalidate entitlements cache (để cập nhật UI quyền lợi mới)
 *     b) Refetch wallet balance (vì đã trừ Diamond)
 *   - subscriptionKeys: factory pattern cho query keys, dễ invalidate chọn lọc.
 * ===================================================================
 */

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getSubscriptionPlansAction, getMyEntitlementsAction, subscribeToPlanAction } from '../application/actions';
import { useWalletStore } from '@/store/walletStore';

/*
 * Query key factory: Tổ chức keys theo cấu trúc phân cấp.
 * ['subscriptions'] → tất cả data liên quan subscription
 * ['subscriptions', 'plans'] → chỉ danh sách gói
 * ['subscriptions', 'entitlements'] → chỉ số dư quyền lợi
 * Khi cần invalidate toàn bộ subscription data: dùng subscriptionKeys.all
 */
export const subscriptionKeys = {
  all: ['subscriptions'] as const,
  plans: () => [...subscriptionKeys.all, 'plans'] as const,
  entitlements: () => [...subscriptionKeys.all, 'entitlements'] as const,
};

/*
 * Hook lấy danh sách gói đăng ký đang mở bán.
 * - Gọi getSubscriptionPlansAction() (Server Action, không cần token vì endpoint AllowAnonymous).
 * - staleTime = 5 phút: danh sách gói ít thay đổi, tránh gọi API lại khi chuyển tab.
 * - Nếu ActionResult.success = false → throw Error để React Query hiểu là lỗi.
 */
export const useSubscriptionPlans = () => {
  return useQuery({
    queryKey: subscriptionKeys.plans(),
    queryFn: async () => {
      const res = await getSubscriptionPlansAction();
      if (!res.success) throw new Error(res.error);
      return res.data;
    },
    staleTime: 5 * 60 * 1000, // 5 phút cache
  });
};

/*
 * Hook lấy số dư quyền lợi hiện tại của user đã đăng nhập.
 * - Cần token JWT (Server Action tự lấy từ cookie).
 * - Trả về EntitlementBalance[]: mỗi item có {entitlementKey, dailyQuotaTotal, usedToday, remainingToday}.
 * - Nếu user chưa mua gói nào → trả về mảng rỗng.
 */
export const useMyEntitlements = () => {
  return useQuery({
    queryKey: subscriptionKeys.entitlements(),
    queryFn: async () => {
      const res = await getMyEntitlementsAction();
      if (!res.success) throw new Error(res.error);
      return res.data;
    },
  });
};

/*
 * Hook mutation mua gói đăng ký.
 * - Input: { planId: string, idempotencyKey: string }
 * - Server Action sẽ: validate → trừ Diamond → tạo UserSubscription + Buckets.
 * - onSuccess callback:
 *   a) Invalidate entitlements: cập nhật widget quyền lợi với data mới.
 *   b) fetchWalletBalance(): refetch số dư ví vì đã trừ Diamond.
 */
export const useSubscribe = () => {
  const queryClient = useQueryClient();

  /* Lấy hàm fetchBalance từ Zustand store để refetch wallet sau khi mua */
  const fetchWalletBalance = useWalletStore((state) => state.fetchBalance);

  return useMutation({
    mutationFn: async (req: import('../types').SubscribeRequest) => {
      const res = await subscribeToPlanAction(req);
      if (!res.success) throw new Error(res.error);
      return res.data;
    },
    onSuccess: () => {
      /* Invalidate cache entitlements để UI cập nhật ngay quyền lợi mới mua */
      queryClient.invalidateQueries({ queryKey: subscriptionKeys.entitlements() });
      /* Refetch wallet vì đã trừ Diamond — Navbar cần hiển thị số dư mới */
      fetchWalletBalance();
    },
  });
};
