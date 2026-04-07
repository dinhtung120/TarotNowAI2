"use client";

import { useState } from "react";
import { useStreakStatus } from "../application/hooks";
import CheckinFreezeButton from "./CheckinFreezeButton";
import { CheckinButton } from "./CheckinButton";
import { StreakDisplay } from "./StreakDisplay";
import { StreakFreezeModal } from "./StreakFreezeModal";

export const CheckinStreakCard = () => {
  const { data: streakData, isLoading } = useStreakStatus();
  const [isFreezeModalOpen, setIsFreezeModalOpen] = useState(false);

  if (isLoading || !streakData) {
    return (
      <div className="h-28 w-full animate-pulse rounded-2xl border border-slate-700/50 bg-slate-800/50" />
    );
  }

  return (
    <>
      <div className="relative flex w-full flex-col items-center justify-between gap-5 overflow-hidden rounded-2xl border border-slate-700/50 bg-slate-900/40 p-5 backdrop-blur-md sm:flex-row">
        <div className="absolute -top-16 -right-16 h-32 w-32 rounded-full bg-orange-500/10 blur-[40px]" />

        <div className="flex flex-col gap-2">
          <StreakDisplay
            isBroken={streakData.isStreakBroken}
            streak={streakData.currentStreak}
          />
          {streakData.canBuyFreeze && (
            <CheckinFreezeButton
              preBreakStreak={streakData.preBreakStreak}
              onClick={() => setIsFreezeModalOpen(true)}
            />
          )}
        </div>

        <div className="z-10 flex w-full flex-col gap-1.5 sm:max-w-xs">
          <CheckinButton isCheckedIn={streakData.todayCheckedIn} />
          <p className="font-inter text-center text-[10px] text-slate-500">
            Rút bài AI để tăng lửa Streak hàng ngày.
          </p>
        </div>
      </div>

      <StreakFreezeModal
        freezePrice={streakData.freezePrice}
        isOpen={isFreezeModalOpen}
        preBreakStreak={streakData.preBreakStreak}
        remainingSeconds={streakData.freezeWindowRemainingSeconds}
        onClose={() => setIsFreezeModalOpen(false)}
      />
    </>
  );
};
