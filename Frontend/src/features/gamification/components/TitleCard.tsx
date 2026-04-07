"use client";

import { CheckCircle2, Lock, Medal } from "lucide-react";
import type { TitleDefinition } from "@/features/gamification/gamification.types";
import { rarityColors } from "@/features/gamification/components/title-card/rarityColors";

interface TitleCardProps {
 title: TitleDefinition;
 isOwned: boolean;
 isActive: boolean;
 localize: (vi: string, en: string) => string;
 hiddenLabel: string;
 onClick: () => void;
}

export function TitleCard({ title, isOwned, isActive, localize, hiddenLabel, onClick }: TitleCardProps) {
 const style = rarityColors[title.rarity] || rarityColors.Common;
 const ringClass = style.ring.replace("border-", "ring-").replace("/40", "/30");

 return (
  <div onClick={onClick} className={`group relative overflow-hidden rounded-2xl border p-5 flex flex-col items-center text-center transition-all duration-500 cursor-pointer ${isActive ? `bg-gradient-to-b from-slate-800/80 ${style.bg} ${style.ring} shadow-lg scale-[1.02] ring-1 ${ringClass}` : isOwned ? "bg-slate-900/60 border-slate-700 hover:-translate-y-1 hover:border-slate-600" : "bg-slate-900/40 border-slate-800 opacity-60 grayscale hover:grayscale-0 transition-all"} backdrop-blur-xl`}>
   <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_top,_var(--tw-gradient-stops))] from-white/5 to-transparent opacity-0 group-hover:opacity-100 transition-opacity" />
   <div className="relative mb-4"><div className={`w-16 h-16 rounded-full flex items-center justify-center border-2 transition-all duration-500 ${isActive ? style.iconBg : isOwned ? "border-slate-700 bg-slate-800 group-hover:border-slate-600" : "border-slate-800 bg-slate-900"}`}>{isActive ? <CheckCircle2 className={`w-8 h-8 ${style.iconText}`} /> : isOwned ? <Medal className="w-6 h-6 text-slate-500 group-hover:text-slate-400 transition-colors" /> : <Lock className="w-6 h-6 text-slate-600" />}</div></div>
   <h3 className={`text-sm font-bold mb-1 ${isActive ? style.textTitle : isOwned ? "text-slate-300 group-hover:text-slate-200" : "text-slate-500"}`}>{localize(title.nameVi, title.nameEn)}</h3>
   <p className={`text-xs ${isActive ? style.text : "text-slate-400"} line-clamp-3 leading-relaxed mt-1 opacity-80`}>{isOwned ? localize(title.descriptionVi, title.descriptionEn) : hiddenLabel}</p>
   <div className="mt-auto pt-4 text-[10px] uppercase font-semibold tracking-wider text-slate-500">{title.rarity}</div>
  </div>
 );
}
