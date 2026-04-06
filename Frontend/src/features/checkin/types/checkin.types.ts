/*
 * ===================================================================
 * FILE: checkin.types.ts
 * NAMESPACE: features/checkin/types
 * ===================================================================
 * MỤC ĐÍCH:
 *   Định nghĩa các interface cho DTO trả về từ API CheckIn, Streak.
 *   Dùng để map data chặt chẽ (strictly typed) trong React & TanStack Query.
 * ===================================================================
 */

export interface IDailyCheckInResult {
  goldRewarded: number;
  isAlreadyCheckedIn: boolean;
  businessDate: string;
  currentStreak: number;
}

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

export interface IPurchaseStreakFreezeResult {
  success: boolean;
  restoredStreak: number;
  diamondCost: number;
}

export interface IPurchaseStreakFreezeCommand {
  idempotencyKey: string;
}
