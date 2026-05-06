export const rarityColors: Record<string, { bg: string; ring: string; iconBg: string; iconText: string; text: string; textTitle: string }> = {
 Common: {
  bg: "to-slate-600/20",
  ring: "border-slate-500/40",
  iconBg: "bg-slate-500/10 border-slate-500/50 shadow-[0_0_20px_rgba(100,116,139,0.3)]",
  iconText: "text-slate-400 drop-shadow-[0_0_8px_rgba(100,116,139,0.8)]",
  text: "text-slate-300",
  textTitle: "text-slate-200",
 },
 Rare: {
  bg: "to-blue-600/20",
  ring: "border-blue-500/40",
  iconBg: "bg-blue-500/10 border-blue-500/50 shadow-[0_0_20px_rgba(59,130,246,0.3)]",
  iconText: "text-blue-400 drop-shadow-[0_0_8px_rgba(59,130,246,0.8)]",
  text: "text-blue-300",
  textTitle: "text-blue-200",
 },
 Epic: {
  bg: "to-purple-600/20",
  ring: "border-purple-500/40",
  iconBg: "bg-purple-500/10 border-purple-500/50 shadow-[0_0_20px_rgba(168,85,247,0.3)]",
  iconText: "text-purple-400 drop-shadow-[0_0_8px_rgba(168,85,247,0.8)]",
  text: "text-purple-300",
  textTitle: "text-purple-200",
 },
 Legendary: {
  bg: "to-amber-500/20",
  ring: "border-amber-500/40",
  iconBg: "bg-amber-500/10 border-amber-500/50 shadow-[0_0_20px_rgba(245,158,11,0.3)]",
  iconText: "text-amber-400 drop-shadow-[0_0_8px_rgba(245,158,11,0.8)]",
  text: "text-amber-300",
  textTitle: "text-amber-200",
 },
};
