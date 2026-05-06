export const leaderboardRowClass = (index: number) => {
  if (index === 0) {
    return "bg-gradient-to-r from-amber-500/15 to-transparent border border-amber-500/20 shadow-[0_4px_30px_rgba(245,158,11,0.05)]";
  }

  if (index === 1) {
    return "bg-gradient-to-r from-slate-300/10 to-transparent border border-slate-300/20";
  }

  if (index === 2) {
    return "bg-gradient-to-r from-amber-700/10 to-transparent border border-amber-700/20";
  }

  return "bg-slate-900/40 border border-slate-800/60 hover:bg-slate-800/60 hover:translate-x-1";
};
