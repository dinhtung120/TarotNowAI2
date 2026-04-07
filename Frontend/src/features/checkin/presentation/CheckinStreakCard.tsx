"use client";

import CheckinFreezeButton from "./CheckinFreezeButton";
import { CheckinButton } from "./CheckinButton";
import { StreakDisplay } from "./StreakDisplay";
import { StreakFreezeModal } from "./StreakFreezeModal";
import { useCheckinStreakCardState } from "./hooks/useCheckinStreakCardState";

export const CheckinStreakCard = () => {
  const vm = useCheckinStreakCardState();

  if (vm.isLoading || !vm.streakData) {
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
            isBroken={vm.streakData.isStreakBroken}
            streak={vm.streakData.currentStreak}
          />
          {vm.streakData.canBuyFreeze && (
            <CheckinFreezeButton
              preBreakStreak={vm.streakData.preBreakStreak}
              onClick={vm.openFreezeModal}
            />
          )}
        </div>

        <div className="z-10 flex w-full flex-col gap-1.5 sm:max-w-xs">
          <CheckinButton isCheckedIn={vm.streakData.todayCheckedIn} />
          <p className="font-inter text-center text-[10px] text-slate-500">
            Rút bài AI để tăng lửa Streak hàng ngày.
          </p>
        </div>
      </div>

      <StreakFreezeModal
        freezePrice={vm.streakData.freezePrice}
        isOpen={vm.isFreezeModalOpen}
        preBreakStreak={vm.streakData.preBreakStreak}
        remainingSeconds={vm.streakData.freezeWindowRemainingSeconds}
        onClose={vm.closeFreezeModal}
      />
    </>
  );
};
