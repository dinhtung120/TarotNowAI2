'use client';

import React from 'react';
import { cn } from '@/lib/utils';

export const StreakDisplay = ({ streak, isBroken }: { streak: number; isBroken: boolean }) => {
  return (
    <div className={cn("flex items-center gap-2")}>
      {}
      <div
        className={cn(
          "flex",
          "h-10",
          "w-10",
          "items-center",
          "justify-center",
          "rounded-full",
          "shadow-inner",
          isBroken ? "bg-gray-700/50 grayscale" : "bg-orange-500/20",
        )}
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 24 24"
          fill={isBroken ? '#9ca3af' : '#f97316'}
          className={cn("h-6", "w-6", !isBroken && streak > 0 ? "animate-pulse" : null)}
        >
          <path
            fillRule="evenodd"
            d="M12.963 2.286a.75.75 0 00-1.071-.136 9.742 9.742 0 00-3.539 6.177A7.547 7.547 0 016.648 6.61a.75.75 0 00-1.152.082A9 9 0 1015.68 4.534a7.46 7.46 0 01-2.717-2.248z"
            clipRule="evenodd"
          />
        </svg>
      </div>

      <div className={cn("flex flex-col")}>
        <span className={cn("font-space", "text-xl", "font-bold", isBroken ? "text-gray-400" : "text-orange-400")}>
          {streak} Ngày
        </span>
        <span className={cn("text-xs text-gray-400 font-inter")}>
          {isBroken ? 'Đứt chuỗi' : 'Chuỗi đăng nhập'}
        </span>
      </div>
    </div>
  );
};
