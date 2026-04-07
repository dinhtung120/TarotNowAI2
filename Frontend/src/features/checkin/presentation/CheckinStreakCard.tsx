"use client";

import CheckinFreezeButton from "./CheckinFreezeButton";
import { CheckinButton } from "./CheckinButton";
import { StreakDisplay } from "./StreakDisplay";
import { StreakFreezeModal } from "./StreakFreezeModal";
import { useCheckinStreakCardState } from "./hooks/useCheckinStreakCardState";
import { cn } from "@/lib/utils";

export const CheckinStreakCard = () => {
  const vm = useCheckinStreakCardState();

  if (vm.isLoading || !vm.streakData) {
    return (
      <div className={cn("h-28", "w-full", "animate-pulse", "rounded-2xl", "border", "border-slate-700/50", "bg-slate-800/50")} />
    );
  }

  return (
    <>
      <div
        className={cn(
          "relative",
          "flex",
          "w-full",
          "flex-row",
          "items-center",
          "justify-between",
          "gap-5",
          "overflow-hidden",
          "rounded-2xl",
          "border",
          "border-slate-700/50",
          "bg-slate-900/40",
          "p-5",
          "backdrop-blur-md",
        )}
      >
        <div className={cn("absolute", "-right-16", "-top-16", "h-32", "w-32", "rounded-full", "bg-orange-500/10", "blur-2xl")} />

        <div className={cn("flex", "flex-col", "gap-2")}>
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

        <div className={cn("z-10", "flex", "w-full", "max-w-xs", "flex-col", "gap-1.5")}>
          <CheckinButton isCheckedIn={vm.streakData.todayCheckedIn} />
          <p className={cn("text-center", "font-inter", "text-xs", "text-slate-500")}>
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
