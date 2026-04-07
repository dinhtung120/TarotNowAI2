'use client';

import React from 'react';
import { cn } from '@/lib/utils';
import { useCheckinButtonState } from './hooks/useCheckinButtonState';

interface CheckinButtonProps {
  isCheckedIn: boolean;
}

export const CheckinButton = ({ isCheckedIn }: CheckinButtonProps) => {
  const vm = useCheckinButtonState({ isCheckedIn });

  if (isCheckedIn) {
    return (
      <button 
        type="button"
        disabled
        className={cn("relative overflow-hidden w-full py-3 px-4 bg-gradient-to-r from-emerald-500/20 to-teal-500/20 border border-emerald-500/30 text-emerald-400 rounded-xl font-medium cursor-not-allowed flex items-center justify-center gap-2")}
      >
        <svg className={cn("w-5 h-5")} fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
        </svg>
        Đã Điểm Danh
      </button>
    );
  }

  return (
    <button
      type="button"
      onClick={vm.handleClick}
      disabled={vm.isPending}
      className={`relative overflow-hidden w-full py-3 px-4 bg-gradient-to-r from-amber-500 to-yellow-400 hover:from-amber-400 hover:to-yellow-300 text-amber-950 rounded-xl font-bold font-inter shadow-[0_0_15px_rgba(251,191,36,0.3)] hover:shadow-[0_0_25px_rgba(251,191,36,0.5)] transition-all flex items-center justify-center gap-2 group ${vm.animating ? 'scale-95' : 'hover:-translate-y-0.5'}`}
    >
      <div className={`absolute inset-0 bg-white/30 skew-x-12 -translate-x-full ${vm.animating ? 'animate-[shimmer_0.8s_ease-out]' : 'group-hover:animate-[shimmer_1s_ease-out]'}`} />
      
      {vm.isPending ? (
        <span className={cn("animate-pulse")}>Đang Lượm...</span>
      ) : (
        <>
          <svg className={cn("w-5 h-5 drop-shadow-sm")} viewBox="0 0 24 24" fill="currentColor">
            <path d="M12 2C6.48 2 2 6.48 2 12C2 17.52 6.48 22 12 22C17.52 22 22 17.52 22 12C22 6.48 17.52 2 12 2ZM12 20C7.59 20 4 16.41 4 12C4 7.59 7.59 4 12 4C16.41 4 20 7.59 20 12C20 16.41 16.41 20 12 20ZM12 6C8.69 6 6 8.69 6 12C6 15.31 8.69 18 12 18C15.31 18 18 15.31 18 12C18 8.69 15.31 6 12 6Z" />
          </svg>
          Nhận Gold Điểm Danh
        </>
      )}
    </button>
  );
};
