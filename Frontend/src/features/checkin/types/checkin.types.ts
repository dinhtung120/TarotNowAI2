

export interface IStreakStatusResult {
  currentStreak: number;
  lastStreakDate: string | null;
  preBreakStreak: number;
  isStreakBroken: boolean;
  freezePrice: number;
  freezeWindowRemainingSeconds: number;
  canBuyFreeze: boolean;
  todayCheckedIn: boolean;
  expMultiplier: number;
}
