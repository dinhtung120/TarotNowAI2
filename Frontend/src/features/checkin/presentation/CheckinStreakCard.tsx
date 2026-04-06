'use client';

import React, { useState } from 'react';
import { useStreakStatus } from '../application/hooks';
import { StreakDisplay } from './StreakDisplay';
import { CheckinButton } from './CheckinButton';
import { StreakFreezeModal } from './StreakFreezeModal';

/*
 * ===================================================================
 * COMPONENT: CheckinStreakCard
 * MỤC ĐÍCH:
 *   Banner thần tài chưng ở màn hình HomePage. Tích hợp điểm danh Gold + Show lửa Streak.
 *   Nếu phát hiện hệ thống báo IsStreakBroken (gãy cày) -> Bật nút khơi gợi mua Freeze.
 * ===================================================================
 */
export const CheckinStreakCard = () => {
  const { data: streakData, isLoading } = useStreakStatus();
  const [isFreezeModalOpen, setIsFreezeModalOpen] = useState(false);

  if (isLoading || !streakData) {
    return (
      <div className="w-full h-28 bg-slate-800/50 rounded-2xl animate-pulse border border-slate-700/50" />
    );
  }

  const { 
    currentStreak, 
    isStreakBroken, 
    preBreakStreak, 
    todayCheckedIn, 
    canBuyFreeze, 
    freezePrice, 
    freezeWindowRemainingSeconds 
  } = streakData;

  return (
    <>
      <div className="w-full bg-slate-900/40 backdrop-blur-md rounded-2xl border border-slate-700/50 p-5 flex flex-col sm:flex-row items-center justify-between gap-5 relative overflow-hidden">
        
        {/* Lớp áo ánh sáng nhẹ cho sang trọng */}
        <div className="absolute top-0 right-0 -mr-16 -mt-16 w-32 h-32 bg-orange-500/10 rounded-full blur-[40px]" />
        
        {/* Trái: Combo Streak Display + Mồi chài mua Freeze */}
        <div className="flex flex-col gap-2">
          <StreakDisplay streak={currentStreak} isBroken={isStreakBroken} />
          
          {canBuyFreeze && (
            <button 
              onClick={() => setIsFreezeModalOpen(true)}
              className="text-xs font-inter font-medium text-cyan-400 hover:text-cyan-300 bg-cyan-950/40 border border-cyan-800/50 rounded-md px-3 py-1.5 transition-colors self-start flex items-center gap-1.5 shadow-[0_0_10px_rgba(6,182,212,0.15)]"
            >
              <svg className="w-3.5 h-3.5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 10V3L4 14h7v7l9-11h-7z" />
              </svg>
              Cứu đứt chuỗi {preBreakStreak} ngày
            </button>
          )}
        </div>

        {/* Phải: Combo Điểm danh mót Gold */}
        <div className="w-full sm:max-w-xs flex flex-col gap-1.5 z-10">
          <CheckinButton isCheckedIn={todayCheckedIn} />
          <p className="text-[10px] text-center text-slate-500 font-inter">
            Rút bài AI để tăng lửa Streak hàng ngày.
          </p>
        </div>
      </div>

      <StreakFreezeModal
        isOpen={isFreezeModalOpen}
        onClose={() => setIsFreezeModalOpen(false)}
        preBreakStreak={preBreakStreak}
        freezePrice={freezePrice}
        remainingSeconds={freezeWindowRemainingSeconds}
      />
    </>
  );
};
