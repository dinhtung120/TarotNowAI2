'use client';

/*
 * ===================================================================
 * FILE: SubscriptionPlanCard.tsx
 * ===================================================================
 * MỤC ĐÍCH:
 *   Component hiển thị thông tin chi tiết của một gói đăng ký (Subscription Plan).
 *   Bao gồm: tên gói, giá, thời hạn, danh sách quyền lợi, và nút mua.
 *
 * THIẾT KẾ:
 *   - Sử dụng Glassmorphism theme (bg-slate-800/50, border, hover gradient) 
 *     để đồng nhất với design system TarotNowAI.
 *   - Check số dư Diamond ở phía client chỉ là gợi ý UX, 
 *     backend vẫn là nguồn chính xác cuối cùng (guard clause).
 *   - Nút mua bị disable khi đang xử lý (isPending) để tránh double-click.
 *   - IdempotencyKey sinh từ planId + timestamp để chống trùng lặp.
 *   - Error handling dùng err.message thay vì err.response.data.title (Server Actions trả ActionResult).
 * ===================================================================
 */

import React from 'react';
import { useTranslations } from 'next-intl';
import { SubscriptionPlan } from '../types';
import { useSubscribe } from '../hooks/useSubscriptions';
import { useWalletStore } from '@/store/walletStore';
import { toast } from 'react-hot-toast';

interface SubscriptionPlanCardProps {
  plan: SubscriptionPlan;
}

export const SubscriptionPlanCard: React.FC<SubscriptionPlanCardProps> = ({ plan }) => {
  const t = useTranslations('Subscription');
  const { mutate: subscribe, isPending } = useSubscribe();

  /*
   * Lấy số dư Diamond hiện tại từ Zustand store.
   * Lưu ý: Đây chỉ là giá trị cache phía client, có thể bị stale (FUTURE-5).
   * Backend sẽ thực hiện kiểm tra chính xác qua Wallet.DebitAsync.
   * Client check chỉ để UX: ngăn user bấm nút khi biết chắc không đủ tiền.
   */
  const walletBalance = useWalletStore(state => state.balance?.diamondBalance || 0);

  /**
   * Xử lý khi user bấm nút "Mua ngay".
   * 1. Check client-side: đủ Diamond không? (UX only, không phải security check)
   * 2. Tạo idempotencyKey duy nhất để chống double-click
   * 3. Gọi Server Action subscribeToPlanAction qua useMutation
   * 4. Hiển thị toast thành công/thất bại
   */
  const handleSubscribe = () => {
    /* 
     * FUTURE-5 MITIGATE: Thêm cảnh báo rõ ràng hơn cho trường hợp stale balance.
     * Nếu client cache nói "đủ tiền" nhưng thực tế không đủ, backend sẽ reject 
     * và trả error message có nghĩa (thay vì crash).
     */
    if (walletBalance < plan.priceDiamond) {
      toast.error(t('notEnoughDiamond'));
      return;
    }

    // Tạo idempotency key: planId + timestamp → độc nhất cho mỗi lần bấm
    const idempotencyKey = `sub_${plan.id}_${Date.now()}`;
    subscribe(
      { planId: plan.id, idempotencyKey },
      {
        onSuccess: () => toast.success(t('subscribeSuccess', { name: plan.name })),
        /* 
         * DEBT-6 FIX: Error từ Server Actions là Error object có .message,
         * KHÔNG phải Axios response có .response.data.title.
         * ActionResult pattern: throw new Error(res.error) → err.message chứa lỗi.
         */
        onError: (err: Error) => toast.error(err.message || t('subscribeError')),
      }
    );
  };

  return (
    <div className="bg-slate-800/50 border border-slate-700/50 rounded-2xl p-6 flex flex-col h-full hover:border-[#F6D365]/30 transition-colors">
      {/* Phần header: tên gói và mô tả */}
      <div className="mb-4">
        <h3 className="text-xl font-bold text-white mb-2">{plan.name}</h3>
        <p className="text-slate-400 text-sm">{plan.description}</p>
      </div>

      {/* Phần giá: hiển thị số Diamond và thời hạn */}
      <div className="mb-6 flex items-baseline gap-2">
        <span className="text-3xl font-extrabold text-[#F6D365]">{plan.priceDiamond}</span>
        <span className="text-sm font-medium text-slate-400">
          / {plan.durationDays} {t('days')}
        </span>
      </div>

      {/* Danh sách quyền lợi: duyệt qua entitlements map {key: quota} */}
      <div className="flex-grow">
        <ul className="space-y-3 mb-6">
          {Object.entries(plan.entitlements).map(([key, quota]) => (
            <li key={key} className="flex items-start text-sm text-slate-300">
              <svg className="w-5 h-5 text-emerald-400 mr-3 shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
              </svg>
              {quota > 0 ? (
                <span>{quota} <span className="text-slate-400 ml-1">{t(`entitlement_${key}`)}</span></span>
              ) : (
                <span>{t('unlimited')} <span className="text-slate-400 ml-1">{t(`entitlement_${key}`)}</span></span>
              )}
            </li>
          ))}
        </ul>
      </div>

      {/* Nút mua: disable khi đang xử lý để tránh multi-click */}
      <button
        onClick={handleSubscribe}
        disabled={isPending}
        className="w-full mt-auto py-3 px-4 bg-gradient-to-r from-[#F6D365] to-[#FDA085] hover:opacity-90 disabled:opacity-50 text-slate-900 font-bold rounded-xl shadow-lg transition-all"
      >
        {isPending ? t('processing') : t('buyNow')}
      </button>
    </div>
  );
};
