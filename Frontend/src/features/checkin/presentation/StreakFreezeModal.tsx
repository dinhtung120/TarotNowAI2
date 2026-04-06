'use client';

import React, { useState } from 'react';
import { v4 as uuidv4 } from 'uuid';
import { usePurchaseFreeze } from '../application/hooks';

interface StreakFreezeModalProps {
  isOpen: boolean;
  onClose: () => void;
  preBreakStreak: number;
  freezePrice: number;
  remainingSeconds: number;
}

export const StreakFreezeModal = ({
  isOpen,
  onClose,
  preBreakStreak,
  freezePrice,
  remainingSeconds,
}: StreakFreezeModalProps) => {
  const { mutate: purchaseFreeze, isPending } = usePurchaseFreeze();
  const [errorDesc, setErrorDesc] = useState<string | null>(null);

  const getErrorDescription = (error: unknown): string => {
    if (error instanceof Error && error.message) {
      return error.message;
    }

    if (typeof error === 'object' && error !== null) {
      const maybeHttpError = error as {
        response?: {
          data?: {
            message?: string;
          };
        };
      };

      if (maybeHttpError.response?.data?.message) {
        return maybeHttpError.response.data.message;
      }
    }

    return 'Có lỗi khi mua quyền cứu vớt.';
  };

  if (!isOpen) return null;

  // Tính lùi thời gian cho vui mắt UX
  const hours = Math.floor(remainingSeconds / 3600);
  const minutes = Math.floor((remainingSeconds % 3600) / 60);

  const handleBuy = () => {
    setErrorDesc(null);
    purchaseFreeze(
      { idempotencyKey: uuidv4() },
      {
        onSuccess: () => {
          onClose(); // Thành công rồi, đóng phắc cái rẹt. UI ngoài kia tự Query invalidate & đổi lửa.
        },
        onError: (err: unknown) => {
          setErrorDesc(getErrorDescription(err));
        },
      }
    );
  };

  return (
    <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm animate-in fade-in duration-200">
      <div className="bg-slate-900 border border-slate-800 rounded-2xl w-full max-w-sm p-6 shadow-2xl relative">
        <button
          onClick={onClose}
          className="absolute top-4 right-4 text-slate-400 hover:text-white transition-colors"
        >
          <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>

        <div className="text-center">
          <div className="w-16 h-16 bg-blue-500/20 text-blue-400 rounded-full flex items-center justify-center mx-auto mb-4">
             <svg className="w-8 h-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
             </svg>
          </div>
          
          <h3 className="text-xl font-bold font-space text-white mb-2">Đóng băng Streak</h3>
          <p className="text-slate-400 text-sm mb-4">
             Bạn đã đánh mất chuỗi {preBreakStreak} ngày kiên trì! Khôi phục ngay trước khi quá muộn.
          </p>

          <div className="bg-slate-800/50 rounded-xl p-3 mb-6">
             <div className="flex justify-between items-center text-sm font-inter">
                <span className="text-slate-300">Giá chuộc (Kim Cương)</span>
                <span className="font-bold text-cyan-400 flex items-center gap-1">
                   <svg className="w-4 h-4" viewBox="0 0 24 24" fill="currentColor">
                     <path d="M11 2L2 14.5L12 22L22 14.5L13 2H11ZM12 4.4L18.4 13L12 18.5L5.6 13L12 4.4Z" />
                   </svg>
                   {freezePrice}
                </span>
             </div>
             <div className="flex justify-between items-center mt-2 text-sm font-inter">
                <span className="text-slate-300">Thời gian còn lại</span>
                <span className="font-semibold text-red-400">
                   {hours}h {minutes}m
                </span>
             </div>
          </div>

          {errorDesc && (
            <p className="text-red-400 text-xs mb-4">{errorDesc}</p>
          )}

          <div className="flex gap-3">
             <button
               onClick={onClose}
               className="flex-1 py-2 px-4 bg-slate-800 hover:bg-slate-700 text-white rounded-lg transition-colors font-medium text-sm"
             >
               Mặc Kệ
             </button>
             <button
               onClick={handleBuy}
               disabled={isPending}
               className="flex-1 py-2 px-4 bg-gradient-to-r from-blue-600 to-cyan-500 hover:from-blue-500 hover:to-cyan-400 text-white rounded-lg transition-all shadow-[0_0_15px_rgba(56,189,248,0.4)] hover:shadow-[0_0_20px_rgba(56,189,248,0.6)] font-medium text-sm disabled:opacity-50"
             >
               {isPending ? 'Đang Mua...' : 'Mua Khôi Phục'}
             </button>
          </div>
        </div>
      </div>
    </div>
  );
};
