'use client';

import { CheckCircle2, Medal } from 'lucide-react';
import { cn } from '@/lib/utils';

interface NoTitleCardProps {
  isNoTitleActive: boolean;
  noTitleLabel: string;
  hintLabel: string;
  onClick: () => void;
}

export function NoTitleCard({ isNoTitleActive, noTitleLabel, hintLabel, onClick }: NoTitleCardProps) {
  return (
    <div
      onClick={onClick}
      className={`group relative overflow-hidden rounded-2xl border p-5 flex flex-col items-center text-center transition-all duration-500 cursor-pointer ${
        isNoTitleActive
          ? 'bg-gradient-to-b from-slate-800/80 to-blue-900/20 border-blue-500/40 shadow-lg scale-[1.02] ring-1 ring-blue-500/30'
          : 'bg-slate-900/60 border-slate-800 hover:-translate-y-1 hover:border-slate-700'
      } backdrop-blur-xl`}
    >
      <div className={cn("absolute inset-0 bg-[radial-gradient(ellipse_at_top,_var(--tw-gradient-stops))] from-white/5 to-transparent opacity-0 group-hover:opacity-100 transition-opacity")} />

      <div className={cn("relative mb-4")}>
        <div
          className={`w-16 h-16 rounded-full flex items-center justify-center border-2 ${
            isNoTitleActive
              ? 'border-blue-400/50 bg-blue-400/10 shadow-[0_0_20px_rgba(59,130,246,0.3)]'
              : 'border-slate-700 bg-slate-800'
          }`}
        >
          {isNoTitleActive ? (
            <CheckCircle2 className={cn("w-8 h-8 text-blue-400 drop-shadow-[0_0_8px_rgba(59,130,246,0.8)]")} />
          ) : (
            <Medal className={cn("w-6 h-6 text-slate-600")} />
          )}
        </div>
      </div>

      <h3 className={`text-sm font-bold mb-1 ${isNoTitleActive ? 'text-blue-200' : 'text-slate-400'}`}>{noTitleLabel}</h3>

      <p className={cn("text-xs text-slate-500 line-clamp-3 leading-relaxed mt-1")}>{hintLabel}</p>
    </div>
  );
}
